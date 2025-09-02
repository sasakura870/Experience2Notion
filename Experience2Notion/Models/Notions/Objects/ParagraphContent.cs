using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class ParagraphContent
{
    [JsonPropertyName("rich_text")]
    public List<TextObject> RichText { get; set; } = [];
}
