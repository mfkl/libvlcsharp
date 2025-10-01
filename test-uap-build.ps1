# Quick test script to build only the UAP target framework
# This helps test UWP SDK configuration without waiting for full CI

Write-Host "Testing UAP 10.0.18362 build..." -ForegroundColor Cyan

# Check if Windows SDK 18362 is installed
function Get-InstalledWindowsSDKRoot {
    $key = "HKLM:\SOFTWARE\Microsoft\Windows Kits\Installed Roots"
    try {
        $vals = Get-ItemProperty -Path $key -ErrorAction Stop
        return $vals.KitsRoot10
    }
    catch {
        return $null
    }
}

$sdkRoot = Get-InstalledWindowsSDKRoot
if ($sdkRoot) {
    Write-Host "Windows SDK Root: $sdkRoot" -ForegroundColor Green
    $sdkPath = Join-Path $sdkRoot "Include\10.0.18362.0"
    if (Test-Path $sdkPath) {
        Write-Host "SDK 10.0.18362 found at: $sdkPath" -ForegroundColor Green
    } else {
        Write-Warning "SDK 10.0.18362 not found. Run install-uwp-sdk-18362.ps1 first."
    }
} else {
    Write-Warning "Windows SDK not found in registry"
}

# Set environment variables that MSBuild might need
if ($sdkRoot) {
    $env:WindowsSdkDir = $sdkRoot
    $env:WindowsSDKVersion = "10.0.18362.0\"
    Write-Host "Set WindowsSdkDir=$env:WindowsSdkDir"
    Write-Host "Set WindowsSDKVersion=$env:WindowsSDKVersion"
}

Write-Host "`nRestoring NuGet packages..." -ForegroundColor Cyan
dotnet restore src\LibVLCSharp\LibVLCSharp.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Error "NuGet restore failed"
    exit $LASTEXITCODE
}

Write-Host "`nBuilding LibVLCSharp for uap10.0.18362 only..." -ForegroundColor Cyan

# Find MSBuild.exe
$msbuildPath = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe | Select-Object -First 1

if (-not $msbuildPath) {
    Write-Error "Could not find MSBuild.exe"
    exit 1
}

Write-Host "Using MSBuild: $msbuildPath" -ForegroundColor Green

& $msbuildPath src\LibVLCSharp\LibVLCSharp.csproj /t:Build /p:TargetFramework=uap10.0.18362 /v:n

$exitCode = $LASTEXITCODE
Write-Host "`nBuild exit code: $exitCode" -ForegroundColor $(if ($exitCode -eq 0) { "Green" } else { "Red" })
exit $exitCode
