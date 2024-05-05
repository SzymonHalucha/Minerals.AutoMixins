using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceGeneratorQuery.Declarations
{
    public class ArgumentDeclaration
    {
        public ArgumentDeclaration(AttributeArgumentSyntax node)
        {
            this.SyntaxNode = node;
        }

        public readonly AttributeArgumentSyntax SyntaxNode;

        public string Expression =>
            SyntaxNode.Expression.ToString();
    }
    public class AttributeDeclaration
    {
        public AttributeDeclaration(AttributeSyntax node)
        {
            this.SyntaxNode = node;
        }

        public readonly AttributeSyntax SyntaxNode;

        public string Name =>
            SyntaxNode.Name.ToString();
        public IEnumerable<ArgumentDeclaration> Arguments =>
            SyntaxNode.ArgumentList?.Arguments.Select(a => new ArgumentDeclaration(a)) ??
            Array.Empty<ArgumentDeclaration>();
    }
}
