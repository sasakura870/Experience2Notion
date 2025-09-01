using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class RichTextProperty
{
    [JsonPropertyName("rich_text")]
    public TextObject[] RichText { get; set; } = [];
}

