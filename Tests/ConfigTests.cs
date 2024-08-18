namespace Tests
{
    public partial class ConfigTests
    {
        private readonly Regex _configPathRegex = ConfigPathRegex();

        [Fact]
        public void ParseConfigFile_WithValidConfigFile_ParsesConfig()
        {
            string path = _configPathRegex.Replace(Path.GetFullPath("./"), "ConfigTestFile.yaml");

            var app = Host.CreateApplicationBuilder().ParseConfigFile(path).Build();

            var config = app.Services.GetRequiredService<CloudflareConfiguration>();

            Assert.Single(config.Zones);
            Assert.Equal("TestBearerToken", config.Zones[0].BearerToken);
            Assert.Equal(2, config.Zones[0].DnsRecords.Count);
            Assert.Equal("example.com", config.Zones[0].DnsRecords[0].Name);
        }

        [GeneratedRegex("bin\\\\Debug\\\\net8.0.")]
        private static partial Regex ConfigPathRegex();
    }
}
