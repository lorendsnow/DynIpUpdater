namespace DynIpUpdater
{
    /// <summary>
    /// Interface for the Cloudflare client.
    /// </summary>
    public interface ICloudflareClient
    {
        /// <summary>
        /// We use a factory to create HTTP clients because the Cloudflare client will be captured
        /// by a long-running singleton service.
        /// </summary>
        public IHttpClientFactory Factory { get; }

        /// <summary>
        /// Get all A records for a given zone.
        /// </summary>
        /// <param name="request">A ListARecordsRequest object</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The response from Cloudflare</returns>
        public Task<ListRecordsResponse> GetARecordsAsync(
            ListARecordsRequest request,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Get all A records for a given zone.
        /// </summary>
        /// <param name="request">A ListARecordsRequest object</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The response from Cloudflare</returns>
        public Task<ListRecordsResponse> GetCnameRecordsAsync(
            ListCnameRecordsRequest request,
            CancellationToken cancellationToken
        );
    }
}
