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
        [MaxLength(100)]
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
