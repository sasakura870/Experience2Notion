using Experience2Notion.Models.Notions.Objects;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Properties;
public class DateProperty : PropertyBase
{
    [JsonPropertyName("date")]
    public DateValue Date { get; set; } = new DateValue();
}