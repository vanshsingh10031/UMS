$ErrorActionPreference = "Stop"

$src = "outputs\solidcp_package\app"
$dst = "outputs\solidcp_package\ftp_upload"

if (Test-Path $dst) {
  Remove-Item $dst -Recurse -Force
}

Copy-Item $src $dst -Recurse -Force

Remove-Item (Join-Path $dst "appsettings.Development.json") -Force -ErrorAction SilentlyContinue
Remove-Item (Join-Path $dst "UMS.Api.pdb") -Force -ErrorAction SilentlyContinue
Remove-Item (Join-Path $dst "runtimes\unix") -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item (Join-Path $dst "runtimes\win-arm") -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item (Join-Path $dst "runtimes\win-arm64") -Recurse -Force -ErrorAction SilentlyContinue

Get-ChildItem $dst -Recurse -File | Measure-Object Length -Sum
Get-ChildItem $dst -Recurse -File | Sort-Object Length -Descending | Select-Object -First 10 FullName, Length
