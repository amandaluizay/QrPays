using Microsoft.EntityFrameworkCore;
using QrPay.Domain.Entities;
using QrPay.Domain.Interfaces;
using QrPay.Shared.Interfaces;

namespace QrPay.Database
{
    public class QrPayDbContext(DbContextOptions<QrPayDbContext> options, IUserContext userContext) : DbContext(options)
    {
        private readonly IUserContext _currentUserService = userContext;

        DbSet<User> Users { get; set; }
        DbSet<Account> Accounts { get; set; }
        DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUserService.UserName;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = _currentUserService.UserName;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
