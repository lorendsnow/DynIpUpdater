namespace Tests
{
    public class ModelTests
    {
        [Fact]
        public void InitializeRecord_NameTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                DnsRecord record =
                    new()
                    {
                        Address = "123.123.123.123",
                        Name = new string('a', 658),
                        Proxied = true,
                        RecordType = "A",
                        Id = "023e105f4ecef8ad9ca31a8372d0c353",
                    };
            });
        }

        [Fact]
        public void InitiateRecord_IdTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                DnsRecord record =
                    new()
                    {
                        Address = "123.123.123.123",
                        Name = "example.com",
                        Proxied = true,
                        RecordType = "A",
                        Id = new string('a', 33),
                    };
            });
        }

        [Fact]
        public void InitiateDnsRecord_RecordTypeInvalid_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                DnsRecord record =
                    new()
                    {
                        Address = "123.123.123.123",
                        Name = "example.com",
                        Proxied = true,
                        RecordType = "MX",
                        Id = "12345",
                    };
            });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10154675)]
        public void InitiateRecord_TTLOutOfRange_ThrowsValidationException(int ttl)
        {
            Assert.Throws<ValidationException>(() =>
            {
                DnsRecord record =
                    new()
                    {
                        Address = "123.123.123.123",
                        Name = "example.com",
                        Proxied = true,
                        RecordType = "A",
                        TTL = ttl
                    };
            });
        }
    }
}
