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
    public class PostgresShiftRepository : BaseGraphQLRepository, IRepository<Shift>
    {
        public bool Delete(int id)
        {
            return RunSync(() => DeleteByIdAsync(id));
        }

        public List<Shift> GetAll()
        {
            return RunSync(() => GetAllAsync());
        }

        public Shift GetById(int id)
        {
            return RunSync(() => GetByIdAsync(id));
        }

        public bool Insert(Shift item)
        {
            return RunSync(() => InsertAsync(item));
        }

        public bool Update(int id, Shift item)
        {
            return RunSync(() => UpdateAsync(id, item));
        }

        public int CreateGetId(Shift item)
        {
            return RunSync(() => CreateGetIdAsync(item));
        }


        private async Task<List<Shift>> GetAllAsync()
        {
            string query = @"{
              allShifts {
                nodes {
                  shiftId
                  startTime
                  endTime
                  openingCash
                  closingCash
                  totalSales
                  totalOrders
                  status
                }
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            var shifts = new List<Shift>();

            if (result["data"]?["allShifts"]?["nodes"] is JArray nodes)
            {
                foreach (var node in nodes)
                {
                    var shift = new Shift
                    {
                        ShiftID = node["shiftId"].Value<int>(),
                        StartTime = DateTime.Parse(node["startTime"].Value<string>()),
                        OpeningCash = node["openingCash"].Value<float>(),
                        ClosingCash = node["closingCash"].Value<float>(),
                        TotalSales = node["totalSales"].Value<float>(),
                        TotalOrders = node["totalOrders"].Value<int>(),
                        Status = node["status"].Value<string>()
                    };

                    // Handle nullable end time
                    if (node["endTime"] != null)
                    {
                        shift.EndTime = DateTime.Parse(node["endTime"].Value<string>());
                    }
                    else
                    {
                        shift.EndTime = null;
                    }

                    shifts.Add(shift);
                }
                return shifts;
            }
            throw new Exception("Invalid response format");
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            string query = @"mutation {{
              deleteShiftByShiftId(input: {shiftId: " + id + @"}) {
                clientMutationId
                deletedShiftId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "deleteShiftByShiftId");
        }

        private async Task<Shift> GetByIdAsync(int id)
        {
            string query = @"{
              shiftByShiftId(shiftId: " + id + @") {
                shiftId
                startTime
                endTime
                openingCash
                closingCash
                totalSales
                totalOrders
                status
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            var shiftData = result["data"]?["shiftByShiftId"];

            if (shiftData == null)
            {
                return null;
            }

            var shift = new Shift
            {
                ShiftID = shiftData["shiftId"].Value<int>(),
                StartTime = DateTime.Parse(shiftData["startTime"].Value<string>()),
                OpeningCash = shiftData["openingCash"].Value<float>(),
                ClosingCash = shiftData["closingCash"].Value<float>(),
                TotalSales = shiftData["totalSales"].Value<float>(),
                TotalOrders = shiftData["totalOrders"].Value<int>(),
                Status = shiftData["status"].Value<string>()
            };

            // Handle nullable end time
            if (shiftData["endTime"] != null)
            {
                shift.EndTime = DateTime.Parse(shiftData["endTime"].Value<string>());
            }
            else
            {
                shift.EndTime = null;
            }

            return shift;
        }

        private async Task<int> CreateGetIdAsync(Shift shift)
        {
            // Format the start time for GraphQL ISO format
            string startTimeStr = shift.StartTime.ToString("o");
            // Handle nullable end time for the query
            string endTimeStr = shift.EndTime.HasValue
                ? $@"""{shift.EndTime.Value.ToString("o")}"""
                : "null";
            string query = @"
            mutation {
              createShift(
                input: {shift: {
                  startTime: """ + startTimeStr + @""", 
                  endTime: " + endTimeStr + @", 
                  openingCash: " + shift.OpeningCash + @",
                  closingCash: " + shift.ClosingCash + @", 
                  totalSales: " + shift.TotalSales + @", 
                  totalOrders: " + shift.TotalOrders + @", 
                  status: """ + shift.Status + @"""
                }}
              ) {
                clientMutationId
                  shift {
                      shiftId
                    }
              }
            }";
            var result = await ExecuteGraphQLAsync(query);
            var id = result["data"]?["createShift"]?["shift"]?["shiftId"]?.Value<int>();
            return id ?? 0;
        }
        private async Task<bool> InsertAsync(Shift shift)
        {
            // Format the start time for GraphQL ISO format
            string startTimeStr = shift.StartTime.ToString("o");

            // Handle nullable end time for the query
            string endTimeStr = shift.EndTime.HasValue
                ? $@"""{shift.EndTime.Value.ToString("o")}"""
                : "null";

            string query = @"
            mutation {
              createShift(
                input: {shift: {
                  startTime: """ + startTimeStr + @""", 
                  endTime: " + endTimeStr + @", 
                  openingCash: " + shift.OpeningCash + @",
                  closingCash: " + shift.ClosingCash + @", 
                  totalSales: " + shift.TotalSales + @", 
                  totalOrders: " + shift.TotalOrders + @", 
                  status: """ + shift.Status + @"""
                }}
              ) {
                clientMutationId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "createShift");
        }

        private async Task<bool> UpdateAsync(int id, Shift shift)
        {
            // Format the start time for GraphQL ISO format
            string startTimeStr = shift.StartTime.ToString("o");

            // Handle nullable end time for the query
            string endTimeStr = shift.EndTime.HasValue
                ? $@"""{shift.EndTime.Value.ToString("o")}"""
                : "null";

            string query = @"
            mutation {
              updateShiftByShiftId(
                input: {shiftPatch: {
                  startTime: """ + startTimeStr + @""", 
                  endTime: " + endTimeStr + @", 
                  openingCash: " + shift.OpeningCash + @",
                  closingCash: " + shift.ClosingCash + @",
                  totalSales: " + shift.TotalSales + @", 
                  totalOrders: " + shift.TotalOrders + @", 
                  status: """ + shift.Status + @"""
                },
                shiftId: " + id + @"}
              ) {
                clientMutationId
              }
            }";

            var result = await ExecuteGraphQLAsync(query);
            return IsOperationSuccessful(result, "updateShiftByShiftId");
        }

    
    }
}