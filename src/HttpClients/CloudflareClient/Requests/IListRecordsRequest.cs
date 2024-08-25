namespace DynIpUpdater
{
    /// <summary>
    /// Represents a request to list records.
    /// </summary>
    public interface IListRecordsRequest
    {
        public string ZoneId { get; init; }
        public string RecordType { get; }
    }
}
