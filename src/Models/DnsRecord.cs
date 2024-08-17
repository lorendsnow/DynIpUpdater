namespace DynIpUpdater
{
    /// <summary>
    /// Represents an A or CNAME DNS record in Cloudflare.
    /// </summary>
    /// <param name="Address">A valid IPv4 address</param>
    /// <param name="Name">
    ///     The DNS record name in punycode. Must be less than 255 characters.
    /// </param>
    /// <param name="Proxied">
    ///     Whether the record is receiving the performance and security benefits of Cloudflare.
    /// </param>
    /// <param name="RecordType">The type of DNS record. Must be one of "A" or "CNAME".</param>
    /// <param name="Comment">
    ///     Comments or notes about the DNS record. This field has no effect on DNS responses.
    /// </param>
    /// <param name="Id">Identification. Must be 32 characters or less.</param>
    /// <param name="Tags">
    ///     Custom tags for the DNS record. This field has no effect on DNS responses.
    /// </param>
    /// <param name="TTL">
    ///     Time To Live (TTL) of the DNS record in seconds. Setting to 1 means 'automatic' for
    ///     Cloudflare purposes. If not set to 1, value must be between 60 and 86400, with the
    ///     minimum reduced to 30 for Enterprise zones.
    /// </param>
    public record DnsRecord(
        string Address,
        string Name,
        bool Proxied,
        string RecordType,
        string? Comment,
        string Id,
        string[]? Tags,
        int TTL
    )
    {
        public string Name { get; init; } =
            Name.Length <= 255
                ? Name
                : throw new ValidationException("Name property must be less than 255 characters");

        public string RecordType { get; init; } =
            RecordType == "A" || RecordType == "CNAME"
                ? RecordType
                : throw new ValidationException("RecordType must be one of 'A' or 'CNAME'");

        public string Id { get; init; } =
            Id.Length <= 32
                ? Id
                : throw new ValidationException("Id property must be less than 32 characters");

        public int TTL { get; init; } = ValidateTTL(TTL);

        private static int ValidateTTL(int ttl)
        {
            if (ttl == 1)
            {
                return 1;
            }
            else if (ttl >= 60 && ttl <= 86400)
            {
                return ttl;
            }
            else
            {
                throw new ValidationException(
                    "TTL must be 1 for automatic, or between 60 and 86400 for manual"
                );
            }
        }
    }
}
