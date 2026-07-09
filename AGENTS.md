# AGENTS.md

## リポジトリの目的

Experience2Notion は、個人の Notion データベースに書籍、音楽アルバム、飲食店の感想ページを作成するための .NET 9 クラスライブラリです。

Azure Functions の API 本体は関連リポジトリ `Experience2Notion_App` にあり、このリポジトリは外部サービス検索、Notion API ペイロード作成、画像アップロード、Notion ページ作成を担当します。

主な利用フローは以下です。

1. iPhone ショートカットで書籍、音楽アルバム、飲食店の情報を取得する。
2. ショートカットが Azure Functions API を呼び出す。
3. API がこのライブラリを呼び出す。
4. このライブラリが必要に応じて外部サービスを検索し、Notion ページを作成する。
5. API レスポンスをもとに、iPhone 側で作成された Notion ページを開く。

## 技術スタック

- 言語: C#
- ターゲットフレームワーク: .NET 9
- 出力形式: クラスライブラリ
- ログ: `Microsoft.Extensions.Logging`
- Google API は Google API クライアントライブラリを利用する。
- Notion、Spotify、MusicBrainz、Cover Art Archive、Brave Search、Open Library の画像エンドポイントは `HttpClient` で呼び出す。

## ディレクトリ構成

- `Experience2Notion/Services/`: Notion と外部サービスのクライアント。
- `Experience2Notion/Models/Notions/`: Notion API のリクエスト/レスポンスモデル。
- `Experience2Notion/Models/Spotifies/`: Spotify API のレスポンスモデル。
- `Experience2Notion/Models/Braves/`: Brave Search API のレスポンスモデル。
- `Experience2Notion/Models/MusicAlbums/`: 音楽アルバム検索で共通利用するモデル。
- `Experience2Notion/Configs/`: Notion プロパティ名やアイコン URL などの定数。
- `Experience2Notion/Exceptions/`: プロジェクト固有の例外。

## 実装時の注意

- API リポジトリ側も同時に変更しない限り、公開クラスや公開メソッドの形は維持する。
- Notion データベースのプロパティ名は日本語文字列に依存している。プロパティ名の変更は互換性に影響するため慎重に扱う。
- `NotionClient` はコンストラクタで Notion データベースの選択肢を読み込む。`NOTION_API_KEY` と `NOTION_DB_ID` が必要。
- `BraveSearchClient` は `BRAVE_SEARCH_API_KEY` が必要。検索結果0件およびAPIエラー時は `Experience2NotionException` を投げる。
- Notion API バージョンは `2022-06-28` を利用している。
- 画像は Notion file upload API にアップロードしてから、ページの child block に追加する。
- 書籍ページでは `発売日`、飲食店ページでは `開始日` と `完了日`、飲食店の住所では `場所` を利用する。
- 外部 API 呼び出しは service クラスに閉じ込め、Notion のペイロード構造は Notion model 側に寄せる。
- シークレット、API キー、データベース ID、ローカル環境値はコミットしない。

## 検証

- コード変更後はリポジトリルートで `dotnet build` を実行する。
- 現時点ではテストプロジェクトは存在しない。今後テストを追加する場合は、外部 HTTP/API 呼び出しをライブサービスではなくモック化する。
- Azure Functions API との契約に影響する変更は、関連リポジトリ `Experience2Notion_App` と合わせて確認する。
