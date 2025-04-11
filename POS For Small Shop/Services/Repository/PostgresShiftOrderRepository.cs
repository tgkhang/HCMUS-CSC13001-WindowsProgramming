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
    public class PostgresShiftOrderRepository : BaseGraphQLRepository, IRepository<ShiftOrder>
    {
        public bool Delete(int id)
        {
            return RunSync(() => DeleteByIdAsync(id));
        }

        public List<ShiftOrder> GetAll()
        {
            return RunSync(() => GetAllAsync());
        }

        public ShiftOrder GetById(int id)
        {
            return RunSync(() => GetByIdAsync(id));
        }

        public bool Insert(ShiftOrder item)
        {
            return RunSync(() => InsertAsync(item));
        }

        public bool Update(int id, ShiftOrder item)
        {
            return RunSync(() => UpdateAsync(id, item));
        }

        private async Task<List<ShiftOrder>> GetAllAsync()
        {
            string query = @"{
              allShiftOrders {
                nodes {
                  shiftOrderId
                  shiftId
                  orderId
                }
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            var shiftOrders = new List<ShiftOrder>();

            if (result["data"]?["allShiftOrders"]?["nodes"] is JArray nodes)
            {
                foreach (var node in nodes)
                {
                    var shiftOrder = new ShiftOrder
                    {
                        ShiftOrderID = node["shiftOrderId"].Value<int>(),
                        ShiftID = node["shiftId"].Value<int>(),
                        OrderID = node["orderId"].Value<int>()
                    };
                    shiftOrders.Add(shiftOrder);
                }
                return shiftOrders;
            }
            throw new Exception("Invalid response format");
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"mutation {{
              deleteShiftOrderByShiftOrderId(input: {shiftOrderId: " + id + @"}) {
                clientMutationId
                deletedShiftOrderId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteShiftOrderByShiftOrderId");
        }

        private async Task<ShiftOrder> GetByIdAsync(int id)
        {
            string query = @"{
              shiftOrderByShiftOrderId(shiftOrderId: " + id + @") {
                shiftOrderId
                shiftId
                orderId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            var shiftOrderData = result["data"]?["shiftOrderByShiftOrderId"];

            if (shiftOrderData == null)
            {
                return null;
            }

            return new ShiftOrder
            {
                ShiftOrderID = shiftOrderData["shiftOrderId"].Value<int>(),
                ShiftID = shiftOrderData["shiftId"].Value<int>(),
                OrderID = shiftOrderData["orderId"].Value<int>()
            };
        }

        private async Task<bool> InsertAsync(ShiftOrder shiftOrder)
        {
            string query = @"
            mutation {
              createShiftOrder(
                input: {shiftOrder: {
                  shiftId: " + shiftOrder.ShiftID + @", 
                  orderId: " + shiftOrder.OrderID + @"
                }}
              ) {
                clientMutationId
              }
            }";
            Debug.WriteLine(query);
            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "createShiftOrder");
        }

        private async Task<bool> UpdateAsync(int id, ShiftOrder shiftOrder)
        {
            string query = @"
            mutation {
              updateShiftOrderByShiftOrderId(
                input: {shiftOrderPatch: {
                  shiftId: " + shiftOrder.ShiftID + @",
                  orderId: " + shiftOrder.OrderID + @"
                },
                shiftOrderId: " + id + @"}
              ) {
                clientMutationId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateShiftOrderByShiftOrderId");
        }

        public int CreateGetId(ShiftOrder item)
        {
            throw new NotImplementedException();
            Debug.WriteLine("CreateGetId method is not implemented in PostgresShiftOrderRepository.");
        }
    }
}