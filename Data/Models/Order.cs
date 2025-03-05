using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_For_Small_Shop.Data.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public int? CustomerID { get; set; }

        public int ShiftID { get; set; }

        public float TotalAmount { get; set; }

        public float Discount { get; set; }

        public float FinalAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = "Cash";

        [Required]
        public string Status { get; set; } = "Pending"; 
    }


}
