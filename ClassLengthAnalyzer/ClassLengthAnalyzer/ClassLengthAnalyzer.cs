using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ClassLengthAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassLengthAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ClassTooLong";

        private static readonly LocalizableString Title = Resources.AnalyzerTitle;
        private static readonly LocalizableString MessageFormat = Resources.AnalyzerMessageFormat;

        private const string Category = "Design";

        //private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        private static readonly DiagnosticDescriptor ClassLengthRule = new DiagnosticDescriptor(DiagnosticId, Title,
            MessageFormat, Category, DiagnosticSeverity.Warning, true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(ClassLengthRule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            if (!Settings.Default.Enabled)
            {
                return;
            }

            var childNodes = context.Node.ChildNodes().ToImmutableArray();
            var blockChildren = childNodes.Length;

            if (blockChildren <= 1 || !(context.Node is ClassDeclarationSyntax node) ||
                node.Modifiers.IndexOf(SyntaxKind.PartialKeyword) != -1)
            {
                return;
            }

            var blockLines = context.Node.ToString().Count(c => c.Equals('\n'));

            if (blockChildren > Settings.Default.MaxMemberCount)
            {
                context.ReportDiagnostic(Diagnostic.Create(ClassLengthRule, context.ContainingSymbol.Locations[0],
                    context.ContainingSymbol.Name, blockChildren + " members", Settings.Default.MaxMemberCount));
                return;
            }

            if (blockLines > Settings.Default.MaxLinesCount)
            {
                context.ReportDiagnostic(Diagnostic.Create(ClassLengthRule, context.ContainingSymbol.Locations[0],
                    context.ContainingSymbol.Name, blockLines + " lines", Settings.Default.MaxLinesCount));
                return;
            }
        }
    }
}
