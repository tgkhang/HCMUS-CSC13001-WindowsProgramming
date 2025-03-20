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
        BuyOneGetOne,
        BuyTwoGetOne
    }

    [AddINotifyPropertyChangedInterface]
    public class PromotionDetails
    {
        [Key]
        public int PromoDetailsID { get; set; }

        [Required]
        public int PromoID { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; } = DiscountType.Percentage;

        [ForeignKey("PromoID")]
        public virtual Promotion Promotion { get; set; }


        public float DiscountValue { get; set; }
        public string? Description { get; set; }
    }
}
