using MyFirstChatUI.Models.Groq;
using Newtonsoft.Json;

namespace MyFirstChatUI.Helpers.Groq
{
	public class GroqResponseHelper
	{
		public static GroqResponse GetResponse(string jsonResponse)
		{
			GroqResponse response = null!;
			try
			{
				response = JsonConvert.DeserializeObject<GroqResponse>(jsonResponse);
			}
			catch (JsonException ex)
			{
				Console.Error.WriteLine($"JSON Deserialization Error: {ex.Message}");
			}
			return response!;
		}

		public static GroqMessage GetMessage(string jsonResponse)
		{
			GroqMessage latestMessage = null!;
			try
			{
				GroqResponse response = JsonConvert.DeserializeObject<GroqResponse>(jsonResponse);

				if (response != null && response.Choices != null && response.Choices.Length > 0)
				{
					latestMessage = response.Choices[0].Message;
				}
				else
				{
					Console.WriteLine("No message found in the response.");
				}
			}
			catch (JsonException ex)
			{
				Console.Error.WriteLine($"JSON Deserialization Error: {ex.Message}");
			}

			return latestMessage;
		}
	}
}