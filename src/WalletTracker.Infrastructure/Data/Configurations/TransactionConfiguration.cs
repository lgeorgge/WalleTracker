using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount).HasPrecision(18, 2);

        builder.Property(x => x.Description).IsRequired().HasMaxLength(500);

        builder.Property(x => x.Type).IsRequired();

        builder.Property(x => x.WalletId).IsRequired();
    }
}
