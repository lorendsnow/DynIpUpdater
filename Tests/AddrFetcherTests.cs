using DynIpUpdater;

namespace Tests
{
    public class AddrFetcherTests
    {
        [Fact]
        public async Task FetchAddress_AddressReturned()
        {
            AddrFetcher fetcher = new();

            IAddress address = await fetcher.FetchAddressAsync();

            Assert.NotNull(address.Address);
        }
    }
}
