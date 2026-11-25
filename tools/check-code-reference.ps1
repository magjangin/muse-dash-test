# 모드 소스(.cs) 파일마다 docs/architecture/CODE_REFERENCE.md 또는 ARCHITECTURE.md에 이름이 한 번이라도 나오는지 확인합니다.
# 새 파일을 추가하고 문서에 적지 않으면 실패합니다(CI "Logic Tests" 워크플로에서 실행).
#
# 왜: 2026-11-25 감사 9차에서 모드 소스 161개 중 73개가 레퍼런스 문서에 한 번도 나오지 않았습니다.
# 문서는 파일이 생길 때마다 손으로 따라가야 했고, 아무도 확인하지 않아 절반 가까이가 빠진 채 남았습니다.
#
# 사용: .\tools\check-code-reference.ps1
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$sourceRoot = Join-Path $root 'muse dash test'
$docPaths = @(
    (Join-Path $root 'docs/architecture/CODE_REFERENCE.md'),
    (Join-Path $root 'docs/architecture/ARCHITECTURE.md')
)

$docText = ($docPaths | ForEach-Object { Get-Content -LiteralPath $_ -Raw -Encoding utf8 }) -join "`n"

$missing = Get-ChildItem -LiteralPath $sourceRoot -Recurse -Filter '*.cs' -File |
    Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' } |
    ForEach-Object { [System.IO.Path]::GetFileNameWithoutExtension($_.Name) } |
    Where-Object { -not $docText.Contains($_) } |
    Sort-Object -Unique

if ($missing) {
    Write-Host "CODE_REFERENCE.md(또는 ARCHITECTURE.md)에 없는 소스 파일 $($missing.Count)개:" -ForegroundColor Red
    $missing | ForEach-Object { Write-Host "  - $_" }
    Write-Host "docs/architecture/CODE_REFERENCE.md의 '12. 전체 파일 지도'에 한 줄씩 추가하십시오."
    exit 1
}

Write-Host "CODE_REFERENCE 대조 통과: 모든 소스 파일이 문서에 있습니다." -ForegroundColor Green
exit 0
