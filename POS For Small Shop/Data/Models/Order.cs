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
        [JsonProperty("order_id")]
        public int OrderID { get; set; }

        [JsonProperty("customer_id")]
        public int? CustomerID { get; set; }

        [JsonProperty("shift_id")]
        public int ShiftID { get; set; }

        [JsonProperty("total_amount")]
        public float TotalAmount { get; set; }

        [JsonProperty("discount")]
        public float Discount { get; set; }

        [JsonProperty("final_amount")]
        public float FinalAmount { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; } = "Cash";

        [JsonProperty("status")]
        public string Status { get; set; } = "Pending";
    }


}
