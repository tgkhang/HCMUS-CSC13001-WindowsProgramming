using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyChanged;

namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Order
    {
        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("CustomerID")]
        public int? CustomerID { get; set; }

        [JsonProperty("ShiftID")]
        public int ShiftID { get; set; }

        [JsonProperty("TotalAmount")]
        public float TotalAmount { get; set; }

        [JsonProperty("Discount")]
        public float Discount { get; set; }

        [JsonProperty("FinalAmount")]
        public float FinalAmount { get; set; }

        [JsonProperty("PaymentMethod")]
        public string PaymentMethod { get; set; } = "Cash";

        [JsonProperty("Status")]
        public string Status { get; set; } = "Pending";
    }


}
