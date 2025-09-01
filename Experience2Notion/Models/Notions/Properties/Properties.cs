using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class Properties
{
    [JsonPropertyName("タイトル")]
    public TitleProperty Title { get; set; } = new TitleProperty();

    [JsonPropertyName("著者/アーティスト")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextProperty? Authors { get; set; }

    [JsonPropertyName("ステータス")]
    public StatusProperty Status { get; set; } = new StatusProperty();

    [JsonPropertyName("ジャンル")]
    public MultiSelectProperty Genre { get; set; } = new MultiSelectProperty();

    [JsonPropertyName("発売日")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateProperty? PublishedDate { get; set; }
}