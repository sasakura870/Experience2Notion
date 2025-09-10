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
    private readonly HttpClient _client = new();

    private readonly string _singleSuffix = " - Single";
    private readonly string _epSuffix = " - EP";

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
        _logger.LogInformation("Spotifyから音楽アルバムを検索します。アルバム名: {AlbumName}, アーティスト: {Artist}", albumName, artist);
        if (albumName.EndsWith(_singleSuffix, StringComparison.OrdinalIgnoreCase))
        {
            albumName = albumName[..^_singleSuffix.Length].Trim();
        }
        else if (albumName.EndsWith(_epSuffix, StringComparison.OrdinalIgnoreCase))
        {
            albumName = albumName[..^_epSuffix.Length].Trim();
        }
        var url = $"https://api.spotify.com/v1/search?{CreateSearchUrl(albumName, artist)}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<SpotifySearchResponse>(json);
        if (data is null || data.Albums.Items.Count == 0)
        {
            _logger.LogWarning("アルバムが見つかりませんでした。アルバム名: {AlbumName}, アーティスト: {Artist}", albumName, artist);
            throw new Experience2NotionException($"アルバムが見つかりませんでした。アルバム名: {albumName}, アーティスト: {artist}");
        }

        var targetAlbum = data.Albums.Items.FirstOrDefault()!;
        _logger.LogInformation("アルバムが見つかりました。" +
            "アルバム名: {AlbumName}, アーティスト: {Artist}, Spotify URL: {Url}", targetAlbum.Name, string.Join(", ", targetAlbum.Artists.Select(a => a.Name)), targetAlbum.ExternalUrl);
        return targetAlbum;
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
