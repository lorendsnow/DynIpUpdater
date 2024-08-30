namespace DynIpUpdater
{
    public record CreateDnsRecordResponse
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
        public required ResponseError[] Messages { get; init; }
    }
}
