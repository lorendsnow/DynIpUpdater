namespace DynIpUpdater
{
    /// <summary>
    /// Represents a message returned in a response from the Cloudflare API.
    /// </summary>
    public readonly struct ResponseMessage
    {
        [JsonPropertyName("code")]
        public int Code { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; }

        public override string ToString()
        {
            return $"ResponseMessage: " + $"Code: {Code}, " + $"Message: {Message}";
        }
    }
}
