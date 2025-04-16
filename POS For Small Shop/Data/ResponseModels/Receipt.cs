using System.Collections.Generic;
using Newtonsoft.Json;
using PropertyChanged;

namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Receipt
    {
        [JsonProperty("data")]
        public OrderDetailResponse Data { get; set; }
    }
    [AddINotifyPropertyChangedInterface]
    public class OrderDetailWithMenuItem
    {
        [JsonProperty("orderDetailId")]
        public int OrderDetailId { get; set; }

        [JsonProperty("orderId")]
        public int OrderId { get; set; }

        [JsonProperty("menuItemId")]
        public int MenuItemId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unitPrice")]
        public float UnitPrice { get; set; }

        [JsonProperty("subtotal")]
        public float Subtotal { get; set; }

        [JsonProperty("menuItemByMenuItemId")]
        public MenuItemInfo MenuItemByMenuItemId { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class MenuItemInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sellingPrice")]
        public float SellingPrice { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class OrderDetailNodes
    {
        [JsonProperty("nodes")]
        public List<OrderDetailWithMenuItem> Nodes { get; set; }

        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class OrderDetailResponse
    {
        [JsonProperty("allOrderDetails")]
        public OrderDetailNodes AllOrderDetails { get; set; }
    }

    
  
}