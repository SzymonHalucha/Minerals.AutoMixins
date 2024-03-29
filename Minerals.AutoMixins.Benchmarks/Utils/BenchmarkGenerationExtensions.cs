namespace Minerals.AutoMixins.Benchmarks.Utils
{
    public static class BenchmarkGenerationExtensions
    {
        public static void RunGeneration(this BenchmarkGeneration instance)
        {
            instance.Driver.RunGenerators(instance.Compilation);
        }

        public static BenchmarkGeneration CreateGeneration(string source)
        {
            return CreateGeneration(source, Array.Empty<IIncrementalGenerator>(), Array.Empty<IIncrementalGenerator>());
        }

        public static BenchmarkGeneration CreateGeneration
        (
            string source,
            IIncrementalGenerator target
        )
        {
            return CreateGeneration(source, new IIncrementalGenerator[] { target }, Array.Empty<IIncrementalGenerator>());
        }

        public static BenchmarkGeneration CreateGeneration
        (
            string source,
            IIncrementalGenerator target,
            IIncrementalGenerator[] additional
        )
        {
            return CreateGeneration(source, new IIncrementalGenerator[] { target }, additional);
        }

        public static BenchmarkGeneration CreateGeneration
        (
            string source,
            IIncrementalGenerator[] targets
        )
        {
            return CreateGeneration(source, targets, Array.Empty<IIncrementalGenerator>());
        }

        public static BenchmarkGeneration CreateGeneration
        (
            string source,
            IIncrementalGenerator[] targets,
            IIncrementalGenerator[] additional
        )
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            var compilation = CSharpCompilation.Create
            (
                "Minerals.Benchmarks",
                new SyntaxTree[] { tree },
                new MetadataReference[] { MetadataReference.CreateFromFile(tree.GetType().Assembly.Location) }
            );

            CSharpGeneratorDriver.Create(additional)
                .RunGeneratorsAndUpdateCompilation
                (
                    compilation,
                    out var newCompilation,
                    out _
                );

            return new BenchmarkGeneration(CSharpGeneratorDriver.Create(targets), newCompilation);
        }
    }
}