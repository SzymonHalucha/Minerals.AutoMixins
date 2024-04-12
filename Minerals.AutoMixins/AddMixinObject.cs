namespace Minerals.AutoMixins
{
    public readonly struct AddMixinObject
    {
        public string[] Modifiers { get; }
        public string Name { get; }
        public string Base { get; }
        public string Namespace { get; }
        public IEnumerable<ISymbol?> Mixins { get; }

        public AddMixinObject(GeneratorAttributeSyntaxContext context)
        {
            Modifiers = GetModifiersOf(context.TargetNode);
            Name = GetNameOf(context.TargetNode);
            Base = GetBaseClassOf(context.TargetNode);
            Namespace = GetNamespaceFrom(context.TargetNode);
            Mixins = GetMixinsOf(context);
        }

        public override bool Equals(object obj)
        {
            return obj is AddMixinObject addObj
            && addObj.Modifiers.SequenceEqual(Modifiers)
            && addObj.Name.Equals(Name)
            && addObj.Base.Equals(Base)
            && addObj.Namespace.Equals(Namespace)
            && addObj.Mixins.SequenceEqual(Mixins, SymbolEqualityComparer.IncludeNullability);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Modifiers, Name, Base, Namespace);
        }

        private static string[] GetModifiersOf(SyntaxNode node)
        {
            return ((MemberDeclarationSyntax)node).Modifiers.Select(x => x.ValueText).ToArray();
        }

        private static string GetNameOf(SyntaxNode node)
        {
            return ((BaseTypeDeclarationSyntax)node).Identifier.ValueText;
        }

        private static string GetBaseClassOf(SyntaxNode node)
        {
            return ((BaseTypeDeclarationSyntax)node).BaseList?.Types.ToString() ?? string.Empty;
        }

        private static string GetNamespaceFrom(SyntaxNode from)
        {
            return from.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString() ?? string.Empty;
        }

        private static IEnumerable<ISymbol?> GetMixinsOf(GeneratorAttributeSyntaxContext target)
        {
            return target.Attributes.SelectMany(x => x.ConstructorArguments.Select(y => y.Type));
        }
    }
}