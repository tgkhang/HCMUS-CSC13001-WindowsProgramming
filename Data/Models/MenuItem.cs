

using System.ComponentModel.DataAnnotations;

namespace POS_For_Small_Shop.Data.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemID { get; set; }

        [Required]
        public string Name { get; set; } = ""; // Default value prevents warning

        public int CategoryID { get; set; }

        public float SellingPrice { get; set; }

        public string? ImagePath { get; set; } // Nullable (Fix warning)
    }


}
