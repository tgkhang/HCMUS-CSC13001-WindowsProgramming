using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_For_Small_Shop.Data.Models
{
    public class ShiftOrder
    {
        [Key]
        public int ShiftOrderID { get; set; }

        public int ShiftID { get; set; }

        public int OrderID { get; set; }
    }

}
