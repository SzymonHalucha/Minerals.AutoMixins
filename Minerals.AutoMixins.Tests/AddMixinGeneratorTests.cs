namespace Minerals.AutoMixins.Tests
{
    [TestClass]
    public class AddMixinGeneratorTests : VerifyBase
    {
        public AddMixinGeneratorTests()
        {
            VerifyExtensions.Initialize();
        }

        [TestMethod]
        public Task SingleMixinWithoutNamespace_ShouldGenerate()
        {
            const string source = """
            using System;
            using Minerals.AutoMixins;

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

            [AddMixin(typeof(TestMixin))]
            public partial class TestClass { }
            """;
            IIncrementalGenerator[] additional =
            [
                new GenerateMixinAttributeGenerator(),
                new AddMixinAttributeGenerator()
            ];
            return this.VerifyIncrementalGenerators(source, new AddMixinGenerator(), additional);
        }

        [TestMethod]
        public Task SingleMixinWithNamespace_ShouldGenerate()
        {
            const string source = """
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
            IIncrementalGenerator[] additional =
            [
                new GenerateMixinAttributeGenerator(),
                new AddMixinAttributeGenerator()
            ];
            return this.VerifyIncrementalGenerators(source, new AddMixinGenerator(), additional);
        }

        [TestMethod]
        public Task GenerateMultiMixin_ShouldGenerate()
        {
            const string source = """
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
                [AddMixin(typeof(TestMixin1), typeof(TestMixin2))]
                public partial class TestClass { }
            }
            """;
            IIncrementalGenerator[] additional =
            [
                new GenerateMixinAttributeGenerator(),
                new AddMixinAttributeGenerator()
            ];
            return this.VerifyIncrementalGenerators(source, new AddMixinGenerator(), additional);
        }

        [TestMethod]
        public Task GenerateAndAddMultiMixin_ShouldGenerate()
        {
            const string source = """
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
                [AddMixin(typeof(TestMixin1), typeof(TestMixin2))]
                public partial class TestClass1 { }

                [AddMixin(typeof(TestMixin1), typeof(TestMixin2))]
                public partial class TestClass2 { }
            }
            """;
            IIncrementalGenerator[] additional =
            [
                new GenerateMixinAttributeGenerator(),
                new AddMixinAttributeGenerator()
            ];
            return this.VerifyIncrementalGenerators(source, new AddMixinGenerator(), additional);
        }
    }
}