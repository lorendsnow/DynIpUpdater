using System.ComponentModel.DataAnnotations;
using DynIpUpdater;

namespace Tests
{
    public class CloudflareClientTests
    {
        [Fact]
        public void UpdateDnsRecordRequest_NameTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                UpdateDnsRecordRequest request =
                    new(
                        "123.123.123.123",
                        new string('a', 658),
                        "A",
                        "023e105f4ecef8ad9ca31a8372d0c353"
                    );
            });
        }

        [Fact]
        public void UpdateDnsRecordRequest_IdTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                UpdateDnsRecordRequest request =
                    new("123.123.123.123", "example.com", "A", new string('a', 33));
            });
        }

        [Fact]
        public void UpdateDnsRecordRequest_RecordTypeInvalid_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                UpdateDnsRecordRequest request =
                    new("123.123.123.123", "example.com", "MX", "12345");
            });
        }
    }
}
