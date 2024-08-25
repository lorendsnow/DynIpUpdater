namespace DynIpUpdater
{
    public record CreateDnsRecordResponse
    {
        public RecordResponse? Result { get; init; }
        public required ResponseError[] Errors { get; init; }
        public required ResponseError[] Messages { get; init; }
        public bool Success { get; init; }
    }
}
