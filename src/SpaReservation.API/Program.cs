using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.OpenApi.Models;
using Hangfire;
using SpaReservation.API.Demo;
using SpaReservation.Infrastructure;
using SpaReservation.Infrastructure.Data;
using SpaReservation.Application;
using SpaReservation.Application.BackgroundJobs;
using SpaReservation.API.Middleware;

// No ASPNETCORE_ENVIRONMENT means nobody launched us through a profile: this is
// the standalone demo exe being run directly. Two things follow from that.
//
// 1. Default the environment to Demo, before CreateBuilder reads configuration.
// 2. Pin the working directory to the executable's own folder. ASP.NET derives
//    its content root — and therefore wwwroot — from the current directory, so
//    launching the exe from anywhere other than its own folder would otherwise
//    hide the bundled SPA.
//
// Tests and `dotnet run` set ASPNETCORE_ENVIRONMENT explicitly, so neither is
// affected by this block.
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
{
    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", DemoEnvironment.Name);
    Directory.SetCurrentDirectory(DemoAssetGuard.ResolveContentRoot());
}

var builder = WebApplication.CreateBuilder(args);
var isDemo = builder.Environment.IsEnvironment(DemoEnvironment.Name);

if (isDemo)
{
    builder.WebHost.UseUrls(DemoEnvironment.ResolveUrl(DemoEnvironment.PreferredPort));
}

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application layer services (MediatR)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SpaReservation.Application.Bookings.Commands.CreateBookingCommand).Assembly));

var app = builder.Build();

// A demo build with no SPA inside it would start cleanly and serve 404s, which
// reads as a broken app rather than a broken build. Say what actually happened.
if (isDemo && !DemoAssetGuard.SpaAssetsPresent(app.Environment.WebRootPath))
{
    Console.Error.WriteLine(DemoAssetGuard.MissingAssetsMessage);
    Console.Error.WriteLine(DemoAssetGuard.DescribeSearchedPaths(
        app.Environment.ContentRootPath, app.Environment.WebRootPath));
    return 1;
}

// Seed Database on Startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DbSeeder.SeedAsync(context);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || isDemo)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// The demo is plain http on loopback; redirecting would send every request to
// an https endpoint that is not bound.
if (!isDemo)
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

if (isDemo)
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseAuthentication();
app.UseAuthorization();

// Hangfire Dashboard & Jobs
app.UseHangfireDashboard("/hangfire");

using (var scope = app.Services.CreateScope())
{
    var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJobs.AddOrUpdate<IBookingReminderJob>("reminder-24h", job => job.Send24hRemindersAsync(), Cron.Daily);
    recurringJobs.AddOrUpdate<IBookingReminderJob>("reminder-1h", job => job.Send1hRemindersAsync(), Cron.Hourly);
    recurringJobs.AddOrUpdate<IBookingReminderJob>("waitlist-expire", job => job.ExpireWaitlistEntriesAsync(), Cron.Daily);
}

app.MapControllers();

if (isDemo)
{
    // More specific than the catch-all below, so unmatched /api routes answer
    // 404 instead of being handed the SPA document.
    app.MapFallback("/api/{**slug}", () => Results.NotFound());

    // Everything else is a client-side route.
    app.MapFallbackToFile("index.html");

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var addresses = app.Services.GetRequiredService<IServer>()
            .Features.Get<IServerAddressesFeature>()?.Addresses
            ?? Array.Empty<string>();

        var url = DemoLauncher.ResolveListeningUrl(addresses)
                  ?? $"http://localhost:{DemoEnvironment.PreferredPort}";

        Console.WriteLine(DemoLauncher.BuildBanner(url));

        if (!DemoLauncher.TryOpenBrowser(url, out var error))
        {
            Console.WriteLine($"Could not open a browser automatically — open {url} manually. ({error})");
        }
    });
}

app.Run();

return 0;

// Top-level statements generate an internal Program class. WebApplicationFactory
// needs it public, so widen it here.
public partial class Program { }
