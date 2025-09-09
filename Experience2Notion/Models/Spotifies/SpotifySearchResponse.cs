namespace Experience2Notion.Models.Spotifies;
public class SpotifySearchResponse
{
    public SpotifyAlbumContainer Albums { get; set; } = new();
}

public class SpotifyAlbumContainer
{
    public List<Album> Items { get; set; } = [];
}
