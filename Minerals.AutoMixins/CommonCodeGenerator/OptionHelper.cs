namespace CommonCodeGenerator.SourceGenerator;

using System.ComponentModel;

using Microsoft.CodeAnalysis.Diagnostics;

internal static class OptionHelper
{
    public static T GetPropertyValue<T>(AnalyzerConfigOptions options, string key)
    {
        if (options.TryGetValue($"build_property.CommonCodeGenerator{key}", out var value))
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)value;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(typeof(string)))
            {
                return (T)converter.ConvertFrom(value)!;
            }
        }

        return default!;
    }
}
