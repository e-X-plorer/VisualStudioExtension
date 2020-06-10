using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using EnvDTE;
using ClassLengthAnalyzer;
using Extension.Properties;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell.Interop;
using Document = Microsoft.CodeAnalysis.Document;

namespace Extension
{
    public partial class RefactorDialog : Form
    {
        /// <summary>
        /// Currently active document in a Visual Studio instance.
        /// </summary>
        private readonly Document _activeDocument;

        public RefactorDialog()
        {
            InitializeComponent();
            ThreadHelper.ThrowIfNotOnUIThread();

            var solution = ((IComponentModel) Package.GetGlobalService(typeof(SComponentModel)))
                .GetService<VisualStudioWorkspace>().CurrentSolution;
            var activeDocument = ((DTE) Package.GetGlobalService(typeof(DTE))).ActiveDocument;
            var documentId = solution.GetDocumentIdsWithFilePath(activeDocument.FullName).FirstOrDefault();
            if (documentId == null)
            {
                throw new Exception(Resources.UnexpectedErrorMessage);
            }

            _activeDocument = solution.GetDocument(documentId);
            var syntaxRootTask = _activeDocument.GetSyntaxRootAsync();
            var classesInCurrentFile = syntaxRootTask.Result.GetClassesFromNode();
            ComboBox.DataSource = classesInCurrentFile;
            ComboBox.DisplayMember = "Identifier";
            ComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(ComboBox.SelectedItem is ClassDeclarationSyntax selectedClass))
            {
                throw new Exception(Resources.UnexpectedErrorMessage);
            }

            var members = selectedClass.Members.ToList();
            CheckedListBox.DataSource = members;
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (CheckedListBox.CheckedItems.Count == 0)
            {
                VsShellUtilities.ShowMessageBox(ServiceProvider.GlobalProvider,
                    Resources.NoMembersSelectedMessage, Resources.NoMembersSelectedTitle,
                    OLEMSGICON.OLEMSGICON_WARNING, OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                return;
            }

            if (FileNameBox.Text.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            {
                VsShellUtilities.ShowMessageBox(ServiceProvider.GlobalProvider,
                    Resources.IllegalCharactersMessage, Resources.IllegalCharactersTitle,
                    OLEMSGICON.OLEMSGICON_WARNING, OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                return;
            }

            if (!(ComboBox.SelectedItem is ClassDeclarationSyntax selectedClassOld))
            {
                throw new Exception(Resources.UnexpectedErrorMessage);
            }

            var newFileName = FileNameBox.Text;
            if (!newFileName.EndsWith(".cs"))
            {
                newFileName += ".cs";
            }

            var solution = _activeDocument.Project.Solution;

            var checkedMembers = CheckedListBox.CheckedItems.Cast<MemberDeclarationSyntax>();
            var selectedMembers = new HashSet<MemberDeclarationSyntax>(checkedMembers, new NodeEqualityComparer());
            var nodesToSeparate = selectedClassOld.Members.Where(node => selectedMembers.Contains(node)).ToList();

            try
            {
                solution.Workspace.TryApplyChanges(MakePartialCodeFixProvider
                    .MoveMembersToNewFile(nodesToSeparate, _activeDocument, newFileName, selectedClassOld,
                        CancellationToken.None)
                    .Result);
            }
            catch (ArgumentException exception)
            {
                VsShellUtilities.ShowMessageBox(ServiceProvider.GlobalProvider,
                    exception.Message + "\n\nOperation cancelled.",
                    "Exception occured",
                    OLEMSGICON.OLEMSGICON_WARNING, OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }

            Close();
        }
    }
}
