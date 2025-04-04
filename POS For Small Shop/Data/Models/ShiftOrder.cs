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
    public class ShiftOrder
    {
        [JsonProperty("shift_order_id")]
        public int ShiftOrderID { get; set; }

        [JsonProperty("shift_id")]
        public int ShiftID { get; set; }

        [JsonProperty("order_id")]
        public int OrderID { get; set; }
    }

}
