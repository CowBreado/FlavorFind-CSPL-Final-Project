using System.Net.Http.Headers;
using System.Text;
using FlavorFind.BusLogic.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Markdig;

namespace FlavorFind.Controllers
{
    public class AIRecipeController : Controller
    {
        
        private static readonly HttpClient client = new HttpClient();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(RecipeRequest model)
        {
            string prompt = $"You are a recipe assistant. The user has a budget of ₱{model.Budget}, available ingredients: {model.Ingredients}, and preferences: {model.Preferences}. " +
                            $"The recipe should be for a {model.DishType} dish intended for {model.MealTime}. " +
                            $"Recommend 1 simple recipes with estimated cost and easy-to-follow instructions. " + 
                            "Provide name, description, ingredients, instruction and category section";

            string rawResponse = await CallGroqLLM(prompt);

            var pipeline = new MarkdownPipelineBuilder()
                                .UseAdvancedExtensions()
                                .Build();

            string htmlResponse = Markdown.ToHtml(rawResponse ?? string.Empty, pipeline);

            var recipeViewModel = new RecipeResponse
            {
                Reply = rawResponse,
                ReplyHtml = htmlResponse
            };

            return View("Result", recipeViewModel);
        }

        private async Task<string> CallGroqLLM(string prompt)
        {
            var apiKey = "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";

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful recipe assistant." },
                    new { role = "user", content = prompt }
                }
            };

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var errorContent = await httpResponse.Content.ReadAsStringAsync();
                return $"Error: {httpResponse.StatusCode} - {errorContent}";
            }

            var jsonString = await httpResponse.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(jsonString);

            if (json == null || json.choices == null || json.choices.Count == 0)
            {
                return "The response did not contain the expected data.";
            }

            return json.choices[0].message.content;
        }
    }
}
