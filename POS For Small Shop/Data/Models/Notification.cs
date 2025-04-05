using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyChanged;

namespace POS_For_Small_Shop.Data.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Notification
    {
        [JsonProperty("notification_id")]
        public int NotificationID { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = "";

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("is_read")]
        public bool IsRead { get; set; }
    }
}
