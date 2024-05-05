
namespace SourceGenerator.Helper.CopyCode;

public static partial class MemoryExtensions
{

    public static SpanSplitEnumerator<char> Split(this ReadOnlySpan<char> span, char separator)
        => new SpanSplitEnumerator<char>(span, separator);

}

public ref struct SpanSplitEnumerator<T> where T : IEquatable<T>
{
    private readonly ReadOnlySpan<T> toSplit;
    private readonly T separator;
    private int offset;
    private int index;

    public readonly SpanSplitEnumerator<T> GetEnumerator() => this;

    internal SpanSplitEnumerator(ReadOnlySpan<T> span, T separator)
    {
        toSplit = span;
        this.separator = separator;
        index = 0;
        offset = 0;
    }

    public readonly ReadOnlySpan<T> Current => toSplit.Slice(offset, index - 1);

    public bool MoveNext()
    {
        if (toSplit.Length - offset < index) { return false; }
        var slice = toSplit.Slice(offset += index);

        var nextIndex = slice.IndexOf(separator);
        index = (nextIndex != -1 ? nextIndex : slice.Length) + 1;
        return true;
    }
}

