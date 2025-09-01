using Experience2Notion.Models.Notions.Objects;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class SelectProperty : PropertyBase
{
    [JsonPropertyName("options")]
    public SelectOption[] Options { get; set; } = [];

    public SelectProperty()
    {
        Type = "select";
    }
}
