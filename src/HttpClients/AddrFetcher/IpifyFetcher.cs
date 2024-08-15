namespace DynIpUpdater
{
    /// <summary>
    /// Implementation of <see cref="IAddrFetcher"/> which fetches the IP address from ipify.org.
    /// </summary>
    public class IpifyFetcher(IHttpClientFactory factory) : IAddrFetcher
    {
        public IHttpClientFactory Factory { get; } = factory;
        public static string ClientName
        {
            get => NamedHttpClients.IpifyFetcher.ToString();
        }

        /// <summary>
        /// Fetches the IP address from ipify.org.
        /// </summary>
        /// <returns>an <see cref="IAddress"/> object.</returns>
        public async Task<IAddress> FetchAddressAsync()
        {
            using HttpClient client = Factory.CreateClient(ClientName);

            return await client.GetFromJsonAsync<IpifyAddress>("?format=json");
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
