#requires -Version 5.1
<#
    Builds the offline demo executable.

    Output: dist/demo/SpaSalonDemo.exe - self-contained win-x64, needs no
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
