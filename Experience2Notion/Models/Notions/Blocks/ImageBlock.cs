using Experience2Notion.Models.Notions.Objects;
using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Blocks;
public class ImageBlock : BlockBase
{
    [JsonPropertyName("type")]
    public override string Type { get; set; }

    [JsonPropertyName("image")]
    public ImageContent Image { get; set; } = new();

    public ImageBlock()
    {
        Type = "image";
    }
}
