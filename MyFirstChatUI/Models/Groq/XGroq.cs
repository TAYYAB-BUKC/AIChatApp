using Newtonsoft.Json;

namespace MyFirstChatUI.Models.Groq
{
	public class XGroq
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("seed")]
		public int Seed { get; set; }
	}
}