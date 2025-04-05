using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using POS_For_Small_Shop.Data.Models;
using Windows.Media.Protection.PlayReady;

namespace POS_For_Small_Shop.Services.Repository
{
    public class PostgresCustomerRepository : IRepository<Customer>
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string postgraphileUrl = "http://localhost:5000/graphql"; // Your PostGraphile server URL

        public List<Customer> GetAll()
        {
            return Task.Run(() => GetAllAsync()).GetAwaiter().GetResult();
        }
        public bool Delete(int id)
        {
            return Task.Run(() => DeleteByIdAsync(id)).GetAwaiter().GetResult();
        }
        public Customer GetById(int id)
        {
            return Task.Run(() => GetByIdAsync(id)).GetAwaiter().GetResult();
        }
        public bool Insert(Customer item)
        {
            return Task.Run(() => InsertAsync(item)).GetAwaiter().GetResult();
        }
        public bool Update(int id, Customer item)
        {
            return Task.Run(() => UpdateAsync(id, item)).GetAwaiter().GetResult();
        }
        private async Task<List<Customer>> GetAllAsync()
        {
            string query = @"{
              allCustomers {
                nodes {
                  address
                  customerId
                  email
                  loyaltyPoints
                  name
                  phone
                }
              }
            }
            ";

            var graphQLRequest = new { query = query };
            var content = new StringContent(
                JsonConvert.SerializeObject(graphQLRequest),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(postgraphileUrl, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(jsonResponse);

            var customers = new List<Customer>();//return list

            if (result["data"] != null && result["data"]["allCustomers"] != null && result["data"]["allCustomers"]["nodes"] != null)
            {
                var nodes = result["data"]["allCustomers"]["nodes"];

                foreach (var node in nodes)
                {
                    var customer = new Customer
                    {
                        CustomerID = node["customerId"].Value<int>(),
                        Name = node["name"].Value<string>(),
                        Phone = node["phone"].Value<string>(),
                        Email = node["email"]?.Value<string>(),
                        Address = node["address"]?.Value<string>(),
                        LoyaltyPoints = node["loyaltyPoints"].Value<int>()
                    };
                    customers.Add(customer);
                    Console.WriteLine(customer.Name);
                }

                
                return customers;
            }
            else
            {
                throw new Exception("Invalid response format");
            }
        }
       
        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"{
                  deleteCustomerByCustomerId(input: {customerId: $id }) {
                    clientMutationId
                    deletedCustomerId
                  }
                }
            ";
            query = query.Replace("$id", id.ToString());

            var graphQLRequest = new { query = query };
            var content = new StringContent(
                JsonConvert.SerializeObject(graphQLRequest),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(postgraphileUrl, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(jsonResponse);

            // Check if there are any errors in the response
            if (result["errors"] != null)
            {
                // Error occurred - deletion failed
                return false;
            }

            var deletedCustomerId = result["data"]?["deleteCustomerByCustomerId"]?["deletedCustomerId"];
            return deletedCustomerId != null;
        }

        private async Task<Customer> GetByIdAsync(int id)
        {
            
            string query = @"{
                  customerByCustomerId(customerId: " + id + @") {
                    customerId
                    name
                    phone
                    email
                    address
                    loyaltyPoints
                  }
                }
            ";

            var graphQLRequest = new { query = query };
            var content = new StringContent(
                JsonConvert.SerializeObject(graphQLRequest),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(postgraphileUrl, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(jsonResponse);

            // Check if the customer data exists in the response
            var customerData = result["data"]?["customerByCustomerId"];

            if (customerData == null)
            {
                // Customer not found
                return null;
            }

            // Parse the customer data
            return new Customer
            {
                CustomerID = customerData["customerId"].Value<int>(),
                Name = customerData["name"].Value<string>(),
                Phone = customerData["phone"].Value<string>(),
                Email = customerData["email"]?.Value<string>(),
                Address = customerData["address"]?.Value<string>(),
                LoyaltyPoints = customerData["loyaltyPoints"].Value<int>()
            };
        }

        private async Task<bool> InsertAsync(Customer customer)
        {
            string query = @"
                mutation {
                  createCustomer(
                    input: {customer: {
                      name: """ + customer.Name + @""",
                      phone: """ + customer.Phone + @""",
                      loyaltyPoints: " + customer.LoyaltyPoints + @",
                      email: """ + customer.Email + @""", 
                      address: """ + customer.Address + @"""
                    }}
                  ) {
                    clientMutationId
                  }
                }
            ";

            var graphQLRequest = new { query = query };
            var content = new StringContent(
                JsonConvert.SerializeObject(graphQLRequest),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(postgraphileUrl, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(jsonResponse);

            // Check if there are any errors in the response
            if (result["errors"] != null)
            {
                return false;
            }

            // Check if the customer was created successfully
            var createCustomerResult = result["data"]?["createCustomer"];
            return createCustomerResult != null;
        }

        private async Task<bool> UpdateAsync(int id, Customer customer)
        {
            string query = @"
                mutation {
                  updateCustomerByCustomerId(
                    input: {
                      customerPatch: {
                        address: """ + customer.Address + @""",
                        email: """ + customer.Email + @""", 
                        loyaltyPoints: " + customer.LoyaltyPoints + @",
                        name: """ + customer.Name + @""",
                        phone: """ + customer.Phone + @"""
                      }, 
                      customerId: " + id + @"
                    }
                  ) {
                    clientMutationId
                  }
                }
            ";

            var graphQLRequest = new { query = query };
            var content = new StringContent(
                JsonConvert.SerializeObject(graphQLRequest),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(postgraphileUrl, content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(jsonResponse);

            // Check if there are any errors in the response
            if (result["errors"] != null)
            {
                return false;
            }

            // Check if the update was successful
            var updateResult = result["data"]?["updateCustomerByCustomerId"];
            return updateResult != null;
        }
    }
}