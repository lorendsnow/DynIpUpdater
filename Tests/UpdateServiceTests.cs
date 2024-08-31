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

        [Fact]
        public async Task HandleUpdateResult_SuccessfulUpdate_RecordAddressUpdated()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateManyFakeARecords(),
                GenerateConfig("ConfigWithMultipleA.yaml")
            );
            DnsRecord record =
                new()
                {
                    Address = "321.321.321.321",
                    Name = "example.com",
                    Proxied = true,
                    Id = "023e105f4ecef8ad9ca31a8372d0c353"
                };
            var records = await service.GetExistingRecords(
                service._config.Zones[0],
                CancellationToken.None
            );
            SingleRecordResponse response =
                new()
                {
                    Result = records[0],
                    Success = true,
                    Errors = [],
                    Messages = []
                };

            service.HandleUpdateResult(response, record, response.Result.Content);

            Assert.Equal(response.Result.Content, record.Address);
        }

        [Fact]
        public void HandleUpdateResult_ErrorReturned_RecordAddressUnchanged()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateManyFakeARecords(),
                GenerateConfig("ConfigWithMultipleA.yaml")
            );
            DnsRecord record =
                new()
                {
                    Address = "321.321.321.321",
                    Name = "example.com",
                    Proxied = true,
                    Id = "023e105f4ecef8ad9ca31a8372d0c353"
                };
            SingleRecordResponse response =
                new()
                {
                    Result = null,
                    Success = false,
                    Errors = [new() { Code = 1, Message = "Test error message" }],
                    Messages = []
                };

            service.HandleUpdateResult(response, record, "123.123.123.123");

            Assert.Equal("321.321.321.321", record.Address);
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
