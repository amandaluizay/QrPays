using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrPay.Database.Configuration.Shared;
using QrPay.Domain.Entities;

namespace QrPay.Database.Configuration
{
    public class TransactionEntityConfiguration : EntityConfiguration<Transaction>
    {
        public override void Configure(EntityTypeBuilder<Transaction> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.AccountId).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Type).IsRequired();
            builder.Property(p => p.Amount).HasPrecision(18, 2);
            builder.Property(p => p.TransactionDate).IsRequired();
        }
    }
}
