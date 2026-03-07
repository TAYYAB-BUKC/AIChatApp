using Newtonsoft.Json;

namespace MyFirstChatUI.Models.Groq
{
	public class GroqChoice
	{
		[JsonProperty("index")]
		public int Index { get; set; }

		[JsonProperty("message")]
		public GroqMessage Message { get; set; }
	}
}