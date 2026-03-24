using GroqApiLibrary;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.AI;
using MyFirstChatUI.Components.Listing;
using MyFirstChatUI.Helpers.Groq;
using MyFirstChatUI.Models;
using System.Text.Json.Nodes;

namespace MyFirstChatUI.Agents
{
	public class CoffeeFileAgent
	{
		private GroqApiClient client;
		private readonly CoffeeData coffeeDataService;
		private readonly IConfiguration configuration;
		private string apiKey = string.Empty;
		public string systemPrompt = @"
			The document store contains the text of coffee descriptions.
			Always analyze the document store to provide an answer to the user's question.
			Never rely on your knowledge not included in the document store.
			Always format response using markdown.
        ";

		private CoffeeFileAgent(CoffeeData coffeeDataService, IConfiguration configuration)
		{
			this.coffeeDataService = coffeeDataService;
			this.configuration = configuration;
		}

		// Static factory method for async initialization
		public static async Task<CoffeeFileAgent> CreateAsync(CoffeeData coffeeDataService, IConfiguration configuration)
		{
			var agent = new CoffeeFileAgent(coffeeDataService, configuration);
			await agent.InitializeAsync();
			return agent;
		}

		private async Task InitializeAsync()
		{
			apiKey = configuration["Chat:AI:ApiKey"] ?? throw new InvalidOperationException("Missing configuration: Endpoint. See the README for details");
			client = new GroqApiClient(apiKey!);
		}

		public async Task<JsonArray> GetDocumentsAsync()
		{
			var documents = new JsonArray();
			var markdownContents = await coffeeDataService.ReadMarkdownFilesAsDictionaryAsync();
			foreach (var content in markdownContents)
			{
				var document = new JsonObject
				{
					["title"] = content.Key,
					["content"] = content.Value
				};

				documents.Append(document);
			}

			return documents;
		}
	}
}