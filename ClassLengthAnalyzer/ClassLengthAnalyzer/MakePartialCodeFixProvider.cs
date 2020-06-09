using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace ClassLengthAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MakePartialCodeFixProvider)), Shared]
    public class MakePartialCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Make partial";

        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(ClassLengthAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            //Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf()
                .OfType<ClassDeclarationSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    Title,
                    c => MakePartialAsync(context.Document, declaration, c),
                    Title),
                diagnostic);
        }

        public static async Task<Solution> MoveMembersToNewFile(IList<MemberDeclarationSyntax> nodesToSeparate,
            Document currentDocument, string newDocumentName, ClassDeclarationSyntax memberContainer,
            CancellationToken cancellationToken)
        {
            var nestedHierarchy = memberContainer.GetParentClasses().ToImmutableList();

            var newNodeOldFile = memberContainer.RemoveNodes(nodesToSeparate, SyntaxRemoveOptions.KeepNoTrivia)
                .AddPartialModifierIfNotPresent();

            var newNodeNewFile = memberContainer.WithMembers(new SyntaxList<MemberDeclarationSyntax>(nodesToSeparate))
                .AddPartialModifierIfNotPresent();

            var namespaceDeclaration = memberContainer.GetParentNamespace();
            var previousClassDeclaration = memberContainer;
            foreach (var classDeclaration in nestedHierarchy)
            {
                newNodeOldFile = classDeclaration.ReplaceNode(previousClassDeclaration, newNodeOldFile)
                    .AddPartialModifierIfNotPresent();
                newNodeNewFile = classDeclaration.WithMembers(new SyntaxList<MemberDeclarationSyntax>(newNodeNewFile))
                    .AddPartialModifierIfNotPresent();
                previousClassDeclaration = classDeclaration;
            }

            var rootOfOldFile =
                await memberContainer.SyntaxTree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
            rootOfOldFile =
                rootOfOldFile.ReplaceNode(nestedHierarchy.IsEmpty ? memberContainer : nestedHierarchy.Last(),
                    newNodeOldFile);

            var rootOfNewFile = SyntaxFactory.CompilationUnit()
                .AddUsings(rootOfOldFile.Usings.ToArray())
                .AddExterns(rootOfOldFile.Externs.ToArray());

            rootOfNewFile = namespaceDeclaration == null
                ? rootOfNewFile.AddMembers(newNodeNewFile)
                : rootOfNewFile.AddMembers(SyntaxFactory.NamespaceDeclaration(namespaceDeclaration.Name,
                    namespaceDeclaration.Externs,
                    namespaceDeclaration.Usings,
                    new SyntaxList<MemberDeclarationSyntax>(newNodeNewFile)));

            var solution = currentDocument.Project.Solution;
            return solution
                .WithDocumentSyntaxRoot(currentDocument.Id, Formatter.Format(rootOfOldFile, solution.Workspace))
                .AddDocument(DocumentId.CreateNewId(currentDocument.Project.Id), newDocumentName,
                    Formatter.Format(rootOfNewFile, solution.Workspace));
        }

        private async Task<Solution> MakePartialAsync(Document document, ClassDeclarationSyntax classDeclaration,
            CancellationToken cancellationToken)
        {
            var oldNode = classDeclaration;

            var indexIfLong = oldNode.IndexOfChildContainingNthOccurrence('\n', Settings.Default.MaxLinesCount + 1);
            var rangeStart = Math.Min(indexIfLong == -1 ? int.MaxValue : indexIfLong,
                Settings.Default.MaxMemberCount);

            var nodesToSeparate = oldNode.Members.ToList();
            nodesToSeparate = nodesToSeparate.GetRange(rangeStart, nodesToSeparate.Count - rangeStart);

            return await MoveMembersToNewFile(nodesToSeparate, document, document.Name, oldNode, cancellationToken);
        }
    }
}
