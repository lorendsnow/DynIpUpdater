namespace Tests
{
    public partial class UpdateServiceTests
    {
        [Fact]
        public async Task GetExistingRecords_ExistingAandCnameRecords_ReturnsRecords()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateManyFakeARecords(),
                GenerateConfig("ConfigWithMultipleA.yaml")
            );

            var records = await service.GetExistingRecords(
                service._config.Zones[0],
                CancellationToken.None
            );

            Assert.Equal(2, records.Count);
        }

        [Fact]
        public async Task GetExistingRecords_NoExistingRecords_ReturnsEmpty()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateEmpty(),
                GenerateConfig("ConfigWithMultipleA.yaml")
            );

            var records = await service.GetExistingRecords(
                service._config.Zones[0],
                CancellationToken.None
            );

            Assert.Empty(records);
        }

        [Fact]
        public async Task GetExistingRecords_RequestThrowsError_ReturnsEmpty()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateEmptyWithError(),
                GenerateConfig("ConfigWithMultipleA.yaml")
            );

            var records = await service.GetExistingRecords(
                service._config.Zones[0],
                CancellationToken.None
            );

            Assert.Empty(records);
        }

        public static UpdateService GenerateServiceInstance(
            ListRecordsResponse aRecords,
            CloudflareConfiguration config
        )
        {
            MockCloudflareClient cfClient = new(aRecords);
            MockAddrFetcher addrFetcher = new();
            FakeLogger<UpdateService> logger = new();

            return new UpdateService(logger, addrFetcher, cfClient, config);
        }

        public static CloudflareConfiguration GenerateConfig(string fileName)
        {
            string path = PathGenerator.Generate(fileName);
            CloudflareConfiguration config;

            using (var reader = new StreamReader(path))
            {
                string stringResults = reader.ReadToEnd();
                var deserializer = new DeserializerBuilder().Build();

                config = deserializer.Deserialize<CloudflareConfiguration>(stringResults);
            }

            return config;
        }
    }
}
