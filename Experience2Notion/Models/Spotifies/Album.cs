namespace Experience2Notion.Models.Spotifies;
public class Album
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<Image> Images { get; set; } = [];

    public List<Artist> Artists { get; set; } = [];

    public string ReleaseDate { get; set; } = string.Empty;

    public string ExternalUrl => $"https://open.spotify.com/album/{Id}";
}