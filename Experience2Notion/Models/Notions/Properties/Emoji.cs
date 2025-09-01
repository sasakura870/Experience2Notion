using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class Emoji
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "emoji";

    [JsonPropertyName("emoji")]
    public string EmojiChar { get; set; } = string.Empty;
}