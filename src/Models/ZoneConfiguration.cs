namespace DynIpUpdater
{
    /// <summary>
    /// Represents configuration for a single zone in Cloudflare.
    /// </summary>
    public class ZoneConfiguration
    {
        /// <summary>
        /// Bearer token associated with this zone.
        /// </summary>
        public required string BearerToken { get; init; }

        /// <summary>
        /// API key associated with this zone.
        /// </summary>
        public required string ApiKey { get; init; }

        /// <summary>
        /// Email associated with this zone.
        /// </summary>
        public required string Email { get; init; }

        /// <summary>
        /// Zone ID associated with this zone.
        /// </summary>
        public required string ZoneId { get; init; }

        /// <summary>
        /// List of DNS records to be created and/or updated under this zone.
        /// </summary>
        public required List<DnsRecord> DnsRecords { get; set; }

        /// <summary>
        /// Returns a string representation of this ZoneConfiguration.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ZoneConfiguration "
                + "{ "
                + $"BearerToken: {BearerToken} "
                + $"ApiKey: {ApiKey} "
                + $"Email: {Email} "
                + $"ZoneId: {ZoneId} "
                + $"DnsRecords: [{string.Join(", ", DnsRecords)}] "
                + "}";
        }
    }
}
