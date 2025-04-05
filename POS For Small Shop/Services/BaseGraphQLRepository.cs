using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace POS_For_Small_Shop.Services
{
    public abstract class BaseGraphQLRepository
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string postgraphileUrl = "http://localhost:5000/graphql";
        protected async Task<JObject> ExecuteGraphQLAsync(string query)
        {
            var graphQLRequest = new { query = query };
            var content = new StringContent(
                JsonConvert.SerializeObject(graphQLRequest),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(postgraphileUrl, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JObject.Parse(jsonResponse);
        }

        // Check if the operation was successful (no errors)
        protected bool IsOperationSuccessful(JObject result, string operationPath)
        {
            // Check for errors first
            if (result["errors"] != null)
            {
                return false;
            }

            // Check if the specified operation result exists
            var operationResult = result["data"]?[operationPath];
            return operationResult != null;
        }

        // Helper for wrapping async operations with Task.Run for sync methods
        protected T RunSync<T>(Func<Task<T>> asyncOperation)
        {
            return Task.Run(asyncOperation).GetAwaiter().GetResult();
        }
    }
}
