namespace Tests.Helpers
{
    public class MockAddrFetcher : IAddrFetcher
    {
        public async Task<IAddress> FetchAddressAsync()
        {
            return await Task.FromResult(new TestAddress() { Address = "123.123.123.123" });
        }
    }

    public class TestAddress : IAddress
    {
        public required string Address { get; init; }
    }
}
