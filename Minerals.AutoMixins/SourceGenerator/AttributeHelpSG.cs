
namespace SourceGenerator.SourceGenerator;
using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.Misc;
using CommonBasicLibraries.CollectionClasses;
using global::SourceGenerator.SGHelp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;

[Generator]
public class AttributeHelpSG : IIncrementalGenerator
{
    private BasicList<string> GetList<T>(BasicList<T> list, Func<T, string> field) => list.Select(field).ToBasicList();
    private BasicList<string> GetAttributeProperties(BasicList<IPropertySymbol> properties, string className)
    {

        int upto = 0;
        int index;
        BasicList<string> output = new();
        foreach (var property in properties)
        {
            index = -1;
            if (property.IsRequiredAttributeUsed())
            {
                index = upto;
                upto++;
            }
            StringBuilder builder = new();
            output.Add($@"    public static AttributeProperty Get{property.Name}Info => new(""{property.Name}"", {index});");
            builder.AppendLine($@"    public static string {property.Name} => ""{property.Name}"";");
            output.Add(builder.ToString());
        }
        string shorts = className.Replace("Attribute", "");
        output.Add($@"    public static string {className} => ""{shorts}"";");
        return output;
    }
    private ClassInfo GetClass(string name, string content)
    {
        string propertyCode;
        propertyCode = $@"    private const string _code{name} = @""{content}"";";
        string compilation = $"        compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(_code{name}, Encoding.UTF8), options));";
        string source = $@"        context.AddSource(""{name}"", _code{name});";
        return new(propertyCode, compilation, source);
    }
    private string GetConstructors(ClassDeclarationSyntax clazz, Compilation compilation)
    {
        var ss = compilation.GetClassSymbol(clazz);
        var properties = ss.GetRequiredProperties();
        if (properties.Count == 0)
        {
            return "";
        }
        StringBuilder builder = new();
        StrCat cats = new();
        string name = clazz.Identifier.ValueText;
        cats.AddToString($"    public {name}(");
        properties.ForEach(p =>
        {
            string firsts = p.Name;
            string vType = p.Type.ToDisplayString();
            string parameter = firsts.ChangeCasingForVariable(EnumVariableCategory.ParameterCamelCase);
            cats.AddToString($"{vType} {parameter}", ", ");

        });
        cats.AddToString(")");
        cats.AddToString(@"
");
        cats.AddToString("    {");
        properties.ForEach(p =>
        {
            string firsts = p.Name;
            string parameter = firsts.ChangeCasingForVariable(EnumVariableCategory.ParameterCamelCase);
            cats.AddToString($"        {firsts} = {parameter};", @"
");
        });
        cats.AddToString(@"
");
        cats.AddToString("    }");
        string results = cats.GetInfo();
        results = results.Replace("(, ", "("); //unfortunately had to do this part to remove the beginning , part.
        return results;
    }
    private bool IsSyntaxTarget(SyntaxNode syntax) => syntax is ClassDeclarationSyntax ctx && ctx.Implements("Attribute");
    private ClassDeclarationSyntax? GetTarget(GeneratorSyntaxContext context) => context.GetClassNode();
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ClassDeclarationSyntax> declares = context.SyntaxProvider.CreateSyntaxProvider(
            (s, _) => IsSyntaxTarget(s),
            (t, _) => GetTarget(t))
            .Where(m => m != null)!;
        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilation
            = context.CompilationProvider.Combine(declares.Collect());
        context.RegisterSourceOutput(compilation, (spc, source) =>
        {
            Execute(source.Item1, source.Item2, spc);
        });
    }
    private void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> list, SourceProductionContext context)
    {
        BasicList<ClassInfo> others = new();
        string source;
        foreach (var ourClass in list)
        {
            string className = ourClass.Identifier.ValueText;
            string content = ourClass.SyntaxTree.ToString();

            string extras = GetConstructors(ourClass, compilation);
            if (string.IsNullOrWhiteSpace(extras) == false)
            {
                content = ourClass.AppendCodeText(content, extras);
            }
            content = content.GetCSharpString();
            content = content.RemoveAttribute("Required");
            INamedTypeSymbol symbol = compilation.GetClassSymbol(ourClass);
            var properties = symbol.GetProperties();
            string shortName = className.Replace("Attribute", "");
            others.Add(GetClass(shortName, content));
            BasicList<string> helps = GetAttributeProperties(properties, className);
            source = @$"namespace {compilation.AssemblyName}.AttributeHelpers;
internal static class {shortName}
{{
{string.Join(@"
", helps)}
}}
";
            context.AddSource($"{shortName}.g", source);
        }
        //i am guessing that only the standard ones needs the extension to getcompilationwithattributes.
        //the incremental one hopefully does not require it.
        //only keep it to support incremental source generators.
        source = @$"global using aa = {compilation.AssemblyName}.AttributeHelpers;
namespace {compilation.AssemblyName};
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.Linq;
internal static class Extensions
{{
{string.Join(@"
", GetList(others, xx => xx.Code))}
    internal static Compilation GetCompilationWithAttributes(this GeneratorExecutionContext context)
    {{
{string.Join(@"
", GetList(others, xx => xx.AddSource))}
        var options = context.Compilation.SyntaxTrees.First().Options as CSharpParseOptions;
        Compilation compilation = context.Compilation;
{string.Join(@"
", GetList(others, xx => xx.Compilation))}
        return compilation;
    }}
    internal static void AddAttributesToSourceOnly(this IAddSource context)
    {{
{string.Join(@"
", GetList(others, xx => xx.AddSource))}
    }}
}}";
        context.AddSource("Extensions.g", source);
    }
}