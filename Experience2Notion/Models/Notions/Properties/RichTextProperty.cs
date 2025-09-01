using Experience2Notion.Models.Notions.Objects;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class RichTextProperty : PropertyBase
{
    [JsonPropertyName("rich_text")]
    public TextObject[] RichText { get; set; } = [];

    public RichTextProperty()
    {
        Type = "rich_text";
    }
}

