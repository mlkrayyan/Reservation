using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaReservation.Domain.Waitlist;

namespace SpaReservation.Infrastructure.Data.Configurations;

public class WaitlistEntryConfiguration : IEntityTypeConfiguration<WaitlistEntry>
{
    public void Configure(EntityTypeBuilder<WaitlistEntry> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Status).HasConversion<string>().HasMaxLength(20);
        
        builder.HasOne<SpaReservation.Domain.Customers.Customer>()
            .WithMany()
            .HasForeignKey(w => w.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SpaReservation.Domain.Services.SalonService>()
            .WithMany()
            .HasForeignKey(w => w.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SpaReservation.Domain.Staff.StaffMember>()
            .WithMany()
            .HasForeignKey(w => w.PreferredStaffId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
