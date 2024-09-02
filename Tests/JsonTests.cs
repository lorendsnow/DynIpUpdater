namespace Tests;

public class JsonTests
{
    private static readonly string[] _tags = ["test", "example"];

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
            Tags: _tags,
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
            Tags: _tags,
            TTL: 1800
        );

        string json = JsonSerializer.Serialize(request);

        var expectedJson =
            @"{
            ""content"": ""192.0.2.1"",
            ""name"": ""example.com"",
            ""proxied"": true,
            ""type"": ""A"",
            ""comment"": ""Updated test record"",
            ""id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
            ""tags"": [
                ""test"",
                ""example""
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

    [Fact]
    public void DeserializeRecordResponse_NoComments_ShouldCreateValidRecord()
    {
        string json =
            @"{
                ""id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                ""zone_id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                ""zone_name"": ""example.com"",
                ""name"": ""example.com"",
                ""type"": ""A"",
                ""content"": ""192.0.2.1"",
                ""proxiable"": true,
                ""proxied"": true,
                ""ttl"": 120,
                ""meta"": {
                    ""auto_added"": false,
                    ""managed_by_apps"": false,
                    ""managed_by_argo_tunnel"": false
                },
                ""tags"": [],
                ""created_on"": ""2024-02-14T00:00:00Z"",
                ""modified_on"": ""2024-02-14T00:00:00Z""
            }";

        RecordResponse? response = JsonSerializer.Deserialize<RecordResponse>(json);

        Assert.NotNull(response);
        Assert.Equal("023e105f4ecef8ad9ca31a8372d0c353", response?.Id);
    }

    [Fact]
    public void DeserializeRecordResponse_WithAllFields_ShouldCreateValidRecord()
    {
        string json =
            @"{
                ""id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                ""zone_id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                ""zone_name"": ""example.com"",
                ""name"": ""example.com"",
                ""type"": ""A"",
                ""content"": ""192.0.2.1"",
                ""proxiable"": true,
                ""proxied"": true,
                ""ttl"": 120,
                ""meta"": {
                    ""auto_added"": false,
                    ""managed_by_apps"": false,
                    ""managed_by_argo_tunnel"": false
                },
                ""comment"": ""This is a test comment"",
                ""comment_modified_on"": ""2024-02-14T00:00:00Z"",
                ""tags"": [""test"", ""example""],
                ""tags_modified_on"": ""2024-02-14T00:00:00Z"",
                ""created_on"": ""2024-02-14T00:00:00Z"",
                ""modified_on"": ""2024-02-14T00:00:00Z""
            }";

        RecordResponse? response = JsonSerializer.Deserialize<RecordResponse>(json);

        Assert.NotNull(response);
        Assert.Equal("023e105f4ecef8ad9ca31a8372d0c353", response?.Id);
        Assert.Equal("This is a test comment", response?.Comment);
        Assert.Equal(_tags, response?.Tags);
        Assert.Equal(new DateTime(2024, 2, 14, 0, 0, 0), response?.CreatedOn.DateTime);
        Assert.Equal(new DateTime(2024, 2, 14, 0, 0, 0), response?.ModifiedOn.DateTime);
    }

    [Fact]
    public void DeserializeSingleRecordResponse_ShouldCreateValidRecord()
    {
        string json =
            @"{
                ""result"": {
                    ""id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                    ""zone_id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                    ""zone_name"": ""example.com"",
                    ""name"": ""example.com"",
                    ""type"": ""A"",
                    ""content"": ""192.0.2.1"",
                    ""proxiable"": true,
                    ""proxied"": true,
                    ""ttl"": 120,
                    ""meta"": {
                        ""auto_added"": false,
                        ""managed_by_apps"": false,
                        ""managed_by_argo_tunnel"": false
                    },
                    ""comment"": ""This is a test comment"",
                    ""comment_modified_on"": ""2024-02-14T00:00:00Z"",
                    ""tags"": [""test"", ""example""],
                    ""tags_modified_on"": ""2024-02-14T00:00:00Z"",
                    ""created_on"": ""2024-02-14T00:00:00Z"",
                    ""modified_on"": ""2024-02-14T00:00:00Z""
                },
                ""success"": true,
                ""errors"": [],
                ""messages"": []
            }";

        SingleRecordResponse? response = JsonSerializer.Deserialize<SingleRecordResponse>(json);

        Assert.NotNull(response);
        Assert.Equal("023e105f4ecef8ad9ca31a8372d0c353", response?.Result?.Id);
        Assert.Equal("This is a test comment", response?.Result?.Comment);
        Assert.Equal(_tags, response?.Result?.Tags);
        Assert.Equal(new DateTime(2024, 2, 14, 0, 0, 0), response?.Result?.CreatedOn.DateTime);
        Assert.Equal(new DateTime(2024, 2, 14, 0, 0, 0), response?.Result?.ModifiedOn.DateTime);
    }

    [Fact]
    public void DeserializeSingleRecordResponse_NullResult_ShouldCreateValidRecord()
    {
        string json =
            @"{
                ""result"": null,
                ""success"": true,
                ""errors"": [],
                ""messages"": []
            }";

        SingleRecordResponse? response = JsonSerializer.Deserialize<SingleRecordResponse>(json);

        Assert.NotNull(response);
        Assert.Null(response?.Result);
    }

    [Fact]
    public void DeserializeListRecordsResponse_ShouldCreateValidRecord()
    {
        string json =
            @"{
                ""result"": [
                    {
                        ""id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                        ""zone_id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                        ""zone_name"": ""example.com"",
                        ""name"": ""example.com"",
                        ""type"": ""A"",
                        ""content"": ""192.0.2.1"",
                        ""proxiable"": true,
                        ""proxied"": true,
                        ""ttl"": 120,
                        ""meta"": {
                            ""auto_added"": false,
                            ""managed_by_apps"": false,
                            ""managed_by_argo_tunnel"": false
                        },
                        ""comment"": ""This is a test comment"",
                        ""comment_modified_on"": ""2024-02-14T00:00:00Z"",
                        ""tags"": [""test"", ""example""],
                        ""tags_modified_on"": ""2024-02-14T00:00:00Z"",
                        ""created_on"": ""2024-02-14T00:00:00Z"",
                        ""modified_on"": ""2024-02-14T00:00:00Z""
                    }, 
                    {
                        ""id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                        ""zone_id"": ""023e105f4ecef8ad9ca31a8372d0c353"",
                        ""zone_name"": ""example.com"",
                        ""name"": ""test"",
                        ""type"": ""A"",
                        ""content"": ""192.0.2.1"",
                        ""proxiable"": true,
                        ""proxied"": true,
                        ""ttl"": 120,
                        ""meta"": {
                            ""auto_added"": false,
                            ""managed_by_apps"": false,
                            ""managed_by_argo_tunnel"": false
                        },
                        ""comment"": ""This is a test comment"",
                        ""comment_modified_on"": ""2024-02-14T00:00:00Z"",
                        ""tags"": [""test"", ""example""],
                        ""tags_modified_on"": ""2024-02-14T00:00:00Z"",
                        ""created_on"": ""2024-02-14T00:00:00Z"",
                        ""modified_on"": ""2024-02-14T00:00:00Z""
                    }
                ],
                ""success"": true,
                ""errors"": [],
                ""messages"": [],
                ""result_info"": {
                    ""page"": 1,
                    ""per_page"": 20,
                    ""count"": 2,
                    ""total_count"": 2,
                    ""total_pages"": 1
                }
            }";

        ListRecordsResponse? response = JsonSerializer.Deserialize<ListRecordsResponse>(json);

        Assert.NotNull(response);
        Assert.Equal(1, response?.ResultInfo.Page);
        Assert.Equal(20, response?.ResultInfo.PerPage);
        Assert.Equal(2, response?.ResultInfo.Count);
        Assert.Equal(2, response?.ResultInfo.TotalCount);
        Assert.Equal(1, response?.ResultInfo.TotalPages);
    }
}
