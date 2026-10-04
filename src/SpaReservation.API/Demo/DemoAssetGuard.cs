namespace SpaReservation.API.Demo;

/// <summary>
/// Guards against shipping a demo build whose SPA assets were never bundled.
/// </summary>
public static class DemoAssetGuard
{
    public const string MissingAssetsMessage =
        "SPA assets were not bundled into this build (wwwroot/index.html is missing). "
        + "Re-run build-demo.ps1.";

    public static bool SpaAssetsPresent(string? webRootPath) =>
        !string.IsNullOrWhiteSpace(webRootPath)
        && File.Exists(Path.Combine(webRootPath, "index.html"));
}
