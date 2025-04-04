
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PropertyChanged;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Customer
    {
        [JsonProperty("customer_id")]
        public int CustomerID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("phone")]
        public string Phone { get; set; } = "";

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("loyalty_points")]
        public int LoyaltyPoints { get; set; }
    }
}
