

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PropertyChanged;

namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class MenuItem
    {
        [JsonProperty("menu_item_id")]
        public int MenuItemID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("category_id")]
        public int CategoryID { get; set; }

        [JsonProperty("selling_price")]
        public float SellingPrice { get; set; }

        [JsonProperty("image_path")]
        public string ImagePath { get; set; } = "";
    }
}
