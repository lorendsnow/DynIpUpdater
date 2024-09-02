namespace Tests
{
    public class HttpClientExtensionTests()
    {
        [Fact]
        public void AddIpifyClient_ClientAdded()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddIpifyClient(new Uri("https://api.ipify.org"));

            var app = builder.Build();
            using HttpClient client = app
                .Services.GetRequiredService<IHttpClientFactory>()
                .CreateClient(NamedHttpClients.IpifyFetcher.ToString());

            Assert.True(client.BaseAddress == new Uri("https://api.ipify.org"));
        }

        [Fact]
        public void AddCloudflareClient_ClientAdded()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddCloudflareClient();

            var app = builder.Build();
            using HttpClient client = app
                .Services.GetRequiredService<IHttpClientFactory>()
                .CreateClient(NamedHttpClients.CloudflareClient.ToString());

            Assert.NotNull(client);
        }
    }
}
