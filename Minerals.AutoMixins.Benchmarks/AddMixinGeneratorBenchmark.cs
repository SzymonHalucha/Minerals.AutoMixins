namespace Minerals.AutoMixins.Benchmarks
{
    public class AddMixinGeneratorBenchmark
    {
        public BenchmarkGeneration Baseline { get; set; }
        public BenchmarkGeneration WithAttributes { get; set; }
        public BenchmarkGeneration WithMixin { get; set; }

        private const string _withoutAttributes = """
        using System;

        namespace Minerals.Test1
        {
            public class TestMixin
            {
                [Obsolete] public int Property1 { get; set; } = 1;
                public int Field1 = 1;
                private int _filed2 = 2;

                public int Method1(int arg0, int arg1)
                {
                    return arg0 + arg1;
                }

                protected int Method2(int arg0, int arg1)
                {
                    return arg0 - arg1;
                }
            }
        }

        namespace Minerals.Test2
        {
            public partial class TestClass { }
        }
        """;

        private const string _withAttributes = """
        using System;
        using Minerals.AutoMixins;

        namespace Minerals.Test1
        {
            [GenerateMixin]
            public class TestMixin
            {
                [Obsolete] public int Property1 { get; set; } = 1;
                public int Field1 = 1;
                private int _filed2 = 2;

                public int Method1(int arg0, int arg1)
                {
                    return arg0 + arg1;
                }

                protected int Method2(int arg0, int arg1)
                {
                    return arg0 - arg1;
                }
            }
        }

        namespace Minerals.Test2
        {
            [AddMixin(typeof(TestMixin))]
            public partial class TestClass { }
        }
        """;

        [GlobalSetup]
        public void Initialize()
        {
            Baseline = BenchmarkGenerationExtensions.CreateGeneration(_withoutAttributes);
            WithAttributes = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withoutAttributes,
                [new AddMixinAttributeGenerator(), new GenerateMixinAttributeGenerator()]
            );
            WithMixin = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withAttributes,
                new AddMixinGenerator(),
                [new AddMixinAttributeGenerator(), new GenerateMixinAttributeGenerator()]
            );
        }

        [Benchmark(Baseline = true)]
        public void Generation_Baseline()
        {
            Baseline.RunGeneration();
        }

        [Benchmark]
        public void Generation_Attributes()
        {
            WithAttributes.RunGeneration();
        }

        [Benchmark]
        public void Generation_MixinGenerator()
        {
            WithMixin.RunGeneration();
        }
    }
}