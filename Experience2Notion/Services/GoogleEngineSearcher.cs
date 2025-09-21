using Experience2Notion.Exceptions;
using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.CustomSearchAPI.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;

namespace Experience2Notion.Services;
public class GoogleEngineSearcher
{
    readonly ILogger<GoogleEngineSearcher> _logger;
    readonly CustomSearchAPIService _searcher;
    readonly string _imageCx;
    readonly string _basicCx;

    public GoogleEngineSearcher(ILogger<GoogleEngineSearcher> logger)
    {
        _logger = logger;
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_CUSTOM_SEARCH_API_KEY");
        _searcher = new CustomSearchAPIService(new BaseClientService.Initializer
        {
            ApiKey = apiKey,
            ApplicationName = "Experience2Notion"
        });
        _imageCx = Environment.GetEnvironmentVariable("GOOGLE_CUSTOM_SEARCH_IMAGE_ENGINE_ID") ?? throw new ArgumentException("Google Custom Searchの画像検索エンジンIDが指定されていません。");
        _basicCx = Environment.GetEnvironmentVariable("GOOGLE_CUSTOM_SEARCH_BASIC_ENGINE_ID") ?? throw new ArgumentException("Google Custom Searchの検索エンジンIDが指定されていません。");
    }

    public async Task<(byte[], string)> DownloadImageAsync(string query)
    {
        _logger.LogInformation("画像検索を開始します。クエリ: {Query}", query);
        var listRequest = _searcher.Cse.List();
        listRequest.Q = query;
        listRequest.Cx = _imageCx;
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

    public async Task<IList<Result>> GetSearchResultAsync(string query, int pageNumber = 1)
    {
        _logger.LogInformation("ウェブ検索を開始します。クエリ: {Query}, ページ: {PageNumber}", query, pageNumber);
        var listRequest = _searcher.Cse.List();
        listRequest.Q = query;
        listRequest.Cx = _basicCx;
        listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.SearchTypeUndefined;
        listRequest.Num = 10;
        listRequest.Start = (pageNumber - 1) * listRequest.Num + 1;
        var search = await listRequest.ExecuteAsync();
        var results = search.Items ?? throw new Experience2NotionException($"指定されたクエリ「{query}」の検索結果が見つかりませんでした。");
        _logger.LogInformation("検索結果を取得しました。");
        return results;
    }
}
