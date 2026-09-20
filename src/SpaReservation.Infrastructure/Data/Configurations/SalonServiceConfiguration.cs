using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaReservation.Domain.Services;

namespace SpaReservation.Infrastructure.Data.Configurations;

public class SalonServiceConfiguration : IEntityTypeConfiguration<SalonService>
{
    public void Configure(EntityTypeBuilder<SalonService> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.NameAr).IsRequired().HasMaxLength(150);
        builder.Property(s => s.NameEn).IsRequired().HasMaxLength(150);
        builder.Property(s => s.DescriptionAr).HasMaxLength(1000);
        builder.Property(s => s.DescriptionEn).HasMaxLength(1000);
        builder.Property(s => s.Category).HasConversion<string>().HasMaxLength(50);
        builder.Property(s => s.Price).HasPrecision(18, 2);
    }
}
