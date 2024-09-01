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
        /// Create a new "A" record in Cloudflare.
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="zoneId">The zone ID</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>
        ///     The Cloudflare response with the created record or any errors/messages
        /// </returns>
        public Task<SingleRecordResponse> CreateRecordAsync(
            CreateDnsRecordRequest request,
            string zoneId,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Update an existing "A" record in Cloudflare.
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="zoneId">The record's zone ID</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>
        ///     The Cloudflare response with the updated record or any errors/messages
        /// </returns>
        public Task<SingleRecordResponse> UpdateRecordAsync(
            UpdateDnsRecordRequest request,
            string zoneId,
            CancellationToken cancellationToken
        );
    }
}
