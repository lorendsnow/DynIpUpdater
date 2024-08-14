namespace DynIpUpdater
{
    public class AddrFetcher : IAddrFetcher
    {
        public HttpClient Client { get; } =
            new HttpClient() { BaseAddress = new Uri("https://api.ipify.org") };

        public async Task<IAddress> FetchAddressAsync()
        {
            return await Client.GetFromJsonAsync<IpifyAddress>("?format=json");
        }
    }

    public readonly struct IpifyAddress : IAddress
    {
        [JsonPropertyName("ip")]
        public string Address { get; init; }

        public override string ToString() => Address;
    }
}
