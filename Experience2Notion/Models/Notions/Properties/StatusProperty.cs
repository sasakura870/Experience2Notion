using Experience2Notion.Models.Notions.Objects;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class StatusProperty
{
    [JsonPropertyName("status")]
    public StatusValue Status { get; set; } = new StatusValue();
}