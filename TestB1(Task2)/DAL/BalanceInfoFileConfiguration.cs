using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestB1_Task2_.Models;

namespace TestB1_Task2_.DAL
{
    public class BalanceInfoFileConfiguration : IEntityTypeConfiguration<BalanceInfoFile>
    {
        public void Configure(EntityTypeBuilder<BalanceInfoFile> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
