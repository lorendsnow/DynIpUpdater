namespace DynIpUpdater
{
    public class UpdateService(
        ILogger<UpdateService> logger,
        IAddrFetcher addrFetcher,
        ICloudflareClient cloudflareClient,
        CloudflareConfiguration config
    ) : BackgroundService
    {
        public readonly ILogger<UpdateService> _logger = logger;
        public readonly IAddrFetcher _addrFetcher = addrFetcher;
        public readonly ICloudflareClient _cloudflareClient = cloudflareClient;
        public readonly CloudflareConfiguration _config = config;

        public IAddress? CurrentAddress { get; set; }

        public bool AddressSet
        {
            get => CurrentAddress is not null;
        }

        public TimeSpan Interval
        {
            get => TimeSpan.FromMinutes(_config.Interval);
        }

        public Dictionary<string, List<DnsRecord>> Records { get; set; } = [];

        /// <summary>
        /// Performs initiation tasks and then enters a loop to check and update DNS records.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            CurrentAddress = await _addrFetcher.FetchAddressAsync();

            /* On initialization, we build an inventory of records that will need to be updated.
             * If the records don't already exist in Cloudflare, we create them.
            */
            foreach (ZoneConfiguration zone in _config.Zones)
            {
                Records.Add(zone.ZoneId, []);
                await InitiateRecordsAsync(zone, stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!AddressSet)
                {
                    // Something is wrong - should have been set before entering this loop.
                    InvalidOperationException ex =
                        new("IP Address was not set before entering loop");
                    _logger.LogError(ex, "{message}", ex.Message);
                    throw ex;
                }
                else
                {
                    IAddress address = await _addrFetcher.FetchAddressAsync();
                    if (address.Address != CurrentAddress.Address)
                    {
                        _logger.LogInformation(
                            "IP address has changed from {oldAddress} to {newAddress}, "
                                + "updating records",
                            CurrentAddress.Address,
                            address.Address
                        );
                        CurrentAddress = address;
                        foreach (string zoneId in Records.Keys)
                        {
                            await UpdateRecordsAsync(zoneId, address.Address, stoppingToken);
                        }
                        _logger.LogInformation("Records updated successfully");
                    }
                }
                await Task.Delay(Interval, stoppingToken);
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

            foreach (DnsRecord record in zone.DnsRecords)
            {
                if (!RecordExists(existingRecords, record))
                {
                    _logger.LogDebug(
                        "Record {recordName} not found in existing Cloudflare records, attempting "
                            + "to create new record",
                        record.Name
                    );

                    TryCreateNewCloudflareRecord(
                        record,
                        zone.ZoneId,
                        stoppingToken,
                        out DnsRecord? result
                    );
                    if (result is not null)
                    {
                        Records[zone.ZoneId].Add(result);
                    }
                }
                else
                {
                    _logger.LogDebug(
                        "Record {recordName} already exists in Cloudflare",
                        record.Name
                    );
                    Records[zone.ZoneId].Add(record);
                }
            }
        }

        /// <summary>
        /// Gets all existing "A" records from Cloudflare.
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

            return records;
        }

        /// <summary>
        /// Check if a DNS record already exists in the list of records retrieved from Cloudflare.
        /// </summary>
        /// <param name="records">The list of Cloudflare records to check against.</param>
        /// <param name="record">The subject DNS record</param>
        /// <returns>true or false</returns>
        public static bool RecordExists(List<RecordResponse> records, DnsRecord record)
        {
            return records.Any(existingRecord =>
                existingRecord.Name == record.Name && existingRecord.Type == DnsRecord.RecordType
            );
        }

        /// <summary>
        /// Trys to create a new A record in Cloudflare.
        /// </summary>
        /// <param name="record">The record to create.</param>
        /// <param name="zoneId">The zone to create the record in.</param>
        /// <param name="stoppingToken">The token to monitor for cancellation requests.</param>
        /// <param name="result">The resulting record if successful.</param>
        /// <returns>true if successful, otherwise false</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public bool TryCreateNewCloudflareRecord(
            DnsRecord record,
            string zoneId,
            CancellationToken stoppingToken,
            out DnsRecord? result
        )
        {
            if (CurrentAddress is null)
            {
                InvalidOperationException ex =
                    new("CurrentAddress is null; unable to add new records");
                _logger.LogError(ex, "{message}", ex.Message);
                throw ex;
            }

            CreateDnsRecordRequest request =
                new(
                    CurrentAddress.Address,
                    record.Name,
                    record.Proxied,
                    record.Comment,
                    record.Tags,
                    record.TTL
                );

            SingleRecordResponse response = _cloudflareClient
                .CreateRecordAsync(request, zoneId, stoppingToken)
                .Result;

            if (response.Success && response.Result is not null)
            {
                _logger.LogInformation(
                    "Successfully created record {recordName} with id {recordId}",
                    record.Name,
                    response.Result.Id
                );

                result = new DnsRecord()
                {
                    Address = response.Result.Content,
                    Name = response.Result.Name,
                    Proxied = response.Result.Proxied,
                    Comment = response.Result.Comment,
                    Id = response.Result.Id,
                    Tags = response.Result.Tags ?? [],
                    TTL = response.Result.TTL
                };

                return true;
            }
            else
            {
                _logger.LogError(
                    "Failed to create record {recordName}: {errors}",
                    record.Name,
                    string.Join(", ", response.Errors)
                );

                result = null;
                return false;
            }
        }

        /// <summary>
        /// Updates all records in a given zone with a new address.
        /// </summary>
        /// <param name="zoneId">Id of the zone being updated</param>
        /// <param name="newAddress">The IP address to update to</param>
        /// <param name="stoppingToken">The token to monitor for cancellation requests.</param>
        /// <returns></returns>
        public async Task UpdateRecordsAsync(
            string zoneId,
            string newAddress,
            CancellationToken stoppingToken
        )
        {
            foreach (DnsRecord record in Records[zoneId])
            {
                if (record.Id is null)
                {
                    _logger.LogError(
                        "Record {recordName} does not have an ID, skipping update",
                        record.Name
                    );
                    continue;
                }

                UpdateDnsRecordRequest request =
                    new(
                        newAddress,
                        record.Name,
                        record.Proxied,
                        record.Comment,
                        record.Id,
                        record.Tags,
                        record.TTL
                    );

                SingleRecordResponse response = await _cloudflareClient.UpdateRecordAsync(
                    request,
                    zoneId,
                    stoppingToken
                );

                HandleUpdateResult(response, record, newAddress);
            }
        }

        /// <summary>
        /// Handles the result of an update operation, by parsing the updated record or logging
        /// errors, and also logging any messages returned.
        /// </summary>
        /// <param name="response">The response being handled.</param>
        /// <param name="record">The record trying to be updated.</param>
        /// <param name="newAddress">The IP address we wanted to update the record with.</param>
        public void HandleUpdateResult(
            SingleRecordResponse response,
            DnsRecord record,
            string newAddress
        )
        {
            if (response.Success)
            {
                _logger.LogInformation(
                    "Successfully updated record {recordName} with id {recordId}",
                    record.Name,
                    record.Id
                );
                record.Address = newAddress;
            }
            else
            {
                _logger.LogError(
                    "Failed to update record {recordName}: {errors}",
                    record.Name,
                    string.Join(", ", response.Errors)
                );
            }

            if (response.Messages.Length > 0)
            {
                _logger.LogInformation(
                    "Messages for record {recordName}: {messages}",
                    record.Name,
                    string.Join(", ", response.Messages)
                );
            }
        }
    }
}
