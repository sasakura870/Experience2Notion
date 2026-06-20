using System.Text.Json.Serialization;

namespace Experience2Notion.Models.Braves;

public class BraveSearchResponse
{
    [JsonPropertyName("web")]
    public BraveWebSearch? Web { get; set; }
}

public class BraveWebSearch
{
    [JsonPropertyName("results")]
    public List<BraveSearchResult> Results { get; set; } = [];
}

public class BraveSearchResult
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
