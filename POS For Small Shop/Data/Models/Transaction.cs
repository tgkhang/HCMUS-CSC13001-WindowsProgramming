using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PropertyChanged;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Transaction
    {
        [JsonProperty("transaction_id")]
        public int TransactionID { get; set; }

        [JsonProperty("order_id")]
        public int OrderID { get; set; }

        [JsonProperty("amount_paid")]
        public float AmountPaid { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; } = "";
    }
}
