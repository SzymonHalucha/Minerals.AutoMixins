namespace Minerals.AutoMixins
{
    public readonly struct AddMixinObject : IEquatable<AddMixinObject>
    {
        public string[] Modifiers { get; }
        public string Name { get; }
        public string Base { get; }
        public string Namespace { get; }
        public ISymbol?[] Mixins { get; }

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
            && addObj.Mixins.SequenceEqual(Mixins, SymbolEqualityComparer.Default);
        }

        public bool Equals(AddMixinObject other)
        {
            return other.Modifiers.SequenceEqual(Modifiers)
            && other.Name.Equals(Name)
            && other.Base.Equals(Base)
            && other.Namespace.Equals(Namespace)
            && other.Mixins.SequenceEqual(Mixins, SymbolEqualityComparer.Default);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Modifiers, Name, Base, Namespace, Mixins);
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
            var nameSyntax = from.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name;
            nameSyntax ??= ((FileScopedNamespaceDeclarationSyntax?)from.FirstAncestorOrSelf<CompilationUnitSyntax>()?
                .ChildNodes().FirstOrDefault(x => x is FileScopedNamespaceDeclarationSyntax))?.Name;
            return nameSyntax?.ToString() ?? string.Empty;
        }

        private static ISymbol?[] GetMixinsOf(GeneratorAttributeSyntaxContext target)
        {
            return target.Attributes.SelectMany(x =>
            {
                return x.ConstructorArguments.SelectMany(y =>
                {
                    return y.Values.Select(z =>
                    {
                        return z.Value as ISymbol;
                    });
                });
            }).ToArray();
        }
    }
}