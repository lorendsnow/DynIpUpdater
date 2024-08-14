namespace DynIpUpdater
{
    /// <summary>
    /// Implementation of <see cref="IAddrFetcher"/> which fetches the IP address from ipify.org.
    /// </summary>
    public class AddrFetcher : IAddrFetcher
    {
        /// <summary>
        /// Client used to fetch the IP address; initialized with the base address of ipify.org.
        /// </summary>
        public HttpClient Client { get; } =
            new HttpClient() { BaseAddress = new Uri("https://api.ipify.org") };

        /// <summary>
        /// Fetches the IP address from ipify.org.
        /// </summary>
        /// <returns>an <see cref="IAddress"/> object.</returns>
        public async Task<IAddress> FetchAddressAsync()
        {
            return await Client.GetFromJsonAsync<IpifyAddress>("?format=json");
        }
    }

    /// <summary>
    /// Implementation of <see cref="IAddress"/> which represents an IP address fetched as JSON
    /// from ipify.org.
    /// </summary>
    public readonly struct IpifyAddress : IAddress
    {
        /// <summary>
        /// The IP address fetched from ipify.org.
        /// </summary>
        [JsonPropertyName("ip")]
        public string Address { get; init; }

        /// <summary>
        /// Returns just the IP address as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Address;
    }
}
