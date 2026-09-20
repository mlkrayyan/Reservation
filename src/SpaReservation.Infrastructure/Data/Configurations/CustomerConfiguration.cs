using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaReservation.Domain.Customers;

namespace SpaReservation.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.FullName).IsRequired().HasMaxLength(150);
        builder.Property(c => c.Email).HasMaxLength(100);
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.PreferredLanguage).HasMaxLength(10).HasDefaultValue("ar");
        builder.Property(c => c.ExternalAuthId).HasMaxLength(250);
        builder.Property(c => c.AuthProvider).HasMaxLength(50);

        builder.HasMany(c => c.LoyaltyTransactions)
            .WithOne()
            .HasForeignKey(lt => lt.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class LoyaltyTransactionConfiguration : IEntityTypeConfiguration<LoyaltyTransaction>
{
    public void Configure(EntityTypeBuilder<LoyaltyTransaction> builder)
    {
        builder.HasKey(lt => lt.Id);
        builder.Property(lt => lt.Reason).IsRequired().HasMaxLength(250);
    }
}
