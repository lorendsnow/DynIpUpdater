using DynIpUpdater;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace Tests
{
    public class HttpClientExtensionTests(ITestOutputHelper helper)
    {
        private readonly ITestOutputHelper _helper = helper;

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
    }
}
