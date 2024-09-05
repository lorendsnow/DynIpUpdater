using Xunit.Abstractions;
using YamlDotNet.Core;

namespace Tests
{
    public class LoadConfigurationTests(ITestOutputHelper output)
    {
        private readonly ITestOutputHelper _output = output;

        [Fact]
        public void TryLoadFile_WithValidFullPath_ReturnsFileContents()
        {
            string path = PathGenerator.Generate("ConfigWithMultipleA.yaml");

            string fileContents = LoadConfiguration.TryLoadFile(path);

            Assert.Equal("Interval: 1", fileContents[..11]);
        }

        [Fact]
        public void TryLoadFile_WithValidRelativePath_ReturnsFileContents()
        {
            // See PathGenerator.cs for how this works and why we're doing this.
            File.Copy(
                PathGenerator.Generate("ConfigWithMultipleA.yaml"),
                "ConfigWithMultipleA.yaml"
            );

            string fileContents = LoadConfiguration.TryLoadFile("ConfigWithMultipleA.yaml");

            Assert.Equal("Interval: 1", fileContents[..11]);

            // Clean up after ourselves
            File.Delete("ConfigWithMultipleA.yaml");
        }

        [Fact]
        public void TryLoadFile_WithInvalidPath_ThrowsFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(
                () => LoadConfiguration.TryLoadFile("InvalidPath.yaml")
            );
        }

        [Fact]
        public void DeserializeConfig_WithValidConfig_ReturnsCloudflareConfiguration()
        {
            string path = PathGenerator.Generate("ConfigWithMultipleA.yaml");

            string fileContents = LoadConfiguration.TryLoadFile(path);

            CloudflareConfiguration config = LoadConfiguration.DeserializeConfig(fileContents);

            Assert.Equal(1, config.Interval);
        }

        [Fact]
        public void DeserializeConfig_WithInvalidConfig_ThrowsException()
        {
            string path = PathGenerator.Generate("BadConfig.yaml");

            string fileContents = LoadConfiguration.TryLoadFile(path);

            Assert.Throws<YamlException>(() => LoadConfiguration.DeserializeConfig(fileContents));
        }

        [Fact]
        public void DeserializeConfig_WithMissingFields_ReturnsDefaultValues()
        {
            string path = PathGenerator.Generate("MissingFields.yaml");

            string fileContents = LoadConfiguration.TryLoadFile(path);

            CloudflareConfiguration config = LoadConfiguration.DeserializeConfig(fileContents);

            Assert.Equal(5, config.Interval);
            Assert.Equal(3, config.Verbosity);
            Assert.Equal(3, config.Zones[0].DnsRecords.Count);
            Assert.Single(config.Zones[1].DnsRecords);
            foreach (ZoneConfiguration zone in config.Zones)
            {
                foreach (DnsRecord record in config.Zones[0].DnsRecords)
                {
                    Assert.Null(record.Address);
                    Assert.Null(record.Comment);
                    Assert.Null(record.Id);
                    Assert.Empty(record.Tags);
                    Assert.Equal(1, record.TTL);
                }
            }
        }
    }
}
