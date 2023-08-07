using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestB1_Task2_.Models
{
    public class FileRecord
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public FileInfo File { get; set; }

        public int AccountNumber { get; set; }
    }
}
