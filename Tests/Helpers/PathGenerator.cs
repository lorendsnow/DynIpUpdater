namespace Tests.Helpers
{
    public static partial class PathGenerator
    {
        public static string Generate(string fileName) =>
            ConfigPath().Replace(Path.GetFullPath("./"), fileName);

        [GeneratedRegex("bin\\\\Debug\\\\net8.0.")]
        private static partial Regex ConfigPath();
    }
}
