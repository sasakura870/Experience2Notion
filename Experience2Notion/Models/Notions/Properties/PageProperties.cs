using Experience2Notion.Models.Notions.Objects;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class PageProperties
{
    [JsonPropertyName("タイトル")]
    public TitleProperty Title { get; set; } = new();

    [JsonPropertyName("著者/アーティスト")]
    public MultiSelectValueByPage Authors { get; set; } = new();

    [JsonPropertyName("ステータス")]
    public StatusValueByPage Status { get; set; } = new();

    [JsonPropertyName("ジャンル")]
    public SelectValueByPage Genre { get; set; } = new();
}

public class MultiSelectValueByPage
{
    [JsonPropertyName("multi_select")]
    public List<SelectOption> MultiSelect { get; set; } = new();
}

public class SelectValueByPage
{
    [JsonPropertyName("select")]
    public SelectOption Select { get; set; } = new();
}

public class StatusValueByPage
{
    [JsonPropertyName("status")]
    public SelectOption Status { get; set; } = new();
}