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
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    /// <summary>
    /// Brave Search API でウェブ検索を行う。
    /// </summary>
    /// <param name="query">検索クエリ。</param>
    /// <param name="offset">取得開始位置 (ページング用)。0 始まり。</param>
    /// <param name="count">取得件数。</param>
    public async Task<IReadOnlyList<BraveSearchResult>> SearchAsync(string query, int offset = 0, int count = 10)
    {
        _logger.LogInformation("Brave Searchで検索します。クエリ: {Query}, オフセット: {Offset}, 件数: {Count}", query, offset, count);

        var qs = HttpUtility.ParseQueryString(string.Empty);
        qs["q"] = query;
        qs["country"] = "JP";
        // Brave Search API の日本語の言語コードは ISO 639-1 の "ja" ではなく "jp"
        qs["search_lang"] = "jp";
        qs["ui_lang"] = "ja-JP";
        qs["count"] = count.ToString();
        qs["offset"] = offset.ToString();

        var url = $"https://api.search.brave.com/res/v1/web/search?{qs}";

        var response = await _client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Brave Searchの呼び出しに失敗しました。ステータスコード: {StatusCode}, レスポンス: {Body}", (int)response.StatusCode, json);
            throw new Experience2NotionException($"Brave Searchの呼び出しに失敗しました。ステータスコード: {(int)response.StatusCode}");
        }

        var result = JsonSerializer.Deserialize<BraveSearchResponse>(json);

        var items = result?.Web?.Results;
        if (items is null || items.Count == 0)
        {
            throw new Experience2NotionException($"指定されたクエリ『{query}』の検索結果が見つかりませんでした。");
        }

        _logger.LogInformation("検索結果を取得しました。件数: {ResultCount}", items.Count);
        return items;
    }
}
