using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestB1_Task2_.Models;

namespace TestB1_Task2_.DAL
{
    public class FileInfoConfiguration : IEntityTypeConfiguration<FileInfo>
    {
        public void Configure(EntityTypeBuilder<FileInfo> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
