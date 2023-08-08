using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestB1_Task2_.Models;

namespace TestB1_Task2_
{
    public interface IFileManagmentService
    {
        Task<List<BalanceInfoFile>> GetFiles();
        Task<BalanceInfoFile> GetFile(int fileId);
        BalanceInfoFile GetFileByName(string fileName);
        Task<BalanceInfoRecord> GetRootRecordOfFileContent(int fileId);
        Task UploadFile(string filePath);
    }
}
