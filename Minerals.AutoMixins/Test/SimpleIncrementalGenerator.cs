namespace Minerals.AutoMixins.Generators;

[Generator]
public class SimpleIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider, (sourceProductionContext, compilation) =>
        {
            var codeBuilder = new StringBuilder();
            codeBuilder.AppendLine("using System;");
            codeBuilder.AppendLine("namespace Generated;");
            codeBuilder.AppendLine("public static class HelloWorld");
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine("    public static string SayHello() => \"Hello, World!\";");
            codeBuilder.AppendLine("}");

            sourceProductionContext.AddSource("HelloWorldGenerator", codeBuilder.ToString());
        });
    }
}