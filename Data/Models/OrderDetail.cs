
using System.ComponentModel.DataAnnotations;


namespace POS_For_Small_Shop.Data.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int MenuItemID { get; set; }

        public int Quantity { get; set; }

        public float UnitPrice { get; set; }

        public float Subtotal { get; set; }
    }

}
