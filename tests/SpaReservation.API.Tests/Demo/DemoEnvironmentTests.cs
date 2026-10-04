using System.Net;
using System.Net.Sockets;
using FluentAssertions;
using SpaReservation.API.Demo;

namespace SpaReservation.API.Tests.Demo;

public class DemoEnvironmentTests
{
    [Fact]
    public void IsPortAvailable_WhenNothingIsListening_ReturnsTrue()
    {
        var port = GetFreePort();

        DemoEnvironment.IsPortAvailable(port).Should().BeTrue();
    }

    [Fact]
    public void IsPortAvailable_WhenPortIsOccupied_ReturnsFalse()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var occupiedPort = ((IPEndPoint)listener.LocalEndpoint).Port;

        try
        {
            DemoEnvironment.IsPortAvailable(occupiedPort).Should().BeFalse();
        }
        finally
        {
            listener.Stop();
        }
    }

    [Fact]
    public void ResolveUrl_WhenPreferredPortIsFree_UsesPreferredPort()
    {
        var port = GetFreePort();

        DemoEnvironment.ResolveUrl(port).Should().Be($"http://localhost:{port}");
    }

    [Fact]
    public void ResolveUrl_WhenPreferredPortIsOccupied_FallsBackToEphemeralPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var occupiedPort = ((IPEndPoint)listener.LocalEndpoint).Port;

        try
        {
            DemoEnvironment.ResolveUrl(occupiedPort).Should().Be("http://localhost:0");
        }
        finally
        {
            listener.Stop();
        }
    }

    private static int GetFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
