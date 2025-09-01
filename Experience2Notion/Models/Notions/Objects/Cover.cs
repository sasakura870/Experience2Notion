using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class Cover
{
    [JsonPropertyName("external")]
    public External External { get; set; } = new External();
}