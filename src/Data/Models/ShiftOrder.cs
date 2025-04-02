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
    public class ShiftOrder
    {
        [Key]
        public int ShiftOrderID { get; set; }

        public int ShiftID { get; set; }

        public int OrderID { get; set; }
    }

}
