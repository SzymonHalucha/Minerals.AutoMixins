namespace Minerals.AutoMixins
{
    public readonly struct GenerateMixinObject
    {
        public string Name { get; }
        public string Namespace { get; }
        public string[] Usings { get; }
        public string[] Members { get; }

        public GenerateMixinObject(GeneratorAttributeSyntaxContext context)
        {
            Name = GetNameOf(context.TargetNode);
            Namespace = GetNamespaceFrom(context.TargetNode);
            Usings = GetUsingsFrom(context.TargetNode);
            Members = GetMembersOf(context.TargetNode);
        }

        public override bool Equals(object obj)
        {
            return obj is GenerateMixinObject genObj
            && genObj.Name.Equals(Name)
            && genObj.Usings.SequenceEqual(Usings)
            && genObj.Members.SequenceEqual(Members);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Usings, Members);
        }

        private static string GetNameOf(SyntaxNode node)
        {
            return ((BaseTypeDeclarationSyntax)node).Identifier.ValueText;
        }

        private static string GetNamespaceFrom(SyntaxNode from)
        {
            return from.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString() ?? string.Empty;
        }

        private static string[] GetUsingsFrom(SyntaxNode from)
        {
            var usings = from.FirstAncestorOrSelf<CompilationUnitSyntax>()?.Usings
                .Where(x => x.GlobalKeyword.IsKind(SyntaxKind.None))
                .Select(x => x.Name!.ToString());
            return usings != null ? usings.ToArray() : [];
        }

        private static string[] GetMembersOf(SyntaxNode node)
        {
            return ((TypeDeclarationSyntax)node).Members.Select(x => x.ToString()).ToArray();
        }
    }
}