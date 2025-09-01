using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class StatusValue
{
    [JsonPropertyName("options")]
    public SelectOption[] Options { get; set; } = [];

    [JsonPropertyName("groups")]
    public GroupOption[] Groups { get; set; } = [];
}
