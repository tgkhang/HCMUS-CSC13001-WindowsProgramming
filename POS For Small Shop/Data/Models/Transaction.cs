using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PropertyChanged;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Transaction
    {
        [JsonProperty("TransactionID")]
        public int TransactionID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("AmountPaid")]
        public float AmountPaid { get; set; }

        [JsonProperty("PaymentMethod")]
        public string PaymentMethod { get; set; } = "";
    }
}
