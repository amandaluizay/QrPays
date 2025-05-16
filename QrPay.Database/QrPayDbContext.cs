using Microsoft.EntityFrameworkCore;
using QrPay.Domain.Entities;

namespace QrPay.Database
{
    public class QrPayDbContext(DbContextOptions<QrPayDbContext> options) : DbContext(options)
    {
        DbSet<User> Users { get; set; }
        DbSet<Account> Accounts { get; set; }
        DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
