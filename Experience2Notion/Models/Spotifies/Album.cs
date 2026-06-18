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

    [JsonPropertyName("external_urls")]
    public SpotifyExternalUrls ExternalUrls { get; set; } = new();

    public string ExternalUrl { get; set; } = string.Empty;

    public Experience2Notion.Models.MusicAlbums.Album ToAlbum()
    {
        return new Experience2Notion.Models.MusicAlbums.Album
        {
            Id = Id,
            Name = Name,
            Artists = Artists.Select(artist => new Experience2Notion.Models.MusicAlbums.Artist
            {
                Name = artist.Name,
            }).ToList(),
            Images = Images.Select(image => new Experience2Notion.Models.MusicAlbums.Image
            {
                Url = image.Url,
                Height = image.Height,
                Width = image.Width,
            }).ToList(),
            ReleaseDate = ReleaseDate,
            ExternalUrl = string.IsNullOrWhiteSpace(ExternalUrl) ? ExternalUrls.Spotify : ExternalUrl,
        };
    }
}

public class SpotifyExternalUrls
{
    [JsonPropertyName("spotify")]
    public string Spotify { get; set; } = string.Empty;
}
