using System.Text.Json.Serialization;

namespace Experience2Notion.Models;
public class NotionPage
{
    [JsonPropertyName("parent")]
    public Parent Parent { get; set; } = new Parent();

    [JsonPropertyName("icon")]
    public Emoji Icon { get; set; } = new Emoji();

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; } = new Properties();

    [JsonPropertyName("cover")]
    public Cover Cover { get; set; } = new Cover();
}

public class Parent
{
    [JsonPropertyName("database_id")]
    public string DatabaseId { get; set; } = string.Empty;
}

public class Emoji
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "emoji";

    [JsonPropertyName("emoji")]
    public string EmojiChar { get; set; } = string.Empty;
}

public class Properties
{
    [JsonPropertyName("タイトル")]
    public NameProperty Name { get; set; } = new NameProperty();

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

public class NameProperty
{
    [JsonPropertyName("title")]
    public TextObject[] Title { get; set; } = [];
}

public class StatusProperty
{
    [JsonPropertyName("status")]
    public StatusValue Status { get; set; } = new StatusValue();
}

public class RichTextProperty
{
    [JsonPropertyName("rich_text")]
    public TextObject[] RichText { get; set; } = [];
}

public class DateProperty
{
    [JsonPropertyName("date")]
    public DateValue Date { get; set; } = new DateValue();
}

public class MultiSelectProperty
{
    [JsonPropertyName("multi_select")]
    public SelectOption[] MultiSelect { get; set; } = [];
}

public class TextObject
{
    [JsonPropertyName("text")]
    public TextContent Text { get; set; } = new TextContent();
}

public class TextContent
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

public class DateValue
{
    [JsonPropertyName("start")]
    public string Start { get; set; } = string.Empty;
}

public class StatusValue
{
    [JsonPropertyName("color")]
    public string Color { get; set; } = "red";

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class SelectOption
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class Cover
{
    [JsonPropertyName("external")]
    public External External { get; set; } = new External();
}

public class External
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}