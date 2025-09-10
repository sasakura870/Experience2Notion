using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Spotifies;
public class SpotifySearchResponse
{
    [JsonPropertyName("albums")]
    public SpotifyAlbumContainer Albums { get; set; } = new();
}

public class SpotifyAlbumContainer
{
    [JsonPropertyName("items")]
    public List<Album> Items { get; set; } = [];
}
