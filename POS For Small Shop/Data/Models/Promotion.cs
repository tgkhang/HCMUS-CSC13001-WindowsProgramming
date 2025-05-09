﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Newtonsoft.Json;

namespace POS_For_Small_Shop.Data.Models
{

    [AddINotifyPropertyChangedInterface]
    public class Promotion
    {

        [Key]
        [JsonProperty("promo_id")]
        public int PromoID { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonProperty("promo_name")]
        public string PromoName { get; set; } = String.Empty;

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("menu_item_ids")]
        public List<int> ItemIDs { get; set; } = new List<int>();

        [JsonProperty("details")]
        public virtual PromotionDetails Details { get; set; }

        public Promotion()
        {
            Details = new PromotionDetails();
        }
    }
}
