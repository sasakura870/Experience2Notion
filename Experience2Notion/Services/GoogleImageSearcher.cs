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

    public async Task<string?> SearchImageAsync(string query)
    {
        var listRequest = _searcher.Cse.List();
        listRequest.Q = query;
        listRequest.Cx = _cx;
        listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
        listRequest.Num = 1;
        var search = await listRequest.ExecuteAsync();
        return search.Items?.FirstOrDefault()?.Link;
    }
}
