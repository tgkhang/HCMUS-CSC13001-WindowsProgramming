
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PropertyChanged;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class OrderDetail
    {
        [JsonProperty("OrderDetailID")]
        public int OrderDetailID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("MenuItemID")]
        public int MenuItemID { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("UnitPrice")]
        public float UnitPrice { get; set; }

        [JsonProperty("Subtotal")]
        public float Subtotal { get; set; }
    }
}
