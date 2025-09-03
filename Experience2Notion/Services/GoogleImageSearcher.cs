using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.Services;

namespace Experience2Notion.Services;
public class GoogleImageSearcher
{
    readonly CustomSearchAPIService _searcher;
    readonly string _cx;

    public GoogleImageSearcher()
    {
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
        Console.WriteLine($"画像検索を開始します。 : {query}");
        var listRequest = _searcher.Cse.List();
        listRequest.Q = query;
        listRequest.Cx = _cx;
        listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
        listRequest.Num = 1;
        var search = await listRequest.ExecuteAsync();
        var resultItem = search.Items?.FirstOrDefault();
        if (resultItem is null)
        {
            throw new Exception($"指定されたクエリ「{query}」の画像が見つかりませんでした。");
        }
        var url = resultItem.Link;
        using var client = new HttpClient();
        var byteData = await client.GetByteArrayAsync(url);
        Console.WriteLine("画像を取得しました。");
        return (byteData, resultItem.Mime);
    }
}
