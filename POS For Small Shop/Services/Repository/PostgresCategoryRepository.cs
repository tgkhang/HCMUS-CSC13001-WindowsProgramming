using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services.Repository
{
    public class PostgresCategoryRepository : BaseGraphQLRepository, IRepository<Category>
    {
        public List<Category> GetAll()
        {
            return RunSync(() => GetAllAsync());
        }

        public Category GetById(int id)
        {
            return RunSync(() => GetByIdAsync(id));
        }

        public bool Insert(Category item)
        {
            return RunSync(() => InsertAsync(item));
        }

        public bool Update(int id, Category item)
        {
            return RunSync(() => UpdateAsync(id, item));
        }

        public bool Delete(int id)
        {
            return RunSync(() => DeleteByIdAsync(id));
        }
        private async Task<List<Category>> GetAllAsync()
        {
            string query = @"{
              allCategories {
                nodes {
                  categoryId
                  description
                  name
                }
              }
            }
            ";
            var result = await ExecuteGraphQLAsync(query);

            var categories = new List<Category>();//return list

            if (result["data"]?["allCategories"]?["nodes"] is JArray nodes)
            {
                foreach (var node in nodes)
                {
                    var category = new Category
                    {
                        CategoryID = node["categoryId"].Value<int>(),
                        Name = node["name"].Value<string>(),
                        Description = node["description"]?.Value<string>()
                    };
                    categories.Add(category);
                }
                return categories;
            }
            throw new Exception("Invalid response format");
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"
                mutation MyMutation {
                  deleteCategoryByCategoryId(input: {categoryId: " + id + @"}) {
                    clientMutationId
                    deletedCategoryId
                  }
                }
            ";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteMenuItemByMenuItemId");
        }

        private async Task<Category> GetByIdAsync(int id)
        {
            string query = @"{
                  categoryByCategoryId(categoryId: " + id + @" ) {
                    categoryId
                    description
                    name
                  }
                }
            ";
            var result = await ExecuteGraphQLAsync(query);
            var categoryData = result["data"]?["categoryByCategoryId"];

            if (categoryData == null)
            {
                return null;
            }

            return new Category
            {
                CategoryID = categoryData["categoryId"].Value<int>(),
                Name = categoryData["name"].Value<string>(),
                Description = categoryData["description"]?.Value<string>()
            };
        }

        private async Task<bool> InsertAsync(Category category)
        {
            string query = @"
                mutation MyMutation {
                  createCategory(input: 
                    {
                      category: {
                        name: """ + category.Name + @""",
                        description: """ + category.Description + @""",
                      }
                    }
                  ) {
                    clientMutationId
                  }
                }
            ";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "createCategory");
        }

        private async Task<bool> UpdateAsync(int id, Category category)
        {
            string query = @"
            mutation MyMutation {
              updateCategoryByCategoryId(
                input: {
                  categoryPatch: 
                  {
                    description: """ + category.Description + @""",
                    name: """ + category.Name + @""",
                  },
                  categoryId: " + id + @"}
              ) {
                clientMutationId
              }
            }
            ";
            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateCategoryByCategoryId");
        }

        public int CreateGetId(Category item)
        {
            throw new NotImplementedException();
        }
    }
}
