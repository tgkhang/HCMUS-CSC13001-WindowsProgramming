using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services.Repository
{
    public class PostgresMenuItemRepository : BaseGraphQLRepository, IRepository<MenuItem>
    {
        public bool Delete(int id)
        {
            return RunSync(() => DeleteByIdAsync(id));
        }

        public List<MenuItem> GetAll()
        {
            return RunSync(() => GetAllAsync());
        }

        public MenuItem GetById(int id)
        {
            return RunSync(() => GetByIdAsync(id));
        }

        public bool Insert(MenuItem item)
        {
            return RunSync(() => InsertAsync(item));
        }

        public bool Update(int id, MenuItem item)
        {
            return RunSync(() => UpdateAsync(id, item));
        }

        private async Task<List<MenuItem>> GetAllAsync()
        {
            string query = @"
            {
              allMenuItems {
                nodes {
                  categoryId
                  imagePath
                  menuItemId
                  name
                  sellingPrice
                }
              }
            }
            ";
            var result = await ExecuteGraphQLAsync(query);

            var menuItems = new List<MenuItem>();//return list

            if (result["data"]?["allMenuItems"]?["nodes"] is JArray nodes)
            {

                foreach (var node in nodes)
                {
                    var menuItem = new MenuItem
                    {
                        MenuItemID = node["menuItemId"].Value<int>(),
                        Name = node["name"].Value<string>(),
                        CategoryID = node["categoryId"].Value<int>(),
                        ImagePath = node["imagePath"].Value<string>(),
                        SellingPrice = node["sellingPrice"]?.Value<float>() ?? 0.0f
                    };
                    menuItems.Add(menuItem);
                }
                return menuItems;
            }
            throw new Exception("Invalid response format");
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"mutation MyMutation {
              deleteMenuItemByMenuItemId(input: {menuItemId: " + id + @"}) {
                clientMutationId
                deletedMenuItemId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteMenuItemByMenuItemId");
        }

        private async Task<MenuItem> GetByIdAsync(int id)
        {
            string query = @"{
              menuItemByMenuItemId(menuItemId: " + id + @") {
                categoryId
                imagePath
                menuItemId
                name
                sellingPrice
              }
            }
            ";

            var result = await ExecuteGraphQLAsync(query);
            var menuItemData = result["data"]?["menuItemByMenuItemId"];

            if (menuItemData == null)
            {
                return null;
            }

            return new MenuItem
            {
                MenuItemID = menuItemData["menuItemId"].Value<int>(),
                Name = menuItemData["name"].Value<string>(),
                CategoryID = menuItemData["categoryId"].Value<int>(),
                ImagePath = menuItemData["imagePath"].Value<string>(),
                SellingPrice = menuItemData["sellingPrice"]?.Value<float>() ?? 0.0f
            };
        }

        private async Task<bool> InsertAsync(MenuItem menuItem)
        {
            try
            {
                // Create safe image path by escaping backslashes
                string safeImagePath = menuItem.ImagePath?.Replace(@"\", @"\\") ?? string.Empty;

                // Use string interpolation consistently as in your UpdateAsync method
                string query = $@"
                    mutation {{
                      createMenuItem(
                        input: {{
                          menuItem: {{
                            name: ""{menuItem.Name?.Replace("\"", "\\\"") ?? ""}"",
                            sellingPrice: {menuItem.SellingPrice}, 
                            imagePath: ""{safeImagePath}"", 
                            categoryId: {menuItem.CategoryID}
                          }}
                        }}
                      ) {{
                        menuItem {{
                          menuItemId
                        }}
                        clientMutationId
                      }}
                    }}
                ";

                // For debugging
                Debug.WriteLine("Final GraphQL query: " + query);

                var result = await ExecuteGraphQLAsync(query);
                //Debug.WriteLine("GraphQL result: " + result.ToString());
                //Debug.Write(IsOperationSuccessful(result, "createMenuItem"));
                return IsOperationSuccessful(result, "createMenuItem");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in InsertAsync: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                // If there's an inner exception, log that too
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Debug.WriteLine($"Inner stack trace: {ex.InnerException.StackTrace}");
                }
                return false;
            }
        }

        private async Task<bool> UpdateAsync(int id, MenuItem menuItem)
        {
            string safeImagePath = menuItem.ImagePath.Replace(@"\", @"\\");
            string query = $@"
            mutation {{
              updateMenuItemByMenuItemId(
                input: {{
                  menuItemPatch: {{
                    name: ""{menuItem.Name}"",
                    sellingPrice: {menuItem.SellingPrice}, 
                    imagePath: ""{safeImagePath}"", 
                    categoryId: {menuItem.CategoryID}
                  }}, 
                  menuItemId: {id}
                }}
              ) {{
                clientMutationId
              }}
            }}";

            Debug.WriteLine("Final GraphQL query: " + query);

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateMenuItemByMenuItemId");
        }

        public int CreateGetId(MenuItem item)
        {
            throw new NotImplementedException();
        }
    }
}
