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
        [JsonProperty("IngredientID")]
        public int IngredientID { get; set; }

        [JsonProperty("IngredientName")]
        public string IngredientName { get; set; } = "";

        [JsonProperty("CategoryID")]
        public int CategoryID { get; set; }

        [JsonProperty("Stock")]
        public float Stock { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; } = "kg";

        [JsonProperty("PurchasePrice")]
        public float PurchasePrice { get; set; }

        [JsonProperty("Supplier")]
        public string Supplier { get; set; } = "";

        [JsonProperty("ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }
    }

}
