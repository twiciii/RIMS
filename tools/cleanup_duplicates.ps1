param(
    [switch]$Execute
)

<#
.SYNOPSIS
  Find and move legacy/duplicate entity files from `Models` (outside `Models\Entities`) into a timestamped backup.

.DESCRIPTION
  - Dry-run by default: lists candidate files and duplicate type occurrences.
  - Use -Execute to actually move files.
  - Use -ReportOnly to only emit the JSON report (no moves), useful for automation.

.USAGE
  # Dry-run
  pwsh .\tools\cleanup_duplicates.ps1

# Where we put backups when moving files
$backupRoot = Join-Path $repoRoot ("deleted_entities_backup_{0}" -f (Get-Date -Format 'yyyyMMddHHmmss'))

# Candidate detection patterns
$knownEntities = @(
    'Resident','Document','DocumentApplication','ResidentCategory',
    'AuditTrail','Address','Streets','UserAction','UserActions',
    'Users','Roles','RoleClaims','UserClaims','UserLogins','UserRoles',
    'Assistance'
)

# Regex patterns
$tableAttributePattern = '\[Table\(\s*""?rims'
$classPattern = 'class\s+([A-Za-z_][A-Za-z0-9_]*)\b'
$namespaceRimsModelsPattern = 'namespace\s+RIMS(\.Models(\.Entities)?)?\b'

Write-Host "Scanning repository for legacy/duplicate entity files..." -ForegroundColor Cyan

# Gather all .cs files except within Models\Entities and excluded folders
$allCsFiles = Get-ChildItem -Path $repoRoot -Filter *.cs -Recurse -File |
    Where-Object {
        ($_.FullName -notmatch [regex]::Escape($entitiesDir)) -and
        ($excludePaths -notcontains ($_.FullName.Split([IO.Path]::DirectorySeparatorChar) | Where-Object {$_ -in $excludePaths} | Select-Object -First 1))
    }

if ($allCsFiles.Count -eq 0) {
    Write-Host "No .cs files found to analyze." -ForegroundColor Yellow# Gather all .cs files except within Models\Entities and excluded folders
$allCsFiles = Get-ChildItem -Path $repoRoot -Filter *.cs -Recurse -File |
    Where-Object {
        ($_.FullName -notmatch [regex]::Escape($entitiesDir)) -and
        ($excludePaths -notcontains ($_.FullName.Split([IO.Path]::DirectorySeparatorChar) | Where-Object {$_ -in $excludePaths} | Select-Object -First 1))
    }

if ($allCsFiles.Count -eq 0) {
    Write-Host "No .cs files found to analyze." -ForegroundColor Yellow
    exit 0
}

$matches = @()

foreach ($f in $files) {
    $content = Get-Content -Raw -ErrorAction SilentlyContinue $f.FullName
    if (-not $content) { continue }

    if ($content -match $entityClassPattern -or $content -match $tableAttributePattern -or $content -match 'namespace\s+RIMS\.Models\b') {
        $relative = Resolve-Path -Path $f.FullName | ForEach-Object { $_.Path.Substring($modelsDir.Length).TrimStart('\') }
        $dstDir = Join-Path $backupRoot (Split-Path $relative -Parent)
        $dstPath = Join-Path $dstDir $f.Name
        $matches += [pscustomobject]@{ File = $f.FullName; Relative = $relative; Destination = $dstPath }
    }
}

if ($matches.Count -eq 0) {
    Write-Host "No duplicate/legacy entity files detected under Models (outside Models\\Entities)."
    exit 0
}

Write-Host "Detected the following candidate files to move to backup:"
$matches | ForEach-Object { Write-Host "- $($_.Relative)" }

if (-not $Execute) {
    Write-Host ""
    Write-Host "This is a dry-run. To actually move the files run with the -Execute switch:"
    Write-Host "  pwsh .\\tools\\cleanup_duplicates.ps1 -Execute"
    exit 0
}

# Execute move (create backup structure)
foreach ($m in $matches) {
    $dstDir = Split-Path -Path $m.Destination -Parent
    if (-not (Test-Path $dstDir)) { New-Item -ItemType Directory -Path $dstDir -Force | Out-Null }
    Move-Item -Path $m.File -Destination $m.Destination -Force
    Write-Host "Moved: $($m.Relative) -> $($m.Destination)"
}

Write-Host ""
Write-Host "Moved $($matches.Count) file(s) to: $backupRoot"
Write-Host "Review files in the backup folder, then run __Build > Rebuild Solution__ in Visual Studio."