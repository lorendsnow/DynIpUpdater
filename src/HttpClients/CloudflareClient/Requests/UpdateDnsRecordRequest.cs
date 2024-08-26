namespace DynIpUpdater
{
    /// <summary>
    /// Represents a request body for updating an A record in Cloudflare.
    /// </summary>
    public record UpdateDnsRecordRequest(
        string Address,
        string Name,
        bool Proxied,
        string? Comment,
        string Id,
        string[] Tags,
        int TTL
    )
    {
        /// <summary>
        /// A valid IPv4 address
        /// </summary>
        [JsonPropertyName("content")]
        public string Address { get; init; } = Address;

        /// <summary>
        /// The DNS record name in punycode
        /// </summary>
        /// <remarks>Must be less than 255 characters.</remarks>
        [JsonPropertyName("name")]
        public string Name { get; init; } =
            Name.Length <= 255
                ? Name
                : throw new ValidationException("Name property must be less than 255 characters");

        /// <summary>
        /// Whether the record is receiving the performance and security benefits of Cloudflare
        /// </summary>
        [JsonPropertyName("proxied")]
        public bool Proxied { get; init; } = Proxied;

        /// <summary>
        /// The type of DNS record.
        /// </summary>
        [JsonPropertyName("type")]
        public static string RecordType
        {
            get => "A";
        }

        /// <summary>
        /// Comments or notes about the DNS record. This field has no effect on DNS responses.
        /// </summary>
        [JsonPropertyName("comment")]
        public string? Comment { get; init; } = Comment;

        /// <summary>
        /// Identification
        /// </summary>
        /// <example>023e105f4ecef8ad9ca31a8372d0c353</example>
        /// <remarks>Must be less than 32 characters.</remarks>
        [JsonPropertyName("id")]
        public string Id { get; init; } =
            Id.Length <= 32
                ? Id
                : throw new ValidationException("Id property must be less than 32 characters");

        /// <summary>
        /// Custom tags for the DNS record. This field has no effect on DNS responses.
        /// </summary>
        [JsonPropertyName("tags")]
        public string[] Tags { get; init; } = Tags;

        /// <summary>
        /// Time To Live (TTL) of the DNS record in seconds. Setting to 1 means 'automatic'.
        /// </summary>
        /// <remarks>
        /// Value must be between 60 and 86400, with the minimum reduced to 30 for Enterprise
        /// zones.
        /// </remarks>
        [JsonPropertyName("ttl")]
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
                    "TTL must be 1 or between 60 and 86400, with the minimum reduced to 30 for Enterprise zones"
                );
            }
        }
    }
}
