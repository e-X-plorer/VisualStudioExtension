using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer1
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Analyzer1Analyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Analyzer1";
        public const int MaxLineCount = 10;
        public const int MaxChildNodeCount = 5;

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        //private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        private static DiagnosticDescriptor ClassLengthRule = new DiagnosticDescriptor("ClassLength", "Class is too long", 
            "{0} contains {1} lines or more than {2} children. Children: {3}", "Length", DiagnosticSeverity.Warning,
            isEnabledByDefault: true, description: "test");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(ClassLengthRule);

        public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            //context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType, SymbolKind.Field);
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
            
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var childNodes = context.Node.ChildNodes().ToImmutableArray();
            var blockLines = childNodes.Sum(node => node.ToString().Count(c => c.Equals('\n')));
            var blockChildren = childNodes.Length;
            var childrenString = Enumerable.Aggregate(childNodes, string.Empty, (current, child) => current + (child.ToString() + '\n'));

            if (blockLines > MaxLineCount || blockChildren > MaxChildNodeCount)
            {
                context.ReportDiagnostic(Diagnostic.Create(ClassLengthRule, context.ContainingSymbol.Locations[0],
                context.ContainingSymbol.Name, blockLines, blockChildren, childrenString));
            }
        }

        // private static void AnalyzeCodeBlock(CodeBlockAnalysisContext context)
        // {
        //     var blockLength = context.CodeBlock.ToFullString().Count(c => c.Equals('\n')) + 1;
        //     if (blockLength > 10)
        //     {
        //         context.ReportDiagnostic(Diagnostic.Create(Rule2, context.OwningSymbol.Locations[0], context.OwningSymbol.Name));
        //     }
        // }

        /*private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = /*(INamedTypeSymbol)#1#context.Symbol;

            // Find just those named type symbols with names containing lowercase letters.
            if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }*/
    }
}
