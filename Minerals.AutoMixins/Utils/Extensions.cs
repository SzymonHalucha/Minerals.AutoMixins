namespace Minerals.AutoMixins.Utils
{
    public static class Extensions
    {
        public static string GetTargetNodeName(this GeneratorAttributeSyntaxContext context)
        {
            return ((BaseTypeDeclarationSyntax)context.TargetNode).Identifier.ValueText;
        }

        public static NamespaceDeclarationSyntax? GetTargetNodeNamespace(this GeneratorAttributeSyntaxContext context)
        {
            return context.TargetNode.FirstAncestorOrSelf<NamespaceDeclarationSyntax>();
        }

        public static CompilationUnitSyntax? GetTargetNodeCompilationUnit(this GeneratorAttributeSyntaxContext context)
        {
            return context.TargetNode.FirstAncestorOrSelf<CompilationUnitSyntax>();
        }

        public static SyntaxToken GetTargetNodeAccessModifier(this GeneratorAttributeSyntaxContext context)
        {
            return ((MemberDeclarationSyntax)context.TargetNode).Modifiers.First(x =>
                x.IsKind(SyntaxKind.PrivateKeyword)
                || x.IsKind(SyntaxKind.InternalKeyword)
                || x.IsKind(SyntaxKind.ProtectedKeyword)
                || x.IsKind(SyntaxKind.PublicKeyword));
        }

        public static bool HasTargetNodeModifier(this GeneratorAttributeSyntaxContext context, SyntaxKind kind)
        {
            return ((MemberDeclarationSyntax)context.TargetNode).Modifiers.Any(x => x.IsKind(kind));
        }
    }
}