using YamlDotNet.Core;

namespace DynIpUpdater
{
    public static class LoadConfiguration
    {
        /// <summary>
        /// Loads a YAML configuration file and creates a strongly typed configuration object.
        /// </summary>
        /// <param name="args">Command line args (if any).</param>
        /// <returns>A strongly typed configuration object.</returns>
        public static CloudflareConfiguration ParseConfigFile(string[] args)
        {
            string path = args.Length > 0 ? args[0] : "config.yaml";

            string rawConfig = TryLoadFile(path);

            return DeserializeConfig(rawConfig);
        }

        /// <summary>
        /// Tries to load a configuration file from the given path.
        /// </summary>
        /// <param name="path">The path to the file to load.</param>
        /// <returns>The contents of the file.</returns>
        /// <exception cref="FileNotFoundException">
        /// Thrown if the file does not exist or is not accessible.
        /// </exception>
        public static string TryLoadFile(string path)
        {
            string fullPath = Path.IsPathFullyQualified(path) ? path : Path.GetFullPath(path);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(
                    $"Couldn't find config file: {fullPath}, or it was not accessible."
                );
            }

            using StreamReader reader = new(fullPath);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Deserializes a raw YAML configuration string into a strongly typed
        /// CloudflareConfiguration object.
        /// </summary>
        /// <param name="rawConfig">The raw YAML configuration string.</param>
        /// <returns>A strongly typed CloudflareConfiguration object.</returns>
        public static CloudflareConfiguration DeserializeConfig(string rawConfig)
        {
            /*
             * TODO: write a custom deserializer/parser so that we can throw better error
             * descriptions of what is wrong with the config when we have invalid configs.
             */
            var deserializer = new DeserializerBuilder().Build();

            return deserializer.Deserialize<CloudflareConfiguration>(rawConfig);
        }

        /// <summary>
        /// Configures the logging level for the application.
        /// </summary>
        /// <param name="builder">The <see cref="HostApplicationBuilder"/> to configure.</param>
        /// <param name="verbosity">The verbosity level to set.</param>
        /// <returns>The <see cref="HostApplicationBuilder"/>.</returns>
        public static HostApplicationBuilder ConfigureLogging(
            this HostApplicationBuilder builder,
            int verbosity
        )
        {
            switch (verbosity)
            {
                case 0:
                    builder.Logging.SetMinimumLevel(LogLevel.None);
                    break;
                case 1:
                    builder.Logging.SetMinimumLevel(LogLevel.Error);
                    break;
                case 2:
                    builder.Logging.SetMinimumLevel(LogLevel.Warning);
                    break;
                case 3:
                    builder.Logging.SetMinimumLevel(LogLevel.Information);
                    builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
                    builder.Logging.AddFilter("System", LogLevel.Warning);
                    break;
                case 4:
                    builder.Logging.SetMinimumLevel(LogLevel.Information);
                    builder.Logging.AddFilter("Microsoft", LogLevel.Information);
                    builder.Logging.AddFilter("System", LogLevel.Information);
                    break;
                default:
                    Console.WriteLine(
                        $"Invalid verbosity level {verbosity}, defaulting to 3. Verbosity must be "
                            + "an integer value of 0, 1, 2, 3 or 4."
                    );
                    builder.Logging.SetMinimumLevel(LogLevel.Information);
                    builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
                    builder.Logging.AddFilter("System", LogLevel.Warning);
                    break;
            }

            return builder;
        }
    }
}
