using System.Collections.Generic;

namespace TestB1_Task2_.Models
{
    public class BalanceInfoRecord
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int AccountNumber { get; set; }
        public int? ParentAccountNumber { get; set; }


        public string Description { get; set; }
        public decimal OpeningBalanceAsset { get; set; }
        public decimal OpeningBalanceLiability { get; set; }
        public decimal DebitTurnover { get; set; }
        public decimal CreditTurnover { get; set; }
        public decimal ClosingBalanceAsset { get; set; }
        public decimal ClosingBalanceLiability { get; set; }

        public int FileInfoId { get; set; }
        public BalanceInfoFile FileInfo { get; set; }

        public List<BalanceInfoRecord> Children { get; set; } = new List<BalanceInfoRecord>();
    }
}
