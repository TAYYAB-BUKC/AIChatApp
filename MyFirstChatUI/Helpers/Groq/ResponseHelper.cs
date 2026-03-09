using MyFirstChatUI.Models.Groq;
using Newtonsoft.Json;

namespace MyFirstChatUI.Helpers.Groq
{
	public class GroqResponseHelper
	{
		public static GroqResponse? GetResponse(string jsonResponse)
		{
			return DeserializeResponse(jsonResponse);
		}

		public static GroqMessage? GetMessage(string jsonResponse)
		{
			var response = DeserializeResponse(jsonResponse);
			return response?.Choices?.FirstOrDefault()?.Message;
		}

		private static GroqResponse? DeserializeResponse(string jsonResponse)
		{
			try
			{
				return JsonConvert.DeserializeObject<GroqResponse>(jsonResponse);
			}
			catch (JsonException ex)
			{
				Console.Error.WriteLine($"JSON Deserialization Error: {ex.Message}");
				return null;
			}
		}
	}
}