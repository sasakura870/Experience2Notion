using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class Cover
{
    [JsonPropertyName("external")]
    public External External { get; set; } = new External();
}