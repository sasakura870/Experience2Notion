using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class SelectOption
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}