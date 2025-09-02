using Experience2Notion.Models.Notions.Objects;
using Experience2Notion.Models.Notions.Properties;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions;
public class NotionPageBase
{
    [JsonPropertyName("parent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Parent? Parent { get; set; }

    [JsonPropertyName("icon")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Icon? Icon { get; set; }
}