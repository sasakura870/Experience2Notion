using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class GroupOption
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = "default";

    [JsonPropertyName("option_ids")]
    public string[] OptionIds { get; set; } = [];
}
