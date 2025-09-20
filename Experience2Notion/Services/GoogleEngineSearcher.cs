using Experience2Notion.Exceptions;
using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;

namespace Experience2Notion.Services;
public class GoogleEngineSearcher
{
    readonly ILogger<GoogleEngineSearcher> _logger;
    readonly CustomSearchAPIService _searcher;
    readonly string _cx;

    public GoogleEngineSearcher(ILogger<GoogleEngineSearcher> logger)
    {
        _logger = logger;
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_CUSTOM_SEARCH_API_KEY");
        _searcher = new CustomSearchAPIService(new BaseClientService.Initializer
        {
            ApiKey = apiKey,
            ApplicationName = "Experience2Notion"
        });
        _cx = Environment.GetEnvironmentVariable("GOOGLE_CUSTOM_SEARCH_ENGINE_ID") ?? throw new ArgumentException("Google Custom Searchの検索エンジンIDが指定されていません。");
    }

    public async Task<(byte[], string)> DownloadImageAsync(string query)
    {
        _logger.LogInformation("画像検索を開始します。クエリ: {Query}", query);
        var listRequest = _searcher.Cse.List();
        listRequest.Q = query;
        listRequest.Cx = _cx;
        listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
        listRequest.Num = 1;
        var search = await listRequest.ExecuteAsync();
        var resultItem = (search.Items?.FirstOrDefault()) ?? throw new Experience2NotionException($"指定されたクエリ「{query}」の画像が見つかりませんでした。");
        var url = resultItem.Link;
        using var client = new HttpClient();
        var byteData = await client.GetByteArrayAsync(url);
        _logger.LogInformation("画像を取得しました。URL: {Url}", url);
        return (byteData, resultItem.Mime);
    }
}
