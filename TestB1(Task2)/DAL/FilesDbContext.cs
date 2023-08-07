using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TestB1_Task2_.Models;

namespace TestB1_Task2_.DAL
{
    public class FilesDbContext: DbContext
    {
        public DbSet<BalanceInfoFile> FileInfos { get; set; }
        public DbSet<BalanceInfoRecord> FileRecords { get; set; }

        public FilesDbContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BalanceInfoFileConfiguration());
            modelBuilder.ApplyConfiguration(new BalanceInfoRecordConfiguration());
        }
    }
}
