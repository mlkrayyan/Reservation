using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaReservation.Domain.Staff;

namespace SpaReservation.Infrastructure.Data.Configurations;

public class StaffMemberConfiguration : IEntityTypeConfiguration<StaffMember>
{
    public void Configure(EntityTypeBuilder<StaffMember> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.FullName).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Bio).HasMaxLength(1000);
        builder.Property(s => s.PhotoUrl).HasMaxLength(500);
        
        builder.HasMany(s => s.WorkingSchedules)
            .WithOne()
            .HasForeignKey(ws => ws.StaffMemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.TimeOffs)
            .WithOne()
            .HasForeignKey(t => t.StaffMemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
