using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE;
using System.Windows.Controls;
using ClassLengthAnalyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
                //do something
                return;
            }

            _activeDocument = solution.GetDocument(documentId);
            var syntaxRootTask = _activeDocument.GetSyntaxRootAsync();
            var classesInCurrentFile = syntaxRootTask.Result.GetClassesFromNode();
            comboBox1.DataSource = classesInCurrentFile;
            comboBox1.DisplayMember = "Identifier";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(comboBox1.SelectedItem is ClassDeclarationSyntax selectedClass))
            {
                //do something
                return;
            }

            var members = selectedClass.Members.ToList();
            checkedListBox1.DataSource = members;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                VsShellUtilities.ShowMessageBox(ServiceProvider.GlobalProvider,
                    "Select at least one member to move to a separate file.", "No members selected",
                    OLEMSGICON.OLEMSGICON_CRITICAL, OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                return;
            }

            if (!(comboBox1.SelectedItem is ClassDeclarationSyntax selectedClassOld))
            {
                //do something
                return;
            }

            var solution = _activeDocument.Project.Solution;

            var checkedMembers = checkedListBox1.CheckedItems.Cast<MemberDeclarationSyntax>();
            var selectedMembers = new HashSet<MemberDeclarationSyntax>(checkedMembers, new NodeEqualityComparer());
            var nodesToSeparate = selectedClassOld.Members.Where(node => selectedMembers.Contains(node)).ToList();

            solution.Workspace.TryApplyChanges(MakePartialCodeFixProvider
                .MoveMembersToNewFile(nodesToSeparate, _activeDocument, selectedClassOld, CancellationToken.None)
                .Result);

            Close();
        }
    }
}
