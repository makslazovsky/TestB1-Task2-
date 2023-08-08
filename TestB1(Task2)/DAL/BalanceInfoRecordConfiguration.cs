using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using TestB1_Task2_.Models;

namespace TestB1_Task2_.DAL
{
    public class BalanceInfoRecordConfiguration : IEntityTypeConfiguration<BalanceInfoRecord>
    {
        public void Configure(EntityTypeBuilder<BalanceInfoRecord> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.ParentAccountNumber).IsRequired(false);
            
            builder.Ignore(x => x.Children);
            
            builder.HasOne(x => x.FileInfo).WithMany(x => x.Records).HasForeignKey(x => x.FileInfoId);
        }
    }
}
