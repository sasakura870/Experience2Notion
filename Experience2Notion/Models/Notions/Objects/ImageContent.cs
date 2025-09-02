using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class ImageContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "external";

    [JsonPropertyName("external")]
    public External External { get; set; } = new();
}