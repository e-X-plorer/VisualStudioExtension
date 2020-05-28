using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Design.Serialization;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Workspaces;

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

        private async Task<Solution> MakePartialAsync(Document document, ClassDeclarationSyntax classDeclaration,
            CancellationToken cancellationToken)
        {
            var solution = document.Project.Solution;
            var oldNode = classDeclaration;

            var indexIfLong = oldNode.IndexOfChildContainingNthOccurrence('\n', 16);
            var rangeStart = Math.Min(indexIfLong == -1 ? int.MaxValue : indexIfLong, 5);

            var children = oldNode.ChildNodes().ToList();
            var newNodeOldFile = oldNode
                .RemoveNodes(children.GetRange(rangeStart, children.Count - rangeStart),
                    SyntaxRemoveOptions.KeepNoTrivia)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            var rootOfOldFile = await classDeclaration.SyntaxTree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
            rootOfOldFile = rootOfOldFile.ReplaceNode(oldNode, newNodeOldFile);

            var newNodeNewFile = oldNode.RemoveNodes(children.GetRange(0, rangeStart),
                SyntaxRemoveOptions.KeepNoTrivia).WithoutTrivia();
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

            return solution.WithDocumentSyntaxRoot(document.Id, rootOfOldFile)
                .AddDocument(DocumentId.CreateNewId(document.Project.Id), document.Name, rootOfNewFile);
        }

        //To be removed.
        private async Task<Solution> MakeUppercaseAsync(Document document, TypeDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        {
            // Compute new uppercase name.
            var identifierToken = typeDecl.Identifier;
            var newName = identifierToken.Text.ToUpperInvariant();

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDecl, cancellationToken);

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return newSolution;
        }
    }
}
