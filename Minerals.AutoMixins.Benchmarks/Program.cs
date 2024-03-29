namespace Minerals.AutoMixins.Benchmarks
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<AddMixinGeneratorBenchmark>
            (
                DefaultConfig.Instance
                    .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest))
                    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60))
                    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core80))
                    .AddValidator(JitOptimizationsValidator.FailOnError)
                    .AddDiagnoser(MemoryDiagnoser.Default)
            );
        }
    }
}