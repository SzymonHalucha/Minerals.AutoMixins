namespace Minerals.AutoMixins.Utils
{
    public class CodeBuilder
    {
        private readonly StringBuilder _builder;
        private readonly int _indentationSize;
        private int _indentationLevel;

        public CodeBuilder(StringBuilder builder, int indentationSize = 4, int indentationLevel = 0)
        {
            _indentationLevel = indentationLevel;
            _indentationSize = indentationSize;
            _builder = builder;
        }

        public CodeBuilder(int builderStartCapacity = 1024, int indentationSize = 4, int indentationLevel = 0)
        {
            _indentationLevel = indentationLevel;
            _indentationSize = indentationSize;
            _builder = new StringBuilder(builderStartCapacity);
        }

        public CodeBuilder Write(string text)
        {
            Append(text);
            return this;
        }

        public CodeBuilder WriteLine(string text)
        {
            AppendLine(text);
            return this;
        }

        public CodeBuilder WriteIteration(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                AppendLine(line);
            }
            return this;
        }

        public CodeBuilder WriteIteration(IEnumerable<string> lines, Action<CodeBuilder, string, bool> iterator)
        {
            if (lines.Any() is false)
            {
                return this;
            }
            var enumerator = lines.GetEnumerator();
            var moveNext = enumerator.MoveNext();
            var current = enumerator.Current;
            while (moveNext)
            {
                moveNext = enumerator.MoveNext();
                iterator(this, current, moveNext);
                if (moveNext)
                {
                    current = enumerator.Current;
                }
            }
            return this;
        }

        public CodeBuilder WriteIteration(IReadOnlyCollection<string> lines)
        {
            foreach (var line in lines)
            {
                AppendLine(line);
            }
            return this;
        }

        public CodeBuilder WriteIteration(IReadOnlyCollection<string> lines, Action<CodeBuilder, string, bool> iterator)
        {
            if (lines.Any() is false)
            {
                return this;
            }
            var enumerator = lines.GetEnumerator();
            var moveNext = enumerator.MoveNext();
            var current = enumerator.Current;
            while (moveNext)
            {
                moveNext = enumerator.MoveNext();
                iterator(this, current, moveNext);
                if (moveNext)
                {
                    current = enumerator.Current;
                }
            }
            return this;
        }

        public CodeBuilder OpenBlock()
        {
            AppendLine("{");
            _indentationLevel++;
            return this;
        }

        public CodeBuilder CloseBlock()
        {
            _indentationLevel--;
            AppendLine("}");
            return this;
        }

        public CodeBuilder CloseBlock(bool appendSemicolon)
        {
            _indentationLevel--;
            AppendLine("}");
            if (appendSemicolon)
            {
                Append(";");
            }
            return this;
        }

        public CodeBuilder CloseAllBlocks()
        {
            for (int i = 0; i <= _indentationLevel; i++)
            {
                CloseBlock();
            }
            return this;
        }

        public CodeBuilder WriteBlock(string text)
        {
            foreach (var line in text.Split('\n'))
            {
                Append(line, true);
            }
            return this;
        }

        public CodeBuilder WriteBlock(Action<CodeBuilder> writer)
        {
            writer(this);
            return this;
        }

        public CodeBuilder? If(bool condition)
        {
            if (condition)
            {
                return this;
            }
            return null;
        }

        public CodeBuilder NewLine()
        {
            _builder.AppendLine();
            return this;
        }

        public void Clear()
        {
            _builder.Clear();
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Append(string text, bool forceIndentation = false)
        {
            AppendIndentation(forceIndentation);
            _builder.Append(text);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AppendLine(string text)
        {
            _builder.AppendLine();
            AppendIndentation();
            _builder.Append(text);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AppendIndentation(bool force = false)
        {
            if (force || (_builder.Length > 0 && _builder[_builder.Length - 1].Equals('\n')))
            {
                _builder.Append(' ', _indentationSize * _indentationLevel);
            }
        }
    }
}