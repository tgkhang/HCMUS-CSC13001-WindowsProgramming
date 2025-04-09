using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services.Repository
{
    public class PostgresCashFlowRepository : BaseGraphQLRepository, IRepository<CashFlow>
    {
        public bool Delete(int id)
        {
            return RunSync(() => DeleteByIdAsync(id));
        }

        public List<CashFlow> GetAll()
        {
            return RunSync(() => GetAllAsync());
        }

        public CashFlow GetById(int id)
        {
            return RunSync(() => GetByIdAsync(id));
        }

        public bool Insert(CashFlow item)
        {
            return RunSync(() => InsertAsync(item));
        }

        public bool Update(int id, CashFlow item)
        {
            return RunSync(() => UpdateAsync(id, item));
        }

        private async Task<List<CashFlow>> GetAllAsync()
        {
            string query = @"{
              allCashFlows {
                nodes {
                  cashFlowId
                  shiftId
                  transactionType
                  amount
                  timestamp
                }
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            var cashFlows = new List<CashFlow>();

            if (result["data"]?["allCashFlows"]?["nodes"] is JArray nodes)
            {
                foreach (var node in nodes)
                {
                    var cashFlow = new CashFlow
                    {
                        CashFlowID = node["cashFlowId"].Value<int>(),
                        ShiftID = node["shiftId"].Value<int>(),
                        TransactionType = node["transactionType"].Value<string>(),
                        Amount = node["amount"].Value<float>(),
                        Timestamp = DateTime.Parse(node["timestamp"].Value<string>())
                    };
                    cashFlows.Add(cashFlow);
                }
                return cashFlows;
            }
            throw new Exception("Invalid response format");
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"mutation {{
              deleteCashFlowByCashFlowId(input: {cashFlowId: " + id + @"}) {
                clientMutationId
                deletedCashFlowId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteCashFlowByCashFlowId");
        }

        private async Task<CashFlow> GetByIdAsync(int id)
        {
            string query = @"{
              cashFlowByCashFlowId(cashFlowId: " + id + @") {
                cashFlowId
                shiftId
                transactionType
                amount
                timestamp
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            var cashFlowData = result["data"]?["cashFlowByCashFlowId"];

            if (cashFlowData == null)
            {
                return null;
            }

            return new CashFlow
            {
                CashFlowID = cashFlowData["cashFlowId"].Value<int>(),
                ShiftID = cashFlowData["shiftId"].Value<int>(),
                TransactionType = cashFlowData["transactionType"].Value<string>(),
                Amount = cashFlowData["amount"].Value<float>(),
                Timestamp = DateTime.Parse(cashFlowData["timestamp"].Value<string>())
            };
        }

        private async Task<bool> InsertAsync(CashFlow cashFlow)
        {
            // Format the timestamp for GraphQL ISO format
            string timestampStr = cashFlow.Timestamp.ToString("o");

            string query = @"
            mutation {
              createCashFlow(
                input: {cashFlow: {
                  shiftId: " + cashFlow.ShiftID + @", 
                  transactionType: """ + cashFlow.TransactionType + @""", 
                  amount: " + cashFlow.Amount + @", 
                  timestamp: """ + timestampStr + @"""
                }}
              ) {
                clientMutationId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "createCashFlow");
        }

        private async Task<bool> UpdateAsync(int id, CashFlow cashFlow)
        {
            // Format the timestamp for GraphQL ISO format
            string timestampStr = cashFlow.Timestamp.ToString("o");

            string query = @"
            mutation {
              updateCashFlowByCashFlowId(
                input: {cashFlowPatch: {
                  shiftId: " + cashFlow.ShiftID + @",
                  transactionType: """ + cashFlow.TransactionType + @""",
                  amount: " + cashFlow.Amount + @",
                  timestamp: """ + timestampStr + @"""
                },
                cashFlowId: " + id + @"}
              ) {
                clientMutationId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateCashFlowByCashFlowId");
        }

        public int CreateGetId(CashFlow item)
        {
            throw new NotImplementedException();
        }
    }
}