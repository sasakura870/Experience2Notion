using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class SelectOption
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = "default";

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}