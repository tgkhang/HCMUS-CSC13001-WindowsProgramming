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
    public class PostgresOrderDetailRepository : BaseGraphQLRepository, IRepository<OrderDetail>
    {
        public bool Delete(int id)
        {
            return RunSync(() => DeleteByIdAsync(id));
        }

        public List<OrderDetail> GetAll()
        {
            return RunSync(() => GetAllAsync());
        }

        public OrderDetail GetById(int id)
        {
            return RunSync(() => GetByIdAsync(id));
        }

        public bool Insert(OrderDetail item)
        {
            return RunSync(() => InsertAsync(item));
        }

        public bool Update(int id, OrderDetail item)
        {
            return RunSync(() => UpdateAsync(id, item));
        }

        private async Task<List<OrderDetail>> GetAllAsync()
        {
            string query = @"{
              allOrderDetails {
                nodes {
                  orderDetailId
                  orderId
                  menuItemId
                  quantity
                  unitPrice
                  subtotal
                }
              }
            }";
            
            var result = await ExecuteGraphQLAsync(query);
            var orderDetails = new List<OrderDetail>();

            if (result["data"]?["allOrderDetails"]?["nodes"] is JArray nodes)
            {
                foreach (var node in nodes)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderDetailID = node["orderDetailId"].Value<int>(),
                        OrderID = node["orderId"].Value<int>(),
                        MenuItemID = node["menuItemId"].Value<int>(),
                        Quantity = node["quantity"].Value<int>(),
                        UnitPrice = node["unitPrice"].Value<float>(),
                        Subtotal = node["subtotal"].Value<float>()
                    };
                    orderDetails.Add(orderDetail);
                }
                return orderDetails;
            }
            throw new Exception("Invalid response format");
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"mutation {{
              deleteOrderDetailByOrderDetailId(input: {orderDetailId: " + id + @"}) {
                clientMutationId
                deletedOrderDetailId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteOrderDetailByOrderDetailId");
        }

        private async Task<OrderDetail> GetByIdAsync(int id)
        {
            string query = @"{
              orderDetailByOrderDetailId(orderDetailId: " + id + @") {
                orderDetailId
                orderId
                menuItemId
                quantity
                unitPrice
                subtotal
              }
            }";
            
            var result = await ExecuteGraphQLAsync(query);
            var orderDetailData = result["data"]?["orderDetailByOrderDetailId"];

            if (orderDetailData == null)
            {
                return null;
            }

            return new OrderDetail
            {
                OrderDetailID = orderDetailData["orderDetailId"].Value<int>(),
                OrderID = orderDetailData["orderId"].Value<int>(),
                MenuItemID = orderDetailData["menuItemId"].Value<int>(),
                Quantity = orderDetailData["quantity"].Value<int>(),
                UnitPrice = orderDetailData["unitPrice"].Value<float>(),
                Subtotal = orderDetailData["subtotal"].Value<float>()
            };
        }

        private async Task<bool> InsertAsync(OrderDetail orderDetail)
        {
            string query = @"
            mutation {
              createOrderDetail(
                input: {orderDetail: {
                  orderId: " + orderDetail.OrderID + @", 
                  menuItemId: " + orderDetail.MenuItemID + @", 
                  quantity: " + orderDetail.Quantity + @", 
                  unitPrice: " + orderDetail.UnitPrice + @", 
                  subtotal: " + orderDetail.Subtotal + @"
                }}
              ) {
                clientMutationId
              }
            }";

            //Debug.WriteLine(query);

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "createOrderDetail");
        }

        private async Task<bool> UpdateAsync(int id, OrderDetail orderDetail)
        {
            string query = @"
            mutation {
              updateOrderDetailByOrderDetailId(
                input: {orderDetailPatch: {
                  orderId: " + orderDetail.OrderID + @",
                  menuItemId: " + orderDetail.MenuItemID + @",
                  quantity: " + orderDetail.Quantity + @",
                  unitPrice: " + orderDetail.UnitPrice + @",
                  subtotal: " + orderDetail.Subtotal + @"
                },
                orderDetailId: " + id + @"}
              ) {
                clientMutationId
              }
            }";
            
            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateOrderDetailByOrderDetailId");
        }

        public int CreateGetId(OrderDetail item)
        {
            throw new NotImplementedException();
        }
    }
}