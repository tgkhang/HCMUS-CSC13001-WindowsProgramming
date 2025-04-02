using System.ComponentModel.DataAnnotations;
using PropertyChanged;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        public int OrderID { get; set; }

        public float AmountPaid { get; set; }

        public string? PaymentMethod { get; set; }
    }


}
