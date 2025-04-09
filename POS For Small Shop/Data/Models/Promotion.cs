using System;
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

        public virtual PromotionDetails Details { get; set; }

        public List<int> ItemIDs { get; set; } = new List<int>();

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public Promotion()
        {
            Details = new PromotionDetails();
        }
    }
}
