# Run with Windows PowerShell 5.1 as Administrator, on the IIS machine.
[CmdletBinding()]
param(
    [string]$RepositoryRoot = 'C:\Code\OjitexSystem',
    [string]$SiteName = 'OjitexSystem',
    [string]$ReleasesRoot = 'C:\inetpub\OjitexSystem-releases',
    [string]$ProductionSettingsPath = 'C:\ProgramData\OjitexSystem\appsettings.Production.json',
    [string]$WebsiteUrl = 'http://172.88.1.72:8080'
)

$ErrorActionPreference = 'Stop'

function Assert-NativeSuccess([string]$Operation) {
    if ($LASTEXITCODE -ne 0) { throw "$Operation failed (exit $LASTEXITCODE)." }
}

function Wait-PoolState([string]$PoolName, [string]$ExpectedState) {
    $deadline = [DateTime]::UtcNow.AddSeconds(30)
    while ((Get-WebAppPoolState -Name $PoolName).Value -ne $ExpectedState) {
        if ([DateTime]::UtcNow -ge $deadline) { throw "App pool did not reach $ExpectedState." }
        Start-Sleep -Milliseconds 250
    }
}

$identity = [Security.Principal.WindowsIdentity]::GetCurrent()
$principal = New-Object Security.Principal.WindowsPrincipal($identity)
if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    throw 'Open Windows PowerShell as Administrator.'
}
Import-Module WebAdministration
$site = Get-Website -Name $SiteName
if (-not $site) { throw "IIS site not found: $SiteName" }
if ($site.State -ne 'Started') { throw 'Start the existing IIS site before deploying.' }
$poolName = [string]$site.applicationPool
if ((Get-WebAppPoolState -Name $poolName).Value -ne 'Started') {
    throw 'Start the existing application pool before deploying.'
}
if (@(Get-Website | Where-Object { $_.applicationPool -eq $poolName }).Count -ne 1 -or
    @(Get-WebApplication | Where-Object { $_.applicationPool -eq $poolName }).Count -gt 0) {
    throw 'This script requires an application pool dedicated to this site.'
}
if (-not (Test-Path -LiteralPath $ProductionSettingsPath -PathType Leaf)) {
    throw "Prepare the production settings file first: $ProductionSettingsPath"
}
$previousPath = [string]$site.physicalPath
$releaseId = (Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + [Guid]::NewGuid().ToString('N').Substring(0, 8)
$stage = Join-Path $RepositoryRoot "artifacts\iis-staging\$releaseId"
$package = Join-Path $stage 'backend'
$frontendOutput = Join-Path $stage 'frontend'
$release = Join-Path $ReleasesRoot $releaseId

Push-Location $RepositoryRoot
try {
    # Dependencies must already be restored/installed, as described in README.
    dotnet build OjitexSystem_Backend/OjitexSystem_Backend.slnx -c Release --no-restore
    Assert-NativeSuccess 'Backend build'
    dotnet test OjitexSystem_Backend/OjitexSystem_Backend.slnx -c Release --no-build --no-restore
    Assert-NativeSuccess 'Backend tests'

    Push-Location OjitexSystem_Frontend
    try {
        npm.cmd run format:check
        Assert-NativeSuccess 'Frontend formatting'
        npm.cmd run test:ci
        Assert-NativeSuccess 'Frontend tests'
        npm.cmd run build -- --configuration production --output-path $frontendOutput
        Assert-NativeSuccess 'Frontend build'
    } finally { Pop-Location }

    dotnet publish OjitexSystem_Backend/OjitexSystem-Backend/OjitexSystem-Backend.csproj -c Release --no-restore -o $package /p:EnvironmentName=Production
    Assert-NativeSuccess 'Backend publish'
    $browserFiles = Join-Path $frontendOutput 'browser'
    if (-not (Test-Path -LiteralPath (Join-Path $browserFiles 'index.html'))) {
        throw 'Angular browser/index.html was not generated.'
    }
    $wwwroot = Join-Path $package 'wwwroot'
    New-Item -ItemType Directory -Path $wwwroot -Force | Out-Null
    Copy-Item -Path (Join-Path $browserFiles '*') -Destination $wwwroot -Recurse -Force
    Copy-Item -LiteralPath $ProductionSettingsPath -Destination (Join-Path $package 'appsettings.Production.json') -Force
    foreach ($file in @('appsettings.json', 'web.config', 'wwwroot\index.html')) {
        if (-not (Test-Path -LiteralPath (Join-Path $package $file))) { throw "Package missing: $file" }
    }
    New-Item -ItemType Directory -Path $ReleasesRoot -Force | Out-Null
    Copy-Item -LiteralPath $package -Destination $release -Recurse
    $readPermission = 'IIS AppPool\' + $poolName + ':(OI)(CI)RX'
    icacls.exe $release /grant $readPermission /T /Q
    Assert-NativeSuccess 'Release read permissions'
    $record = [ordered]@{ Site = $SiteName; Pool = $poolName; PreviousPath = $previousPath; NewPath = $release }
    $record | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $stage 'deployment.json') -Encoding UTF8

    # Build and packaging finish before interrupting the live application.
    try {
        Stop-WebAppPool -Name $poolName
        Wait-PoolState $poolName 'Stopped'
        Set-ItemProperty -LiteralPath "IIS:\Sites\$SiteName" -Name physicalPath -Value $release
        Start-WebAppPool -Name $poolName
        Wait-PoolState $poolName 'Started'
        $healthy = $false
        for ($attempt = 0; $attempt -lt 3; $attempt++) {
            try {
                $response = Invoke-WebRequest -Uri $WebsiteUrl -UseBasicParsing -TimeoutSec 15
                if ($response.StatusCode -eq 200) { $healthy = $true; break }
            } catch { }
            Start-Sleep -Seconds 2
        }
        if (-not $healthy) { throw 'Website did not return HTTP 200.' }
    } catch {
        $deploymentError = $_
        try {
            if ((Get-WebAppPoolState -Name $poolName).Value -ne 'Stopped') {
                Stop-WebAppPool -Name $poolName
                Wait-PoolState $poolName 'Stopped'
            }
            Set-ItemProperty -LiteralPath "IIS:\Sites\$SiteName" -Name physicalPath -Value $previousPath
            Start-WebAppPool -Name $poolName
            Wait-PoolState $poolName 'Started'
            Write-Warning "Restored previous IIS physical path: $previousPath"
        } catch {
            Write-Warning "Automatic rollback failed. Restore IIS physical path manually to: $previousPath"
        }
        throw $deploymentError
    }
    Write-Host "Deployed: $release"
    Write-Host "Previous version (keep for rollback): $previousPath"
    Write-Host 'Check login and a database-backed report in your browser.'
} finally { Pop-Location }
