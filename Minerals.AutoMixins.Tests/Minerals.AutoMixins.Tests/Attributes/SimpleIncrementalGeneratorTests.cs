using Minerals.AutoMixins.Generators;

namespace Minerals.AutoMixins.Tests.Attributes;
[TestClass]
public class SimpleIncrementalGeneratorTests : VerifyBase
{
    public SimpleIncrementalGeneratorTests()
    {
        var references = VerifyExtensions.GetAppReferences
        (
            typeof(object),
            typeof(SimpleIncrementalGenerator),
            typeof(Assembly)
        );
        VerifyExtensions.Initialize(references);
    }

    [TestMethod]
    public Task Generator_ShouldGenerateHelloWorldClass()
    {
        return this.VerifyIncrementalGenerators(new SimpleIncrementalGenerator());
    }
}