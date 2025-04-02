
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
        [JsonProperty("CategoryID")]
        public int CategoryID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; } = "";

        [JsonProperty("Description")]
        public string? Description { get; set; }
    }
}