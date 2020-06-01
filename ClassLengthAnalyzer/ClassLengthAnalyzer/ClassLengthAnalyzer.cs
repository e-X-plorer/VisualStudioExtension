using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Design";

        //private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        private static readonly DiagnosticDescriptor ClassLengthRule = new DiagnosticDescriptor(DiagnosticId,
            "Class is too long",
            "Class {0} contains {1}. Maximum allowed is {2}. Class can be divided.",
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "test");

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(ClassLengthRule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var childNodes = context.Node.ChildNodes().ToImmutableArray();
            var blockChildren = childNodes.Length;

            if (blockChildren <= 1 || !(context.Node is ClassDeclarationSyntax node) ||
                node.Modifiers.IndexOf(SyntaxKind.PartialKeyword) != -1)
            {
                return;
            }

            var blockLines = context.Node.ToString().Count(c => c.Equals('\n'));

            if (blockChildren > GlobalUserSettings.MaxMemberCount)
            {
                context.ReportDiagnostic(Diagnostic.Create(ClassLengthRule, context.ContainingSymbol.Locations[0],
                    context.ContainingSymbol.Name, blockChildren + " members", GlobalUserSettings.MaxMemberCount));
                return;
            }

            if (blockLines > GlobalUserSettings.MaxLinesCount)
            {
                context.ReportDiagnostic(Diagnostic.Create(ClassLengthRule, context.ContainingSymbol.Locations[0],
                    context.ContainingSymbol.Name, blockLines + " lines", GlobalUserSettings.MaxLinesCount));
                return;
            }
        }
    }
}
