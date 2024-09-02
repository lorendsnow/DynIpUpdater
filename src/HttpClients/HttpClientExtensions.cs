namespace DynIpUpdater
{
    /// <summary>
    /// Extensions to add named HttpClients to the service collection.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Adds an HttpClient for the IpifyFetcher class to use.
        /// </summary>
        /// <param name="services">The hostbuilder's IServiceCollection</param>
        /// <returns>an updated services collection with the named HttpClient addded.</returns>
        public static IServiceCollection AddIpifyClient(
            this IServiceCollection services,
            Uri baseAddress
        )
        {
            services.AddHttpClient(
                NamedHttpClients.IpifyFetcher.ToString(),
                client =>
                {
                    client.BaseAddress = baseAddress;
                }
            );

            return services;
        }

        /// <summary>
        /// Adds an HttpClient for the CloudflareClient class to use.
        /// </summary>
        /// <param name="services">The hostbuilder's IServiceCollection</param>
        /// <returns>an updated services collection with the named HttpClient addded.</returns>
        public static IServiceCollection AddCloudflareClient(this IServiceCollection services)
        {
            services.AddHttpClient(NamedHttpClients.CloudflareClient.ToString());
            return services;
        }
    }
}
