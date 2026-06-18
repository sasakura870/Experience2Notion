using System.Text.Json.Serialization;

namespace Experience2Notion.Models.MusicAlbums;

public class Album
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("artists")]
    public List<Artist> Artists { get; set; } = [];

    [JsonPropertyName("images")]
    public List<Image> Images { get; set; } = [];

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; } = string.Empty;

    public string ExternalUrl { get; set; } = string.Empty;
}
