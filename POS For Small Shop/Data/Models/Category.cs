
using System.ComponentModel.DataAnnotations;
using PropertyChanged;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Category
    {
        [JsonProperty("category_id")]
        public int CategoryID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}