using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestB1_Task2_.Models;

namespace TestB1_Task2_.DAL
{
    public interface IDBAccessor
    {
        Task<List<BalanceInfoFile>> GetFiles();
        Task<BalanceInfoFile> GetFile(int fileId);
        BalanceInfoFile GetFileByName(string fileName);
        Task<List<BalanceInfoRecord>> GetFileContent(int fileId);
        Task UploadFile(BalanceInfoFile fileInfo, List<BalanceInfoRecord> records);
    }
}
