using Experience2Notion.Models;

namespace Experience2Notion;
public class NotionClient
{
    HttpClient _client;
    string _databaseId;

    public NotionClient()
    {
        var notionApiKey = Environment.GetEnvironmentVariable("NOTION_API_KEY");
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {notionApiKey}");
        _client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");

        _databaseId = Environment.GetEnvironmentVariable("NOTION_DATABASE_ID") ?? string.Empty;
    }

    public async Task CreateBookPageAsync(string title, string author)
    {
        var url = "https://api.notion.com/v1/pages";
        var payload = new NotionPage
        {
            Parent = new Parent { DatabaseId = _databaseId },
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
                Authors = new RichTextProperty
                {
                    RichText = [
                        new TextObject
                        {
                            Text = new TextContent { Content = author }
                        }
                    ]
                }
            },
        };
        var jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
    }
}
