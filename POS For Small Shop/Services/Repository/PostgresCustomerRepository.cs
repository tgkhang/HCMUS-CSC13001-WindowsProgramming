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
    public class PostgresCustomerRepository : BaseGraphQLRepository, IRepository<Customer>
    {
        //private static readonly HttpClient client = new HttpClient();
        //private static readonly string postgraphileUrl = "http://localhost:5000/graphql"; // Your PostGraphile server URL

        public List<Customer> GetAll()
        {
            //return Task.Run(() => GetAllAsync()).GetAwaiter().GetResult();
            return RunSync(() => GetAllAsync());
        }
        public bool Delete(int id)
        {
            //return Task.Run(() => DeleteByIdAsync(id)).GetAwaiter().GetResult();
            return RunSync(() => DeleteByIdAsync(id));
        }
        public Customer GetById(int id)
        {
            //return Task.Run(() => GetByIdAsync(id)).GetAwaiter().GetResult();
            return RunSync(() => GetByIdAsync(id));
        }
        public bool Insert(Customer item)
        {
            //return Task.Run(() => InsertAsync(item)).GetAwaiter().GetResult();
            return RunSync(() => InsertAsync(item));
        }
        public bool Update(int id, Customer item)
        {
            return RunSync(() => UpdateAsync(id, item));
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

            //var graphQLRequest = new { query = query };
            //var content = new StringContent(
            //    JsonConvert.SerializeObject(graphQLRequest),
            //    Encoding.UTF8,
            //    "application/json");

            //var response = await client.PostAsync(postgraphileUrl, content);
            //response.EnsureSuccessStatusCode();

            //var jsonResponse = await response.Content.ReadAsStringAsync();
            //var result = JObject.Parse(jsonResponse);

            var result = await ExecuteGraphQLAsync(query);

            var customers = new List<Customer>();//return list

            if (result["data"]?["allCustomers"]?["nodes"] is JArray nodes)
            {
            
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
                }
                return customers;
            }
            throw new Exception("Invalid response format");
        }
       
        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"mutation {
                  deleteCustomerByCustomerId(input: {customerId: " + id + @" }) {
                    clientMutationId
                    deletedCustomerId
                  }
                }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteCustomerByCustomerId");
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

            var result = await ExecuteGraphQLAsync(query);
            var customerData = result["data"]?["customerByCustomerId"];

            if (customerData == null)
            {
                return null;
            }

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

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "createCustomer");
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
            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateCustomerByCustomerId");
        }

        public int CreateGetId(Customer item)
        {
            throw new NotImplementedException();
        }
    }
}