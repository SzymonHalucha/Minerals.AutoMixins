using BenchmarkDotNet.Attributes;
using Minerals.AutoMixins.Benchmarks.Utils;
using Minerals.AutoMixins.Attributes;

namespace Minerals.AutoMixins.Benchmarks
{
    public class AddMixinGeneratorBenchmarks
    {
        public BenchmarkGeneration Baseline { get; set; } = default!;
        public BenchmarkGeneration AttributesGeneration { get; set; } = default!;
        public BenchmarkGeneration MixinsGeneration { get; set; } = default!;
        public BenchmarkGeneration BaselineDouble { get; set; } = default!;
        public BenchmarkGeneration MixinsGenerationDouble { get; set; } = default!;

        private const string _withoutAttributes = """
        using System;

        namespace Minerals.Test1
        {
            public class TestMixin1
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

            public class TestMixin2
            {
                [Obsolete] public int Property1_1 { get; set; } = 1;
                public int Field1_1 = 1;
                private int _filed2_1 = 2;

                public int Method1_1(int arg0, int arg1)
                {
                    return arg0 + arg1;
                }

                protected int Method2_1(int arg0, int arg1)
                {
                    return arg0 - arg1;
                }
            }
        }

        namespace Minerals.Test2
        {
            public partial class TestClass1 { }

            public partial class TestClass2 { }
        }
        """;

        private const string _withAttributes = """
        using System;
        using Minerals.AutoMixins;

        namespace Minerals.Test1
        {
            [GenerateMixin]
            public class TestMixin1
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

            [GenerateMixin]
            public class TestMixin2
            {
                [Obsolete] public int Property1_1 { get; set; } = 1;
                public int Field1_1 = 1;
                private int _filed2_1 = 2;

                public int Method1_1(int arg0, int arg1)
                {
                    return arg0 + arg1;
                }

                protected int Method2_1(int arg0, int arg1)
                {
                    return arg0 - arg1;
                }
            }
        }

        namespace Minerals.Test2
        {
            [AddMixin(typeof(TestMixin2))]
            public partial class TestClass1 { }

            [AddMixin(typeof(TestMixin1))]
            public partial class TestClass2 { }
        }
        """;

        [GlobalSetup]
        public void Initialize()
        {
            var references = BenchmarkGenerationExtensions.GetAppReferences
            (
                typeof(object),
                typeof(AddMixinAttributeGenerator),
                typeof(AddMixinGenerator),
                typeof(AddMixinObject)
            );
            Baseline = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withoutAttributes,
                references
            );
            AttributesGeneration = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withoutAttributes,
                [new AddMixinAttributeGenerator(), new GenerateMixinAttributeGenerator()],
                references
            );
            MixinsGeneration = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withAttributes,
                new AddMixinGenerator(),
                [new AddMixinAttributeGenerator(), new GenerateMixinAttributeGenerator()],
                references
            );
            BaselineDouble = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withoutAttributes,
                references
            );
            MixinsGenerationDouble = BenchmarkGenerationExtensions.CreateGeneration
            (
                _withAttributes,
                new AddMixinGenerator(),
                [new AddMixinAttributeGenerator(), new GenerateMixinAttributeGenerator()],
                references
            );
            BaselineDouble.RunAndSaveGeneration();
            BaselineDouble.AddSourceCode("// Test Comment");
            MixinsGenerationDouble.RunAndSaveGeneration();
            MixinsGenerationDouble.AddSourceCode("// Test Comment");
        }

        [Benchmark(Baseline = true)]
        public void SingleGeneration_Baseline()
        {
            Baseline.RunGeneration();
        }

        [Benchmark]
        public void SingleGeneration_OnlyAttributes()
        {
            AttributesGeneration.RunGeneration();
        }

        [Benchmark]
        public void SingleGeneration_FullMixin()
        {
            MixinsGeneration.RunGeneration();
        }

        [Benchmark]
        public void DoubleGeneration_Baseline()
        {
            BaselineDouble.RunGeneration();
        }

        [Benchmark]
        public void DoubleGeneration_FullMixin()
        {
            MixinsGenerationDouble.RunGeneration();
        }
    }
}