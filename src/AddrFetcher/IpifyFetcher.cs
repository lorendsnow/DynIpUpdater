namespace DynIpUpdater
{
    /// <summary>
    /// Implementation of <see cref="IAddrFetcher"/> which fetches the IP address from ipify.org.
    /// </summary>
    public class IpifyFetcher : IAddrFetcher
    {
        /// <summary>
        /// Fetches the IP address from ipify.org.
        /// </summary>
        /// <returns>an <see cref="IAddress"/> object.</returns>
        public async Task<IAddress> FetchAddressAsync()
        {
            using HttpClient client = IpifyClientFactory.CreateClient();

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
