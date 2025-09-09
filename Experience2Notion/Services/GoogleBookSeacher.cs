using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Microsoft.Extensions.Logging;

namespace Experience2Notion.Services;
public class GoogleBookSeacher(ILogger<GoogleBookSeacher> logger)
{
    readonly ILogger<GoogleBookSeacher> _logger = logger;
    readonly BooksService _service = new();

    public async Task<Volume.VolumeInfoData> SearchByIsbnAsync(string isbn)
    {
        var request = _service.Volumes.List($"isbn:{isbn}");
        _logger.LogInformation("本を検索します。ISBN: {Isbn}", isbn);
        var response = await request.ExecuteAsync();
        if (response.Items is null || response.Items.Count == 0)
        {
            throw new Exception("指定された本が見つかりませんでした。");
        }
        _logger.LogInformation("本が見つかりました。タイトル: {Title}, 著者: {Authors}",
            response.Items[0].VolumeInfo.Title,
            string.Join(", ", response.Items[0].VolumeInfo.Authors ?? []));
        return response.Items[0].VolumeInfo;
    }
}
