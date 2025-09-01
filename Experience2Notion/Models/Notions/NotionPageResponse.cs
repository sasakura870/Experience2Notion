using Experience2Notion.Models.Notions.Properties;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions;

public class NotionPageResponse : NotionPageBase
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("created_time")]
    public DateTime CreatedTime { get; set; }

    [JsonPropertyName("last_edited_time")]
    public DateTime LastEditedTime { get; set; }

    [JsonPropertyName("created_by")]
    public User? CreatedBy { get; set; }

    [JsonPropertyName("last_edited_by")]
    public User? LastEditedBy { get; set; }

    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    [JsonPropertyName("in_trash")]
    public bool InTrash { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("public_url")]
    public string? PublicUrl { get; set; }

    [JsonPropertyName("request_id")]
    public string? RequestId { get; set; }
}