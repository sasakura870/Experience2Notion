# Experience2Notion

Experience2Notion は、個人の Notion データベースに感想ページを作成するための C# クラスライブラリです。iPhone ショートカットから呼び出される Azure Functions API によって利用され、書籍、音楽アルバム、飲食店のページ作成を支援します。

## 全体の流れ

1. iPhone で「Notionページを作成」ショートカットを起動する。
2. ショートカットがページ種別に応じた情報を取得する。
3. ショートカットが `Experience2Notion_App` の Azure Functions API を呼び出す。
4. API がこのライブラリを利用し、外部サービス検索や Notion ページ作成を行う。
5. API のレスポンスをもとに、iPhone 側で作成された Notion ページを開く。

## 対応しているページ種別

- 書籍
  - ISBN をもとに Google Books で書籍情報を検索する。
  - Open Library Covers を優先し、見つからない場合は Google Books の画像 URL から表紙画像を取得する。
  - タイトル、著者、リンク、発売日、表紙画像をもとに Notion ページを作成する。
- 音楽アルバム
  - Spotify または MusicBrainz でアルバム情報を検索する。
  - Spotify の画像 URL または Cover Art Archive からジャケット画像を取得する。
  - タイトル、アーティスト、リンク、発売日、ジャケット画像をもとに Notion ページを作成する。
- 飲食店
  - API 側から渡された店名、住所、リンク、訪問日、画像をもとに Notion ページを作成する。

## 主なサービスクラス

- `NotionClient`: 画像アップロード、Notion データベースプロパティ取得、Notion ページ作成を行う。
- `GoogleBookSeacher`: ISBN から Google Books で書籍を検索する。
- `BookCoverClient`: Open Library Covers または Google Books から書籍表紙を取得する。
- `SpotifyClient`: Spotify で音楽アルバムを検索する。
- `MusicAlbumSearchClient`: MusicBrainz と Cover Art Archive で音楽アルバム情報を検索する。
- `GoogleEngineSearcher`: Google Custom Search でウェブ検索と画像検索を行う。

## Notion データベースの前提

対象の Notion データベースには、以下のプロパティが存在する前提です。

- `タイトル`
- `著者/アーティスト`
- `リンク`
- `ステータス`
- `ジャンル`
- `開始日`
- `完了日`
- `発売日`
- `場所`

`ステータス` には `未着手`、`ジャンル` には `書籍`、`音楽アルバム`、`飲食店` が含まれている必要があります。

## 環境変数

このライブラリを利用するアプリケーション側で、必要に応じて以下の環境変数を設定します。

- `NOTION_API_KEY`
- `NOTION_DB_ID`
- `GOOGLE_BOOKS_API_KEY`
- `GOOGLE_CUSTOM_SEARCH_API_KEY`
- `GOOGLE_CUSTOM_SEARCH_IMAGE_ENGINE_ID`
- `GOOGLE_CUSTOM_SEARCH_BASIC_ENGINE_ID`
- `SPOTIFY_CLIENT_ID`
- `SPOTIFY_CLIENT_SECRET`

## ビルド

```powershell
dotnet build
```

## 関連リポジトリ

- `Experience2Notion_App`: このライブラリを呼び出す Azure Functions API。
