using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Validators;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Jobs;

namespace Minerals.AutoMixins.Benchmarks
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<AddMixinGeneratorBenchmarks>
            (
                DefaultConfig.Instance
                    .WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest))
                    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core80))
                    .AddJob(Job.Default.WithRuntime(CoreRuntime.Core90))
                    .AddValidator(JitOptimizationsValidator.FailOnError)
                    .AddDiagnoser(MemoryDiagnoser.Default)
            );
        }
    }
}