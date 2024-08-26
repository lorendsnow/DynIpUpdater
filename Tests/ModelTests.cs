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
                        Id = new string('a', 33),
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
                        TTL = ttl
                    };
            });
        }
    }
}
