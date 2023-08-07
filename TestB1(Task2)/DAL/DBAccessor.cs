using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestB1_Task2_.Models;

namespace TestB1_Task2_.DAL
{
    public class DBAccessor : IDBAccessor
    {
        IFilesDbContextFactory dbContextFactory;

        public DBAccessor(IFilesDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public Task<List<FileRecord>> GetFileContent(int fileId)
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                return context.FileRecords.AsNoTracking().Where(x => x.FileId == fileId).ToListAsync();
            }
        }

        public Task<FileInfo> GetFile(int fileId)
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                return context.FileInfos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fileId);
            }
        }

        public Task<List<FileInfo>> GetFiles()
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                return context.FileInfos.AsNoTracking().ToListAsync();
            }
        }

        public async Task UploadFile(FileInfo fileInfo, List<FileRecord> records)
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                var proxyFileInfo = context.FileInfos.Add(fileInfo).Entity;

                foreach(var record in records)
                {
                    record.FileId = proxyFileInfo.Id;
                    fileInfo.Records.Add(record);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
