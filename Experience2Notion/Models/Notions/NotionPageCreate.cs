using Experience2Notion.Models.Notions.Blocks;
using Experience2Notion.Models.Notions.Properties;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions;
public class NotionPageCreate : NotionPageBase
{

    [JsonPropertyName("properties")]
    public PageProperties Properties { get; set; } = new();

    [JsonPropertyName("children")]
    public List<BlockBase> Children { get; set; } = [];
}
