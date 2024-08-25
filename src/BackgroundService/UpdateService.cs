namespace DynIpUpdater
{
    public class UpdateService(
        ILogger<UpdateService> logger,
        IAddrFetcher addrFetcher,
        ICloudflareClient cloudflareClient,
        CloudflareConfiguration config
    ) : BackgroundService
    {
        private readonly ILogger<UpdateService> _logger = logger;
        private readonly IAddrFetcher _addrFetcher = addrFetcher;
        private readonly ICloudflareClient _cloudflareClient = cloudflareClient;
        private readonly CloudflareConfiguration _config = config;

        public IAddress? CurrentAddress { get; set; }

        public bool AddressSet
        {
            get => CurrentAddress is not null;
        }

        /// <summary>
        /// Performs initiation tasks and then enters a loop to check and update DNS records.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /* On initialization, we build an inventory of records that will need to be updated.
             * If the records don't already exist in Cloudflare, we create them.
            */
            foreach (ZoneConfiguration zone in _config.Zones)
            {
                await InitiateRecordsAsync(zone, stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!AddressSet)
                {
                    CurrentAddress = await _addrFetcher.FetchAddressAsync();
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation("Ip Address is {address}", CurrentAddress.Address);
                    }
                }
                else
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation(
                            "Ip Address is already set to {address}",
                            CurrentAddress?.Address
                        );
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// Runs a sequence to create our inventory of records that will be updated according to
        /// the provided config. First, we get the existing records from Cloudflare. For each
        /// record in the config, we check if it exists in the Cloudflare records. If it doesn't,
        /// we create it.
        /// </summary>
        /// <param name="zone">The zone to get records for.</param>
        /// <param name="stoppingToken">The token to monitor for cancellation requests.</param>
        /// <returns>A Task representing the creation process.</returns>
        public async Task InitiateRecordsAsync(
            ZoneConfiguration zone,
            CancellationToken stoppingToken
        )
        {
            List<RecordResponse> existingRecords = await GetExistingRecords(zone, stoppingToken);
        }

        /// <summary>
        /// Gets all existing "A" and "CNAME" records from Cloudflare.
        /// </summary>
        /// <param name="zone">The zone to get records for.</param>
        /// <param name="stoppingToken">The token to monitor for cancellation requests.</param>
        /// <returns>A list of all returned records.</returns>
        public async Task<List<RecordResponse>> GetExistingRecords(
            ZoneConfiguration zone,
            CancellationToken stoppingToken
        )
        {
            List<RecordResponse> records = [];

            if (zone.HasARecords)
            {
                ListARecordsRequest req = new() { ZoneId = zone.ZoneId };
                ListRecordsResponse aRecords = await _cloudflareClient.GetARecordsAsync(
                    req,
                    stoppingToken
                );

                if (aRecords.Success && aRecords.Result is not null)
                {
                    records.AddRange(aRecords.Result);
                }
                else if (!aRecords.Success)
                {
                    _logger.LogError(
                        "Failed to get A records: {errors}",
                        string.Join(", ", aRecords.Errors)
                    );
                }
                else
                {
                    _logger.LogInformation(
                        "No existing A records found for zone {zoneId}",
                        zone.ZoneId
                    );
                }
            }

            if (zone.HasCnameRecords)
            {
                ListCnameRecordsRequest req = new() { ZoneId = zone.ZoneId };
                ListRecordsResponse cnameRecords = await _cloudflareClient.GetCnameRecordsAsync(
                    req,
                    stoppingToken
                );

                if (cnameRecords.Success && cnameRecords.Result is not null)
                {
                    records.AddRange(cnameRecords.Result);
                }
                else if (!cnameRecords.Success)
                {
                    _logger.LogError(
                        "Failed to get CNAME records: {errors}",
                        string.Join(", ", cnameRecords.Errors)
                    );
                }
                else
                {
                    _logger.LogInformation(
                        "No existing CNAME records found for zone {zoneId}",
                        zone.ZoneId
                    );
                }
            }

            return records;
        }
    }
}
