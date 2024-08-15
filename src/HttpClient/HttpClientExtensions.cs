namespace DynIpUpdater
{
    /// <summary>
    /// Extensions to add typed HttpClients to the service collection.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Adds an HttpClient for the IpifyFetcher class to use.
        /// </summary>
        /// <param name="services">The hostbuilder's IServiceCollection</param>
        /// <returns>an updated services collection with the typed HttpClient addded.</returns>
        public static IServiceCollection AddIpifyClient(this IServiceCollection services)
        {
            services.AddHttpClient(
                NamedHttpClients.IpifyFetcher.ToString(),
                client =>
                {
                    client.BaseAddress = new Uri("https://api.ipify.org");
                }
            );

            return services;
        }
    }
}
