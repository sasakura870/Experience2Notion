using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class User
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}