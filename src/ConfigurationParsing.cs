namespace DynIpUpdater
{
    public static class ConfigurationParsing
    {
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
