using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class DateProperty
{
    [JsonPropertyName("date")]
    public DateValue Date { get; set; } = new DateValue();
}