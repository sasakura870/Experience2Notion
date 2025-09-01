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

//public class SelectOptions
//{
//    [JsonPropertyName("options")]
//    public List<SelectOption> Options { get; set; } = new();
//}

//public class StatusOptions
//{
//    [JsonPropertyName("options")]
//    public List<SelectOption> Options { get; set; } = new();

//    [JsonPropertyName("groups")]
//    public List<StatusGroup> Groups { get; set; } = new();
//}

//public class StatusGroup
//{
//    [JsonPropertyName("id")]
//    public string Id { get; set; } = string.Empty;

//    [JsonPropertyName("name")]
//    public string Name { get; set; } = string.Empty;

//    [JsonPropertyName("color")]
//    public string Color { get; set; } = "default";

//    [JsonPropertyName("option_ids")]
//    public List<string> OptionIds { get; set; } = new();
//}
