namespace Tests.Helpers
{
    public class MockCloudflareClient(
        ListRecordsResponse aRecordResponse,
        ListRecordsResponse cnameRecordResponse
    ) : ICloudflareClient
    {
        private readonly ListRecordsResponse _aRecordResponse = aRecordResponse;
        private readonly ListRecordsResponse _cnameRecordResponse = cnameRecordResponse;

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

        public async Task<ListRecordsResponse> GetCnameRecordsAsync(
            ListCnameRecordsRequest request,
            CancellationToken cancellationToken
        )
        {
            return await Task.FromResult(_cnameRecordResponse);
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

        public static ListRecordsResponse GenerateManyFakeCnameRecords()
        {
            return new ListRecordsResponse()
            {
                Result =
                [
                    new RecordResponse()
                    {
                        Content = "blog.example.com",
                        Name = "example.com",
                        Proxied = true,
                        Type = "CNAME",
                        CreatedOn = DateTimeOffset.Now,
                        Id = "023e105f4ecef8ad9ca31a8372d0c353",
                        Meta = new RecordMeta() { AutoAdded = true, Source = "Source" },
                        ModifiedOn = DateTimeOffset.Now,
                        Proxiable = true,
                        TTL = 1,
                    },
                    new RecordResponse()
                    {
                        Content = "mail.example.com",
                        Name = "example.com",
                        Proxied = true,
                        Type = "CNAME",
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
