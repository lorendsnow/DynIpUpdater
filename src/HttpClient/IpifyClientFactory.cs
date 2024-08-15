namespace DynIpUpdater
{
    /// <summary>
    /// Simple static class which handles creating single HttpClient instances for fetching IP
    /// addresses.
    /// </summary>
    public static class IpifyClientFactory
    {
        /// <summary>
        /// Creates a new HttpClient instance with the base address set to ipify.org.
        /// </summary>
        /// <returns>
        /// A new <see cref="HttpClient"/> instance with the base address set to ipify.org.
        /// </returns>
        public static HttpClient CreateClient()
        {
            return new HttpClient() { BaseAddress = new Uri("https://api.ipify.org") };
        }
    }
}
