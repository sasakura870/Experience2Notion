using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class UrlProperty : PropertyBase
{
    [JsonPropertyName("url")]
    public object Url { get; set; } = new object();

    public UrlProperty()
    {
        Type = "url";
    }
}
