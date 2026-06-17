using Experience2Notion.Exceptions;
using Google.Apis.Books.v1.Data;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Experience2Notion.Services;

public class BookCoverClient(ILogger<BookCoverClient> logger)
{
    private readonly ILogger<BookCoverClient> _logger = logger;
    private readonly HttpClient _client = new();

    public async Task<(byte[] ImageData, string Mime)> DownloadCoverAsync(string isbn, Volume.VolumeInfoData book)
    {
        var openLibraryCover = await TryDownloadOpenLibraryCoverAsync(isbn);
        if (openLibraryCover is not null)
        {
            return openLibraryCover.Value;
        }

        var googleBooksCover = await TryDownloadGoogleBooksCoverAsync(book);
        if (googleBooksCover is not null)
        {
            return googleBooksCover.Value;
        }

        throw new Experience2NotionException("書籍の表紙画像が見つかりませんでした。");
    }

    private async Task<(byte[] ImageData, string Mime)?> TryDownloadOpenLibraryCoverAsync(string isbn)
    {
        var normalizedIsbn = NormalizeIsbn(isbn);
        var url = $"https://covers.openlibrary.org/b/isbn/{normalizedIsbn}-L.jpg?default=false";
        _logger.LogInformation("Open Library Covers APIから書籍表紙を取得します。ISBN: {Isbn}", normalizedIsbn);

        using var response = await _client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation("Open Library Covers APIに書籍表紙が見つかりませんでした。ISBN: {Isbn}", normalizedIsbn);
            return null;
        }

        response.EnsureSuccessStatusCode();
        var imageData = await response.Content.ReadAsByteArrayAsync();
        var mime = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
        _logger.LogInformation("Open Library Covers APIから書籍表紙を取得しました。ISBN: {Isbn}", normalizedIsbn);
        return (imageData, mime);
    }

    private async Task<(byte[] ImageData, string Mime)?> TryDownloadGoogleBooksCoverAsync(Volume.VolumeInfoData book)
    {
        var url = GetGoogleBooksImageUrl(book.ImageLinks);
        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogInformation("Google Booksに書籍表紙画像URLが見つかりませんでした。タイトル: {Title}", book.Title);
            return null;
        }

        _logger.LogInformation("Google Booksから書籍表紙を取得します。タイトル: {Title}, URL: {Url}", book.Title, url);
        using var response = await _client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation("Google Booksの書籍表紙画像が見つかりませんでした。タイトル: {Title}, URL: {Url}", book.Title, url);
            return null;
        }

        response.EnsureSuccessStatusCode();
        var imageData = await response.Content.ReadAsByteArrayAsync();
        var mime = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
        _logger.LogInformation("Google Booksから書籍表紙を取得しました。タイトル: {Title}", book.Title);
        return (imageData, mime);
    }

    private static string? GetGoogleBooksImageUrl(Volume.VolumeInfoData.ImageLinksData? imageLinks)
    {
        if (imageLinks is null)
        {
            return null;
        }

        return imageLinks.ExtraLarge
            ?? imageLinks.Large
            ?? imageLinks.Medium
            ?? imageLinks.Small
            ?? imageLinks.Thumbnail
            ?? imageLinks.SmallThumbnail;
    }

    private static string NormalizeIsbn(string isbn)
    {
        return isbn.Replace("-", string.Empty).Replace(" ", string.Empty).Trim();
    }
}
