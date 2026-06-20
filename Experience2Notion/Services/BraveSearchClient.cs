using Experience2Notion.Exceptions;
using Experience2Notion.Models.Braves;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Web;

namespace Experience2Notion.Services;

public class BraveSearchClient
{
    private readonly ILogger<BraveSearchClient> _logger;
    private readonly HttpClient _client = new();

    public BraveSearchClient(ILogger<BraveSearchClient> logger)
    {
        _logger = logger;

        var apiKey = Environment.GetEnvironmentVariable("BRAVE_SEARCH_API_KEY")
            ?? throw new ArgumentException("BRAVE_SEARCH_API_KEY が設定されていません。");

        _client.DefaultRequestHeaders.Add("X-Subscription-Token", apiKey);
    }

    public async Task<IReadOnlyList<BraveSearchResult>> SearchAsync(string query)
    {
        _logger.LogInformation("Brave Searchで検索します。クエリ: {Query}", query);

        var qs = HttpUtility.ParseQueryString(string.Empty);
        qs["q"] = query;
        qs["country"] = "JP";
        qs["search_lang"] = "ja";
        qs["ui_lang"] = "ja-JP";
        qs["count"] = "10";

        var url = $"https://api.search.brave.com/res/v1/web/search?{qs}";

        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<BraveSearchResponse>(json);

        var items = result?.Web?.Results;
        if (items is null || items.Count == 0)
        {
            throw new Experience2NotionException($"指定されたクエリ『{query}』の検索結果が見つかりませんでした。");
        }

        return items;
    }
}
