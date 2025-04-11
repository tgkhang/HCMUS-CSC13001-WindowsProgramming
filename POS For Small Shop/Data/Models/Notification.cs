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
        [JsonProperty("NotificationID")]
        public int NotificationID { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; } = "";

        [JsonProperty("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("IsRead")]
        public bool IsRead { get; set; }
    }

}
