using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class Properties
{
    [JsonPropertyName("タイトル")]
    public TitleProperty Title { get; set; } = new TitleProperty();

    [JsonPropertyName("著者/アーティスト")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MultiSelectProperty Authors { get; set; } = new MultiSelectProperty();

    [JsonPropertyName("シリーズ")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SelectProperty? Series { get; set; }

    [JsonPropertyName("リンク")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public UrlProperty? Link { get; set; }

    [JsonPropertyName("評価")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SelectProperty? Evaluation { get; set; }

    [JsonPropertyName("ステータス")]
    public StatusProperty Status { get; set; } = new StatusProperty();

    [JsonPropertyName("ジャンル")]
    public SelectProperty Genre { get; set; } = new SelectProperty();

    [JsonPropertyName("開始日")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateProperty? StartedDate { get; set; }

    [JsonPropertyName("完了日")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateProperty? FinishedDate { get; set; }

    [JsonPropertyName("発売日")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateProperty? PublishedDate { get; set; }

    [JsonPropertyName("場所")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextProperty? Location { get; set; }

    [JsonPropertyName("AI 要約")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RichTextProperty? AiSummary { get; set; }
}