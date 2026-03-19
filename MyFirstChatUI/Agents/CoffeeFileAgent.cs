using GroqApiLibrary;
using MyFirstChatUI.Models;

namespace MyFirstChatUI.Agents
{
	public class CoffeeFileAgent
	{
		private CoffeeFileAgent(GroqApiClient client, CoffeeData coffeeDataService)
		{

		}

		// Static factory method for async initialization
		public static async Task<CoffeeFileAgent> CreateAsync(GroqApiClient client, CoffeeData coffeeDataService)
		{
			return new CoffeeFileAgent(client, coffeeDataService);
		}
	}
}