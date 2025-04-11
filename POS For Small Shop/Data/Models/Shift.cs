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
        [JsonProperty("shift_id")]
        public int ShiftID { get; set; }

        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("end_time")]
        public DateTime? EndTime { get; set; }

        [JsonProperty("opening_cash")]
        public float OpeningCash { get; set; }

        [JsonProperty("closing_cash")]
        public float ClosingCash { get; set; }

        [JsonProperty("total_sales")]
        public float TotalSales { get; set; }

        [JsonProperty("total_orders")]
        public int TotalOrders { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = "Open";
    }


}
