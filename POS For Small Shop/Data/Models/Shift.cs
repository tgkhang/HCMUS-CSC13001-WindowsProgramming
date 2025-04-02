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
    public class Shift
    {
        [JsonProperty("ShiftID")]
        public int ShiftID { get; set; }

        [JsonProperty("StartTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("EndTime")]
        public DateTime? EndTime { get; set; }

        [JsonProperty("OpeningCash")]
        public float OpeningCash { get; set; }

        [JsonProperty("TotalSales")]
        public float TotalSales { get; set; }

        [JsonProperty("TotalOrders")]
        public int TotalOrders { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; } = "Open";
    }


}
