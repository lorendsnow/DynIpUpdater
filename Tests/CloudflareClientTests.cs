﻿namespace Tests
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
                        true,
                        null,
                        "023e105f4ecef8ad9ca31a8372d0c353",
                        [],
                        1
                    );
            });
        }

        [Fact]
        public void UpdateDnsRecordRequest_IdTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                UpdateDnsRecordRequest request =
                    new("123.123.123.123", "example.com", true, null, new string('a', 33), [], 1);
            });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10154675)]
        public void UpdateDnsRecordRequest_TTLOutOfRange_ThrowsValidationException(int ttl)
        {
            Assert.Throws<ValidationException>(() =>
            {
                UpdateDnsRecordRequest request =
                    new("123.123.123.123", "example.com", true, null, "12345", [], ttl);
            });
        }

        [Fact]
        public void CreateDnsRecordRequest_NameTooLong_ThrowsValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                CreateDnsRecordRequest request =
                    new("123.123.123.123", new string('a', 658), true, null, [], 1);
            });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10154675)]
        public void CreateDnsRecordRequest_TTLOutOfRange_ThrowsValidationException(int ttl)
        {
            Assert.Throws<ValidationException>(() =>
            {
                CreateDnsRecordRequest request =
                    new("123.123.123.123", "example.com", true, null, [], ttl);
            });
        }
    }
}
