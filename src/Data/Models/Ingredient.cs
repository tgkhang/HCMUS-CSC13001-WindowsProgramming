using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;


namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Ingredient
    {
        [Key]
        public int IngredientID { get; set; }

        [Required]
        public string IngredientName { get; set; } = "";

        public int CategoryID { get; set; }

        public float Stock { get; set; }

        [Required]
        public string Unit { get; set; } = "kg";

        public float PurchasePrice { get; set; }

        [Required]
        public string Supplier { get; set; } = "BachHoaXanh";

        public DateTime? ExpiryDate { get; set; }
    }

}
