using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class Icon
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("emoji")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Emoji { get; set; }

    [JsonPropertyName("external")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ExternalFile? External { get; set; }
}

public class ExternalFile
{
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}