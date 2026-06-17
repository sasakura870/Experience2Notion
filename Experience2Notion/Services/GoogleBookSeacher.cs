using Experience2Notion.Exceptions;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Logging;

namespace Experience2Notion.Services;
public class GoogleBookSeacher
{
    readonly ILogger<GoogleBookSeacher> _logger;
    readonly BooksService _service;

    public GoogleBookSeacher(ILogger<GoogleBookSeacher> logger)
    {
        _logger = logger;
        var apiKey = Environment.GetEnvironmentVariable("GOOGLE_BOOKS_API_KEY") ?? throw new ArgumentException("Google Books APIキーが指定されていません。");
        _service = new BooksService(new BaseClientService.Initializer
        {
            ApiKey = apiKey,
            ApplicationName = "Experience2Notion"
        });
    }

    public async Task<Volume.VolumeInfoData> SearchByIsbnAsync(string isbn)
    {
        var request = _service.Volumes.List($"isbn:{isbn}");
        _logger.LogInformation("本を検索します。ISBN: {Isbn}", isbn);
        var response = await request.ExecuteAsync();
        if (response.Items is null || response.Items.Count == 0)
        {
            throw new Experience2NotionException("指定された本が見つかりませんでした。");
        }
        _logger.LogInformation("本が見つかりました。タイトル: {Title}, 著者: {Authors}",
            response.Items[0].VolumeInfo.Title,
            string.Join(", ", response.Items[0].VolumeInfo.Authors ?? []));
        return response.Items[0].VolumeInfo;
    }
}
