namespace Tests.Helpers
{
    public class MockCloudflareClient(ListRecordsResponse aRecordResponse) : ICloudflareClient
    {
        private readonly ListRecordsResponse _aRecordResponse = aRecordResponse;

        public IHttpClientFactory Factory
        {
            get => new DummyClientFactory();
        }

        public async Task<ListRecordsResponse> GetARecordsAsync(
            ListARecordsRequest request,
            CancellationToken cancellationToken
        )
        {
            return await Task.FromResult(_aRecordResponse);
        }

        public async Task<SingleRecordResponse> CreateRecordAsync(
            CreateDnsRecordRequest request,
            CancellationToken cancellationToken
        )
        {
            RecordResponse result =
                new()
                {
                    ZoneId = "023e105f4ecef8ad9ca31a8372d0c353",
                    ZoneName = "example.com",
                    Content = "123.123.123.123",
                    Name = request.Name,
                    Proxied = request.Proxied,
                    Type = "A",
                    CreatedOn = DateTimeOffset.Now,
                    Id = "023e105f4ecef8ad9ca31a8372d0c353",
                    Meta = new RecordMeta() { AutoAdded = true, Source = "Source" },
                    ModifiedOn = DateTimeOffset.Now,
                    Proxiable = true,
                    TTL = 1,
                };

            return await Task.FromResult(
                new SingleRecordResponse()
                {
                    Result = result,
                    Errors = [],
                    Messages = [],
                    Success = true,
                }
            );
        }

        public async Task<SingleRecordResponse> UpdateRecordAsync(
            UpdateDnsRecordRequest request,
            CancellationToken cancellationToken
        )
        {
            RecordResponse result =
                new()
                {
                    ZoneId = "023e105f4ecef8ad9ca31a8372d0c353",
                    ZoneName = "example.com",
                    Content = "123.123.123.123",
                    Name = request.Name,
                    Proxied = request.Proxied,
                    Type = "A",
                    CreatedOn = DateTimeOffset.Now,
                    Id = "023e105f4ecef8ad9ca31a8372d0c353",
                    Meta = new RecordMeta() { AutoAdded = true, Source = "Source" },
                    ModifiedOn = DateTimeOffset.Now,
                    Proxiable = true,
                    TTL = 1,
                };
            return await Task.FromResult(
                new SingleRecordResponse()
                {
                    Result = result,
                    Errors = [],
                    Messages = [],
                    Success = true,
                }
            );
        }
    }

    public class DummyClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }

    public static class FakeRecordResponseGenerator
    {
        public static ListRecordsResponse GenerateManyFakeARecords()
        {
            return new ListRecordsResponse()
            {
                Result =
                [
                    new RecordResponse()
                    {
                        ZoneId = "023e105f4ecef8ad9ca31a8372d0c353",
                        ZoneName = "example.com",
                        Content = "123.123.123.123",
                        Name = "example.com",
                        Proxied = true,
                        Type = "A",
                        CreatedOn = DateTimeOffset.Now,
                        Id = "023e105f4ecef8ad9ca31a8372d0c353",
                        Meta = new RecordMeta() { AutoAdded = true, Source = "Source" },
                        ModifiedOn = DateTimeOffset.Now,
                        Proxiable = true,
                        TTL = 1,
                    },
                    new RecordResponse()
                    {
                        ZoneId = "023e105f4ecef8ad9ca31a8372d0c353",
                        ZoneName = "example.com",
                        Content = "123.123.123.123",
                        Name = "www",
                        Proxied = true,
                        Type = "A",
                        CreatedOn = DateTimeOffset.Now,
                        Id = "023e105f4ecef8ad9ca31a8372d0c353",
                        Meta = new RecordMeta() { AutoAdded = true, Source = "Source" },
                        ModifiedOn = DateTimeOffset.Now,
                        Proxiable = true,
                        TTL = 1,
                    },
                ],
                Errors = [],
                Messages = [],
                Success = true,
                ResultInfo = new ResultInfo()
                {
                    Count = 2,
                    Page = 1,
                    PerPage = 20,
                    TotalCount = 2
                },
            };
        }

        public static ListRecordsResponse GenerateEmpty()
        {
            return new ListRecordsResponse()
            {
                Result = null,
                Errors = [],
                Messages = [],
                Success = true,
                ResultInfo = new ResultInfo()
                {
                    Count = 0,
                    Page = 1,
                    PerPage = 20,
                    TotalCount = 0
                },
            };
        }

        public static ListRecordsResponse GenerateEmptyWithError()
        {
            return new ListRecordsResponse()
            {
                Result = null,
                Errors = [new() { Code = 1000, Message = "fake error message" }],
                Messages = [],
                Success = false,
                ResultInfo = new ResultInfo()
                {
                    Count = 0,
                    Page = 1,
                    PerPage = 20,
                    TotalCount = 0
                },
            };
        }
    }
}
