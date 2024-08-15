namespace DynIpUpdater
{
    /// <summary>
    /// Represents a request body for updating an A record in Cloudflare.
    /// </summary>
    public record ARecord
    {
        /// <summary>
        /// A valid IPv4 address
        /// </summary>
        [JsonPropertyName("content")]
        public required string Address { get; init; }

        /// <summary>
        /// The DNS record name in punycode
        /// </summary>
        /// <remarks>Must be less than 255 characters.</remarks>
        [JsonPropertyName("name")]
        [MaxLength(255, ErrorMessage = "Name property must be less than 255 characters")]
        public required string Name { get; init; }

        /// <summary>
        /// Whether the record is receiving the performance and security benefits of Cloudflare
        /// </summary>
        [JsonPropertyName("proxied")]
        public bool Proxied { get; init; }

        [JsonPropertyName("type")]
        public static string RecordType => "A";

        /// <summary>
        /// Comments or notes about the DNS record. This field has no effect on DNS responses.
        /// </summary>
        [JsonPropertyName("comment")]
        public string? Comment { get; init; }

        /// <summary>
        /// Identification
        /// </summary>
        /// <example>023e105f4ecef8ad9ca31a8372d0c353</example>
        /// <remarks>Must be less than 32 characters.</remarks>
        [JsonPropertyName("id")]
        [MaxLength(32, ErrorMessage = "Id property must be less than 32 characters")]
        public required string Id { get; init; }

        /// <summary>
        /// Custom tags for the DNS record. This field has no effect on DNS responses.
        /// </summary>
        [JsonPropertyName("tags")]
        public string[] Tags { get; init; } = [];

        /// <summary>
        /// Time To Live (TTL) of the DNS record in seconds. Setting to 1 means 'automatic'.
        /// </summary>
        /// <remarks>
        /// Value must be between 60 and 86400, with the minimum reduced to 30 for Enterprise
        /// zones.
        /// </remarks>
        [JsonPropertyName("ttl")]
        public int TTL { get; init; } = 1;
    }
}
