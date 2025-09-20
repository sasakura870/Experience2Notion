using Experience2Notion.Configs;
using Experience2Notion.Models.Notions;
using Experience2Notion.Models.Notions.Blocks;
using Experience2Notion.Models.Notions.Objects;
using Experience2Notion.Models.Notions.Properties;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Experience2Notion.Services;
public partial class NotionClient
{
    private ILogger<NotionClient> _logger;
    readonly HttpClient _client;
    readonly string _databaseId;

    readonly string _getDbSchmeUrl;
    readonly string _createPagesUrl = "https://api.notion.com/v1/pages";

    List<SelectOption> _authors = [];
    SelectOption _notStartStatus = new();
    List<SelectOption> _genres = [];

    public NotionClient(ILogger<NotionClient> logger)
    {
        _logger = logger;
        var notionApiKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {notionApiKey}");
        _client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");

        _databaseId = Environment.GetEnvironmentVariable("NOTION_DB_ID") ?? string.Empty;
        _getDbSchmeUrl = $"https://api.notion.com/v1/databases/{Guid.Parse(_databaseId)}";

        LoadProperties();
    }

    public async Task<string> UploadImageAsync(string imageName, byte[] imageData, string mime)
    {
        var fileUploadId = await CreateFileUploadAsync(imageName, mime);

        _logger.LogInformation("Notionに画像をアップロードします。file_upload: {FileUploadId}", fileUploadId);
        using var content = new MultipartFormDataContent();
        using var imageContent = new ByteArrayContent(imageData);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mime);
        content.Add(imageContent, "file", imageName);

        var response = await _client.PostAsync($"https://api.notion.com/v1/file_uploads/{fileUploadId}/send", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<NotionFileUploadResponse>(json);
        _logger.LogInformation("画像をアップロードしました。ID: {ImageId}", result!.Id);
        return result!.Id;
    }

    public async Task<CreatePageResponse> CreateBookPageAsync(string title, IEnumerable<string> authors, string link, string publishedDate, string imageId)
    {
        _logger.LogInformation($"Notionに書籍ページを作成します。");
        var payload = CreateNotionPagePayload(title, "書籍", [.. authors], link, publishedDate, [imageId]);
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _client.PostAsync(_createPagesUrl, content);
        response.EnsureSuccessStatusCode();
        _logger.LogInformation($"Notionのページを作成しました。");
        _logger.LogInformation("タイトル: {Title}", title);
        _logger.LogInformation("著者: {Authors}", string.Join(", ", authors));
        _logger.LogInformation("リンク: {Link}", link);
        if (DatetimeRegex().IsMatch(publishedDate))
        {
            _logger.LogInformation("発売日: {PublishedDate}", publishedDate);
        }
        var jsonRes = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CreatePageResponse>(jsonRes)!;
    }

    public async Task<CreatePageResponse> CreateMusicAlbumPageAsync(string title, IEnumerable<string> artists, string link, string releaseDate, string imageId)
    {
        _logger.LogInformation($"Notionに音楽アルバムページを作成します。");
        var payload = CreateNotionPagePayload(title, "音楽アルバム", [.. artists], link, releaseDate, [imageId]);
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _client.PostAsync(_createPagesUrl, content);
        response.EnsureSuccessStatusCode();
        _logger.LogInformation($"Notionのページを作成しました。");
        _logger.LogInformation("タイトル: {Title}", title);
        _logger.LogInformation("アーティスト: {Artist}", string.Join(", ", artists));
        _logger.LogInformation("リンク: {Link}", link);
        _logger.LogInformation("発売日: {ReleaseDate}", releaseDate);
        var jsonRes = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CreatePageResponse>(jsonRes)!;
    }

    public async Task<CreatePageResponse> CreateRestaurantPageAsync(string name, string address, string link, string visitedAt, IEnumerable<string> imageIds)
    {
        _logger.LogInformation($"Notionに飲食店ページを作成します。");
        var payload = CreateNotionPagePayload(name, "飲食店", [], link, visitedAt, imageIds);
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _client.PostAsync(_createPagesUrl, content);
        response.EnsureSuccessStatusCode();
        _logger.LogInformation($"Notionのページを作成しました。");
        _logger.LogInformation("店名: {Name}", name);
        _logger.LogInformation("住所: {Address}", address);
        _logger.LogInformation("リンク: {Link}", link);
        _logger.LogInformation("訪問日: {VisitedAt}", visitedAt);
        var jsonRes = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CreatePageResponse>(jsonRes)!;
    }

    private NotionPageCreate CreateNotionPagePayload(string title, string genre, IEnumerable<string> authors, string link, string publishedDate, IEnumerable<string> imageIds)
    {
        var genreOption = _genres.First(g => g.Name == genre);
        var IconUrl = genre switch
        {
            "書籍" => Consts.BookIconUrl,
            "音楽アルバム" => Consts.MusicIconUrl,
            "飲食店" => Consts.RestaurantIconUrl,
            _ => ""
        };
        var authorOptions = authors.Select(author => _authors.FirstOrDefault(a => a.Name == author) ?? new SelectOption { Name = author }).ToList();

        var payload = new NotionPageCreate
        {
            Parent = new Parent { DatabaseId = _databaseId },
            Icon = new Icon
            {
                Type = "external",
                External = new ExternalFile
                {
                    Url = IconUrl
                }
            },
            Properties = new PageProperties
            {
                Title = new TitleProperty
                {
                    Title = [
                        new TextObject
                        {
                            Text = new TextContent { Content = title }
                        }
                    ]
                },
                Link = new UrlValueByPage
                {
                    Url = link
                },
                Status = new StatusValueByPage
                {
                    Status = _notStartStatus
                },
                Genre = new SelectValueByPage
                {
                    Select = genreOption
                },
            },
            Children = [.. imageIds.Select(imageid => new List<BlockBase> ([
                new ParagraphBlock {
                    Paragraph = new ParagraphContent{
                        RichText = []
                    }
                },
                new ImageBlock
                {
                    Image = new ImageContent
                    {
                        Type = "file_upload",
                        FileUpload = new FileUploadContent{
                            Id = imageid
                        }
                    }
                }
                ])).SelectMany(block => block)]
        };
        if (authorOptions.Count != 0)
        {
            payload.Properties.Authors = new MultiSelectValueByPage
            {
                MultiSelect = authorOptions
            };
        }
        if (DatetimeRegex().IsMatch(publishedDate))
        {
            if (genre == "飲食店")
            {
                payload.Properties.StartedDate = new DateValueByPage
                {
                    Date = new DateValue
                    {
                        Start = publishedDate,
                    }
                };
                payload.Properties.CompletedDate = new DateValueByPage
                {
                    Date = new DateValue
                    {
                        Start = publishedDate,
                    }
                };
            }
            else
            {
                payload.Properties.PublishedDate = new DateValueByPage
                {
                    Date = new DateValue
                    {
                        Start = publishedDate,
                    }
                };
            }
        }
        return payload;
    }

    private void LoadProperties()
    {
        _logger.LogInformation("Notionのデータベースのプロパティを取得します。");
        var response = _client.GetAsync(_getDbSchmeUrl).Result;
        var json = response.Content.ReadAsStringAsync().Result;
        var dbResponse = JsonSerializer.Deserialize<NotionDatabaseResponse>(json);
        response.EnsureSuccessStatusCode();

        var hoge = dbResponse!.Properties[Consts.AuthorKey];
        _authors = [.. dbResponse!.Properties[Consts.AuthorKey].MultiSelect!.Options];
        _notStartStatus = dbResponse!.Properties[Consts.StatusKey].Status!.Options.First(s => s.Name == "未着手");
        _genres = [.. dbResponse!.Properties[Consts.GenreKey].Select!.Options];
        _logger.LogInformation("Notionのデータベースのプロパティを取得しました。");
    }


    private async Task<string> CreateFileUploadAsync(string imageName, string mime)
    {
        _logger.LogInformation("Notionにfile_uploadを作成します。");
        var payload = new { imageName, content_type = mime };
        var jsonPayload = JsonSerializer.Serialize(payload);
        var response = await _client.PostAsync("https://api.notion.com/v1/file_uploads", new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json));
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<NotionFileUploadResponse>(json);
        _logger.LogInformation("file_uploadを作成しました。file_upload: {FileUploadId}", result!.Id);
        return result!.Id;
    }

    [GeneratedRegex(@"^\d{4}-\d{2}-\d{2}$")]
    private static partial Regex DatetimeRegex();
}
