using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions;
public class CreatePageResponse
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
