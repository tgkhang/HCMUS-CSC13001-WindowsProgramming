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
        [JsonProperty("cash_flow_id")]
        public int CashFlowID { get; set; }

        [JsonProperty("shift_id")]
        public int ShiftID { get; set; }

        [JsonProperty("transaction_type")]
        public string TransactionType { get; set; } = "";

        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
