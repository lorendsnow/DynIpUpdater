namespace Tests
{
    public partial class UpdateServiceTests
    {
        [Fact]
        public async Task GetExistingRecords_ExistingAandCnameRecords_ReturnsRecords()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateManyFakeARecords(),
                FakeRecordResponseGenerator.GenerateManyFakeCnameRecords(),
                GenerateConfig("ConfigWithAandCname.yaml")
            );

            var records = await service.GetExistingRecords(
                service._config.Zones[0],
                CancellationToken.None
            );

            Assert.Equal(4, records.Count);
        }

        [Fact]
        public async Task GetExistingRecords_NoExistingRecords_ReturnsEmpty()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateEmpty(),
                FakeRecordResponseGenerator.GenerateEmpty(),
                GenerateConfig("ConfigWithAandCname.yaml")
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
                FakeRecordResponseGenerator.GenerateEmpty(),
                GenerateConfig("ConfigWithAandCname.yaml")
            );

            var records = await service.GetExistingRecords(
                service._config.Zones[0],
                CancellationToken.None
            );

            Assert.Empty(records);
        }

        public static UpdateService GenerateServiceInstance(
            ListRecordsResponse aRecords,
            ListRecordsResponse cnameRecords,
            CloudflareConfiguration config
        )
        {
            MockCloudflareClient cfClient = new(aRecords, cnameRecords);
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
