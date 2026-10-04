using System.Net;
using System.Net.Sockets;

namespace SpaReservation.API.Demo;

/// <summary>
/// Hosting constants and port selection for the offline demo build.
/// </summary>
public static class DemoEnvironment
{
    public const string Name = "Demo";

    /// <summary>Matches the dev API port, so demo URLs look familiar.</summary>
    public const int PreferredPort = 5139;

    public static bool IsPortAvailable(int port)
    {
        TcpListener? listener = null;
        try
        {
            listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            return true;
        }
        catch (SocketException)
        {
            return false;
        }
        finally
        {
            listener?.Stop();
        }
    }

    /// <summary>
    /// Loopback only — never 0.0.0.0. The demo mounts an unauthenticated
    /// Hangfire dashboard, so it must not be reachable from the network.
    ///
    /// Port 0 asks the OS for any free port; the real one is read back from
    /// IServerAddressesFeature once Kestrel is listening. The fallback must use
    /// 127.0.0.1 rather than the "localhost" hostname: Kestrel rejects dynamic
    /// port binding against localhost with "Dynamic port binding is not
    /// supported when binding to localhost."
    /// </summary>
    public static string ResolveUrl(int preferredPort) =>
        IsPortAvailable(preferredPort)
            ? $"http://localhost:{preferredPort}"
            : "http://127.0.0.1:0";
}
