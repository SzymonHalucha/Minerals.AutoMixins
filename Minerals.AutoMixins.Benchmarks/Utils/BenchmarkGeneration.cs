namespace Minerals.AutoMixins.Benchmarks.Utils
{
    public readonly struct BenchmarkGeneration
    {
        public readonly GeneratorDriver Driver { get; }
        public readonly Compilation Compilation { get; }

        public BenchmarkGeneration(GeneratorDriver driver, Compilation compilation)
        {
            Driver = driver;
            Compilation = compilation;
        }
    }
}