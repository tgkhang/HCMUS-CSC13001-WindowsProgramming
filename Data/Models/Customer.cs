
using System.ComponentModel.DataAnnotations;


namespace POS_For_Small_Shop.Data.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        public string Name { get; set; } = ""; // Default value prevents warning

        public string? Phone { get; set; } // Nullable (Fix warning)

        public string? Email { get; set; } // Nullable (Fix warning)

        public string? Address { get; set; } // Nullable (Fix warning)

        public int LoyaltyPoints { get; set; }
    }


}
