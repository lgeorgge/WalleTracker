using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Infrastructure.Data.Configurations;

public class UserCOnfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Set Name
        builder.ToTable("Users");

        // Set Primary Key
        builder.HasKey(U => U.Id);

        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);

        builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);

        builder.Property(x => x.Email).IsRequired().HasMaxLength(255);

        builder.Property(x => x.PasswordHash).IsRequired();
        builder.Property(x => x.UserRole).IsRequired();

        // Indexes
        builder.HasIndex(U => U.Email).IsUnique();
    }
}
