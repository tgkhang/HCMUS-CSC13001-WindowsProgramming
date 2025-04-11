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
    public class PostgresTransactionRepository : BaseGraphQLRepository, IRepository<Transaction>
    {
        public bool Delete(int id)
        {
            return RunSync(() => DeleteByIdAsync(id));
        }

        public List<Transaction> GetAll()
        {
            return RunSync(() => GetAllAsync());
        }

        public Transaction GetById(int id)
        {
            return RunSync(() => GetByIdAsync(id));
        }

        public bool Insert(Transaction item)
        {
            return RunSync(() => InsertAsync(item));
        }

        public bool Update(int id, Transaction item)
        {
            return RunSync(() => UpdateAsync(id, item));
        }

        public int CreateGetId(Transaction item)
        {
            return RunSync(() => CreateGetIdAsync(item));
        }

        private async Task<List<Transaction>> GetAllAsync()
        {
            string query = @"{
              allTransactions {
                nodes {
                  transactionId
                  orderId
                  amountPaid
                  paymentMethod
                }
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            var transactions = new List<Transaction>();

            if (result["data"]?["allTransactions"]?["nodes"] is JArray nodes)
            {
                foreach (var node in nodes)
                {
                    var transaction = new Transaction
                    {
                        TransactionID = node["transactionId"].Value<int>(),
                        OrderID = node["orderId"].Value<int>(),
                        AmountPaid = node["amountPaid"].Value<float>(),
                        PaymentMethod = node["paymentMethod"].Value<string>()
                    };

                    transactions.Add(transaction);
                }
                return transactions;
            }
            throw new Exception("Invalid response format");
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"mutation {{
              deleteTransactionByTransactionId(input: {transactionId: " + id + @"}) {
                clientMutationId
                deletedTransactionId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteTransactionByTransactionId");
        }

        private async Task<Transaction> GetByIdAsync(int id)
        {
            string query = @"{
              transactionByTransactionId(transactionId: " + id + @") {
                transactionId
                orderId
                amountPaid
                paymentMethod
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            var transactionData = result["data"]?["transactionByTransactionId"];

            if (transactionData == null)
            {
                return null;
            }

            var transaction = new Transaction
            {
                TransactionID = transactionData["transactionId"].Value<int>(),
                OrderID = transactionData["orderId"].Value<int>(),
                AmountPaid = transactionData["amountPaid"].Value<float>(),
                PaymentMethod = transactionData["paymentMethod"].Value<string>()
            };

            return transaction;
        }

        private async Task<int> CreateGetIdAsync(Transaction transaction)
        {
            string query = @"
            mutation {
              createTransaction(
                input: {transaction: {
                  orderId: " + transaction.OrderID + @",
                  amountPaid: " + transaction.AmountPaid + @",
                  paymentMethod: """ + transaction.PaymentMethod + @"""
                }}
              ) {
                clientMutationId
                transaction {
                  transactionId
                }
              }
            }";
            var result = await ExecuteGraphQLAsync(query);
            var id = result["data"]?["createTransaction"]?["transaction"]?["transactionId"]?.Value<int>();
            return id ?? 0;
        }

        private async Task<bool> InsertAsync(Transaction transaction)
        {
            string query = @"
            mutation {
              createTransaction(
                input: {transaction: {
                  orderId: " + transaction.OrderID + @",
                  amountPaid: " + transaction.AmountPaid + @",
                  paymentMethod: """ + transaction.PaymentMethod + @"""
                }}
              ) {
                clientMutationId
              }
            }";
            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "createTransaction");
        }

        private async Task<bool> UpdateAsync(int id, Transaction transaction)
        {
            string query = @"
            mutation {
              updateTransactionByTransactionId(
                input: {transactionPatch: {
                  orderId: " + transaction.OrderID + @",
                  amountPaid: " + transaction.AmountPaid + @",
                  paymentMethod: """ + transaction.PaymentMethod + @"""
                },
                transactionId: " + id + @"}
              ) {
                clientMutationId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateTransactionByTransactionId");
        }
    }
}