

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PropertyChanged;

namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class MenuItem
    {
        [JsonProperty("MenuItemID")]
        public int MenuItemID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; } = "";

        [JsonProperty("CategoryID")]
        public int CategoryID { get; set; }

        [JsonProperty("SellingPrice")]
        public float SellingPrice { get; set; }

        [JsonProperty("ImagePath")]
        public string ImagePath { get; set; } = "";
    }
}
