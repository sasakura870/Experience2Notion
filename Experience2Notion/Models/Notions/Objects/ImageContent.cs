using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Notions.Objects;
public class ImageContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "file_upload";

    [JsonPropertyName("file_upload")]
    public FileUploadContent FileUpload { get; set; } = new();
}
