namespace DynIpUpdater
{
    /// <summary>
    /// Represents a DNS record to be created or updated in Cloudflare.
    /// </summary>
    public class DnsRecord
    {
        /// <summary>
        /// A valid IPv4 address.
        /// </summary>
        public string? Address { get; set; }

        private string _name = string.Empty;

        /// <summary>
        /// The name of the DNS record.
        /// </summary>
        public required string Name
        {
            get => _name;
            set => _name = ValidateName(value);
        }

        /// <summary>
        /// Whether the record is proxied through Cloudflare.
        /// </summary>
        public bool Proxied { get; set; }

        private string _recordType = string.Empty;

        /// <summary>
        /// The type of DNS record. Must be one of 'A' or 'CNAME'.
        /// </summary>
        public required string RecordType
        {
            get => _recordType;
            set => _recordType = ValidateRecordType(value);
        }

        /// <summary>
        /// Comments or notes about the DNS record. This field has no effect on DNS responses.
        /// </summary>
        public string? Comment { get; set; }

        private string? _id;

        /// <summary>
        /// The unique identifier for the DNS record.
        /// </summary>
        public string? Id
        {
            get => _id;
            set => _id = ValidateId(value);
        }

        /// <summary>
        /// Custom tags for the DNS record. This field has no effect on DNS responses.
        /// </summary>
        public string[] Tags { get; set; } = [];

        private int _ttl = 1;

        /// <summary>
        /// The TTL (time to live) for the DNS record. Must be 1 for automatic, or between 60 and
        /// 86400 for manual.
        /// </summary>
        public int TTL
        {
            get => _ttl;
            set => _ttl = ValidateTTL(value);
        }

        private static string ValidateName(string value)
        {
            if (value.Length <= 255)
            {
                return value;
            }
            else
            {
                throw new ValidationException($"Property must be less than 255 characters");
            }
        }

        private static string ValidateRecordType(string recordType)
        {
            if (recordType == "A" || recordType == "CNAME")
            {
                return recordType;
            }
            else
            {
                throw new ValidationException("RecordType must be one of 'A' or 'CNAME'");
            }
        }

        private static string? ValidateId(string? id)
        {
            if (id is null || id.Length <= 32)
            {
                return id;
            }
            else
            {
                throw new ValidationException("Id property must be less than 32 characters");
            }
        }

        private static int ValidateTTL(int ttl)
        {
            if (ttl == 1)
            {
                return 1;
            }
            else if (ttl >= 60 && ttl <= 86400)
            {
                return ttl;
            }
            else
            {
                throw new ValidationException(
                    "TTL must be 1 for automatic, or between 60 and 86400 for manual"
                );
            }
        }

        /// <summary>
        /// Returns a string representation of the DNS record.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"DnsRecord "
                + "{ "
                + $"Address: {Address}, "
                + $"Name: {Name}, "
                + $"Proxied: {Proxied}, "
                + $"RecordType: {RecordType}, "
                + $"Comment: {Comment}, "
                + $"Id: {Id}, "
                + $"Tags: [{string.Join(", ", Tags)}] "
                + $"TTL: {TTL} "
                + "}";
        }
    }
}
