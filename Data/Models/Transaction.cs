
using System.ComponentModel.DataAnnotations;


namespace POS_For_Small_Shop.Data.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        public int OrderID { get; set; }

        public float AmountPaid { get; set; }

        public string? PaymentMethod { get; set; }
    }


}
