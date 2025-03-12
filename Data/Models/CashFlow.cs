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
    public class CashFlow
    {
        [Key]
        public int CashFlowID { get; set; }

        public int ShiftID { get; set; }

        public string TransactionType { get; set; } = "Cash";

        public float Amount { get; set; }

        public DateTime Timestamp { get; set; }
    }


}
