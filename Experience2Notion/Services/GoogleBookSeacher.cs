using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;

namespace Experience2Notion.Services;
public class GoogleBookSeacher
{
    readonly BooksService _service = new();

    public async Task<Volume.VolumeInfoData> SearchByIsbnAsync(string isbn)
    {
        var request = _service.Volumes.List($"isbn:{isbn}");
        var response = await request.ExecuteAsync();
        if (response.Items is null || response.Items.Count == 0)
        {
            throw new Exception("指定された本が見つかりませんでした。");
        }
        return response.Items[0].VolumeInfo;
    }
}
