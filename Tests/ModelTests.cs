using System.ComponentModel.DataAnnotations;
using DynIpUpdater;

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
                    new(
                        "123.123.123.123",
                        new string('a', 658),
                        true,
                        "A",
                        null,
                        "023e105f4ecef8ad9ca31a8372d0c353",
                        [],
                        1
                    );
            });
        }

        [Fact]
        public void InitiateRecord_IdTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                DnsRecord record =
                    new(
                        "123.123.123.123",
                        "example.com",
                        true,
                        "A",
                        null,
                        new string('a', 33),
                        [],
                        1
                    );
            });
        }

        [Fact]
        public void InitiateDnsRecord_RecordTypeInvalid_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                DnsRecord record =
                    new("123.123.123.123", "example.com", true, "MX", null, "12345", [], 1);
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
                    new("123.123.123.123", "example.com", true, "A", null, "12345", [], ttl);
            });
        }
    }
}
