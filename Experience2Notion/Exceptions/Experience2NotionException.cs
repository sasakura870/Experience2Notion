namespace Experience2Notion.Exceptions;
[Serializable]
public class Experience2NotionException : Exception
{
	public Experience2NotionException() { }
	public Experience2NotionException(string message) : base(message) { }
	public Experience2NotionException(string message, Exception inner) : base(message, inner) { }
	protected Experience2NotionException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}