using System.Text.RegularExpressions;
using DynIpUpdater;

namespace Tests
{
    public partial class AddrFetcherTests
    {
        private readonly Regex _addrRegex = IpRegex();

        [Fact]
        public async Task FetchAddress_AddressReturned()
        {
            IHttpClientFactory factory = new MockClientFactory();
            IpifyFetcher fetcher = new(factory);

            IAddress address = await fetcher.FetchAddressAsync();

            Assert.Matches(_addrRegex, address.Address);
        }

        [GeneratedRegex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")]
        private static partial Regex IpRegex();
    }

    public class MockClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return new() { BaseAddress = new Uri("https://api.ipify.org") };
        }
    }
}
