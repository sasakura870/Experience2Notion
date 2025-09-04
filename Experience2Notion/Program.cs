using Experience2Notion.Services;

var service = new GoogleBookSeacher();
var book = await service.SearchByIsbnAsync(args[0]);

Console.WriteLine($"タイトル: {book.Title}");
Console.WriteLine($"著者: {string.Join(", ", book.Authors)}");
Console.WriteLine($"出版日: {book.PublishedDate}");

var searcher = new GoogleImageSearcher();
var (imageData, mime) = await searcher.DownloadImageAsync($"{book.Title} {string.Join(' ', book.Authors)}");

var notionClient = new NotionClient();
var imageId = await notionClient.UploadImageAsync($"{book.Title}.jpg", imageData, mime);
await notionClient.CreateBookPageAsync(book.Title, book.Authors, book.CanonicalVolumeLink, book.PublishedDate, imageId);