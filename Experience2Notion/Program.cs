using Google.Apis.Books.v1;
using static Google.Apis.Requests.BatchRequest;

var service = new BooksService();
var request = service.Volumes.List($"isbn:4487817331");
var response = await request.ExecuteAsync();
if (response.Items is null || response.Items.Count == 0)
{
    Console.WriteLine("指定された本が見つかりませんでした。");
    return;
}

var book = response.Items[0].VolumeInfo;

Console.WriteLine($"タイトル: {book.Title}");
Console.WriteLine($"著者: {string.Join(", ", book.Authors ?? new string[] { })}");
Console.WriteLine($"出版社: {book.Publisher}");
Console.WriteLine($"出版日: {book.PublishedDate}");
Console.WriteLine($"ジャンル: {string.Join(", ", book.Categories ?? new string[] { })}");
Console.WriteLine($"説明: {book.Description}");
Console.WriteLine($"サムネイル: {book.ImageLinks?.Thumbnail}");
