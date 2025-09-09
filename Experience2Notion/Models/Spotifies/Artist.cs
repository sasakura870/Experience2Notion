using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Spotifies;
public class Artist
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
