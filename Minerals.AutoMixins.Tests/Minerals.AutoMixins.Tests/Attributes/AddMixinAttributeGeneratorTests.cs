namespace Minerals.AutoMixins.Tests.Attributes
{
    [TestClass]
    public class AddMixinAttributeGeneratorTests : VerifyBase
    {
        public AddMixinAttributeGeneratorTests()
        {
            var references = VerifyExtensions.GetAppReferences
            (
                typeof(object),
                typeof(AddMixinAttributeGenerator),
                typeof(Assembly)
            );
            VerifyExtensions.Initialize(references);
        }

        [TestMethod]
        public Task Attribute_ShouldGenerate()
        {
            return this.VerifyIncrementalGenerators(new AddMixinAttributeGenerator());
        }
    }
}