namespace DynIpUpdater
{
    public class CloudflareClient(
        IHttpClientFactory factory,
        CloudflareConfiguration config,
        ILogger<CloudflareClient> logger,
        Uri baseAddress
    ) : ICloudflareClient
    {
        private readonly CloudflareConfiguration _config = config;
        private readonly ILogger<CloudflareClient> _logger = logger;
        public IHttpClientFactory Factory { get; } = factory;
        public Uri BaseAddress { get; } = baseAddress;

        public HttpClient CreateClient(string zoneId)
        {
            var client = Factory.CreateClient();
            ZoneConfiguration zoneConfig =
                _config.Zones.FirstOrDefault(z => z.ZoneId == zoneId)
                ?? throw new ArgumentException($"Zone {zoneId} not found in configuration");

            client.BaseAddress = BaseAddress;
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {zoneConfig.BearerToken}");
            client.DefaultRequestHeaders.Add("X-Auth-Email", zoneConfig.Email);
            client.DefaultRequestHeaders.Add("X-Auth-Key", zoneConfig.ApiKey);

            return client;
        }

        /// <summary>
        /// Get all A records for a given zone.
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The response from Cloudflare</returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails</exception>
        public async Task<ListRecordsResponse> GetARecordsAsync(
            ListARecordsRequest request,
            CancellationToken cancellationToken
        )
        {
            using var client = CreateClient(request.ZoneId);
            ListRecordsResponse? response = await client.GetFromJsonAsync<ListRecordsResponse>(
                $"/zones/{request.ZoneId}/dns_records?type=A",
                cancellationToken
            );

            if (response == null)
            {
                HttpRequestException ex =
                    new($"Got a null response when requesting A recordsfor zone {request.ZoneId}");
                _logger.LogError(
                    ex,
                    "Got a null response when requesting A records for zone {ZoneId}",
                    request.ZoneId
                );
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Create a new "A" record in Cloudflare.
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="zoneId">The record's zone ID</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The response from Cloudflare</returns>
        /// <exception cref="HttpRequestException">
        ///     Thrown when the request completely fails and returns null
        /// </exception>
        public async Task<SingleRecordResponse> CreateRecordAsync(
            CreateDnsRecordRequest request,
            string zoneId,
            CancellationToken cancellationToken
        )
        {
            using var client = CreateClient(zoneId);
            HttpResponseMessage response = await client.PostAsJsonAsync(
                $"/zones/{zoneId}/dns_records",
                request,
                cancellationToken
            );

            SingleRecordResponse? result =
                await response.Content.ReadFromJsonAsync<SingleRecordResponse>(
                    cancellationToken: cancellationToken
                );

            if (result == null)
            {
                HttpRequestException ex =
                    new(
                        $"Got a null response when creating record {request.Name} "
                            + $"for zone {zoneId}"
                    );
                _logger.LogError(
                    ex,
                    "Got a null response when creating record {RecordName} for zone {ZoneId}",
                    request.Name,
                    zoneId
                );
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Update an existing "A" record in Cloudflare.
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="zoneId">The record's zone ID</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The response from Cloudflare</returns>
        /// <exception cref="HttpRequestException">
        ///     Thrown when the request completely fails and returns null
        /// </exception>
        public async Task<SingleRecordResponse> UpdateRecordAsync(
            UpdateDnsRecordRequest request,
            string zoneId,
            CancellationToken cancellationToken
        )
        {
            using var client = CreateClient(zoneId);
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"/zones/{zoneId}/dns_records/{request.Id}",
                request,
                cancellationToken
            );

            SingleRecordResponse? result =
                await response.Content.ReadFromJsonAsync<SingleRecordResponse>(
                    cancellationToken: cancellationToken
                );

            if (result == null)
            {
                HttpRequestException ex =
                    new(
                        $"Got a null response when updating record {request.Id} "
                            + $"for zone {zoneId}"
                    );
                _logger.LogError(
                    ex,
                    "Got a null response when updating record {RecordId} for zone {ZoneId}",
                    request.Id,
                    zoneId
                );
                throw ex;
            }
            return result;
        }
    }
}
