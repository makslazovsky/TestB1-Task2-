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
        Task<List<FileInfo>> GetFiles();
        Task<FileInfo> GetFile(int fileId);
        Task<List<FileRecord>> GetFileContent(int fileId);
        Task UploadFile(FileInfo fileInfo, List<FileRecord> records);
    }
}
