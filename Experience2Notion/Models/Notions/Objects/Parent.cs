using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class Parent
{
    [JsonPropertyName("database_id")]
    public string DatabaseId { get; set; } = string.Empty;
}