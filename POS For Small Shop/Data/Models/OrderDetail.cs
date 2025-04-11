
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PropertyChanged;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class OrderDetail
    {
        [JsonProperty("order_detail_id")]
        public int OrderDetailID { get; set; }

        [JsonProperty("order_id")]
        public int OrderID { get; set; }

        [JsonProperty("menu_item_id")]
        public int MenuItemID { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unit_price")]
        public float UnitPrice { get; set; }

        [JsonProperty("subtotal")]
        public float Subtotal { get; set; }
    }
}
