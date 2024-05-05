using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace SourceGeneratorQuery;
public class SourceGeneratorQuery
{
    private readonly GeneratorExecutionContext context;
    private readonly string entryPath;

    public SourceGeneratorQuery(GeneratorExecutionContext context)
    {
        this.context = context;
        var objPath = context.Compilation.Assembly.Locations.Last().SourceTree.FilePath;
        entryPath = objPath.Substring(0, objPath.IndexOf("\\obj\\"));
    }

    public IEnumerable<SourceFile> NewQuery()
    {
        return context.Compilation.SyntaxTrees
            .Select(x => new SourceFile(x, entryPath));
    }
}

public static class SourceGeneratorQueryExtensions
{
    public static IEnumerable<SourceFile> NewQuery(this GeneratorExecutionContext context) =>
        new SourceGeneratorQuery(context).NewQuery();
}

[Generator]
public class IncrementalSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // 获取所有语法树
        var syntaxTrees = context.SyntaxProvider.CreateSyntaxProvider(
        predicate: static (node, _) => node is CompilationUnitSyntax,
        transform: static (ctx, _) => ctx.Node.SyntaxTree)
        .Collect();

        // 组合编译信息和语法树
        var compilationAndTrees = context.CompilationProvider.Combine(syntaxTrees);

        // 注册源代码生成操作
        context.RegisterSourceOutput(compilationAndTrees, (spc, source) =>
        {
            var compilation = source.Left;
            var trees = source.Right;
            var entryPath = GetEntryPath(compilation);
            var sourceFiles = trees.Select(tree => new SourceFile(tree, entryPath));

            foreach (var file in sourceFiles)
            {
                var sourceText = GenerateSourceText(file);
                spc.AddSource($"{file.FileName}.g.cs", sourceText);
            }
        });
    }

    private string GetEntryPath(Compilation compilation)
    {
        var objPath = compilation.Assembly.Locations.Last().SourceTree.FilePath;
        return objPath.Substring(0, objPath.IndexOf("\\obj\\"));
    }

    private SourceText GenerateSourceText(SourceFile file)
    {
        var builder = new StringBuilder();
        // 这里添加生成源代码文件的逻辑
        // ...
        return SourceText.From(builder.ToString(), Encoding.UTF8);
    }
}
