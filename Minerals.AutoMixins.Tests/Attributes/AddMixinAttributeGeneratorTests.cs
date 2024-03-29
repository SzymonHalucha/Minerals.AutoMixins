namespace Minerals.AutoMixins.Tests.Attributes
{
    [TestClass]
    public class AddMixinAttributeGeneratorTests : VerifyBase
    {
        public AddMixinAttributeGeneratorTests()
        {
            VerifyExtensions.Initialize();
        }

        [TestMethod]
        public Task Attribute_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new AddMixinAttributeGenerator());
        }
    }
}