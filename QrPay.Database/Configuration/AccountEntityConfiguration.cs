using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrPay.Database.Configuration.Shared;
using QrPay.Domain.Entities;

namespace QrPay.Database.Configuration
{
    public class AccountEntityConfiguration : EntityConfiguration<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Balance).HasPrecision(18, 2);
            builder.Property(p => p.AccountNumber).HasMaxLength(20).IsRequired();
            builder.Property(p => p.UserId).IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(p => p.Accounts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
