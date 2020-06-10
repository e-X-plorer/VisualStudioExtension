using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace ClassLengthAnalyzer
{
    public class NodeEqualityComparer : IEqualityComparer<SyntaxNode>
    {
        public bool Equals(SyntaxNode x, SyntaxNode y)
        {
            return x == y || (x != null && y != null && x.Span.Equals(y.Span));
        }

        public int GetHashCode(SyntaxNode obj)
        {
            return obj.GetHashCode();
        }
    }
}
