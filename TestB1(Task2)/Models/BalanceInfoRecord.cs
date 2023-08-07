using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestB1_Task2_.Models
{
    public class BalanceInfoRecord
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal OpeningBalanceAsset { get; set; }
        public decimal OpeningBalanceLiability { get; set; }
        public decimal DebitTurnover { get; set; }
        public decimal CreditTurnover { get; set; }
        public decimal ClosingBalanceAsset { get; set; }
        public decimal ClosingBalanceLiability { get; set; }
        
        public int FileInfoId { get; set; }
        public BalanceInfoFile FileInfo { get; set; }


    }
}
