using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class External
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}