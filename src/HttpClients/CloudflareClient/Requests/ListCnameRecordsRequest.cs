namespace DynIpUpdater
{
    /// <summary>
    /// Represents a request to list "CNAME" records.
    /// </summary>
    public record ListCnameRecordsRequest : IListRecordsRequest
    {
        public required string ZoneId { get; init; }
        public string RecordType => "CNAME";
    }
}
