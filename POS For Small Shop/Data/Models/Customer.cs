
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PropertyChanged;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Customer
    {
        [JsonProperty("CustomerID")]
        public int CustomerID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; } = "";

        [JsonProperty("Phone")]
        public string Phone { get; set; } = "";

        [JsonProperty("Email")]
        public string? Email { get; set; }

        [JsonProperty("Address")]
        public string? Address { get; set; }

        [JsonProperty("LoyaltyPoints")]
        public int LoyaltyPoints { get; set; }
    }
}
