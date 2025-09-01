using Experience2Notion.Models.Notions.Objects;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class DatabaseProperty : PropertyBase
{
    [JsonPropertyName("select")]
    public SelectProperty? Select { get; set; }

    [JsonPropertyName("multi_select")]
    public MultiSelectProperty? MultiSelect { get; set; }

    [JsonPropertyName("status")]
    public StatusValue? Status { get; set; }

    [JsonPropertyName("rich_text")]
    public object? RichText { get; set; }

    [JsonPropertyName("date")]
    public object? Date { get; set; }

    [JsonPropertyName("url")]
    public object? Url { get; set; }

    [JsonPropertyName("title")]
    public object? Title { get; set; }
}
