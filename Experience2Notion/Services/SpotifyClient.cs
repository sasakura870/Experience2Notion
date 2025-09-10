using Experience2Notion.Models.Spotifies;
using Experience2Notion.Exceptions;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Experience2Notion.Services;
public class SpotifyClient
{
    private readonly ILogger<SpotifyClient> _logger;
    private readonly HttpClient _client = new HttpClient();

    public SpotifyClient(ILogger<SpotifyClient> logger)
    {
        _logger = logger;

        var clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");
        Task.Run(async () =>
        {
            var token = await GetAccessTokenAsync(clientId!, clientSecret!);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }).GetAwaiter().GetResult();
    }

    private async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
    {
        var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, MediaTypeNames.Application.FormUrlEncoded);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(stream);

        return json.RootElement.GetProperty("access_token").GetString()!;
    }

    public async Task<Album?> SearchAlbumAsync(string albumName, string artist)
    {
        var url = $"https://api.spotify.com/v1/search?{CreateSearchUrl(albumName, artist)}";
        var response = await _client.GetAsync(url);
        var hoge = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<SpotifySearchResponse>(json);
        if (data is null || data.Albums.Items.Count == 0)
        {
            _logger.LogWarning("アルバムが見つかりませんでした。AlbumName: {AlbumName}, Artist: {Artist}", albumName, artist);
            throw new Experience2NotionException($"アルバムが見つかりませんでした。AlbumName: {albumName}, Artist: {artist}");
        }

        return data.Albums.Items.FirstOrDefault();
    }

    private string CreateSearchUrl(string albumName, string artist)
    {
        var query = new StringBuilder();
        query.Append($"artist:\"{artist}\"");
        query.Append(' ');
        query.Append($"album:\"{albumName}\"");

        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["q"] = query.ToString();
        queryString["type"] = "album";
        return queryString.ToString()!;
    }
}
