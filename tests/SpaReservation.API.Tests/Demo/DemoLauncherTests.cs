using FluentAssertions;
using SpaReservation.API.Demo;

namespace SpaReservation.API.Tests.Demo;

public class DemoLauncherTests
{
    [Fact]
    public void ResolveListeningUrl_WhenNoAddresses_ReturnsNull()
    {
        DemoLauncher.ResolveListeningUrl(Array.Empty<string>()).Should().BeNull();
    }

    [Fact]
    public void ResolveListeningUrl_WithLoopbackAddress_ReturnsItUnchanged()
    {
        DemoLauncher.ResolveListeningUrl(new[] { "http://localhost:5139" })
            .Should().Be("http://localhost:5139");
    }

    [Fact]
    public void ResolveListeningUrl_WithWildcardIpv6Address_RewritesToLocalhost()
    {
        DemoLauncher.ResolveListeningUrl(new[] { "http://[::]:5139" })
            .Should().Be("http://localhost:5139");
    }

    [Fact]
    public void ResolveListeningUrl_WithWildcardIpv4Address_RewritesToLocalhost()
    {
        DemoLauncher.ResolveListeningUrl(new[] { "http://0.0.0.0:5139" })
            .Should().Be("http://localhost:5139");
    }

    [Fact]
    public void ResolveListeningUrl_WithSeveralAddresses_TakesTheFirst()
    {
        DemoLauncher.ResolveListeningUrl(new[] { "http://localhost:5139", "http://localhost:9999" })
            .Should().Be("http://localhost:5139");
    }

    [Fact]
    public void BuildBanner_ContainsTheUrlAndHowToStop()
    {
        var banner = DemoLauncher.BuildBanner("http://localhost:5139");

        banner.Should().Contain("http://localhost:5139");
        banner.Should().Contain("Close this window");
    }
}
