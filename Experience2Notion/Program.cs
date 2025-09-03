using Google.Apis.Books.v1;
using Experience2Notion.Services;

var service = new BooksService();
var request = service.Volumes.List($"isbn:{args[0]}");
var response = await request.ExecuteAsync();
if (response.Items is null || response.Items.Count == 0)
{
    Console.WriteLine("指定された本が見つかりませんでした。");
    return;
}

var book = response.Items[0].VolumeInfo;

Console.WriteLine($"タイトル: {book.Title}");
Console.WriteLine($"著者: {string.Join(", ", book.Authors)}");
Console.WriteLine($"出版日: {book.PublishedDate}");
Console.WriteLine($"説明: {book.Description}");
Console.WriteLine($"サムネイル: {book.ImageLinks?.Thumbnail}");

var searcher = new GoogleImageSearcher();
var (imageData, mime) = await searcher.DownloadImageAsync($"{book.Title} {string.Join(' ', book.Authors)}");

var notionClient = new NotionClient();
var imageId = await notionClient.UploadImageAsync($"{book.Title}.jpg", imageData, mime);
await notionClient.CreateBookPageAsync(book.Title, book.Authors, book.CanonicalVolumeLink, book.PublishedDate, imageId);