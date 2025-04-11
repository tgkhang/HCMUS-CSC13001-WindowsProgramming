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
        [JsonProperty("ShiftOrderID")]
        public int ShiftOrderID { get; set; }

        [JsonProperty("ShiftID")]
        public int ShiftID { get; set; }

        [JsonProperty("OrderID")]
        public int OrderID { get; set; }
    }

}
