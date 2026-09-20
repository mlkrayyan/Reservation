using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaReservation.Domain.Bookings;

namespace SpaReservation.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);
        builder.OwnsOne(b => b.Slot, s =>
        {
            s.Property(p => p.Date).HasColumnName("SlotDate");
            s.Property(p => p.StartTime).HasColumnName("SlotStartTime");
            s.Property(p => p.EndTime).HasColumnName("SlotEndTime");
        });
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(b => b.Notes).HasMaxLength(500);
        builder.Property(b => b.CancellationReason).HasMaxLength(250);
        
        builder.HasOne<SpaReservation.Domain.Customers.Customer>()
            .WithMany()
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SpaReservation.Domain.Staff.StaffMember>()
            .WithMany()
            .HasForeignKey(b => b.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SpaReservation.Domain.Services.SalonService>()
            .WithMany()
            .HasForeignKey(b => b.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
