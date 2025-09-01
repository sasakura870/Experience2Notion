using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class TextObject
{
    [JsonPropertyName("text")]
    public TextContent Text { get; set; } = new TextContent();
}
