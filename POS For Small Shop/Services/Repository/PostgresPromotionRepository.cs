using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using POS_For_Small_Shop.Data.Models;
using Microsoft.UI.Dispatching;

namespace POS_For_Small_Shop.Services.Repository
{
    public class PostgresPromotionRepository : BaseGraphQLRepository, IRepository<Promotion>
    {
        private readonly DispatcherQueue _dispatcherQueue;

        // Synchronous wrappers calling async methods
        public List<Promotion> GetAll()
        {
            return RunSync(() => GetAllAsync());
        }

        public Promotion GetById(int id)
        {
            return RunSync(() => GetByIdAsync(id));
        }

        public bool Insert(Promotion item)
        {
            return RunSync(() => InsertAsync(item));
        }

        public bool Update(int id, Promotion item)
        {
            return RunSync(() => UpdateAsync(id, item));
        }

        public bool Delete(int id)
        {
            return RunSync(() => DeleteByIdAsync(id));
        }

        public List<Promotion> GetActivePromotions()
        {
            return RunSync(() => GetActivePromotionsAsync());
        }

        // Asynchronous implementations using GraphQL
        public async Task<List<Promotion>> GetAllAsync()
        {
            var query = @"
                query MyQuery {
                  allPromotions {
                    nodes {
                      startDate
                      endDate
                      menuItemIds
                      promoId
                      promoName
                      promotionDetailsByPromoId {
                        nodes {
                          description
                          discountType
                          discountValue
                          promoDetailsId
                          promoId
                        }
                      }
                    }
                  }
                }";

            var result = await ExecuteGraphQLAsync(query);
            if (!IsOperationSuccessful(result, "allPromotions"))
                return new List<Promotion>();

            var promotions = result["data"]?["allPromotions"]?["nodes"]
                .Select(node =>
                {
                    var detailsNode = node["promotionDetailsByPromoId"]?["nodes"]?.FirstOrDefault(); // Take first details if any
                    var rawStart = node["startDate"]?.Value<DateTime>() ?? DateTime.MinValue;
                    var rawEnd = node["endDate"]?.Value<DateTime>() ?? DateTime.MinValue;

                    return new Promotion
                    {
                        PromoID = node["promoId"].Value<int>(),
                        PromoName = node["promoName"].Value<string>(),
                        StartDate = rawStart.Date,
                        EndDate = rawEnd.Date.AddHours(23).AddMinutes(59).AddSeconds(59),
                        ItemIDs = node["menuItemIds"].ToObject<List<int>>(),
                        Details = detailsNode != null ? new PromotionDetails
                        {
                            PromoDetailsID = detailsNode["promoDetailsId"]?.Value<int>() ?? 0,
                            PromoID = detailsNode["promoId"]?.Value<int>() ?? node["promoId"].Value<int>(),
                            DiscountType = detailsNode["discountType"] != null
                                ? Enum.Parse<DiscountType>(detailsNode["discountType"].Value<string>())
                                : DiscountType.Percentage,
                            DiscountValue = detailsNode["discountValue"]?.Value<float>() ?? 0,
                            Description = detailsNode["description"]?.Value<string>()
                        } : new PromotionDetails()
                    };
                })
                .ToList();

            return promotions;
        }

        public async Task<Promotion> GetByIdAsync(int id)
        {
            var query = $@"query MyQuery {{
              promotionByPromoId(promoId: 1) {{
                endDate
                menuItemIds
                nodeId
                promoId
                promoName
                promotionDetailsByPromoId {{
                  nodes {{
                    description
                    discountType
                    discountValue
                    promoDetailsId
                    promoId
                  }}
                }}
              }}
            }}
            ";

            var result = await ExecuteGraphQLAsync(query);
            if (!IsOperationSuccessful(result, "promotionByPromoId"))
                return null;

            var node = result["data"]?["promotionByPromoId"];
            if (node == null) return null;
            var rawStart = node["startDate"]?.Value<DateTime>() ?? DateTime.MinValue;
            var rawEnd = node["endDate"]?.Value<DateTime>() ?? DateTime.MinValue;

            return new Promotion
            {
                PromoID = node["promoId"].Value<int>(),
                PromoName = node["promoName"].Value<string>(),
                StartDate = rawStart.Date,
                EndDate = rawEnd.Date.AddHours(23).AddMinutes(59).AddSeconds(59),
                ItemIDs = node["menuItemIds"].ToObject<List<int>>(),
                Details = node["promotionDetailByPromoId"] != null ? new PromotionDetails
                {
                    PromoDetailsID = node["promotionDetailByPromoId"]["promoDetailsId"]?.Value<int>() ?? 0,
                    PromoID = node["promoId"].Value<int>(),
                    DiscountType = Enum.Parse<DiscountType>(node["promotionDetailByPromoId"]["discountType"].Value<string>()),
                    DiscountValue = node["promotionDetailByPromoId"]["discountValue"].Value<float>(),
                    Description = node["promotionDetailByPromoId"]["description"]?.Value<string>()
                } : new PromotionDetails()
            };
        }

        public async Task<bool> InsertAsync(Promotion item)
        {
            // Add Promotion
            var mutation = $@"
                mutation MyMutation {{
                    createPromotion(
                        input: {{
                            promotion: {{
                                promoName: ""{item.PromoName}"",
                                startDate: ""{item.StartDate:yyyy-MM-dd}"",
                                endDate: ""{item.EndDate:yyyy-MM-dd}"",
                                menuItemIds: [{string.Join(",", item.ItemIDs)}]
                            }}
                        }}
                    ) {{
                        promotion {{
                            promoId
                        }}
                    }}
                }}";

            var result = await ExecuteGraphQLAsync(mutation);
            if (!IsOperationSuccessful(result, "createPromotion"))
                return false;

            var promoId = result["data"]?["createPromotion"]?["promotion"]?["promoId"]?.Value<int>();
            if (promoId == null) return false;

            //item.PromoID = promoId.Value;

            // Insert PromotionDetails if provided
            if (item.Details != null)
            {
                var detailsMutation = $@"
                mutation MyMutation {{
                    createPromotionDetail(
                        input: {{
                            promotionDetail: {{
                                promoId: {promoId},
                                discountType: ""{item.Details.DiscountType}"",
                                discountValue: {item.Details.DiscountValue},
                                description: {(item.Details.Description != null ? $"\"{item.Details.Description}\"" : "null")}
                            }}
                        }}
                    ) {{
                        promotionDetail {{
                            promoDetailsId
                        }}
                    }}
                }}";

                var detailsResult = await ExecuteGraphQLAsync(detailsMutation);
                if (!IsOperationSuccessful(detailsResult, "createPromotionDetail"))
                    return false;

                // Optionally, set PromoDetailsID if needed
                var promoDetailsId = detailsResult["data"]?["createPromotionDetail"]?["promotionDetail"]?["promoDetailsId"]?.Value<int>();
                
                
                //if (promoDetailsId != null)
                //    item.Details.PromoDetailsID = promoDetailsId.Value;
            }

            return true;
        }


        public async Task<bool> UpdateAsync(int id, Promotion item)
        {
            // Step 1: Update the promotion and get the single promoDetailsId
            var mutation = $@"
                mutation {{
                    updatePromotionByPromoId(input: {{
                        promoId: {id},
                        promotionPatch: {{
                            promoName: ""{item.PromoName}"",
                            startDate: ""{item.StartDate:yyyy-MM-dd}"",
                            endDate: ""{item.EndDate:yyyy-MM-dd}"",
                            menuItemIds: [{string.Join(",", item.ItemIDs)}]
                        }}
                    }}) {{
                        promotion {{
                            promoId
                            promotionDetailsByPromoId {{
                                nodes {{
                                    promoDetailsId
                                }}
                            }}
                        }}
                    }}
                }}";

            var result = await ExecuteGraphQLAsync(mutation);
            if (!IsOperationSuccessful(result, "updatePromotionByPromoId"))
                return false;

            var detailNode = result["data"]?["updatePromotionByPromoId"]?["promotion"]?["promotionDetailsByPromoId"]?["nodes"]?.FirstOrDefault();
            if (detailNode == null)
                return true; // No detail to update

            int? detailId = detailNode["promoDetailsId"]?.Value<int>();
            if (detailId == null)
                return false;

            // Step 2: Update the single promotion detail
            var detail = item.Details;
            if (detail != null)
            {
                var detailMutation = $@"
                    mutation {{
                        updatePromotionDetailByPromoDetailsId(input: {{
                            promoDetailsId: {detailId},
                            promotionDetailPatch: {{
                                discountType: ""{detail.DiscountType}"",
                                discountValue: {detail.DiscountValue.ToString(System.Globalization.CultureInfo.InvariantCulture)},
                                description: {(string.IsNullOrEmpty(detail.Description) ? "null" : $"\"{detail.Description}\"")}
                            }}
                        }}) {{
                            promotionDetail {{
                                promoDetailsId
                            }}
                        }}
                    }}";

                var detailResult = await ExecuteGraphQLAsync(detailMutation);
                return IsOperationSuccessful(detailResult, "updatePromotionDetailByPromoDetailsId");
            }

            return true;
        }


        public async Task<bool> DeleteByIdAsync(int id)
        {
            var mutation = $@"
                mutation {{
                    deletePromotionByPromoId(input: {{
                        promoId: {id}
                    }}) {{
                        deletedPromotionId
                        clientMutationId
                    }}
                }}";

            var result = await ExecuteGraphQLAsync(mutation);
            return IsOperationSuccessful(result, "deletePromotionByPromoId");
        }

        public async Task<List<Promotion>> GetActivePromotionsAsync()
        {
            var query = $@"query ActivePromotions {{
                  allPromotions {{
                    nodes {{
                      endDate
                      menuItemIds
                      promoId
                      promoName
                      startDate
                      promotionDetailsByPromoId {{
                        nodes {{
                          description
                          discountType
                          discountValue
                          promoDetailsId
                          promoId
                        }}
                      }}
                    }}
                  }}
                }}";
            
            var result = await ExecuteGraphQLAsync(query);
            if (!IsOperationSuccessful(result, "allPromotions"))
                return new List<Promotion>();

            var now = DateTime.Now;
            var promotions = result["data"]?["allPromotions"]?["nodes"]
                .Select(node =>
                {
                    var startDate = node["startDate"]?.Value<DateTime>() ?? DateTime.MinValue;
                    var endDate = node["endDate"]?.Value<DateTime>() ?? DateTime.MaxValue;

                    var detailsNode = node["promotionDetailsByPromoId"]?["nodes"]?.FirstOrDefault();
                    return new Promotion
                    {
                        PromoID = node["promoId"].Value<int>(),
                        PromoName = node["promoName"].Value<string>(),
                        StartDate = startDate.Date.AddHours(0),          // force start at 00:00:00
                        EndDate = endDate.Date.AddHours(23).AddMinutes(59), // force end at 23:59
                        ItemIDs = node["menuItemIds"].ToObject<List<int>>(),
                        Details = detailsNode != null ? new PromotionDetails
                        {
                            PromoDetailsID = detailsNode["promoDetailsId"]?.Value<int>() ?? 0,
                            PromoID = detailsNode["promoId"]?.Value<int>() ?? node["promoId"].Value<int>(),
                            DiscountType = detailsNode["discountType"] != null
                                ? Enum.Parse<DiscountType>(detailsNode["discountType"].Value<string>())
                                : DiscountType.Percentage,
                            DiscountValue = detailsNode["discountValue"]?.Value<float>() ?? 0,
                            Description = detailsNode["description"]?.Value<string>()
                        } : new PromotionDetails()
                    };
                })
                .Where(p => p.StartDate <= now && p.EndDate >= now)
                .ToList();

            return promotions;
        }

    }
}