using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services.Repository
{
    public class PostgresIngredientRepository : BaseGraphQLRepository, IRepository<Ingredient>
    {
        public bool Delete(int id) => RunSync(() => DeleteByIdAsync(id));
        public List<Ingredient> GetAll() => RunSync(() => GetAllAsync());
        public Ingredient GetById(int id) => RunSync(() => GetByIdAsync(id));
        public bool Insert(Ingredient item) => RunSync(() => InsertAsync(item));
        public bool Update(int id, Ingredient item) => RunSync(() => UpdateAsync(id, item));
        public int CreateGetId(Ingredient item) => throw new NotImplementedException();

        private async Task<List<Ingredient>> GetAllAsync()
        {
            string query = @"
            {
              allIngredients {
                nodes {
                  ingredientId
                  ingredientName
                  categoryId
                  stock
                  unit
                  supplier
                  purchasePrice
                  expiryDate
                }
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            var ingredients = new List<Ingredient>();

            if (result["data"]?["allIngredients"]?["nodes"] is JArray nodes)
            {
                foreach (var node in nodes)
                {
                    var ingredient = new Ingredient
                    {
                        IngredientID = node["ingredientId"].Value<int>(),
                        IngredientName = node["ingredientName"].Value<string>(),
                        CategoryID = node["categoryId"].Value<int>(),
                        Stock = node["stock"].Value<int>(),
                        Unit = node["unit"].Value<string>(),
                        Supplier = node["supplier"].Value<string>(),
                        PurchasePrice = node["purchasePrice"].Value<float>(),
                        ExpiryDate = DateTime.Parse(node["expiryDate"].Value<string>())
                    };
                    ingredients.Add(ingredient);
                }
                return ingredients;
            }
            throw new Exception("Invalid response format");
        }

        private async Task<Ingredient> GetByIdAsync(int id)
        {
            string query = @"{
              ingredientByIngredientId(ingredientId: " + id + @") {
                ingredientId
                ingredientName
                categoryId
                stock
                unit
                supplier
                purchasePrice
                expiryDate
              }
            }
";

            var result = await ExecuteGraphQLAsync(query);
            var ingredientData = result["data"]?["ingredientByIngredientId"];
            
            if (ingredientData == null) return null;

            return new Ingredient
            {
                IngredientID = ingredientData["ingredientId"].Value<int>(),
                IngredientName = ingredientData["ingredientName"].Value<string>(),
                CategoryID = ingredientData["categoryId"].Value<int>(),
                Stock = ingredientData["stock"].Value<int>(),
                Unit = ingredientData["unit"].Value<string>(),
                Supplier = ingredientData["supplier"].Value<string>(),
                PurchasePrice = ingredientData["purchasePrice"].Value<float>(),
                ExpiryDate = DateTime.Parse(ingredientData["expiryDate"].Value<string>())
            };
        }

        private async Task<bool> InsertAsync(Ingredient item)
        {
            try
            {
                string query = $@"
                    mutation {{
                      createIngredient(
                        input: {{
                          ingredient: {{
                            ingredientName: ""{item.IngredientName?.Replace("\"", "\\\"") ?? ""}"",
                            categoryId: {item.CategoryID},
                            stock: {item.Stock},
                            unit: ""{item.Unit}"",
                            supplier: ""{item.Supplier}"",
                            purchasePrice: {item.PurchasePrice},
                            expiryDate: ""{item.ExpiryDate?.ToString("yyyy-MM-dd")}""
                          }}
                        }}
                      ) {{
                        ingredient {{
                          ingredientId
                        }}
                        clientMutationId
                      }}
                    }}
                ";

                // For debugging
                Debug.WriteLine("Final GraphQL query: " + query);

                var result = await ExecuteGraphQLAsync(query);

                return IsOperationSuccessful(result, "createIngredient");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in InsertAsync: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Debug.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                }
                return false;
            }
        }

        private async Task<bool> UpdateAsync(int id, Ingredient item)
        {
            string query = $@"
            mutation {{
              updateIngredientByIngredientId(
                input: {{
                  ingredientPatch: {{
                    ingredientName: ""{item.IngredientName}"",
                    categoryId: {item.CategoryID},
                    stock: {item.Stock},
                    unit: ""{item.Unit}"",
                    supplier: ""{item.Supplier}"",
                    purchasePrice: {item.PurchasePrice},
                    expiryDate: ""{item.ExpiryDate?.ToString("yyyy-MM-dd")}""
                  }},
                  ingredientId: {id}
                }}
              ) {{
                clientMutationId
              }}
            }}";

            Debug.WriteLine("Update Ingredient Query: " + query);
            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateIngredientByIngredientId");
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = $@"
            mutation {{
              deleteIngredientByIngredientId(input: {{
                ingredientId: {id}
              }}) {{
                deletedIngredientId
              }}
            }}";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteIngredientByIngredientId");
        }
    }
}

