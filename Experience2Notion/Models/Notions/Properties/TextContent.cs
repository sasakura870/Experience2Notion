using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class TextContent
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}