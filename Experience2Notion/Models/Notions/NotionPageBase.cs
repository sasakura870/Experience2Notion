using Experience2Notion.Models.Notions.Properties;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions;
public class NotionPageBase
{
    [JsonPropertyName("parent")]
    public Parent Parent { get; set; } = new();

    [JsonPropertyName("icon")]
    public Emoji Icon { get; set; } = new();

    [JsonPropertyName("properties")]
    public Properties.Properties Properties { get; set; } = new();

    [JsonPropertyName("cover")]
    public Cover Cover { get; set; } = new();
}