namespace DynIpUpdater
{
    /// <summary>
    /// Represents an error returned in a response from the Cloudflare API.
    /// </summary>
    public readonly struct ResponseError
    {
        [JsonPropertyName("code")]
        public int Code { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; }

        public override string ToString()
        {
            return $"ResponseError: " + $"Code: {Code}, " + $"Message: {Message}";
        }
    }
}
