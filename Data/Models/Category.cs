
using System.ComponentModel.DataAnnotations;


namespace POS_For_Small_Shop.Data.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required]
        public string Name { get; set; } = ""; // Default value prevents warning

        public string? Description { get; set; } // Nullable (Fix warning)
    }


}
