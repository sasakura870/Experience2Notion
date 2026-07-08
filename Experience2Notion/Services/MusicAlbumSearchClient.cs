using Experience2Notion.Models.MusicAlbums;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Experience2Notion.Services;

public class MusicAlbumSearchClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly ILogger<MusicAlbumSearchClient> _logger;
    private readonly HttpClient _client = new();

    public MusicAlbumSearchClient(ILogger<MusicAlbumSearchClient> logger)
    {
        _logger = logger;
        _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Experience2Notion", "1.0"));
    }

    public async Task<Album?> SearchAlbumAsync(string albumName, string artist)
    {
        _logger.LogInformation("MusicBrainzから音楽アルバムを検索します。アルバム名: {AlbumName}, アーティスト: {Artist}", albumName, artist);

        var searchResponse = await SearchMusicBrainzAsync(albumName, artist);
        if (searchResponse?.Releases.Count is null or 0)
        {
            _logger.LogWarning("MusicBrainzにアルバムが見つかりませんでした。アルバム名: {AlbumName}, アーティスト: {Artist}", albumName, artist);
            return null;
        }

        var release = SelectRelease(searchResponse.Releases, albumName, artist);
        if (release is null)
        {
            _logger.LogWarning("MusicBrainzの検索結果から対象アルバムを選択できませんでした。アルバム名: {AlbumName}, アーティスト: {Artist}", albumName, artist);
            return null;
        }

        var coverImageUrl = await TryGetCoverImageUrlAsync(release.Id);
        var album = new Album
        {
            Id = release.Id,
            Name = release.Title,
            Artists = release.ArtistCredits.Select(credit => new Artist
            {
                Name = GetArtistName(credit),
            }).Where(artist => !string.IsNullOrWhiteSpace(artist.Name)).ToList(),
            Images = string.IsNullOrWhiteSpace(coverImageUrl)
                ? []
                : [new Image { Url = coverImageUrl }],
            ReleaseDate = release.Date,
            ExternalUrl = $"https://musicbrainz.org/release/{release.Id}",
        };

        _logger.LogInformation("MusicBrainzでアルバムが見つかりました。アルバム名: {AlbumName}, アーティスト: {Artist}, URL: {Url}", album.Name, string.Join(", ", album.Artists.Select(a => a.Name)), album.ExternalUrl);
        return album;
    }

    private async Task<MusicBrainzSearchResponse?> SearchMusicBrainzAsync(string albumName, string artist)
    {
        var query = $"release:\"{albumName}\" AND artist:\"{artist}\"";
        var url = $"https://musicbrainz.org/ws/2/release?query={Uri.EscapeDataString(query)}&fmt=json&limit=10";
        using var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<MusicBrainzSearchResponse>(stream, JsonOptions);
    }

    private static MusicBrainzRelease? SelectRelease(List<MusicBrainzRelease> releases, string albumName, string artist)
    {
        // MusicBrainzのアーティストクレジット名は、完全一致ランキングにおいてリリースアーティストを表すものとします
        return releases
            .OrderByDescending(release => IsExactArtistMatch(release, artist))
            .ThenByDescending(release => IsExactAlbumTitleMatch(release, albumName))
            .ThenBy(release => TryParseReleaseDate(release.Date, out var date) ? date : DateOnly.MaxValue)
            .FirstOrDefault();
    }

    private async Task<string?> TryGetCoverImageUrlAsync(string releaseId)
    {
        var url = $"https://coverartarchive.org/release/{releaseId}";
        using var response = await _client.GetAsync(url);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogInformation("Cover Art Archiveにカバー画像が見つかりませんでした。MusicBrainz Release ID: {ReleaseId}", releaseId);
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Cover Art Archiveからカバー画像を取得できませんでした。MusicBrainz Release ID: {ReleaseId}, StatusCode: {StatusCode}", releaseId, response.StatusCode);
            return null;
        }

        using var stream = await response.Content.ReadAsStreamAsync();
        var coverArt = await JsonSerializer.DeserializeAsync<CoverArtArchiveResponse>(stream, JsonOptions);
        var imageUrl = GetBestCoverImageUrl(coverArt);
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            _logger.LogInformation("Cover Art Archiveレスポンスに使用可能なカバー画像URLがありませんでした。MusicBrainz Release ID: {ReleaseId}", releaseId);
            return null;
        }

        return imageUrl;
    }

    private static string? GetBestCoverImageUrl(CoverArtArchiveResponse? coverArt)
    {
        var image = coverArt?.Images
            .OrderByDescending(image => image.Front)
            .FirstOrDefault();

        if (image is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(image.Image))
        {
            return image.Image;
        }

        return GetThumbnail(image.Thumbnails, "large")
            ?? GetThumbnail(image.Thumbnails, "500")
            ?? GetThumbnail(image.Thumbnails, "small")
            ?? GetThumbnail(image.Thumbnails, "250");
    }

    private static string? GetThumbnail(Dictionary<string, string>? thumbnails, string key)
    {
        return thumbnails is not null && thumbnails.TryGetValue(key, out var url) && !string.IsNullOrWhiteSpace(url)
            ? url
            : null;
    }

    private static bool IsExactArtistMatch(MusicBrainzRelease release, string artist)
    {
        return release.ArtistCredits.Any(credit => string.Equals(Normalize(GetArtistName(credit)), Normalize(artist), StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsExactAlbumTitleMatch(MusicBrainzRelease release, string albumName)
    {
        return string.Equals(Normalize(release.Title), Normalize(albumName), StringComparison.OrdinalIgnoreCase);
    }

    private static string GetArtistName(MusicBrainzArtistCredit credit)
    {
        return string.IsNullOrWhiteSpace(credit.Name) ? credit.Artist.Name : credit.Name;
    }

    private static string Normalize(string value)
    {
        return value.Trim();
    }

    private static bool TryParseReleaseDate(string value, out DateOnly date)
    {
        date = DateOnly.MaxValue;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var parts = value.Split('-', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0 || !int.TryParse(parts[0], out var year))
        {
            return false;
        }

        var month = parts.Length > 1 && int.TryParse(parts[1], out var parsedMonth) ? parsedMonth : 1;
        var day = parts.Length > 2 && int.TryParse(parts[2], out var parsedDay) ? parsedDay : 1;
        return DateOnly.TryParse($"{year:D4}-{month:D2}-{day:D2}", out date);
    }

    private sealed class MusicBrainzSearchResponse
    {
        [JsonPropertyName("releases")]
        public List<MusicBrainzRelease> Releases { get; set; } = [];
    }

    private sealed class MusicBrainzRelease
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("artist-credit")]
        public List<MusicBrainzArtistCredit> ArtistCredits { get; set; } = [];
    }

    private sealed class MusicBrainzArtistCredit
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("artist")]
        public MusicBrainzArtist Artist { get; set; } = new();
    }

    private sealed class MusicBrainzArtist
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    private sealed class CoverArtArchiveResponse
    {
        [JsonPropertyName("images")]
        public List<CoverArtImage> Images { get; set; } = [];
    }

    private sealed class CoverArtImage
    {
        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;

        [JsonPropertyName("front")]
        public bool Front { get; set; }

        [JsonPropertyName("thumbnails")]
        public Dictionary<string, string> Thumbnails { get; set; } = [];
    }
}
