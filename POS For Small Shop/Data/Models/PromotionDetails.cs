using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_For_Small_Shop.Data.Models
{
    public enum DiscountType
    {
        Percentage,
        FixedAmount,
    }

    [AddINotifyPropertyChangedInterface]
    public class PromotionDetails
    {
        [Key]
        [JsonProperty("promo_details_id")]
        public int PromoDetailsID { get; set; }

        [Required]
        [JsonProperty("promo_id")]
        public int PromoID { get; set; }

        [Required]
        [JsonProperty("discount_type")]
        public DiscountType DiscountType { get; set; } = DiscountType.Percentage;

        [JsonProperty("discount_value")]
        public float DiscountValue { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [ForeignKey("PromoID")]
        [JsonProperty("promotion")]
        public virtual Promotion Promotion { get; set; }
    }
}
