using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class DateValue
{
    [JsonPropertyName("start")]
    public string Start { get; set; } = string.Empty;
}