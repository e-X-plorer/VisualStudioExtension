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
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // MakePartialAsync: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
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
            Document currentDocument, ClassDeclarationSyntax memberContainer, CancellationToken cancellationToken)
        {
            var newNodeOldFile = memberContainer.RemoveNodes(nodesToSeparate, SyntaxRemoveOptions.KeepNoTrivia)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            var rootOfOldFile =
                await memberContainer.SyntaxTree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
            rootOfOldFile = rootOfOldFile.ReplaceNode(memberContainer, newNodeOldFile);

            var newNodeNewFile = memberContainer.WithMembers(new SyntaxList<MemberDeclarationSyntax>(nodesToSeparate))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
            var namespaceDeclaration = memberContainer.GetParentNamespace();

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
                .AddDocument(DocumentId.CreateNewId(currentDocument.Project.Id), currentDocument.Name,
                    Formatter.Format(rootOfNewFile, solution.Workspace));
        }

        private async Task<Solution> MakePartialAsync(Document document, ClassDeclarationSyntax classDeclaration,
            CancellationToken cancellationToken)
        {
            var solution = document.Project.Solution;
            var oldNode = classDeclaration;

            var indexIfLong = oldNode.IndexOfChildContainingNthOccurrence('\n', GlobalUserSettings.MaxLinesCount + 1);
            var rangeStart = Math.Min(indexIfLong == -1 ? int.MaxValue : indexIfLong,
                GlobalUserSettings.MaxMemberCount);

            var nodesToSeparate = oldNode.Members.ToList();
            nodesToSeparate = nodesToSeparate.GetRange(rangeStart, nodesToSeparate.Count - rangeStart);

            return await MoveMembersToNewFile(nodesToSeparate, document, oldNode, cancellationToken);

            //To be removed.
            /*var newNodeOldFile = oldNode.RemoveNodes(nodesToSeparate, SyntaxRemoveOptions.KeepNoTrivia)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            var rootOfOldFile = await classDeclaration.SyntaxTree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
            rootOfOldFile = rootOfOldFile.ReplaceNode(oldNode, newNodeOldFile);

            var newNodeNewFile = oldNode.WithMembers(new SyntaxList<MemberDeclarationSyntax>(nodesToSeparate))
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
            var namespaceDeclaration = oldNode.GetParentNamespace();

            var rootOfNewFile = SyntaxFactory.CompilationUnit()
                .AddUsings(rootOfOldFile.Usings.ToArray())
                .AddExterns(rootOfOldFile.Externs.ToArray());
            rootOfNewFile = namespaceDeclaration == null
                ? rootOfNewFile.AddMembers(newNodeNewFile)
                : rootOfNewFile.AddMembers(SyntaxFactory.NamespaceDeclaration(namespaceDeclaration.Name,
                    namespaceDeclaration.Externs,
                    namespaceDeclaration.Usings,
                    new SyntaxList<MemberDeclarationSyntax>(newNodeNewFile)));

            return solution.WithDocumentSyntaxRoot(document.Id, Formatter.Format(rootOfOldFile, solution.Workspace))
                .AddDocument(DocumentId.CreateNewId(document.Project.Id), document.Name,
                    Formatter.Format(rootOfNewFile, solution.Workspace));*/
        }
    }
}
