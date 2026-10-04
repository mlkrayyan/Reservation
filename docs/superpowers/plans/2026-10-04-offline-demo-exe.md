# Offline Demo Executable Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Produce `dist/demo/SpaSalonDemo.exe` — one self-contained Windows file that serves the React SPA and the ASP.NET API from a single loopback port, seeds demo data on startup, and opens the default browser.

**Architecture:** The Vite build output is copied into the API project's `wwwroot` and published as a self-contained single-file win-x64 executable. A new `Demo` ASP.NET environment turns on static-file serving, SPA fallback routing, loopback-only binding and browser auto-launch, leaving Development and Production behaviour untouched. EF Core and Hangfire already use in-memory providers, so no database or external service is involved.

**Tech Stack:** .NET 8 (ASP.NET Core, Kestrel, EF Core in-memory, Hangfire in-memory, MediatR), React 19 + Vite + TypeScript, pnpm, xunit 2.5.3 + FluentAssertions 6.12.0 + `Microsoft.AspNetCore.Mvc.Testing`, PowerShell 5.1.

**Spec:** `docs/superpowers/specs/2026-10-04-offline-demo-exe-design.md`

---

## File Structure

### Created

| Path | Responsibility |
|---|---|
| `src/SpaReservation.API/Demo/DemoEnvironment.cs` | Demo environment name, preferred port, port-availability probe, binding URL selection. No ASP.NET dependencies. |
| `src/SpaReservation.API/Demo/DemoAssetGuard.cs` | Answers one question: were the SPA assets bundled into this build? |
| `src/SpaReservation.API/Demo/DemoLauncher.cs` | Turns Kestrel's bound addresses into a human URL, renders the console banner, opens the browser. |
| `src/SpaReservation.API/appsettings.Demo.json` | Demo-environment configuration overlay. |
| `tests/SpaReservation.API.Tests/SpaReservation.API.Tests.csproj` | Test project for the API host. |
| `tests/SpaReservation.API.Tests/Demo/DemoEnvironmentTests.cs` | Unit tests for port selection. |
| `tests/SpaReservation.API.Tests/Demo/DemoAssetGuardTests.cs` | Unit tests for the asset guard. |
| `tests/SpaReservation.API.Tests/Demo/DemoLauncherTests.cs` | Unit tests for URL normalisation and the banner. |
| `tests/SpaReservation.API.Tests/DemoWebApplicationFactory.cs` | Boots the real app in the Demo environment against a stub web root. |
| `tests/SpaReservation.API.Tests/DemoHostingTests.cs` | Integration tests for static serving, SPA fallback and API 404s. |
| `frontend/src/assets/hero-spa.jpg` | Locally vendored landing-page hero, replacing a CDN URL. |
| `build-demo.ps1` | One-command build of the demo executable. |

### Modified

| Path | Change |
|---|---|
| `frontend/src/services/api.ts` | Absolute `http://localhost:5139/api` base URL becomes relative. |
| `frontend/vite.config.ts` | Dev proxy so `pnpm dev` still reaches a separately-run API. |
| `frontend/src/pages/Home.css` | Hero background switched from the Unsplash CDN to the vendored local image. |
| `src/SpaReservation.API/Program.cs` | Demo environment default, loopback binding, static files, SPA fallback, conditional HTTPS redirect, startup announcement. |
| `.gitignore` | Ignore the generated `src/SpaReservation.API/wwwroot/`. |

### Boundaries

The three `Demo/*.cs` files hold no ASP.NET pipeline logic — they are pure functions plus one `Process.Start`. All pipeline wiring stays in `Program.cs`, which is where this codebase already keeps it. That split is what makes the behaviour unit-testable without booting a host.

---

### Task 1: Frontend serves from its own origin

**Files:**
- Modify: `frontend/src/services/api.ts:3-5`
- Modify: `frontend/vite.config.ts`

- [ ] **Step 1: Make the API base URL relative**

Replace lines 3-5 of `frontend/src/services/api.ts`:

```ts
const api = axios.create({
  baseURL: 'http://localhost:5139/api',
});
```

with:

```ts
// Relative by default so the production build talks to whatever origin served
// it — that is what lets the demo exe host the SPA and the API on one port.
// VITE_API_BASE_URL stays available for pointing a build at a remote API.
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '/api',
});
```

`frontend/tsconfig.app.json` already sets `"types": ["vite/client"]`, so `import.meta.env` type-checks with no further change.

- [ ] **Step 2: Add the dev proxy so `pnpm dev` keeps working**

Replace the whole of `frontend/vite.config.ts`:

```ts
import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    // The SPA now uses a relative /api base. In dev the API is a separate
    // process on 5139, so proxy to it rather than reintroducing an absolute URL.
    proxy: {
      '/api': {
        target: 'http://localhost:5139',
        changeOrigin: true,
      },
    },
  },
})
```

- [ ] **Step 3: Build and verify no absolute API URL survives**

Run:

```bash
cd frontend && pnpm build && grep -r "localhost:5139" dist/ ; echo "grep exit: $?"
```

Expected: the build succeeds (`tsc -b` clean, `vite build` writes `dist/`), and `grep` prints no matches with `grep exit: 1`.

If grep exits 0 it has found a match — some other file hardcodes the API origin. Find it with `grep -rn "localhost:5139" src/` and apply the same relative treatment before continuing.

- [ ] **Step 4: Commit**

```bash
git add frontend/src/services/api.ts frontend/vite.config.ts
git commit -m "refactor(frontend): use relative API base URL with dev proxy

Lets a single host serve both the SPA and the API, which the offline
demo build requires. pnpm dev is unaffected via the new Vite proxy.

Co-Authored-By: Claude Opus 5 (1M context) <noreply@anthropic.com>"
```

---

### Task 1b: Vendor the hero image so the landing page works offline

`frontend/src/pages/Home.css:16` loads the hero photo from `images.unsplash.com`. Offline that leaves the first screen anyone sees with no hero. A repo-wide grep confirms it is the **only** remote asset reference in `frontend/src`, so this one change makes the SPA self-contained.

**Files:**
- Create: `frontend/src/assets/hero-spa.jpg`
- Modify: `frontend/src/pages/Home.css:14-17`

- [ ] **Step 1: Download the image**

This step needs internet access. It is the last point in the build that does.

```bash
cd /e/Projects/Reservation && curl -L -o frontend/src/assets/hero-spa.jpg \
  "https://images.unsplash.com/photo-1540555700478-4be289fbecef?auto=format&fit=crop&w=1600&q=80"
```

- [ ] **Step 2: Verify it is a real image and not an error page**

```bash
cd /e/Projects/Reservation && ls -l frontend/src/assets/hero-spa.jpg && file frontend/src/assets/hero-spa.jpg
```

Expected: a file over 100 KB, reported as `JPEG image data`. If it is a few hundred bytes or reports as HTML/ASCII text, the download failed — stop and report rather than committing a broken asset.

- [ ] **Step 3: Point the CSS at the local file**

In `frontend/src/pages/Home.css`, replace the `background` declaration of the `.hero` rule:

```css
  background: radial-gradient(circle at center, rgba(123, 47, 190, 0.25) 0%, rgba(13, 13, 13, 0.95) 70%),
              url('https://images.unsplash.com/photo-1540555700478-4be289fbecef?auto=format&fit=crop&w=1600&q=80') center/cover no-repeat;
```

with:

```css
  /* Local asset, not a CDN URL — the offline demo has no network. */
  background: radial-gradient(circle at center, rgba(123, 47, 190, 0.25) 0%, rgba(13, 13, 13, 0.95) 70%),
              url('../assets/hero-spa.jpg') center/cover no-repeat;
```

Vite resolves the relative URL at build time and emits a fingerprinted copy into `dist/assets/`.

- [ ] **Step 4: Build and verify no remote asset reference survives**

```bash
cd /e/Projects/Reservation/frontend && pnpm build && grep -rn "images.unsplash.com" dist/ src/ ; echo "grep exit: $?"
```

Expected: build succeeds and `grep exit: 1` (no matches).

- [ ] **Step 5: Confirm the image landed in the build output**

```bash
cd /e/Projects/Reservation/frontend && ls -l dist/assets/ | grep -i hero
```

Expected: one fingerprinted `hero-spa-*.jpg`.

- [ ] **Step 6: Commit**

```bash
git add frontend/src/assets/hero-spa.jpg frontend/src/pages/Home.css
git commit -m "fix(frontend): vendor the hero image instead of loading it from a CDN

The offline demo has no network, so the remote Unsplash hero left the
landing page without its background. This was the only remote asset
reference in frontend/src.

Co-Authored-By: Claude Opus 5 (1M context) <noreply@anthropic.com>"
```

---

### Task 2: Test project and demo port selection

**Files:**
- Create: `tests/SpaReservation.API.Tests/SpaReservation.API.Tests.csproj`
- Create: `tests/SpaReservation.API.Tests/Demo/DemoEnvironmentTests.cs`
- Create: `src/SpaReservation.API/Demo/DemoEnvironment.cs`

- [ ] **Step 1: Create the test project**

Create `tests/SpaReservation.API.Tests/SpaReservation.API.Tests.csproj`. Package versions match the two existing test projects; `Microsoft.AspNetCore.Mvc.Testing` is new and is needed from Task 6 onward.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.0" />
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="xunit" Version="2.5.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\src\SpaReservation.API\SpaReservation.API.csproj" />
  </ItemGroup>

</Project>
```

- [ ] **Step 2: Write the failing tests**

Create `tests/SpaReservation.API.Tests/Demo/DemoEnvironmentTests.cs`:

```csharp
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
```

- [ ] **Step 3: Run the tests to verify they fail**

Run: `dotnet test tests/SpaReservation.API.Tests`

Expected: compilation failure — `CS0234: The type or namespace name 'Demo' does not exist in the namespace 'SpaReservation.API'`.

- [ ] **Step 4: Write the implementation**

Create `src/SpaReservation.API/Demo/DemoEnvironment.cs`:

```csharp
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
    /// Port 0 asks the OS for any free port; the real one is read back from
    /// IServerAddressesFeature once Kestrel is listening.
    /// </summary>
    public static string ResolveUrl(int preferredPort) =>
        IsPortAvailable(preferredPort)
            ? $"http://localhost:{preferredPort}"
            : "http://localhost:0";
}
```

- [ ] **Step 5: Run the tests to verify they pass**

Run: `dotnet test tests/SpaReservation.API.Tests`

Expected: `Passed! - Failed: 0, Passed: 4`.

- [ ] **Step 6: Commit**

```bash
git add tests/SpaReservation.API.Tests src/SpaReservation.API/Demo/DemoEnvironment.cs
git commit -m "feat(demo): add demo port selection with occupied-port fallback

Co-Authored-By: Claude Opus 5 (1M context) <noreply@anthropic.com>"
```

---

### Task 3: Fail loudly when SPA assets were never bundled

A demo exe built without the SPA would start cleanly and serve 404s, which looks like a broken app rather than a broken build. This guard makes that case say so.

**Files:**
- Create: `tests/SpaReservation.API.Tests/Demo/DemoAssetGuardTests.cs`
- Create: `src/SpaReservation.API/Demo/DemoAssetGuard.cs`

- [ ] **Step 1: Write the failing tests**

Create `tests/SpaReservation.API.Tests/Demo/DemoAssetGuardTests.cs`:

```csharp
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
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test tests/SpaReservation.API.Tests --filter DemoAssetGuardTests`

Expected: compilation failure — `CS0103: The name 'DemoAssetGuard' does not exist`.

- [ ] **Step 3: Write the implementation**

Create `src/SpaReservation.API/Demo/DemoAssetGuard.cs`:

```csharp
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
```

- [ ] **Step 4: Run the tests to verify they pass**

Run: `dotnet test tests/SpaReservation.API.Tests --filter DemoAssetGuardTests`

Expected: `Passed! - Failed: 0, Passed: 4`.

- [ ] **Step 5: Commit**

```bash
git add tests/SpaReservation.API.Tests/Demo/DemoAssetGuardTests.cs src/SpaReservation.API/Demo/DemoAssetGuard.cs
git commit -m "feat(demo): fail loudly when SPA assets are missing from the build

Co-Authored-By: Claude Opus 5 (1M context) <noreply@anthropic.com>"
```

---

### Task 4: Startup announcement and browser launch

**Files:**
- Create: `tests/SpaReservation.API.Tests/Demo/DemoLauncherTests.cs`
- Create: `src/SpaReservation.API/Demo/DemoLauncher.cs`

`TryOpenBrowser` is not unit tested — it shells out to the OS and asserting on that would test the OS, not this code. It is verified by hand in Task 8. Everything around it is a pure function and is tested here.

- [ ] **Step 1: Write the failing tests**

Create `tests/SpaReservation.API.Tests/Demo/DemoLauncherTests.cs`:

```csharp
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
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test tests/SpaReservation.API.Tests --filter DemoLauncherTests`

Expected: compilation failure — `CS0103: The name 'DemoLauncher' does not exist`.

- [ ] **Step 3: Write the implementation**

Create `src/SpaReservation.API/Demo/DemoLauncher.cs`:

```csharp
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
```

- [ ] **Step 4: Run the tests to verify they pass**

Run: `dotnet test tests/SpaReservation.API.Tests --filter DemoLauncherTests`

Expected: `Passed! - Failed: 0, Passed: 6`.

- [ ] **Step 5: Commit**

```bash
git add tests/SpaReservation.API.Tests/Demo/DemoLauncherTests.cs src/SpaReservation.API/Demo/DemoLauncher.cs
git commit -m "feat(demo): add startup banner and browser launcher

Co-Authored-By: Claude Opus 5 (1M context) <noreply@anthropic.com>"
```

---

### Task 5: Demo configuration overlay

**Files:**
- Create: `src/SpaReservation.API/appsettings.Demo.json`

- [ ] **Step 1: Create the overlay**

ASP.NET loads `appsettings.json` first and overlays `appsettings.{Environment}.json`, so `JwtSettings`, `ConnectionStrings` and `AllowedHosts` are inherited and must not be repeated here. The only reason this file exists is to quiet the logs so the console banner is readable.

Create `src/SpaReservation.API/appsettings.Demo.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "DemoMode": true
}
```

`AllowedHosts` is deliberately left inherited as `*`. Host filtering is not the control that keeps this demo private — the loopback-only Kestrel binding from Task 2 is, and adding a second overlapping control would only create a way for the demo to fail confusingly on `127.0.0.1`.

- [ ] **Step 2: Verify it is valid JSON and will be published**

Run:

```bash
cd /e/Projects/Reservation && node -e "JSON.parse(require('fs').readFileSync('src/SpaReservation.API/appsettings.Demo.json','utf8'));console.log('valid json')"
```

Expected: `valid json`.

`Microsoft.NET.Sdk.Web` globs `appsettings*.json` into the publish output automatically, so no `.csproj` change is required.

- [ ] **Step 3: Commit**

```bash
git add src/SpaReservation.API/appsettings.Demo.json
git commit -m "feat(demo): add Demo environment configuration overlay

Co-Authored-By: Claude Opus 5 (1M context) <noreply@anthropic.com>"
```

---

### Task 6: Wire the Demo environment into the host

**Files:**
- Modify: `src/SpaReservation.API/Program.cs`
- Create: `tests/SpaReservation.API.Tests/DemoWebApplicationFactory.cs`
- Create: `tests/SpaReservation.API.Tests/DemoHostingTests.cs`

- [ ] **Step 1: Write the failing integration tests**

The factory points the app at a temporary web root holding a stub `index.html`, so these tests never depend on whether anyone has run a frontend build.

Create `tests/SpaReservation.API.Tests/DemoWebApplicationFactory.cs`:

```csharp
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
```

Create `tests/SpaReservation.API.Tests/DemoHostingTests.cs`:

```csharp
using System.Net;
using FluentAssertions;

namespace SpaReservation.API.Tests;

public class DemoHostingTests : IClassFixture<DemoWebApplicationFactory>
{
    private readonly DemoWebApplicationFactory _factory;

    public DemoHostingTests(DemoWebApplicationFactory factory) => _factory = factory;

    [Fact]
    public async Task Root_ServesTheSpaIndexDocument()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync())
            .Should().Contain(DemoWebApplicationFactory.StubIndexMarker);
    }

    [Fact]
    public async Task ClientSideRoute_FallsBackToTheSpaIndexDocument()
    {
        var client = _factory.CreateClient();

        // No controller and no file match /admin/bookings. Without the SPA
        // fallback this 404s, and refreshing any admin page breaks.
        var response = await client.GetAsync("/admin/bookings");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsStringAsync())
            .Should().Contain(DemoWebApplicationFactory.StubIndexMarker);
    }

    [Fact]
    public async Task UnknownApiRoute_Returns404AndNotTheSpaIndexDocument()
    {
        var client = _factory.CreateClient();

        // Regression guard: a naive MapFallbackToFile swallows unmatched /api
        // routes and answers them with 200 + index.html, which turns every
        // client bug into a silent HTML-instead-of-JSON parse failure.
        var response = await client.GetAsync("/api/this-endpoint-does-not-exist");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync())
            .Should().NotContain(DemoWebApplicationFactory.StubIndexMarker);
    }

    [Fact]
    public async Task ServicesEndpoint_ReturnsSeededDemoData()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/services");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().StartWith("[");
        body.Should().NotBe("[]", "DbSeeder should have populated services on startup");
    }

    [Fact]
    public async Task StaffEndpoint_ReturnsSeededDemoData()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/staff");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().StartWith("[");
        body.Should().NotBe("[]", "DbSeeder should have populated staff on startup");
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test tests/SpaReservation.API.Tests --filter DemoHostingTests`

Expected: compilation failure — `CS0122: 'Program' is inaccessible due to its protection level`, because top-level statements generate an internal `Program` class. Step 3 fixes that along with the rest of the wiring.

- [ ] **Step 3: Rewrite `Program.cs`**

Replace the entire contents of `src/SpaReservation.API/Program.cs`:

```csharp
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

// A double-clicked demo exe has no launch profile to set this, so default it
// here — before WebApplication.CreateBuilder reads configuration.
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
{
    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", DemoEnvironment.Name);
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
```

- [ ] **Step 4: Run the integration tests to verify they pass**

Run: `dotnet test tests/SpaReservation.API.Tests`

Expected: `Passed! - Failed: 0, Passed: 19` (4 + 4 + 6 unit tests from Tasks 2-4, plus 5 hosting tests).

If `Root_ServesTheSpaIndexDocument` fails with 404, `UseWebRoot` was not applied before the static-file middleware resolved its file provider — confirm `ConfigureWebHost` in the factory sets it and that `app.UseStaticFiles()` is inside the `isDemo` block with no explicit `StaticFileOptions`.

- [ ] **Step 5: Verify Development behaviour is unchanged**

Run: `dotnet test tests/SpaReservation.Domain.Tests tests/SpaReservation.Application.Tests`

Expected: all existing tests still pass.

Then confirm the dev profile still binds https and skips the SPA pipeline:

```bash
cd /e/Projects/Reservation && ASPNETCORE_ENVIRONMENT=Development dotnet build src/SpaReservation.API -c Debug
```

Expected: `Build succeeded` with 0 errors.

- [ ] **Step 6: Commit**

```bash
git add src/SpaReservation.API/Program.cs tests/SpaReservation.API.Tests
git commit -m "feat(demo): serve the SPA and API from one loopback host in Demo env

Adds static file serving, SPA fallback routing that does not swallow
unmatched /api routes, loopback-only binding, Swagger in Demo, and a
startup banner that opens the browser. Development and Production
pipelines are unchanged.

Co-Authored-By: Claude Opus 5 (1M context) <noreply@anthropic.com>"
```

---

### Task 7: One-command build script

**Files:**
- Create: `build-demo.ps1`
- Modify: `.gitignore`

- [ ] **Step 1: Ignore the generated web root**

Append to `.gitignore`:

```
src/SpaReservation.API/wwwroot/
```

The existing `dist/` entry already covers both `frontend/dist` and the `dist/demo` publish output.

- [ ] **Step 2: Write the build script**

Create `build-demo.ps1` at the repository root:

```powershell
#requires -Version 5.1
<#
    Builds the offline demo executable.

    Output: dist/demo/SpaSalonDemo.exe — self-contained win-x64, needs no
    .NET runtime installed on the target machine.
#>
$ErrorActionPreference = 'Stop'

$root       = $PSScriptRoot
$frontend   = Join-Path $root 'frontend'
$apiProject = Join-Path $root 'src/SpaReservation.API'
$wwwroot    = Join-Path $apiProject 'wwwroot'
$publishDir = Join-Path $root 'dist/demo'

Write-Host '[1/5] Installing frontend dependencies...' -ForegroundColor Cyan
Push-Location $frontend
try {
    pnpm install --frozen-lockfile
    if ($LASTEXITCODE -ne 0) { throw "pnpm install failed with exit code $LASTEXITCODE" }

    Write-Host '[2/5] Building the SPA...' -ForegroundColor Cyan
    pnpm build
    if ($LASTEXITCODE -ne 0) { throw "pnpm build failed with exit code $LASTEXITCODE" }
}
finally {
    Pop-Location
}

Write-Host '[3/5] Copying the SPA into the API web root...' -ForegroundColor Cyan
if (Test-Path $wwwroot) { Remove-Item $wwwroot -Recurse -Force }
New-Item -ItemType Directory -Path $wwwroot -Force | Out-Null
Copy-Item -Path (Join-Path $frontend 'dist/*') -Destination $wwwroot -Recurse -Force
if (-not (Test-Path (Join-Path $wwwroot 'index.html'))) {
    throw 'index.html is missing after the copy - the SPA build produced no output.'
}

Write-Host '[4/5] Publishing the self-contained executable...' -ForegroundColor Cyan
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }
dotnet publish $apiProject `
    -c Release `
    -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeAllContentForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -o $publishDir
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed with exit code $LASTEXITCODE" }

Write-Host '[5/5] Renaming the published host...' -ForegroundColor Cyan
$published = Join-Path $publishDir 'SpaReservation.API.exe'
$final     = Join-Path $publishDir 'SpaSalonDemo.exe'
if (-not (Test-Path $published)) { throw "Expected $published but it was not produced." }
if (Test-Path $final) { Remove-Item $final -Force }
Rename-Item -Path $published -NewName 'SpaSalonDemo.exe'

$sizeMb = [math]::Round((Get-Item $final).Length / 1MB, 1)
Write-Host ''
Write-Host "Demo built: $final ($sizeMb MB)" -ForegroundColor Green
Write-Host 'Double-click it to run the offline demo.' -ForegroundColor Green
```

- [ ] **Step 3: Run the build**

Run: `powershell -ExecutionPolicy Bypass -File ./build-demo.ps1`

Expected: all five stages print, and the final line reads `Demo built: ...\dist\demo\SpaSalonDemo.exe (NN MB)` with a size between roughly 40 MB and 160 MB.

Two failure modes worth recognising:

- `NU1101` naming `Microsoft.AspNetCore.App.Runtime.win-x64` — the self-contained runtime pack is not in the local NuGet cache. This needs internet access **once, at build time**. It has no bearing on whether the produced exe runs offline.
- The exe is produced but Task 8 shows a blank page while `index.html` exists in `wwwroot` — single-file content extraction did not place the web root where the host looks. Fall back by dropping `-p:PublishSingleFile=true` and `-p:IncludeAllContentForSelfExtract=true`, which ships `SpaSalonDemo.exe` alongside a `wwwroot/` folder. Record the change here and tell the user the deliverable became a folder.

- [ ] **Step 4: Confirm nothing generated got staged**

Run: `git status --porcelain`

Expected: `build-demo.ps1` and `.gitignore` appear. Neither `src/SpaReservation.API/wwwroot/` nor `dist/` appears.

- [ ] **Step 5: Commit**

```bash
git add build-demo.ps1 .gitignore
git commit -m "build(demo): add one-command offline demo build script

Co-Authored-By: Claude Opus 5 (1M context) <noreply@anthropic.com>"
```

---

### Task 8: Verify the real artifact

Everything so far tested the code. This task tests the thing that ships. Do not mark the work complete until every check below has actually been run and its result observed.

**Files:** none modified.

- [ ] **Step 1: Launch the executable**

Run `dist/demo/SpaSalonDemo.exe` in the background and capture its console output.

Expected: the banner prints with a `Running at http://localhost:5139` line, and the default browser opens on that URL.

- [ ] **Step 2: Walk the customer journey in Chrome**

Drive the opened page with the Chrome automation tools. Confirm each of these:

| Check | Expected |
|---|---|
| Home page | Renders, no error overlay |
| Services page | Lists the seeded services, not the hardcoded `srv-1..6` fallback fixtures |
| Staff page | Lists the seeded staff |
| Booking wizard | All three steps complete and the POST returns 2xx |
| My Bookings | Renders |
| Loyalty | Renders |
| Waitlist | Renders |
| Browser console | No failed `/api` requests, no CORS errors |

Distinguishing seeded data from fixture fallback matters: every page catches fetch errors and silently substitutes fixtures, so a screen that looks right can still mean the API never answered. Confirm against the network panel, not the rendered list.

- [ ] **Step 3: Verify deep links and the API 404 boundary**

Navigate directly to `http://localhost:5139/admin/bookings` (typed, not clicked) and reload it.

Expected: the admin bookings screen renders. A 404 here means `MapFallbackToFile` is not wired.

Then open `http://localhost:5139/api/this-endpoint-does-not-exist`.

Expected: a 404, not the SPA document.

- [ ] **Step 4: Verify Swagger and the Hangfire dashboard**

Open `http://localhost:5139/swagger` and `http://localhost:5139/hangfire`.

Expected: both render. The three recurring jobs (`reminder-24h`, `reminder-1h`, `waitlist-expire`) are listed in Hangfire.

- [ ] **Step 5: Verify it is genuinely offline**

Disable the network adapter, then close and relaunch the exe and repeat Step 2.

Expected: identical behaviour, **including the home page hero image**, which Task 1b vendored locally. Any failure here means something still reaches the network — most likely a font or icon CDN reference. Re-run the Task 1b Step 4 grep against the whole of `frontend/src` to find it.

- [ ] **Step 6: Verify the port-conflict fallback**

With the demo still running, launch a second instance of `SpaSalonDemo.exe`.

Expected: the second instance prints a banner on a different port and its browser tab works. Both instances serve independently.

- [ ] **Step 7: Verify shutdown**

Close the console window of each instance.

Expected: the process ends and the URL stops responding.

- [ ] **Step 8: Report**

Summarise for the user: the artifact path and size, every check that passed, and anything that failed or was skipped — explicitly including the offline hero image finding from Step 5 and the fact that clean-machine behaviour (a Windows box with no .NET installed) could not be verified from this workstation.

---

## Out of Scope

Recorded so they are not silently picked up. Each was agreed during brainstorming as "package what exists":

- `AdminDashboard` and `AdminAnalytics` keep their hardcoded figures.
- No availability endpoint; the booking wizard keeps its `setTimeout` slot stub.
- Admin staff edits stay `alert()` stubs.
- Login stays fabricated client-side; any email containing `admin` still reaches the admin panel.
- `ExceptionHandlingMiddleware` still returns `exception.StackTrace` in error bodies.
- `/hangfire` still has no authorization filter; loopback-only binding is what contains it.
- `CreateBookingCommandValidator` is still unregistered.
- No code signing, installer, auto-update, or non-Windows build.
