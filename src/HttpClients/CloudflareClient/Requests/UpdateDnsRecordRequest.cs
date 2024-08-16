namespace DynIpUpdater
{
    /// <summary>
    /// Represents a request body for updating an A or CNAME record in Cloudflare.
    /// </summary>
    public class UpdateDnsRecordRequest(string address, string name, string recordType, string id)
    {
        /// <summary>
        /// A valid IPv4 address
        /// </summary>
        [JsonPropertyName("content")]
        public string Address { get; init; } = address;

        /// <summary>
        /// The DNS record name in punycode
        /// </summary>
        /// <remarks>Must be less than 255 characters.</remarks>
        [JsonPropertyName("name")]
        [MaxLength(255, ErrorMessage = "Name property must be less than 255 characters")]
        public string Name { get; init; } =
            name.Length <= 255
                ? name
                : throw new ValidationException("Name property must be less than 255 characters");

        /// <summary>
        /// Whether the record is receiving the performance and security benefits of Cloudflare
        /// </summary>
        [JsonPropertyName("proxied")]
        public bool Proxied { get; init; }

        /// <summary>
        /// The type of DNS record.
        /// </summary>
        /// <remarks>Must be one of "A" or "CNAME"</remarks>
        [JsonPropertyName("type")]
        public string RecordType { get; init; } =
            recordType == "A" || recordType == "CNAME"
                ? recordType
                : throw new ValidationException("RecordType must be one of 'A' or 'CNAME'");

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
        public string Id { get; init; } =
            id.Length <= 32
                ? id
                : throw new ValidationException("Id property must be less than 32 characters");

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
