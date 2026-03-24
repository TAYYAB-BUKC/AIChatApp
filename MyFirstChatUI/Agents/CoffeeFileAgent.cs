using GroqApiLibrary;
using MyFirstChatUI.Models;

namespace MyFirstChatUI.Agents
{
	public class CoffeeFileAgent
	{
		private CoffeeFileAgent(GroqApiClient client, CoffeeData coffeeDataService)
		private string systemPrompt = @"
			The document store contains the text of coffee descriptions.
			Always analyze the document store to provide an answer to the user's question.
			Never rely on your knowledge not included in the document store.
			Always format response using markdown.
        ";
		{

		}

		// Static factory method for async initialization
		public static async Task<CoffeeFileAgent> CreateAsync(GroqApiClient client, CoffeeData coffeeDataService)
		{
			return new CoffeeFileAgent(client, coffeeDataService);
		}
	}
}