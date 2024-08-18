namespace DynIpUpdater
{
    public static class ConfigurationParsing
    {
        /// <summary>
        /// Loads a YAML configuration file and adds a singleton of the configuration to the
        /// service collection.
        /// </summary>
        /// <param name="builder">The program's hostbuilder.</param>
        /// <param name="path">The relative path to the config file.</param>
        /// <returns>The program's hostbuilder.</returns>
        public static HostApplicationBuilder ParseConfigFile(
            this HostApplicationBuilder builder,
            string path
        )
        {
            CloudflareConfiguration config;

            using (var reader = new StreamReader(path))
            {
                string stringResults = reader.ReadToEnd();
                var deserializer = new DeserializerBuilder().Build();

                config = deserializer.Deserialize<CloudflareConfiguration>(stringResults);
            }

            builder.Services.AddSingleton(config);

            return builder;
        }
    }
}
