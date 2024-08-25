namespace DynIpUpdater
{
    /// <summary>
    /// Represents the Cloudflare response to a request to list "A" or "CNAME" records.
    /// </summary>
    public record ListRecordsResponse
    {
        /// <summary>
        /// The records returned by the request.
        /// </summary>
        public required RecordResponse[]? Result { get; init; }

        /// <summary>
        /// A list of errors returned by the request.
        /// </summary>
        public required ResponseError[] Errors { get; init; }

        /// <summary>
        /// A list of messages returned by the request.
        /// </summary>
        public required ResponseError[] Messages { get; init; }

        /// <summary>
        /// Whether the API call was successful
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Additional information about the result.
        /// </summary>
        public ResultInfo ResultInfo { get; init; }
    }

    /// <summary>
    /// Represents an error and/or message returned by the Cloudflare API.
    /// </summary>
    public readonly struct ResponseError
    {
        public int Code { get; init; }
        public string Message { get; init; }
    }

    /// <summary>
    /// Contains information about the result of a Cloudflare API call.
    /// </summary>
    public readonly struct ResultInfo
    {
        /// <summary>
        /// Total number of results for the requested service.
        /// </summary>
        public int Count { get; init; }

        /// <summary>
        /// Current page within paginated list of results.
        /// </summary>
        public int Page { get; init; }

        /// <summary>
        /// Number of results per page of results.
        /// </summary>
        public int PerPage { get; init; }

        /// <summary>
        /// Total results available without any search parameters.
        /// </summary>
        public int TotalCount { get; init; }
    }
}
