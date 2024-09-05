namespace DynIpUpdater
{
    /// <summary>
    /// Represents a response from Cloudflare related to a single record (e.g., updating or
    /// creating a record).
    /// </summary>
    public record SingleRecordResponse
    {
        /// <summary>
        /// The result of the record creation request.
        /// </summary>
        [JsonPropertyName("result")]
        public RecordResponse? Result { get; init; }

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
    }
}
