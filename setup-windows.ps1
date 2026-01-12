# Check admin
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "Please run as Administrator!" -ForegroundColor Red
    exit
}

Write-Host "Installing prerequisites..." -ForegroundColor Cyan

# Chocolatey
if (-not (Get-Command choco -ErrorAction SilentlyContinue)) {
    Write-Host "Installing Chocolatey..." -ForegroundColor Yellow
    Set-ExecutionPolicy Bypass -Scope Process -Force
    [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072
    Invoke-Expression ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
}

# .NET 8
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    choco install dotnet-8.0-sdk -y
}

# Docker
$installDocker = Read-Host "Install Docker Desktop? (y/n)"
if ($installDocker -eq 'y') { choco install docker-desktop -y }

# SQL Server
$installSQL = Read-Host "Install SQL Server Express? (y/n)"
if ($installSQL -eq 'y') { choco install sql-server-express -y }

# EF tools
if (-not (Get-Command dotnet-ef -ErrorAction SilentlyContinue)) { dotnet tool install --global dotnet-ef }

# Restore project
if (Test-Path 'VALHÄUS.sln') { dotnet restore 'VALHÄUS.sln' }

# Final
Write-Host "`nSetup complete!" -ForegroundColor Green
Write-Host 'Run: dotnet run --project VALHÄUS\VALHAUS.csproj' -ForegroundColor Cyan
