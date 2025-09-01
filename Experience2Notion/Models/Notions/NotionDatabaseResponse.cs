using Experience2Notion.Models.Notions.Properties;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions;

public class NotionDatabaseResponse
{
    [JsonPropertyName("properties")]
    public Dictionary<string, DatabaseProperty> Properties { get; set; } = new();
}