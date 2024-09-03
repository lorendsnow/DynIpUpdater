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

        [Fact]
        public void RecordExists_RecordExists_ReturnsTrue()
        {
            List<RecordResponse> records =
            [
                new RecordResponse()
                {
                    Id = "023e105f4ecef8ad9ca31a8372d0c353",
                    ZoneId = "023e105f4ecef8ad9ca31a8372d0c353",
                    ZoneName = "example.com",
                    Name = "example.com",
                    Type = "A",
                    Content = "123.123.123.123",
                    Proxiable = true,
                    Proxied = true,
                    TTL = 1,
                    Meta = new RecordMeta(),
                    CreatedOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                }
            ];
            DnsRecord record =
                new()
                {
                    Name = "example.com",
                    Address = "123.123.123.123",
                    Proxied = true
                };

            bool result = UpdateService.RecordExists(records, record);

            Assert.True(result);
        }

        [Fact]
        public void RecordExists_RecordDoesNotExist_ReturnsFalse()
        {
            List<RecordResponse> records =
            [
                new RecordResponse()
                {
                    Id = "023e105f4ecef8ad9ca31a8372d0c353",
                    ZoneId = "023e105f4ecef8ad9ca31a8372d0c353",
                    ZoneName = "example.com",
                    Name = "example.com",
                    Type = "A",
                    Content = "123.123.123.123",
                    Proxiable = true,
                    Proxied = true,
                    TTL = 1,
                    Meta = new RecordMeta(),
                    CreatedOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                }
            ];
            DnsRecord record =
                new()
                {
                    Name = "wrongname",
                    Address = "123.123.123.123",
                    Proxied = true
                };

            bool result = UpdateService.RecordExists(records, record);

            Assert.False(result);
        }

        [Fact]
        public void TryCreateNewCloudflareRecord_CurrentAddressIsNull_ThrowsInvalidOperationException()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateManyFakeARecords(),
                GenerateConfig("ConfigWithMultipleA.yaml")
            );

            Assert.Throws<InvalidOperationException>(() =>
            {
                service.TryCreateNewCloudflareRecord(
                    new DnsRecord()
                    {
                        Address = null,
                        Name = "example.com",
                        Proxied = true
                    },
                    "12345",
                    CancellationToken.None,
                    out _
                );
            });
        }

        [Fact]
        public void TryCreateNewCloudflareRecord_RecordCreated_ReturnsTrue()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateManyFakeARecords(),
                GenerateConfig("ConfigWithMultipleA.yaml")
            );
            service.CurrentAddress = new IpifyAddress() { Address = "123.123.123.123" };

            bool result = service.TryCreateNewCloudflareRecord(
                new DnsRecord()
                {
                    Address = null,
                    Name = "example.com",
                    Proxied = true
                },
                "12345",
                CancellationToken.None,
                out _
            );
        }

        [Fact]
        public void TryCreateNewCloudflareRecord_RecordNotCreated_ReturnsFalse()
        {
            UpdateService service = GenerateServiceInstance(
                FakeRecordResponseGenerator.GenerateManyFakeARecords(),
                GenerateConfig("ConfigWithMultipleA.yaml"),
                false
            );
            service.CurrentAddress = new IpifyAddress() { Address = "123.123.123.123" };

            bool result = service.TryCreateNewCloudflareRecord(
                new DnsRecord()
                {
                    Address = null,
                    Name = "example.com",
                    Proxied = true
                },
                "12345",
                CancellationToken.None,
                out _
            );

            Assert.False(result);
        }

        public static UpdateService GenerateServiceInstance(
            ListRecordsResponse aRecords,
            CloudflareConfiguration config,
            bool goodResults = true
        )
        {
            ICloudflareClient cfClient;

            if (goodResults)
            {
                cfClient = new MockCloudflareClientGoodResults(aRecords);
            }
            else
            {
                cfClient = new MockCloudflareClientBadResults(aRecords);
            }

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
