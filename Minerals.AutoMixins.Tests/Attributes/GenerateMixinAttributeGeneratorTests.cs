namespace Minerals.AutoMixins.Tests.Attributes
{
    [TestClass]
    public class GenerateMixinAttributeGeneratorTests : VerifyBase
    {
        public GenerateMixinAttributeGeneratorTests()
        {
            VerifyExtensions.Initialize();
        }

        [TestMethod]
        public Task Attribute_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new GenerateMixinAttributeGenerator());
        }
    }
}