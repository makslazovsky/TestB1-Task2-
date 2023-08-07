using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace TestB1_Task2_.DAL
{
    public class FilesDbContextFactory : IDesignTimeDbContextFactory<FilesDbContext>, IFilesDbContextFactory
    {
        string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=b1db;Integrated Security=True;TrustServerCertificate=True";

        public FilesDbContext CreateDbContext(string[] args)
        {
            return CreateDbContext();
        }

        public FilesDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder().UseSqlServer(connectionString);
            return new FilesDbContext(optionsBuilder.Options);
        }
    }
}
