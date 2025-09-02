using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions;
public class NotionFileUploadResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}
