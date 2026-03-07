using Newtonsoft.Json;

namespace MyFirstChatUI.Models.Groq
{
	public class GroqMessage
	{
		[JsonProperty("role")]
		public string Role { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }
	}
}