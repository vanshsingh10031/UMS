$ErrorActionPreference = "Stop"

$root = Resolve-Path "."
$packageRoot = Join-Path $root "outputs\solidcp_package"
$app = Join-Path $packageRoot "app"
$wwwroot = Join-Path $app "wwwroot"
$sql = Join-Path $packageRoot "sql"
$connectionString = $env:UMS_SOLIDCP_CONNECTION_STRING
if ([string]::IsNullOrWhiteSpace($connectionString)) {
  $connectionString = "Server=<sql-host>,<port>;Database=UMSPresentationReplica;User ID=<sql-user>;Password=<sql-password>;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=True"
}

New-Item -ItemType Directory -Force -Path $wwwroot | Out-Null
Copy-Item -Path "dist\*" -Destination $wwwroot -Recurse -Force

$settings = @{
  Logging = @{
    LogLevel = @{
      Default = "Information"
      "Microsoft.AspNetCore" = "Warning"
    }
  }
  AllowedHosts = "*"
  ConnectionStrings = @{
    DefaultConnection = $connectionString
  }
  Jwt = @{
    Key = "solidcp-production-change-this-key-32-chars-minimum"
    Issuer = "UMS"
    Audience = "UMS-Frontend"
    ExpirationMinutes = 120
  }
  Cors = @{
    Origins = @("http://ums.dynode.in", "https://ums.dynode.in")
  }
  Uploads = @{
    Directory = "uploads"
    MaxBytes = 5242880
  }
  GradePoints = @{
    "A+" = 10
    A = 9
    "B+" = 8
    B = 7
    "C+" = 6
    C = 5
    D = 4
    F = 0
  }
}

$settings | ConvertTo-Json -Depth 10 | Set-Content -Path (Join-Path $app "appsettings.json") -Encoding UTF8
New-Item -ItemType Directory -Force -Path (Join-Path $app "uploads") | Out-Null

$readme = @"
SolidCP deployment package for UMS

Upload target:
- Upload the CONTENTS of the app folder into the domain wwwroot shown in SolidCP.
- Keep web.config, UMS.Api.dll, appsettings.json, wwwroot, and uploads together.

Single-domain routing:
- Frontend: /
- API: /api

Database:
- SQL Server: set through UMS_SOLIDCP_CONNECTION_STRING before packaging
- Database expected by package: UMSPresentationReplica

Database setup:
1. Create database UMSPresentationReplica in SolidCP/SQL Server if it does not exist.
2. Run sql/UMS_Presentation_Replica_Full.sql if the host allows full schema creation.
3. If the full script fails because the database already exists, run EF migrations first and then seed.

Important:
- The appsettings.json inside app contains the production SQL connection string.
- For a public deployment, rotate the JWT key after testing.
"@
Set-Content -Path (Join-Path $packageRoot "README_DEPLOYMENT.txt") -Value $readme -Encoding UTF8

New-Item -ItemType Directory -Force -Path $sql | Out-Null
Copy-Item "outputs\demo_dataset\UMS_Presentation_Replica_Full.sql" (Join-Path $sql "UMS_Presentation_Replica_Full.sql") -Force
Copy-Item "outputs\demo_dataset\UMS_Presentation_Demo_Dataset.xlsx" (Join-Path $sql "UMS_Presentation_Demo_Dataset.xlsx") -Force

$zip = Join-Path $packageRoot "UMS_SolidCP_Deployment.zip"
if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path $app, $sql, (Join-Path $packageRoot "README_DEPLOYMENT.txt") -DestinationPath $zip -Force

Get-ChildItem $packageRoot | Select-Object Name, Length
