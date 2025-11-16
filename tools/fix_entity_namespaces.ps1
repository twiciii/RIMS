# Run from repo root
$entities = Get-ChildItem -Path .\Models\Entities -Filter *.cs -Recurse
$backup = ".\tools\backups\fix_entity_namespaces_$(Get-Date -Format yyyyMMddHHmmss)"
New-Item -ItemType Directory -Path $backup -Force | Out-Null

foreach ($f in $entities) {
    $text = Get-Content $f.FullName -Raw
    Copy-Item -Path $f.FullName -Destination (Join-Path $backup $f.Name) -Force
    if ($text -match 'namespace\s+[^\r\n]+') {
        $new = $text -replace 'namespace\s+[^\r\n]+','namespace RIMS.Models.Entities'
    } else {
        $new = "namespace RIMS.Models.Entities`n{`n" + $text + "`n}"
    }
    $new = $new -replace 'using\s+YourNamespace\.Models\s*;','using RIMS.Models.Entities;'
    Set-Content -Path $f.FullName -Value $new -Encoding UTF8
    Write-Host "Normalized: $($f.FullName)"
}
Write-Host "Backuped originals to: $backup"