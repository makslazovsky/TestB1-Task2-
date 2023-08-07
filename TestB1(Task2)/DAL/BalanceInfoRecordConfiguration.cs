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
            builder.HasOne(x => x.FileInfo).WithMany(x => x.Records).HasForeignKey(x => x.FileInfoId);
        }
    }
}
