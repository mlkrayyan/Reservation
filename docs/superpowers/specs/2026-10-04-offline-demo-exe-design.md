# Offline Demo Executable — Design Spec

**Date:** 2026-10-04
**Project:** Ireq SPA Salon — Reservation Website
**Deliverable:** A single self-contained Windows `.exe` that runs the whole app offline and opens it in the default browser, pre-populated with seeded demo data.

---

## 1. Goal

Produce one file a non-technical viewer can copy onto a Windows machine, double-click, and immediately see a working SPA Salon reservation site. No internet connection, no database server, no .NET runtime install, no `pnpm dev`, no separate API process.

### Success criteria

1. `SpaSalonDemo.exe` runs on a clean Windows x64 machine with no .NET runtime installed.
2. Double-clicking it opens the default browser on the app's home page.
3. Services, staff, bookings and loyalty data are present on first load, from `DbSeeder`.
4. The booking wizard completes a round trip against the embedded API.
5. React Router deep links (e.g. `/admin/bookings`) resolve when entered directly or refreshed.
6. Closing the console window stops the demo.
7. No network listener is exposed beyond `localhost`.

### Non-goals

Chosen explicitly during design as "package what exists":

- Not filling in the stubbed screens. `AdminDashboard` and `AdminAnalytics` keep their hardcoded figures; admin staff edits stay `alert()` stubs; no availability endpoint is added.
- Not replacing the fabricated client-side login with server-enforced auth.
- Not persisting data between launches.
- Not building an installer, auto-updater, code signing, or a macOS/Linux build.

---

## 2. What the codebase already gives us

The app is closer to offline-ready than it looks:

| Concern | Current state | Demo impact |
|---|---|---|
| Database | EF Core **in-memory** provider (`AddInfrastructure`) | No DB server needed. Already offline. |
| Background jobs | Hangfire **in-memory** storage | No Redis/SQL needed. Already offline. |
| Demo data | `DbSeeder.SeedAsync` runs on startup | Data exists on every launch for free. |
| Frontend resilience | Every fetching page catches errors and falls back to seeded fixtures | Screens render even if an endpoint is missing. |

The only genuine blockers are that the SPA and API are two processes on two origins, and that the SPA hardcodes an absolute API URL.

Note: the original system spec names SQL Server. The code as built uses the EF Core in-memory provider. This design relies on the code as built, not on that earlier spec.

---

## 3. Architecture

Single ASP.NET Core process serving both the compiled SPA and the API from one origin and one port.

```
SpaSalonDemo.exe  (self-contained, single file, win-x64)
│
├── ASP.NET Core Kestrel  →  http://localhost:5139   (localhost only)
│     ├── /                 static files  ← frontend/dist baked into wwwroot
│     ├── /api/*            existing controllers
│     ├── /swagger          enabled in Demo
│     ├── /hangfire         existing dashboard
│     └── fallback          index.html  (React Router deep links)
│
├── EF Core in-memory DB   ← DbSeeder.SeedAsync on startup
├── Hangfire in-memory server + recurring jobs
└── on startup: print URL, shell-open default browser
```

Same origin for SPA and API means CORS stops mattering and the SPA can use a relative base URL.

---

## 4. Components

### 4.1 Frontend — relative API base URL

`frontend/src/services/api.ts`

```ts
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '/api',
});
```

`frontend/vite.config.ts` gains a dev proxy so `pnpm dev` keeps working against a separately-run API:

```ts
server: {
  proxy: {
    '/api': { target: 'http://localhost:5139', changeOrigin: true },
  },
}
```

**What this does:** production build calls its own origin; dev build proxies to the dev API.
**Depends on:** nothing else in the design.
**Why an env var rather than a hardcoded relative path:** it leaves an escape hatch for pointing a build at a remote API without another code change.

### 4.2 Backend — a `Demo` environment

A new environment name rather than new branches through the app, so normal dev and prod behaviour is untouched. The exe sets `ASPNETCORE_ENVIRONMENT=Demo` for itself at startup if it is not already set.

New file `src/SpaReservation.API/appsettings.Demo.json`: same shape as `appsettings.json`, with the demo URL and a `DemoMode` flag.

Changes in `Program.cs`, each guarded by `app.Environment.IsEnvironment("Demo")`:

| Change | Reason |
|---|---|
| Skip `UseHttpsRedirection()` | It would 307 every plain-http demo request into a nonexistent https endpoint. |
| `UseDefaultFiles()` + `UseStaticFiles()` | Serve the baked SPA. |
| `MapFallbackToFile("index.html")` | Deep links and refreshes on client routes must not 404. Registered after `MapControllers()` so API routes win. |
| Bind `http://localhost:5139` | Loopback only. Binding `0.0.0.0` would expose an unauthenticated `/hangfire` to the LAN. |
| Port fallback | If 5139 is in use, bind port 0, read the assigned port back from `IServerAddressesFeature`, and use that everywhere. |
| Enable Swagger | Currently Development-only; useful in a technical demo. |
| Print banner + open browser | `Process.Start(new ProcessStartInfo(url) { UseShellExecute = true })`, fired after the server is listening so the first request does not race startup. |

Seeding and Hangfire need no change. The in-memory DB is empty at every boot, so `DbSeeder`'s `AnyAsync` guards always pass and seeding always runs.

**Ordering constraint:** the browser must be opened only after Kestrel reports it is listening, and the port must be read back before the URL is printed or opened. Both hang off `IHostApplicationLifetime.ApplicationStarted`.

### 4.3 Build script — `build-demo.ps1`

Repo root. Steps, each failing fast:

1. `pnpm install --frozen-lockfile` in `frontend/`
2. `pnpm build` in `frontend/`
3. Clear `src/SpaReservation.API/wwwroot`, copy `frontend/dist/*` into it
4. `dotnet publish src/SpaReservation.API -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -o dist/demo`
5. Rename the published host to `dist/demo/SpaSalonDemo.exe`
6. Print the output path and size

`Microsoft.NET.Sdk.Web` includes `wwwroot` in publish automatically, and `IncludeAllContentForSelfExtract=true` folds that content into the single file. No `Content Include` entry is required.

`src/SpaReservation.API/wwwroot` is generated output and is added to `.gitignore`.

---

## 5. Data flow

**Startup**

```
exe launch
  → ASPNETCORE_ENVIRONMENT defaulted to Demo
  → host build, AddInfrastructure registers in-memory DbContext + Hangfire
  → DbSeeder.SeedAsync populates services, staff, customers, a sample booking
  → recurring Hangfire jobs registered
  → Kestrel listens on localhost:5139 (or fallback port)
  → ApplicationStarted: resolve real port, print banner, shell-open browser
```

**Request**

```
GET  /               → StaticFiles → wwwroot/index.html
GET  /assets/*.js    → StaticFiles
GET  /api/services   → ServicesController → ISalonServiceRepository → in-memory DB
POST /api/bookings   → BookingsController → MediatR → CreateBookingCommandHandler → in-memory DB
GET  /admin/bookings → no controller match, no static file → fallback index.html → React Router
```

---

## 6. Error handling

| Failure | Behaviour |
|---|---|
| Port 5139 in use | Fall back to an OS-assigned free port; the banner and the opened browser both use the real port. |
| Browser fails to launch | Caught and swallowed to a message: "Could not open a browser automatically — open \<url\> manually." The server keeps running. |
| An API endpoint errors | Existing `ExceptionHandlingMiddleware` path, unchanged. The SPA's existing per-page catch falls back to fixture data, so the screen still renders. |
| `wwwroot` missing from the build | Startup check: if `index.html` is absent in a Demo-environment run, log a clear "SPA assets were not bundled — re-run build-demo.ps1" and exit non-zero, rather than serving a blank 404. |
| Viewer closes the console | Process ends, Kestrel stops, in-memory data is discarded. Intended. |

---

## 7. Testing

Automated coverage is thin here by nature — the risk is in packaging, not in logic — so verification is primarily a scripted manual pass against the real artifact.

1. **Existing unit tests** (`dotnet test`) must stay green; this design touches no domain or handler code.
2. **Build smoke test:** `build-demo.ps1` completes and produces a non-trivial `dist/demo/SpaSalonDemo.exe`.
3. **Runtime walkthrough**, driven in Chrome against the running exe:
   - home renders
   - services list shows seeded services
   - staff list shows seeded staff
   - booking wizard completes and the POST round-trips
   - my bookings, loyalty, waitlist render
   - `/admin/bookings` entered directly as a URL resolves rather than 404ing
   - browser console shows no failed `/api` requests
4. **Offline check:** disable the network adapter and confirm the walkthrough still passes.
5. **Port-conflict check:** occupy 5139, launch, confirm the fallback port is chosen and the banner matches the browser.

A clean-machine test (no .NET installed) is the one case that cannot be verified from this workstation. It is called out as a residual risk.

---

## 8. Risks

| Risk | Mitigation |
|---|---|
| Single-file self-extract makes first launch slow | Accepted; subsequent launches reuse the extracted cache. |
| Self-contained exe is 80–150 MB | Accepted; trimming was rejected during design because MediatR assembly scanning, EF Core and Swashbuckle are reflection-heavy. |
| Windows SmartScreen warns on an unsigned exe | Out of scope. Flag it to whoever presents the demo. |
| Clean-machine behaviour unverified from here | Residual. Self-contained publish is the standard mechanism for this and the risk is low, but it is not zero. |

---

## 9. Known defects left in place

Surfaced during analysis, deliberately out of scope for this packaging work:

- `ExceptionHandlingMiddleware` returns `exception.StackTrace` in every error response body with no environment check.
- `/hangfire` is mounted with no authorization filter. Loopback-only binding contains it for the demo but the defect remains in the codebase.
- `AuthController.GoogleLogin` does not validate the incoming Google token and mints a synthetic external id per call.
- `CreateBookingCommandValidator` is never registered, so create-booking requests are unvalidated.
- The JWT signing secret is plaintext in `appsettings.json`.

These are listed so that packaging a demo is not mistaken for readiness to deploy.
