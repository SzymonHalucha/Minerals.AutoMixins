namespace Minerals.AutoMixins
{
    public readonly struct GenerateMixinObject : IEquatable<GenerateMixinObject>
    {
        public string Name { get; }
        public string Namespace { get; }
        public string[] Usings { get; }
        public MemberDeclarationSyntax[] Members { get; }
        public ISymbol? Symbol { get; }

        public GenerateMixinObject(GeneratorAttributeSyntaxContext context)
        {
            Name = GetNameOf(context.TargetNode);
            Namespace = GetNamespaceFrom(context.TargetNode);
            Usings = GetUsingsFrom(context.TargetNode);
            Members = GetMembersOf(context.TargetNode);
            Symbol = GetSymbolOf(context);
        }

        public override bool Equals(object obj)
        {
            return obj is GenerateMixinObject genObj
            && genObj.Name.Equals(Name)
            && genObj.Namespace.Equals(Namespace)
            && genObj.Usings.SequenceEqual(Usings)
            && genObj.Members.SequenceEqual(Members)
            && SymbolEqualityComparer.Default.Equals(genObj.Symbol, Symbol);
        }

        public bool Equals(GenerateMixinObject other)
        {
            return other.Name.Equals(Name)
            && other.Namespace.Equals(Namespace)
            && other.Usings.SequenceEqual(Usings)
            && other.Members.SequenceEqual(Members)
            && SymbolEqualityComparer.Default.Equals(other.Symbol, Symbol);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Namespace, Usings, Members);
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

        private static MemberDeclarationSyntax[] GetMembersOf(SyntaxNode node)
        {
            return ((TypeDeclarationSyntax)node).Members.ToArray();
        }

        private ISymbol? GetSymbolOf(GeneratorAttributeSyntaxContext context)
        {
            return context.TargetSymbol;
        }
    }
}