using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestB1_Task2_.Models
{
    public class BalanceInfoFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public List<BalanceInfoRecord> Records { get; set; }
    }
}
