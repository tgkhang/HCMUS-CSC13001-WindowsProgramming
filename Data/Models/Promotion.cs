using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Promotion
    {
        [Key]
        public int PromoID { get; set; }

        [Required]
        public string PromoName { get; set; } = "Speacial sales"; // Default value prevents warning

        public string DiscountType { get; set; } = "Percentages"; // Nullable (Fix warning)

        public float DiscountValue { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }


}
