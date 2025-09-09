using Experience2Notion.Models.Spotifies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Mime;

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

    public async Task<SpotifyAlbum?> SearchAlbumAsync(string albumName, string artist)
    {
        var url = $"https://api.spotify.com/v1/search?q=album:{Uri.EscapeDataString(albumName)}%20artist:{Uri.EscapeDataString(artist)}&type=album&limit=1";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<SpotifySearchResponse>(json);

        return data?.Albums.Items.FirstOrDefault();
    }
}
