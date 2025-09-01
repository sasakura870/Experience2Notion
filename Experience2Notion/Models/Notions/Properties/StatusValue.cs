using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class StatusValue
{
    [JsonPropertyName("color")]
    public string Color { get; set; } = "red";

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

