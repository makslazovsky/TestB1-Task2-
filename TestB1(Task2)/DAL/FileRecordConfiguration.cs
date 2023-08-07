using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TestB1_Task2_.Models;

namespace TestB1_Task2_.DAL
{
    public class FileRecordConfiguration : IEntityTypeConfiguration<FileRecord>
    {
        public void Configure(EntityTypeBuilder<FileRecord> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(x => x.File).WithMany(x => x.Records).HasForeignKey(x => x.FileId);
        }
    }
}
