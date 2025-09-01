using Experience2Notion.Models.Notions.Objects;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class TitleProperty
{
    [JsonPropertyName("title")]
    public TextObject[] Title { get; set; } = [];
}