using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrPay.Database.Configuration.Shared;
using QrPay.Domain.Entities;

namespace QrPay.Database.Configuration
{
    public class UserEntityConfiguration : EntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.UserName).HasMaxLength(120).IsRequired();
            builder.Property(p => p.Email).HasMaxLength(120).IsRequired();
            builder.Property(p => p.PasswordHash).HasMaxLength(120).IsRequired();
        }
    }
}
