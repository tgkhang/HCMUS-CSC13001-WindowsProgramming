using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PropertyChanged;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Ingredient
    {
        [JsonProperty("ingredient_id")]
        public int IngredientID { get; set; }

        [JsonProperty("ingredient_name")]
        public string IngredientName { get; set; } = "";

        [JsonProperty("category_id")]
        public int CategoryID { get; set; }

        [JsonProperty("stock")]
        public float Stock { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; } = "kg";

        [JsonProperty("purchasePrice")]
        public float PurchasePrice { get; set; }

        [JsonProperty("supplier")]
        public string Supplier { get; set; } = "";

        [JsonProperty("expiry_date")]
        public DateTime? ExpiryDate { get; set; }
    }

}
