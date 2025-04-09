using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services.Repository
{
    public class PostgresOrderRepository : BaseGraphQLRepository, IRepository<Order>
    {
        public bool Delete(int id)
        {
            return RunSync(() => DeleteByIdAsync(id));
        }

        public List<Order> GetAll()
        {
            return RunSync(() => GetAllAsync());
        }

        public Order GetById(int id)
        {
            return RunSync(() => GetByIdAsync(id));
        }

        public bool Insert(Order item)
        {
            return RunSync(() => InsertAsync(item));
        }

        public bool Update(int id, Order item)
        {
            return RunSync(() => UpdateAsync(id, item));
        }

        private async Task<List<Order>> GetAllAsync()
        {
            string query = @"{
              allOrders {
                nodes {
                  orderId
                  customerId
                  discount
                  finalAmount
                  paymentMethod
                  shiftId
                  status
                  totalAmount
                }
              }
            }
            ";
            
            var result = await ExecuteGraphQLAsync(query);
            var orders= new List<Order>();

            if (result["data"]?["allOrders"]?["nodes"] is JArray nodes)
            {
                foreach (var node in nodes)
                {
                    var order = new Order
                    {
                        OrderID = (int)node["orderId"].Value<int>(),
                        CustomerID = (int)node["customerId"].Value<int>(),
                        Discount = (float)node["discount"].Value<float>(),
                        FinalAmount = (float)node["finalAmount"].Value<float>(),
                        PaymentMethod = node["paymentMethod"].Value<string>(),
                        ShiftID = (int)node["shiftId"].Value<int>(),
                        Status = node["status"].Value<string>(),
                        TotalAmount = (float)node["totalAmount"].Value<float>()
                    };
                    orders.Add(order);
                }
                return orders;
            }
            throw new Exception("Invalid response format");
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"mutation {{
              deleteOrderByOrderId(input: {orderId: " + id + @"}) {
                clientMutationId
                deletedOrderId
              }
            }";

            var result= await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteOrderByOrderId");
        }
        private async Task<Order> GetByIdAsync(int id)
        {
            string query = @"{
                  orderByOrderId(orderId: " + id + @") {
                    customerId
                    discount
                    finalAmount
                    nodeId
                    orderId
                    paymentMethod
                    shiftId
                    status
                    totalAmount
                  }
                }
            ";
            var result = await ExecuteGraphQLAsync(query);
            var orderData = result["data"]?["orderByOrderId"];

            if (orderData == null)
            {
                return null;
            }

            return new Order
            {
                OrderID = (int)orderData["orderId"].Value<int>(),
                CustomerID = (int)orderData["customerId"].Value<int>(),
                Discount = (float)orderData["discount"].Value<float>(),
                FinalAmount = (float)orderData["finalAmount"].Value<float>(),
                PaymentMethod = orderData["paymentMethod"].Value<string>(),
                ShiftID = (int)orderData["shiftId"].Value<int>(),
                Status = orderData["status"].Value<string>(),
                TotalAmount = (float)orderData["totalAmount"].Value<float>()

            };
        }
        private async Task<bool> InsertAsync(Order order)
        {
            string query = @"
            mutation {
              createOrder(
                input: {order: {
                  totalAmount: " + order.TotalAmount + @", 
                  finalAmount: " + order.FinalAmount + @", 
                  paymentMethod: """ + order.PaymentMethod + @""", 
                  status: """ + order.Status + @""", 
                  customerId: " + order.CustomerID+ @", 
                  discount: " + order.Discount + @", 
                  shiftId: " + order.ShiftID + @"
                }}
              ) {
                clientMutationId
              }
        }";
            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "createOrder");
        }

        private async Task<bool> UpdateAsync(int id, Order order)
        {
            string query = @"
                mutation  {
                  updateOrderByOrderId(
                    input: {orderPatch: {
                      customerId:" + order.CustomerID+ @",  
                      discount: " + order.Discount + @", 
                      finalAmount: " + order.FinalAmount + @", 
                      paymentMethod:  """ + order.PaymentMethod + @""",  
                      shiftId: " + order.ShiftID + @"
                      status:  """ + order.Status + @""", 
                      totalAmount: " + order.TotalAmount + @", 
                    },
                      orderId: " + id + @" }
                  ) {
                    clientMutationId
                }
                }
            ";

            var result= await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateOrderByOrderId");
        }

        public int CreateGetId(Order item)
        {
            throw new NotImplementedException();
        }
    }
}
