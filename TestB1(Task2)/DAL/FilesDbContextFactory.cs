using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace TestB1_Task2_.DAL
{
    public class FilesDbContextFactory : IDesignTimeDbContextFactory<FilesDbContext>, IFilesDbContextFactory
    {
        private string connectionString;

        public FilesDbContextFactory(){}
        public FilesDbContextFactory(string connectionString) 
        {
            this.connectionString = connectionString;
        }

        public FilesDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext("");
        }

        public FilesDbContext CreateDbContext()
        {
            return CreateDbContext(connectionString);
        }

        public FilesDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder().UseSqlServer(connectionString);
            return new FilesDbContext(optionsBuilder.Options);
        }
    }
}
