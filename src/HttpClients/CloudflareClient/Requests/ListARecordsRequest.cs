namespace DynIpUpdater
{
    /// <summary>
    /// Represents a request to list "A" records.
    /// </summary>
    public record ListARecordsRequest : IListRecordsRequest
    {
        public required string ZoneId { get; init; }
        public string RecordType => "A";
    }
}
