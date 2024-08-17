using DynIpUpdater;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests
{
    public class HttpClientExtensionTests()
    {
        [Fact]
        public void AddIpifyClient_ClientAdded()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddIpifyClient();

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

            builder.Services.AddCloudflareClient("TEST_KEY");

            var app = builder.Build();
            using HttpClient client = app
                .Services.GetRequiredService<IHttpClientFactory>()
                .CreateClient(NamedHttpClients.CloudflareClient.ToString());

            Assert.True(client.BaseAddress == new Uri("https://api.cloudflare.com/client/v4"));
            Assert.True(client.DefaultRequestHeaders.Authorization?.Scheme == "Bearer");
            Assert.True(client.DefaultRequestHeaders.Authorization?.Parameter == "TEST_KEY");
        }
    }
}
