namespace DynIpUpdater
{
    /// <summary>
    /// Represents an "A" or "CNAME" record response from the Cloudflare API.
    /// </summary>
    public record RecordResponse
    {
        /// <summary>
        /// A valid IPv4 address (in the case of an A record) or a hostname (in the case of a
        /// CNAME record).
        /// </summary>
        public required string Content { get; init; }

        /// <summary>
        /// DNS record name (or @ for the zone apex) in Punycode.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Whether the record is receiving the performance and security benefits of Cloudflare.
        /// </summary>
        public bool Proxied { get; init; }

        /// <summary>
        /// Record type.
        /// </summary>
        public required string Type { get; init; }

        /// <summary>
        /// Comments or notes about the DNS record. This field has no effect on DNS responses.
        /// </summary>
        public string? Comment { get; init; }

        /// <summary>
        /// When the record comment was last modified.
        /// </summary>
        public DateTimeOffset? CommentModifiedOn { get; init; }

        /// <summary>
        /// When the record was created.
        /// </summary>
        public DateTimeOffset CreatedOn { get; init; }

        /// <summary>
        /// Identifier
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// Extra Cloudflare-specific information about the record.
        /// </summary>
        public RecordMeta Meta { get; init; }

        /// <summary>
        /// When the record was last modified.
        /// </summary>
        public DateTimeOffset ModifiedOn { get; init; }

        /// <summary>
        /// Whether the record can be proxied by Cloudflare or not.
        /// </summary>
        public bool Proxiable { get; init; }

        /// <summary>
        /// Custom tags for the DNS record. This field has no effect on DNS responses.
        /// </summary>
        public string[]? Tags { get; init; }

        /// <summary>
        /// When the record tags were last modified.
        /// </summary>
        public DateTimeOffset[]? TagsModifiedOn { get; init; }

        /// <summary>
        /// Time To Live (TTL) of the DNS record in seconds. Setting to 1 means 'automatic'. Value
        /// must be between 60 and 86400, with the minimum reduced to 30 for Enterprise zones.
        /// </summary>
        public int TTL { get; init; }
    }

    /// <summary>
    /// Extra Cloudflare-specific information about a record.
    /// </summary>
    public readonly struct RecordMeta
    {
        /// <summary>
        /// True if Cloudflare automatically added this DNS record during initial setup.
        /// </summary>
        public bool AutoAdded { get; init; }

        /// <summary>
        /// Where the record originated from.
        /// </summary>
        public string? Source { get; init; }
    }
}
