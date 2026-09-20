using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SpaReservation.Infrastructure.Data;
using SpaReservation.Domain.Bookings;
using SpaReservation.Domain.Customers;
using SpaReservation.Domain.Services;
using SpaReservation.Domain.Staff;
using SpaReservation.Domain.Waitlist;
using SpaReservation.Domain.Notifications;
using SpaReservation.Infrastructure.Repositories;

namespace SpaReservation.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISalonServiceRepository, SalonServiceRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IWaitlistRepository, WaitlistRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
