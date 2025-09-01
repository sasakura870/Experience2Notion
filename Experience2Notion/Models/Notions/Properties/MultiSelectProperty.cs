using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class MultiSelectProperty
{
    [JsonPropertyName("multi_select")]
    public SelectOption[] MultiSelect { get; set; } = [];
}