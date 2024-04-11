namespace Minerals.AutoMixins.Tests.Attributes
{
    [TestClass]
    public class GenerateMixinAttributeGeneratorTests : VerifyBase
    {
        public GenerateMixinAttributeGeneratorTests()
        {
            var references = VerifyExtensions.GetAppReferences
            (
                typeof(object),
                typeof(GenerateMixinAttributeGenerator),
                typeof(Assembly)
            );
            VerifyExtensions.Initialize(references);
        }

        [TestMethod]
        public Task Attribute_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new GenerateMixinAttributeGenerator());
        }
    }
}