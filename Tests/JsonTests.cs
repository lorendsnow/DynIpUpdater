namespace Tests;

public class JsonTests
{
    [Fact]
    public void DeserializeResponseMessage_ShouldCreateValidStruct()
    {
        string json =
            @"{
            ""code"": 1003,
            ""message"": ""Invalid or missing API key.""
        }";

        ResponseMessage responseMessage = JsonSerializer.Deserialize<ResponseMessage>(json);

        Assert.Equal(1003, responseMessage.Code);
        Assert.Equal("Invalid or missing API key.", responseMessage.Message);
    }

    [Fact]
    public void ResponseMessage_ToString_ShouldReturnExpectedString()
    {
        ResponseMessage responseMessage =
            new() { Code = 1003, Message = "Invalid or missing API key." };

        string result = responseMessage.ToString();

        Assert.Equal("ResponseMessage: Code: 1003, Message: Invalid or missing API key.", result);
    }

    [Fact]
    public void SerializeCreateDnsRecordRequest_ShouldCreateValidJson()
    {
        var request = new CreateDnsRecordRequest(
            Address: "192.0.2.1",
            Name: "example.com",
            Proxied: true,
            Comment: "Test record",
            Tags: new[] { "test", "example" },
            TTL: 3600
        );

        string json = JsonSerializer.Serialize(request);

        var expectedJson =
            @"{
                ""content"": ""192.0.2.1"",
                ""name"": ""example.com"",
                ""proxied"": true,
                ""type"": ""A"",
                ""comment"": ""Test record"",
                ""id"": ""<dynamic>"",
                ""tags"": [
                    ""test"",
                    ""example""
                ],
                ""ttl"": 3600
            }";

        // Remove whitespace for comparison
        var normalizedJson = json.Replace("\r", "").Replace("\n", "").Replace(" ", "");
        var normalizedExpectedJson = expectedJson
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "");

        // Replace the dynamic ID with a placeholder for comparison
        normalizedJson = normalizedJson.Replace(request.Id, "<dynamic>");

        Assert.Equal(normalizedExpectedJson, normalizedJson);
    }

    [Fact]
    public void SerializeUpdateDnsRecordRequest_ShouldCreateValidJson()
    {
        var request = new UpdateDnsRecordRequest(
            Address: "192.0.2.1",
            Name: "example.com",
            Proxied: true,
            Comment: "Updated test record",
            Id: "023e105f4ecef8ad9ca31a8372d0c353",
            Tags: new[] { "updated", "test" },
            TTL: 1800
        );

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        string json = JsonSerializer.Serialize(request, options);

        var expectedJson =
            @"{
            ""content"": ""192.0.2.1"",
            ""name"": ""example.com"",
            ""proxied"": true,
            ""type"": ""A"",
            ""comment"": ""Updated test record"",
            ""id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
            ""tags"": [
                ""updated"",
                ""test""
            ],
            ""ttl"": 1800
        }";

        // Remove whitespace for comparison
        var normalizedJson = json.Replace("\r", "").Replace("\n", "").Replace(" ", "");
        var normalizedExpectedJson = expectedJson
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "");

        Assert.Equal(normalizedExpectedJson, normalizedJson);
    }
}
