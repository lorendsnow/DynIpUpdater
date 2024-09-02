namespace Tests
{
    public class CloudflareClientTests
    {
        private MockHttpMessageHandler? _mockHandler;

        [Fact]
        public void UpdateDnsRecordRequest_NameTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                UpdateDnsRecordRequest request =
                    new(
                        "123.123.123.123",
                        new string('a', 658),
                        true,
                        null,
                        "023e105f4ecef8ad9ca31a8372d0c353",
                        [],
                        1
                    );
            });
        }

        [Fact]
        public void UpdateDnsRecordRequest_IdTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                UpdateDnsRecordRequest request =
                    new("123.123.123.123", "example.com", true, null, new string('a', 33), [], 1);
            });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10154675)]
        public void UpdateDnsRecordRequest_TTLOutOfRange_ThrowsValidationException(int ttl)
        {
            Assert.Throws<ValidationException>(() =>
            {
                UpdateDnsRecordRequest request =
                    new("123.123.123.123", "example.com", true, null, "12345", [], ttl);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(60)]
        [InlineData(86400)]
        public void UpdateDnsRecordRequest_TTLValid_DoesNotThrowValidationException(int ttl)
        {
            UpdateDnsRecordRequest request =
                new("123.123.123.123", "example.com", true, null, "12345", [], ttl);

            Assert.Equal(ttl, request.TTL);
        }

        [Fact]
        public void CreateDnsRecordRequest_NameTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                CreateDnsRecordRequest request =
                    new("123.123.123.123", new string('a', 658), true, null, [], 1);
            });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10154675)]
        public void CreateDnsRecordRequest_TTLOutOfRange_ThrowsValidationException(int ttl)
        {
            Assert.Throws<ValidationException>(() =>
            {
                CreateDnsRecordRequest request =
                    new("123.123.123.123", "example.com", true, null, [], ttl);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(60)]
        [InlineData(86400)]
        public void CreateDnsRecordRequest_TTLValid_DoesNotThrowValidationException(int ttl)
        {
            CreateDnsRecordRequest request =
                new("123.123.123.123", "example.com", true, null, [], ttl);

            Assert.Equal(ttl, request.TTL);
        }

        [Fact]
        public void ListARecordsRequest_InitiateInstance_SetsRecordTypeToA()
        {
            ListARecordsRequest request = new() { ZoneId = "12345" };
            Assert.Equal("A", request.RecordType);
            Assert.Equal("12345", request.ZoneId);
        }

        [Fact]
        public void CreateClient_ShouldCreateHttpClientWithCorrectHeaders()
        {
            CloudflareClient client = CreateClient();

            HttpClient httpClient = client.CreateClient("12345");

            Assert.Equal(
                "Bearer 12345",
                httpClient.DefaultRequestHeaders.Authorization?.ToString()
            );
            Assert.Equal(
                "test@example.com",
                httpClient.DefaultRequestHeaders.GetValues("X-Auth-Email").First()
            );
            Assert.Equal("12345", httpClient.DefaultRequestHeaders.GetValues("X-Auth-Key").First());
        }

        [Fact]
        public void CreateClient_ZoneNotConfigured_ThrowsArgumentException()
        {
            CloudflareClient client = CreateClient();

            Assert.Throws<ArgumentException>(() => client.CreateClient("67890"));
        }

        [Fact]
        public async Task GetARecordsAsync_ShouldReturnValidResponse()
        {
            CloudflareClient client = CreateClient();
            ListARecordsRequest request = new() { ZoneId = "12345" };
            ListRecordsResponse expectedResponse =
                new()
                {
                    Result =
                    [
                        new RecordResponse()
                        {
                            Id = "12345",
                            ZoneId = "12345",
                            ZoneName = "example.com",
                            Name = "example.com",
                            Type = "A",
                            Content = "123.123.123.123",
                            Proxiable = true,
                            Proxied = true,
                            TTL = 1,
                            Meta = new RecordMeta() { AutoAdded = true, Source = "primary" },
                            CreatedOn = DateTimeOffset.UtcNow,
                            ModifiedOn = DateTimeOffset.UtcNow
                        },
                        new RecordResponse()
                        {
                            Id = "12345",
                            ZoneId = "12345",
                            ZoneName = "example.com",
                            Name = "test",
                            Type = "A",
                            Content = "123.123.123.123",
                            Proxiable = true,
                            Proxied = true,
                            TTL = 1,
                            Meta = new RecordMeta() { AutoAdded = true, Source = "primary" },
                            CreatedOn = DateTimeOffset.UtcNow,
                            ModifiedOn = DateTimeOffset.UtcNow
                        }
                    ],
                    Success = true,
                    Errors = [],
                    Messages = [],
                    ResultInfo = new ResultInfo()
                    {
                        Page = 1,
                        PerPage = 20,
                        Count = 2,
                        TotalCount = 2,
                        TotalPages = 1
                    }
                };
            string jsonResponse = JsonSerializer.Serialize(expectedResponse);
            _mockHandler
                ?.When($"/zones/{request.ZoneId}/dns_records?type=A")
                .Respond(new StringContent(jsonResponse));

            ListRecordsResponse response = await client.GetARecordsAsync(
                request,
                CancellationToken.None
            );

            Assert.Equal(expectedResponse.Result[0].Id, response.Result?[0].Id);
        }

        private CloudflareClient CreateClient()
        {
            _mockHandler = new();
            Uri baseAddress = new("https://api.cloudflare.com/client/v4");
            DnsRecord record1 =
                new()
                {
                    Address = "123.123.123.123",
                    Name = "example.com",
                    Proxied = true,
                    Id = "12345",
                };
            DnsRecord record2 =
                new()
                {
                    Address = "123.123.123.123",
                    Name = "test",
                    Proxied = true,
                    Id = "12345",
                };
            CloudflareConfiguration config =
                new()
                {
                    Zones =
                    [
                        new ZoneConfiguration()
                        {
                            BearerToken = "12345",
                            ApiKey = "12345",
                            Email = "test@example.com",
                            ZoneId = "12345",
                            DnsRecords = [record1, record2]
                        },
                        new ZoneConfiguration()
                        {
                            BearerToken = "54321",
                            ApiKey = "12345",
                            Email = "test@example.com",
                            ZoneId = "54321",
                            DnsRecords = [record1, record2]
                        }
                    ],
                    Interval = 1
                };
            FakeLogger<CloudflareClient> logger = new();
            MockCloudflareClientFactory factory = new(_mockHandler);

            return new CloudflareClient(factory, config, logger, baseAddress);
        }
    }
}
