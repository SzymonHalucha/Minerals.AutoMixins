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
            var enumerator = lines.GetEnumerator();
            var moveNext = enumerator.MoveNext();
            var current = enumerator.Current;
            while (moveNext)
            {
                moveNext = enumerator.MoveNext();
                iterator(this, current, moveNext);
                current = enumerator.Current;
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
            var enumerator = lines.GetEnumerator();
            var moveNext = enumerator.MoveNext();
            var current = enumerator.Current;
            while (moveNext)
            {
                moveNext = enumerator.MoveNext();
                iterator(this, current, moveNext);
                current = enumerator.Current;
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
            OpenBlock();
            AppendLine(text);
            CloseBlock();
            return this;
        }

        public CodeBuilder WriteBlock(string text, bool newLine)
        {
            OpenBlock();
            AppendLine(text);
            CloseBlock();
            if (newLine)
            {
                _builder.AppendLine();
            }
            return this;
        }

        public CodeBuilder WriteBlock(string text, bool newLine, bool appendSemicolon)
        {
            OpenBlock();
            AppendLine(text);
            CloseBlock(appendSemicolon);
            if (newLine)
            {
                _builder.AppendLine();
            }
            return this;
        }

        public CodeBuilder WriteBlock(Action<CodeBuilder> writer)
        {
            OpenBlock();
            writer(this);
            CloseBlock();
            return this;
        }

        public CodeBuilder WriteBlock(Action<CodeBuilder> writer, bool newLine)
        {
            OpenBlock();
            writer(this);
            CloseBlock();
            if (newLine)
            {
                _builder.AppendLine();
            }
            return this;
        }

        public CodeBuilder WriteBlock(Action<CodeBuilder> writer, bool newLine, bool appendSemicolon)
        {
            OpenBlock();
            writer(this);
            CloseBlock(appendSemicolon);
            if (newLine)
            {
                _builder.AppendLine();
            }
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
        private void Append(string text)
        {
            AppendIndentation();
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
        private void AppendIndentation()
        {
            if (_builder.Length > 0 && _builder[_builder.Length - 1].Equals('\n'))
            {
                _builder.Append(' ', _indentationSize * _indentationLevel);
            }
        }
    }
}