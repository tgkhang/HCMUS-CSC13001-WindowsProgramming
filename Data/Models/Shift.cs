using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_For_Small_Shop.Data.Models
{
    public class Shift
    {
        [Key]
        public int ShiftID { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; } // Nullable if shift is still open

        public float OpeningCash { get; set; }

        public float ClosingCash { get; set; } // Nullable if shift is still open

        public float TotalSales { get; set; }

        public int TotalOrders { get; set; }

        public string Status { get; set; } = "Open"; // Default value
    }


}
