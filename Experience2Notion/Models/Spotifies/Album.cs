using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Spotifies;
public class Album
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("images")]
    public List<Image> Images { get; set; } = [];

    [JsonPropertyName("artists")]
    public List<Artist> Artists { get; set; } = [];

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; } = string.Empty;

    public string ExternalUrl => $"https://open.spotify.com/album/{Id}";
}