using GroqApiLibrary;
using Microsoft.Extensions.AI;
using MyFirstChatUI.Helpers.Groq;
using MyFirstChatUI.Models;
using MyFirstChatUI.Models.Groq;
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

		private async Task<JsonArray> GetDocumentsAsync()
		{
			var documents = new JsonArray();
			var markdownContents = await coffeeDataService.ReadMarkdownFilesAsDictionaryAsync();
			foreach (var content in markdownContents)
			{
				var document = new JsonObject
				{
					["id"] = content.Key,
					["source"] = new JsonObject
					{
						["type"] = "text",
						["text"] = content.Value
					}
				};

				documents.Add(document);

				//var document = new JsonObject
				//{
				//	["title"] = content.Key,
				//	["content"] = content.Value
				//};

				//documents.Add(document);
			}

			return documents;
		}

		public async Task<GroqMessage> GetResponseAsync(JsonArray messages)
		{
			JsonObject? response = await client.CreateChatCompletionWithDocumentsAsync(
				messages,
				GroqModels.Llama33_70B,
				await GetDocumentsAsync(),
				enableCitations: false
			);

			return GroqResponseHelper.GetMessage(Convert.ToString(response));
		}

		public async Task<GroqMessage> GetCustomResponseAsync(JsonArray messages)
		{
			var documents = await coffeeDataService.ReadMarkdownFilesAsDictionaryAsync();
			foreach (var document in documents)
			{
				messages.Add(new JsonObject
				{
					["role"] = ChatRole.System.ToString(),
					["content"] = $"Document Name: {document.Key}\n\n\nDocument Content: {document.Value}"
				});
			}

			JsonObject? response = await client.CreateChatCompletionWithReasoningAsync(
				messages,
				GroqModels.GptOss20B,
				reasoningEffort: GroqApiLibrary.ReasoningEffort.Medium,
				reasoningFormat: ReasoningFormat.Parsed
			);

			return GroqResponseHelper.GetMessage(Convert.ToString(response));
		}
	}
}