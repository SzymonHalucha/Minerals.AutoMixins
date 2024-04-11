namespace Minerals.AutoMixins.Benchmarks.Utils
{
    public class BenchmarkGeneration
    {
        public GeneratorDriver StartingDriver { get; set; }
        public GeneratorDriver CurrentDriver { get; set; }
        public Compilation StartingCompilation { get; set; }
        public Compilation CurrentCompilation { get; set; }

        public BenchmarkGeneration(GeneratorDriver driver, Compilation compilation)
        {
            StartingDriver = driver;
            CurrentDriver = driver;
            StartingCompilation = compilation;
            CurrentCompilation = compilation;
        }
    }
}