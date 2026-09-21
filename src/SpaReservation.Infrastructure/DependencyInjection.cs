using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Hangfire;
using Hangfire.InMemory;
using SpaReservation.Infrastructure.Data;
using SpaReservation.Domain.Bookings;
using SpaReservation.Domain.Customers;
using SpaReservation.Domain.Services;
using SpaReservation.Domain.Staff;
using SpaReservation.Domain.Waitlist;
using SpaReservation.Domain.Notifications;
using SpaReservation.Domain.SharedKernel;
using SpaReservation.Infrastructure.Repositories;
using SpaReservation.Application.BackgroundJobs;
using SpaReservation.Infrastructure.BackgroundJobs;

namespace SpaReservation.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("SpaReservationDb"));

        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISalonServiceRepository, SalonServiceRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IWaitlistRepository, WaitlistRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBookingReminderJob, BookingReminderJob>();

        // Hangfire Configuration
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseInMemoryStorage());

        services.AddHangfireServer();

        return services;
    }
}
