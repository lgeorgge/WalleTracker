using Microsoft.EntityFrameworkCore;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Infrastructure.Data;

public class WalletDBContext(DbContextOptions<WalletDBContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletDBContext).Assembly);
    }
}
