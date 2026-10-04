using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using SpaReservation.API.Demo;

namespace SpaReservation.API.Tests;

/// <summary>
/// Boots the real API host in the Demo environment against a stub web root,
/// so SPA routing can be tested without a frontend build.
/// </summary>
public class DemoWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string StubIndexMarker = "<!-- stub-spa-index -->";

    private readonly string _stubWebRoot =
        Path.Combine(Path.GetTempPath(), "spademo-webroot-" + Guid.NewGuid().ToString("N"));

    public DemoWebApplicationFactory()
    {
        // Program.cs reads this before the host is built.
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", DemoEnvironment.Name);

        Directory.CreateDirectory(_stubWebRoot);
        File.WriteAllText(
            Path.Combine(_stubWebRoot, "index.html"),
            $"<html><body>{StubIndexMarker}</body></html>");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(DemoEnvironment.Name);
        builder.UseWebRoot(_stubWebRoot);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing && Directory.Exists(_stubWebRoot))
        {
            Directory.Delete(_stubWebRoot, recursive: true);
        }
    }
}
