using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        public string? Message { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }
    }


}
