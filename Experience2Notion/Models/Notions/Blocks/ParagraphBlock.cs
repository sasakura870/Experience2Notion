using Experience2Notion.Models.Notions.Objects;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Blocks;
public class ParagraphBlock : BlockBase
{
    [JsonPropertyName("type")]
    public override string Type { get; set; }

    [JsonPropertyName("paragraph")]
    public ParagraphContent Paragraph { get; set; } = new();

    public ParagraphBlock()
    {
        Type = "paragraph";
    }
}
