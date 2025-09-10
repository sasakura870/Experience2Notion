using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Spotifies;
public class Image
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }
}