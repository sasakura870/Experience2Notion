using Experience2Notion.Configs;
using Experience2Notion.Models.Notions;
using Experience2Notion.Models.Notions.Objects;
using Experience2Notion.Models.Notions.Properties;
using System.Text;
using System.Text.Json;

namespace Experience2Notion;
public class NotionClient
{
    readonly HttpClient _client;
    readonly string _databaseId;

    readonly string _getDbSchmeUrl;
    readonly string _createPagesUrl = "https://api.notion.com/v1/pages";

    List<SelectOption> _authors = [];
    List<SelectOption> _status = [];
    List<SelectOption> _genres = [];

    public NotionClient()
    {
        var notionApiKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {notionApiKey}");
        _client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");

        _databaseId = Environment.GetEnvironmentVariable("NOTION_DB_ID") ?? string.Empty;
        _getDbSchmeUrl = $"https://api.notion.com/v1/databases/{Guid.Parse(_databaseId)}";
    }

    public async Task LoadProperties()
    {
        var response = await _client.GetAsync(_getDbSchmeUrl);
        var json = await response.Content.ReadAsStringAsync();
        var dbResponse = JsonSerializer.Deserialize<NotionDatabaseResponse>(json);
        response.EnsureSuccessStatusCode();

        var hoge = dbResponse!.Properties[Consts.AuthorKey];
        _authors = [.. (dbResponse!.Properties[Consts.AuthorKey].MultiSelect!).Options];
        _status = [.. (dbResponse!.Properties[Consts.StatusKey].Status!).Options];
        _genres = [.. (dbResponse!.Properties[Consts.GenreKey].MultiSelect!).Options];
    }

    public async Task CreateBookPageAsync(string title, string author)
    {
        var payload = new NotionPageCreate
        {
            Parent = new Parent { DatabaseId = _databaseId },
            Icon = new Emoji { Type = "emoji", EmojiChar = "📚" },
            Properties = new Properties
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
                //Status = new StatusProperty
                //{
                //    Status = new StatusValue
                //    {
                //        //Id = 
                //        Color = "red",
                //        Name = "未着手",

                //    }
                //}
                //Authors = new RichTextProperty
                //{
                //    RichText = [
                //        new TextObject
                //        {
                //            Text = new TextContent { Content = author }
                //        }
                //    ]
                //}
            },
        };
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(_createPagesUrl, content);
        var hoge = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
    }
}
