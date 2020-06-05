using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ClassLengthAnalyzer
{
    public static class SyntaxNodeExtensions
    {
        public static NamespaceDeclarationSyntax GetParentNamespace(this SyntaxNode node)
        {
            SyntaxNode result;
            for (result = node; result != null && !(result is NamespaceDeclarationSyntax); result = result.Parent) { }

            return result as NamespaceDeclarationSyntax;
        }

        public static int IndexOfChildContainingNthOccurrence(this SyntaxNode node, char charToFind, int occurrenceNumber)
        {
            int currentIndex, occurrences;
            for (currentIndex = 0, occurrences = 0;
                currentIndex != -1 && occurrences < occurrenceNumber;
                currentIndex = node.ToString()
                    .IndexOf(charToFind, currentIndex + 1, node.ToString().Length - currentIndex - 1),
                occurrences++) { }

            if (currentIndex == -1)
            {
                return -1;
            }

            SyntaxNode resultNode;
            for (resultNode = node.FindToken(currentIndex + node.SpanStart).Parent;
                resultNode.Parent != node;
                resultNode = resultNode.Parent) { }

            var childNodes = resultNode.Parent.ChildNodes().ToImmutableArray();
            for (var i = 0; i < childNodes.Length; i++)
            {
                if (childNodes[i].Span.Contains(currentIndex + node.SpanStart))
                {
                    return i;
                }
            }

            return -1;
        }

        public static IEnumerable<ClassDeclarationSyntax> GetClassesFromNode(this SyntaxNode root)
        {
            var allClasses = new List<ClassDeclarationSyntax>();
            var currentChildren = root.ChildNodes().ToImmutableArray();
            foreach (var node in currentChildren)
            {
                if (node is ClassDeclarationSyntax classDeclarationSyntax)
                {
                    allClasses.Add(classDeclarationSyntax);
                }

                allClasses.AddRange(GetClassesFromNode(node));
            }

            return allClasses;
        }
    }
}
