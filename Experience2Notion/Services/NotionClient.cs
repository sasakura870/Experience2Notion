using Experience2Notion.Configs;
using Experience2Notion.Models.Notions;
using Experience2Notion.Models.Notions.Blocks;
using Experience2Notion.Models.Notions.Objects;
using Experience2Notion.Models.Notions.Properties;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Experience2Notion.Services;
public class NotionClient
{
    readonly HttpClient _client;
    readonly string _databaseId;

    readonly string _getDbSchmeUrl;
    readonly string _createPagesUrl = "https://api.notion.com/v1/pages";

    List<SelectOption> _authors = [];
    SelectOption _notStartStatus = new();
    List<SelectOption> _genres = [];

    public NotionClient()
    {
        var notionApiKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {notionApiKey}");
        _client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");

        _databaseId = Environment.GetEnvironmentVariable("NOTION_DB_ID") ?? string.Empty;
        _getDbSchmeUrl = $"https://api.notion.com/v1/databases/{Guid.Parse(_databaseId)}";

        LoadProperties();
    }

    public void LoadProperties()
    {
        var response = _client.GetAsync(_getDbSchmeUrl).Result;
        var json = response.Content.ReadAsStringAsync().Result;
        var dbResponse = JsonSerializer.Deserialize<NotionDatabaseResponse>(json);
        response.EnsureSuccessStatusCode();

        var hoge = dbResponse!.Properties[Consts.AuthorKey];
        _authors = [.. dbResponse!.Properties[Consts.AuthorKey].MultiSelect!.Options];
        _notStartStatus = dbResponse!.Properties[Consts.StatusKey].Status!.Options.First(s => s.Name == "未着手");
        _genres = [.. dbResponse!.Properties[Consts.GenreKey].Select!.Options];
    }

    public async Task<string> UploadImageAsync(string imageName, byte[] imageData, string mime)
    {
        var fileUploadId = await CreateFileUploadAsync(imageName, mime);

        using var content = new MultipartFormDataContent();
        using var imageContent = new ByteArrayContent(imageData);
        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        content.Add(imageContent, "file", imageName);

        var response = await _client.PostAsync($"https://api.notion.com/v1/file_uploads/{fileUploadId}/send", content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<NotionFileUploadResponse>(json);
        return result!.Id;
    }

    public async Task CreateBookPageAsync(string title, IList<string> authors, string link, string publishedDate, string imageId)
    {
        var bookGenre = _genres.First(g => g.Name == "書籍");
        var authorOptions = authors.Select(author => _authors.FirstOrDefault(a => a.Name == author) ?? new SelectOption { Name = author }).ToList();
        var payload = new NotionPageCreate
        {
            Parent = new Parent { DatabaseId = _databaseId },
            Icon = new Icon
            {
                Type = "external",
                External = new ExternalFile
                {
                    Url = Consts.BookIconUrl
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
                Authors = new MultiSelectValueByPage
                {
                    MultiSelect = authorOptions
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
                    Select = bookGenre
                },
                PublishedDate = new DateValueByPage
                {
                    Date = new DateValue { Start = publishedDate }
                },
            },
            Children =
            [
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
                            Id = imageId
                        }
                    }
                }
            ]
        };
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _client.PostAsync(_createPagesUrl, content);
        response.EnsureSuccessStatusCode();
    }

    private async Task<string> CreateFileUploadAsync(string imageName, string mime)
    {
        var payload = new { imageName, content_type = mime };
        var jsonPayload = JsonSerializer.Serialize(payload);
        var response = await _client.PostAsync("https://api.notion.com/v1/file_uploads", new StringContent(jsonPayload, Encoding.UTF8, MediaTypeNames.Application.Json));
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<NotionFileUploadResponse>(json);
        return result!.Id;
    }
}
