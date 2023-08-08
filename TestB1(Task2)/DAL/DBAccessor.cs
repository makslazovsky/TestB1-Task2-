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

        public async Task<List<BalanceInfoRecord>> GetFileContent(int fileId)
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                return await context.FileRecords.AsNoTracking().Where(x => x.FileInfoId == fileId).ToListAsync();
            }
        }

        public async Task<BalanceInfoFile> GetFile(int fileId)
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                return await context.FileInfos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == fileId);
            }
        }

        public BalanceInfoFile GetFileByName(string fileName)
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                var test = context.FileInfos.AsNoTracking().FirstOrDefault(x => x.FileName == fileName);
                return test;
            }
        }

        public async Task<List<BalanceInfoFile>> GetFiles()
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                return await context.FileInfos.AsNoTracking().ToListAsync();
            }
        }

        public async Task UploadFile(BalanceInfoFile fileInfo, List<BalanceInfoRecord> records)
        {
            using (var context = dbContextFactory.CreateDbContext())
            {
                var proxyFileInfo = context.FileInfos.Add(fileInfo).Entity;
                await context.SaveChangesAsync();

                foreach (var record in records)
                {
                    record.FileInfoId = proxyFileInfo.Id;
                    context.FileRecords.Add(record);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
