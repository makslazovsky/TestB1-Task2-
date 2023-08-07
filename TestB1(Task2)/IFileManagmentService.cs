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
        Task<List<FileInfo>> GetFiles();
        Task<FileInfo> GetFile(int fileId);
        Task<List<FileRecord>> GetFileContent(int fileId);
        Task UploadFile(string filePath);
    }
}
