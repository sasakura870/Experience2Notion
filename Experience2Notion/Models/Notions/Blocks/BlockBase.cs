using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Blocks;
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ParagraphBlock), "paragraph")]
[JsonDerivedType(typeof(ImageBlock), "image")]
public abstract class BlockBase
{
    [JsonPropertyName("object")]
    public string Object { get; set; } = "block";

    [JsonPropertyName("type")]
    public abstract string Type { get; set; }
}
