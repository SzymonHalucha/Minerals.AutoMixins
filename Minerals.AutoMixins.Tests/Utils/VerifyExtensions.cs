namespace Minerals.AutoMixins.Tests.Utils
{
    public static class VerifyExtensions
    {
        private static bool _scrubCommentLines = true;
        private static bool _scrubEmptyLines = true;
        private static bool _isInitialized = false;

        public static void Initialize(bool removeCommentLines = true, bool removeEmptyLines = true)
        {
            var order = new DiffTool[]
            {
            DiffTool.VisualStudioCode,
            DiffTool.VisualStudio,
            DiffTool.Rider,
            DiffTool.Neovim,
            DiffTool.Vim
            };
            Initialize(removeCommentLines, removeEmptyLines, order);
        }

        public static void Initialize(bool removeCommentLines, bool removeEmptyLines, DiffTool[] order)
        {
            if (!_isInitialized)
            {
                DiffTools.UseOrder(order);
                VerifyBase.UseProjectRelativeDirectory("Snapshots");
                VerifierSettings.UseEncoding(Encoding.UTF8);
                VerifySourceGenerators.Initialize();
                _scrubCommentLines = removeCommentLines;
                _scrubEmptyLines = removeCommentLines;
                _isInitialized = true;
            }
        }

        public static Task VerifyIncrementalGenerators
        (
            this VerifyBase instance,
            IIncrementalGenerator target
        )
        {
            var targets = new IIncrementalGenerator[] { target };
            var additional = Array.Empty<IIncrementalGenerator>();
            return VerifyIncrementalGenerators(instance, targets, additional);
        }

        public static Task VerifyIncrementalGenerators
        (
            this VerifyBase instance,
            IIncrementalGenerator target,
            IIncrementalGenerator[] additional
        )
        {
            var targets = new IIncrementalGenerator[] { target };
            return VerifyIncrementalGenerators(instance, targets, additional);
        }

        public static Task VerifyIncrementalGenerators
        (
            this VerifyBase instance,
            IIncrementalGenerator[] targets,
            IIncrementalGenerator[] additional
        )
        {
            var compilation = CSharpCompilation.Create("Tests");
            CSharpGeneratorDriver.Create(additional)
                .RunGeneratorsAndUpdateCompilation
                (
                    compilation,
                    out var newCompilation,
                    out _
                );

            var driver = CSharpGeneratorDriver.Create(targets)
                .RunGenerators(newCompilation);

            return ApplyDynamicSettings(instance.Verify(driver.GetRunResult()));
        }

        public static Task VerifyIncrementalGenerators
        (
            this VerifyBase instance,
            string source,
            IIncrementalGenerator target
        )
        {
            var targets = new IIncrementalGenerator[] { target };
            var additional = Array.Empty<IIncrementalGenerator>();
            return VerifyIncrementalGenerators(instance, source, targets, additional);
        }

        public static Task VerifyIncrementalGenerators
        (
            this VerifyBase instance,
            string source,
            IIncrementalGenerator target,
            IIncrementalGenerator[] additional
        )
        {
            var targets = new IIncrementalGenerator[] { target };
            return VerifyIncrementalGenerators(instance, source, targets, additional);
        }

        public static Task VerifyIncrementalGenerators
        (
            this VerifyBase instance,
            string source,
            IIncrementalGenerator[] targets,
            IIncrementalGenerator[] additional
        )
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            var compilation = CSharpCompilation.Create
            (
                "Tests",
                new SyntaxTree[] { tree },
                new MetadataReference[] { MetadataReference.CreateFromFile(tree.GetType().Assembly.Location) }
            );

            CSharpGeneratorDriver.Create(additional)
                .RunGeneratorsAndUpdateCompilation
                (
                    compilation,
                    out var newCompilation,
                    out _
                );

            var driver = CSharpGeneratorDriver.Create(targets)
                .RunGenerators(newCompilation);

            return ApplyDynamicSettings(instance.Verify(driver.GetRunResult()));
        }

        private static SettingsTask ApplyDynamicSettings(SettingsTask task)
        {
            if (_scrubCommentLines)
            {
                task.ScrubLines("cs", x =>
                {
                    return x.StartsWith("//");
                });
            }
            if (_scrubEmptyLines)
            {
                task.ScrubEmptyLines();
            }
            return task;
        }
    }
}