using System.Text.Json.Serialization;

namespace Experience2Notion.Models.MusicAlbums;

public class Artist
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
