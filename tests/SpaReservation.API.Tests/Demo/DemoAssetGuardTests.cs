using FluentAssertions;
using SpaReservation.API.Demo;

namespace SpaReservation.API.Tests.Demo;

public class DemoAssetGuardTests : IDisposable
{
    private readonly string _tempRoot =
        Path.Combine(Path.GetTempPath(), "spademo-" + Guid.NewGuid().ToString("N"));

    public DemoAssetGuardTests() => Directory.CreateDirectory(_tempRoot);

    [Fact]
    public void SpaAssetsPresent_WhenIndexHtmlExists_ReturnsTrue()
    {
        File.WriteAllText(Path.Combine(_tempRoot, "index.html"), "<html></html>");

        DemoAssetGuard.SpaAssetsPresent(_tempRoot).Should().BeTrue();
    }

    [Fact]
    public void SpaAssetsPresent_WhenIndexHtmlMissing_ReturnsFalse()
    {
        DemoAssetGuard.SpaAssetsPresent(_tempRoot).Should().BeFalse();
    }

    [Fact]
    public void SpaAssetsPresent_WhenWebRootPathIsNullOrEmpty_ReturnsFalse()
    {
        DemoAssetGuard.SpaAssetsPresent(null).Should().BeFalse();
        DemoAssetGuard.SpaAssetsPresent(string.Empty).Should().BeFalse();
    }

    [Fact]
    public void MissingAssetsMessage_TellsTheOperatorHowToFixIt()
    {
        DemoAssetGuard.MissingAssetsMessage.Should().Contain("build-demo.ps1");
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRoot))
        {
            Directory.Delete(_tempRoot, recursive: true);
        }
    }
}
