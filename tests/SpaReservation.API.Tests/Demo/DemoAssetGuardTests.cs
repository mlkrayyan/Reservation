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

    [Fact]
    public void ResolveContentRoot_PicksTheFirstCandidateThatHoldsAWwwroot()
    {
        var withoutWwwroot = Path.Combine(_tempRoot, "no-spa");
        var withWwwroot = Path.Combine(_tempRoot, "has-spa");
        Directory.CreateDirectory(withoutWwwroot);
        Directory.CreateDirectory(Path.Combine(withWwwroot, "wwwroot"));

        DemoAssetGuard.ResolveContentRoot(withoutWwwroot, withWwwroot)
            .Should().Be(withWwwroot);
    }

    [Fact]
    public void ResolveContentRoot_SkipsNullAndEmptyCandidates()
    {
        var withWwwroot = Path.Combine(_tempRoot, "has-spa");
        Directory.CreateDirectory(Path.Combine(withWwwroot, "wwwroot"));

        DemoAssetGuard.ResolveContentRoot(null, string.Empty, withWwwroot)
            .Should().Be(withWwwroot);
    }

    [Fact]
    public void ResolveContentRoot_WhenNoCandidateHoldsAWwwroot_LeavesCurrentDirectoryAlone()
    {
        var withoutWwwroot = Path.Combine(_tempRoot, "no-spa");
        Directory.CreateDirectory(withoutWwwroot);

        DemoAssetGuard.ResolveContentRoot(withoutWwwroot)
            .Should().Be(Directory.GetCurrentDirectory());
    }

    [Fact]
    public void DescribeSearchedPaths_ReportsBothRootsIncludingNulls()
    {
        var description = DemoAssetGuard.DescribeSearchedPaths(@"C:\content", null);

        description.Should().Contain(@"C:\content");
        description.Should().Contain("(null)");
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempRoot))
        {
            Directory.Delete(_tempRoot, recursive: true);
        }
    }
}
