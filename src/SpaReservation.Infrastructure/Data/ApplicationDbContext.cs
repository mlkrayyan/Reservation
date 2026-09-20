using Microsoft.EntityFrameworkCore;
using SpaReservation.Domain.Bookings;
using SpaReservation.Domain.Customers;
using SpaReservation.Domain.Notifications;
using SpaReservation.Domain.Services;
using SpaReservation.Domain.Staff;
using SpaReservation.Domain.Waitlist;
using System.Reflection;

namespace SpaReservation.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<SalonService> Services => Set<SalonService>();
    public DbSet<StaffMember> Staff => Set<StaffMember>();
    public DbSet<WaitlistEntry> Waitlist => Set<WaitlistEntry>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<LoyaltyTransaction> LoyaltyTransactions => Set<LoyaltyTransaction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
