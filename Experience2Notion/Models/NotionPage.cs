using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Experience2Notion.Models;
public class NotionPage
{
    [JsonPropertyName("parent")]
    public Parent Parent { get; set; } = new Parent();

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; } = new Properties();

    [JsonPropertyName("cover")]
    public Cover Cover { get; set; }
}

public class Parent
{
    [JsonPropertyName("database_id")]
    public string DatabaseId { get; set; } = string.Empty;
}

public class Properties
{
    [JsonPropertyName("タイトル")]
    public TitleProperty Title { get; set; }

    [JsonPropertyName("著者")]
    public RichTextProperty Authors { get; set; }

    [JsonPropertyName("発売日")]
    public DateProperty PublishedDate { get; set; }

    [JsonPropertyName("ジャンル")]
    public MultiSelectProperty Genre { get; set; }
}

public class TitleProperty
{
    [JsonPropertyName("title")]
    public TextObject[] Title { get; set; }
}

public class RichTextProperty
{
    [JsonPropertyName("rich_text")]
    public TextObject[] RichText { get; set; }
}

public class DateProperty
{
    [JsonPropertyName("date")]
    public DateValue Date { get; set; }
}

public class MultiSelectProperty
{
    [JsonPropertyName("multi_select")]
    public SelectOption[] MultiSelect { get; set; }
}

public class TextObject
{
    [JsonPropertyName("text")]
    public TextContent Text { get; set; }
}

public class TextContent
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}

public class DateValue
{
    [JsonPropertyName("start")]
    public string Start { get; set; }
}

public class SelectOption
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class Cover
{
    [JsonProperty("external")]
    public External External { get; set; }
}

public class External
{
    [JsonProperty("url")]
    public string Url { get; set; }
}