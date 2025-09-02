using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class FileUploadContent
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}