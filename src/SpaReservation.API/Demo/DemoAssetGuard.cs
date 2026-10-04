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

    /// <summary>
    /// Picks the directory ASP.NET should treat as the content root when the
    /// demo exe is launched directly.
    ///
    /// For a published single-file app these three differ: Environment.ProcessPath
    /// is the exe itself, AppContext.BaseDirectory is a temp extraction directory,
    /// and the current directory is wherever the caller happened to be. wwwroot
    /// ships as a loose folder beside the exe, so prefer whichever candidate
    /// actually contains it rather than assuming.
    /// </summary>
    public static string ResolveContentRoot() =>
        ResolveContentRoot(
            Path.GetDirectoryName(Environment.ProcessPath),
            AppContext.BaseDirectory,
            Directory.GetCurrentDirectory());

    public static string ResolveContentRoot(params string?[] candidates)
    {
        foreach (var candidate in candidates)
        {
            if (!string.IsNullOrWhiteSpace(candidate)
                && Directory.Exists(Path.Combine(candidate, "wwwroot")))
            {
                return candidate;
            }
        }

        // Nothing holds a wwwroot. Leave the current directory alone so the
        // guard reports the real paths instead of a directory we invented.
        return Directory.GetCurrentDirectory();
    }

    public static string DescribeSearchedPaths(string? contentRoot, string? webRoot) =>
        $"  content root: {contentRoot ?? "(null)"}{Environment.NewLine}"
        + $"  web root:     {webRoot ?? "(null)"}{Environment.NewLine}"
        + $"  exe:          {Environment.ProcessPath ?? "(null)"}";
}
