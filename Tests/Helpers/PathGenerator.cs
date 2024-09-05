namespace Tests.Helpers
{
    public static partial class PathGenerator
    {
        /// <summary>
        /// When running tests, the working directory is /..../Tests/bin/Debug/net8.0. Because we
        /// don't include bin files in the repo, we need to dynamically generate the path back to
        /// the Test folder root to access the test config files.
        /// </summary>
        /// <param name="fileName">
        /// The name of a config file located in the Tests folder that we want to access.
        /// </param>
        /// <returns>A fully qualified path to the requested config file.</returns>
        public static string Generate(string fileName) =>
            ConfigPath().Replace(Path.GetFullPath("./"), fileName);

        [GeneratedRegex("bin\\\\Debug\\\\net8.0.")]
        private static partial Regex ConfigPath();
    }
}
