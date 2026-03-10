using System.Text.Json.Nodes;

namespace MyFirstChatUI.Helpers.Groq
{
	public class RequestHelper
	{
		public static JsonArray DeepCloneMessages(JsonArray messages)
		{
			return JsonNode.Parse(messages.ToJsonString())!.AsArray();
		}
	}
}