using System.Net.Http.Headers;

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

        /// <summary>
        /// Adds an HttpClient for the CloudflareClient class to use.
        /// </summary>
        /// <param name="services">The hostbuilder's IServiceCollection</param>
        /// <returns>an updated services collection with the named HttpClient addded.</returns>
        public static IServiceCollection AddCloudflareClient(
            this IServiceCollection services,
            string apiKey
        )
        {
            services.AddHttpClient(
                NamedHttpClients.CloudflareClient.ToString(),
                client =>
                {
                    client.BaseAddress = new Uri("https://api.cloudflare.com/client/v4");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        apiKey
                    );
                }
            );

            return services;
        }
    }
}
