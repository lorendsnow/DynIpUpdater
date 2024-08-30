namespace DynIpUpdater
{
    /// <summary>
    /// Represents an "A" record response from the Cloudflare API.
    /// </summary>
    public record RecordResponse
    {
        /// <summary>
        /// Identifier
        /// </summary>
        [JsonPropertyName("id")]
        public required string Id { get; init; }

        /// <summary>
        /// Zone identifier
        /// </summary>
        [JsonPropertyName("zone_id")]
        public required string ZoneId { get; init; }

        /// <summary>
        /// Zone name
        /// </summary>
        [JsonPropertyName("zone_name")]
        public required string ZoneName { get; init; }

        /// <summary>
        /// DNS record name (or @ for the zone apex) in Punycode.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; init; }

        /// <summary>
        /// Record type.
        /// </summary>
        [JsonPropertyName("type")]
        public required string Type { get; init; }

        /// <summary>
        /// A valid IPv4 address.
        /// </summary>
        [JsonPropertyName("content")]
        public required string Content { get; init; }

        /// <summary>
        /// Whether the record can be proxied by Cloudflare or not.
        /// </summary>
        [JsonPropertyName("proxiable")]
        public bool Proxiable { get; init; }

        /// <summary>
        /// Whether the record is receiving the performance and security benefits of Cloudflare.
        /// </summary>
        [JsonPropertyName("proxied")]
        public bool Proxied { get; init; }

        /// <summary>
        /// Time To Live (TTL) of the DNS record in seconds. Setting to 1 means 'automatic'. Value
        /// must be between 60 and 86400, with the minimum reduced to 30 for Enterprise zones.
        /// </summary>
        [JsonPropertyName("ttl")]
        public int TTL { get; init; }

        /// <summary>
        /// Extra Cloudflare-specific information about the record.
        /// </summary>
        [JsonPropertyName("meta")]
        public RecordMeta Meta { get; init; }

        /// <summary>
        /// Comments or notes about the DNS record. This field has no effect on DNS responses.
        /// </summary>
        [JsonPropertyName("comment")]
        public string? Comment { get; init; }

        /// <summary>
        /// When the record comment was last modified.
        /// </summary>
        [JsonPropertyName("comment_modified_on")]
        public DateTimeOffset? CommentModifiedOn { get; init; }

        /// <summary>
        /// Custom tags for the DNS record. This field has no effect on DNS responses.
        /// </summary>
        [JsonPropertyName("tags")]
        public string[]? Tags { get; init; }

        /// <summary>
        /// When the record tags were last modified.
        /// </summary>
        [JsonPropertyName("tags_modified_on")]
        public DateTimeOffset[]? TagsModifiedOn { get; init; }

        /// <summary>
        /// When the record was created.
        /// </summary>
        [JsonPropertyName("created_on")]
        public DateTimeOffset CreatedOn { get; init; }

        /// <summary>
        /// When the record was last modified.
        /// </summary>
        [JsonPropertyName("modified_on")]
        public DateTimeOffset ModifiedOn { get; init; }
    }

    /// <summary>
    /// Extra Cloudflare-specific information about a record.
    /// </summary>
    public readonly struct RecordMeta
    {
        /// <summary>
        /// True if Cloudflare automatically added this DNS record during initial setup.
        /// </summary>
        [JsonPropertyName("auto_added")]
        public bool AutoAdded { get; init; }

        /// <summary>
        /// True if the record is managed by Cloudflare Apps.
        /// </summary>
        [JsonPropertyName("managed_by_apps")]
        public bool ManagedByApps { get; init; }

        /// <summary>
        /// True if the record is managed by Argo Tunnel.
        /// </summary>
        [JsonPropertyName("managed_by_argo_tunnel")]
        public bool ManagedByArgoTunnel { get; init; }

        /// <summary>
        /// Where the record originated from.
        /// </summary>
        [JsonPropertyName("source")]
        public string? Source { get; init; }
    }
}
