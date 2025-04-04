using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace POS_For_Small_Shop.Helpers
{
    public static class APIHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string postgrestUrl = "http://localhost:3001"; // Your PostgREST server URL
        private static readonly string postgraphileUrl = "http://localhost:5000/graphql"; // Your PostGraphile server URL


        public static async Task<string> ExecuteGraphQLQuery(string query, object variables = null)
        {
            var graphQLRequest = new
            {
                query = query,
                variables = variables
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(graphQLRequest),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(postgraphileUrl, content);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Check for HTTP errors
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GraphQL request failed with status code {response.StatusCode}. Response: {jsonResponse}");
            }

            return jsonResponse;
        }

    }



}
