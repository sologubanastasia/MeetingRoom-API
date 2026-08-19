$ErrorActionPreference = 'Stop'
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if (-not $isAdmin) {
    Start-Process powershell.exe -Verb RunAs -ArgumentList '-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', ('"{0}"' -f $PSCommandPath)
    exit
}

$repository = Split-Path -Parent $PSCommandPath
$installDir = Join-Path $env:LOCALAPPDATA 'Programs\DockerDesktop'
$dockerDesktop = Join-Path $installDir 'Docker Desktop.exe'
$docker = Join-Path $installDir 'resources\bin\docker.exe'

if (-not (Test-Path $dockerDesktop) -or -not (Test-Path $docker)) {
    throw 'Docker Desktop was not found in AppData\Local\Programs\DockerDesktop.'
}

Write-Host 'Stopping Docker Desktop and WSL...' -ForegroundColor Cyan
Get-Process 'Docker Desktop', 'com.docker.backend' -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
wsl.exe --shutdown

Write-Host 'Starting Docker Desktop...' -ForegroundColor Cyan
Start-Process -FilePath $dockerDesktop

Write-Host 'Waiting for Docker Engine...' -ForegroundColor Cyan
$ready = $false
for ($attempt = 1; $attempt -le 60; $attempt++) {
    Start-Sleep -Seconds 2
    & $docker info *> $null
    if ($LASTEXITCODE -eq 0) { $ready = $true; break }
    Write-Host "  attempt $attempt/60"
}
if (-not $ready) { throw 'Docker Engine did not become ready within two minutes.' }

Write-Host 'Starting MeetingRoom API...' -ForegroundColor Green
Set-Location $repository
& $docker compose down --remove-orphans
if ($LASTEXITCODE -ne 0) { throw 'docker compose down failed.' }
& $docker compose up --build -d
if ($LASTEXITCODE -ne 0) { throw 'docker compose up failed.' }
& $docker compose ps
Write-Host 'Swagger: http://localhost:8080/swagger' -ForegroundColor Green
Read-Host 'Press Enter to close this window'
