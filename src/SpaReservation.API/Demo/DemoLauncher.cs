using System.Diagnostics;

namespace SpaReservation.API.Demo;

/// <summary>
/// Console banner and browser launch for the offline demo build.
/// </summary>
public static class DemoLauncher
{
    /// <summary>
    /// Kestrel reports wildcard bindings as [::] or 0.0.0.0, neither of which
    /// a browser can open. Rewrite them to localhost.
    /// </summary>
    public static string? ResolveListeningUrl(IEnumerable<string> addresses)
    {
        var address = addresses?.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(address))
        {
            return null;
        }

        return address
            .Replace("[::]", "localhost")
            .Replace("0.0.0.0", "localhost");
    }

    public static string BuildBanner(string url) =>
        $"""

        ================================================================
          Ireq SPA Salon - OFFLINE DEMO
        ----------------------------------------------------------------
          Running at: {url}
          Demo data is reset every time you start this program.
          Close this window to stop the demo.
        ================================================================

        """;

    public static bool TryOpenBrowser(string url, out string? error)
    {
        try
        {
            // UseShellExecute is required to hand a URL to the default browser.
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            error = null;
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}
