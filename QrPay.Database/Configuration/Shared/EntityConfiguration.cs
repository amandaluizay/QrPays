using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using QrPay.Domain.Entities.Shared;

namespace QrPay.Database.Configuration.Shared
{
    public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id);

            builder.Property(p => p.CreatedBy).HasMaxLength(120).IsRequired();
            builder.Property(p => p.CreatedAt);

            builder.Property(p => p.UpdatedBy).HasMaxLength(120);
            builder.Property(p => p.UpdatedAt);
        }
    }
}
