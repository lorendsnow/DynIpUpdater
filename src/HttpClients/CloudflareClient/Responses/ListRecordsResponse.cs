namespace DynIpUpdater
{
    /// <summary>
    /// Represents the Cloudflare response to a request to list "A" records.
    /// </summary>
    public record ListRecordsResponse
    {
        /// <summary>
        /// The records returned by the request.
        /// </summary>
        [JsonPropertyName("result")]
        public RecordResponse[]? Result { get; init; }

        /// <summary>
        /// Whether the API call was successful
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; init; }

        /// <summary>
        /// A list of errors returned by the request.
        /// </summary>
        [JsonPropertyName("errors")]
        public required ResponseError[] Errors { get; init; }

        /// <summary>
        /// A list of messages returned by the request.
        /// </summary>
        [JsonPropertyName("messages")]
        public ResponseMessage[]? Messages { get; init; }

        /// <summary>
        /// Additional information about the result.
        /// </summary>
        [JsonPropertyName("result_info")]
        public ResultInfo ResultInfo { get; init; }
    }

    /// <summary>
    /// Contains information about the result of a Cloudflare API call.
    /// </summary>
    public readonly struct ResultInfo
    {
        /// <summary>
        /// Current page within paginated list of results.
        /// </summary>
        [JsonPropertyName("page")]
        public int Page { get; init; }

        /// <summary>
        /// Number of results per page of results.
        /// </summary>
        [JsonPropertyName("per_page")]
        public int PerPage { get; init; }

        /// <summary>
        /// Total number of results for the requested service.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; init; }

        /// <summary>
        /// Total results available without any search parameters.
        /// </summary>
        [JsonPropertyName("total_count")]
        public int TotalCount { get; init; }

        /// <summary>
        /// Total pages available for the requested service.
        /// </summary>
        [JsonPropertyName("total_pages")]
        public int TotalPages { get; init; }
    }
}
