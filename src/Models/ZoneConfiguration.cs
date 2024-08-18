namespace DynIpUpdater
{
    public class ZoneConfiguration
    {
        public required string BearerToken { get; init; }
        public required string ApiKey { get; init; }
        public required string Email { get; init; }
        public required string ZoneId { get; init; }
        public required List<DnsRecord> DnsRecords { get; set; }

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
