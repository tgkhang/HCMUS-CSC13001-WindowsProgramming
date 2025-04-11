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
    public class CashFlow
    {
        [JsonProperty("CashFlowID")]
        public int CashFlowID { get; set; }

        [JsonProperty("ShiftID")]
        public int ShiftID { get; set; }

        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; } = "";

        [JsonProperty("Amount")]
        public float Amount { get; set; }

        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
