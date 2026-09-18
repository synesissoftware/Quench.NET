param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectNameFile = Join-Path $Root ".sis/project_name.txt"
$ProjectName = (Get-Content -Raw $ProjectNameFile).Trim()
$Artifacts = Join-Path $Root "artifacts"

Set-Location $Root

Write-Host "${ProjectName}: restore / build / test / pack ($Configuration)"

dotnet restore Quench.NET.sln
dotnet build Quench.NET.sln --configuration $Configuration --no-restore
dotnet test Quench.NET.sln --configuration $Configuration --no-build --verbosity normal

$Packages = Join-Path $Artifacts "packages"
New-Item -ItemType Directory -Force -Path $Packages | Out-Null

dotnet pack src/Quench.NET/Quench.NET.csproj `
  --configuration $Configuration `
  --no-build `
  --output $Packages

Write-Host "${ProjectName}: packages written to $Packages"
