<#
.SYNOPSIS
  muse-dash-custom-chart 릴리즈: 버전 확인 → git/CI 확인 → Release 빌드 → 로직 테스트 → (선택) 태그·GitHub 릴리즈.

.DESCRIPTION
  버전은 "muse dash test\Core\ModInfo.cs" 의 ModInfo.Version 하나만 기준으로 삼고(MelonInfo·어셈블리 버전이 이 값을 읽음),
  태그는 v<버전> 입니다. CHANGELOG.md 의 "## [<버전>]" 항목이 릴리즈 노트 본문이 됩니다.

  기본 실행은 아무것도 게시하지 않습니다: 검증 후 dist\muse-dash-custom-chart.dll 과 릴리즈 노트 초안만 만듭니다.
  -Publish 를 주면 main 이 origin/main 과 같고, 작업 트리가 깨끗하고, HEAD 의 CI(Logic Tests)가 통과했는지 확인한 뒤
  주석 태그를 만들어 origin 에 푸시하고 gh 로 GitHub 릴리즈를 만들어 DLL 을 올립니다.

  모드 빌드에는 게임의 MelonLoader\net6 와 Il2CppAssemblies 가 필요해 GitHub Actions 에서는 빌드할 수 없습니다.
  그래서 게임이 설치된 PC 에서 실행하는 로컬 스크립트입니다(docs\guides\CHECKLIST.md 4절의 자동화).

  첨부 파일은 v0.16.0 까지와 같은 구성인 muse-dash-custom-chart.dll 하나입니다. 게임 파일·덤프·디컴파일 코드는 넣지 않습니다.

.PARAMETER Publish
  태그 생성·푸시와 GitHub 릴리즈 생성까지 합니다. 없으면 검증과 dist 까지만.

.PARAMETER GamePath
  게임 폴더. 없으면 csproj 기본값(H:\muse dash hwa).

.PARAMETER SkipCiCheck
  -Publish 때 CI 통과 확인을 건너뜁니다(릴리즈 노트에 "CI 확인 생략"이 적힘).

.EXAMPLE
  .\tools\release.ps1              # 검증 + dist\muse-dash-custom-chart.dll
  .\tools\release.ps1 -Publish     # 위 + 태그 v<버전> 푸시 + GitHub 릴리즈
#>
[CmdletBinding()]
param(
    [switch]$Publish,
    [string]$GamePath,
    [switch]$SkipCiCheck
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root
$repo = 'magjangin/muse-dash-test'
$dllName = 'muse-dash-custom-chart.dll'

function Step($msg) { Write-Host "`n== $msg" -ForegroundColor Cyan }
function Invoke-Native {
    param([string]$What, [string]$Exe, [string[]]$Arguments)
    & $Exe @Arguments
    if ($LASTEXITCODE -ne 0) { throw "$What 실패 (종료 코드 $LASTEXITCODE)" }
}
function Write-Utf8NoBom($path, $text) {
    # Windows PowerShell 5.1 의 Set-Content -Encoding utf8 은 BOM 을 붙여 gh 릴리즈 노트 첫 글자가 깨질 수 있음
    [IO.File]::WriteAllText($path, $text, (New-Object Text.UTF8Encoding($false)))
}

# ---------------------------------------------------------------- 1. 버전
Step '버전 확인'
$modInfo = Get-Content (Join-Path $root 'muse dash test\Core\ModInfo.cs') -Raw -Encoding UTF8
if ($modInfo -notmatch 'const\s+string\s+Version\s*=\s*"(?<v>[^"]+)"') { throw 'ModInfo.cs 에서 Version 을 찾지 못했습니다.' }
$version = $Matches['v'].Trim()
if ($version -notmatch '^\d+\.\d+\.\d+$') { throw "버전 형식이 x.y.z 가 아닙니다: $version" }
$tag = "v$version"

$changelogPath = Join-Path $root 'CHANGELOG.md'
$changelog = if (Test-Path $changelogPath) { Get-Content $changelogPath -Raw -Encoding UTF8 } else { '' }
$entryPattern = "(?ms)^## \[$([regex]::Escape($version))\][^\r\n]*\r?\n(?<body>.*?)(?=^## \[|\z)"
$changelogBody = if ($changelog -match $entryPattern) { $Matches['body'].Trim() } else { $null }
if (-not $changelogBody) {
    if ($Publish) { throw "CHANGELOG.md 에 '## [$version]' 항목이 없습니다." }
    Write-Warning "CHANGELOG.md 에 '## [$version]' 항목이 없습니다 (-Publish 때는 중단됩니다)."
}
Write-Host "버전 $version (태그 $tag)"

# ---------------------------------------------------------------- 2. git·CI 상태 (게시할 때만 강제)
Step 'git 상태 확인'
Invoke-Native 'git fetch' git @('fetch', 'origin', '--tags', '--quiet')
$dirty = git status --porcelain
$branch = (git rev-parse --abbrev-ref HEAD).Trim()
$head = (git rev-parse HEAD).Trim()
$originMain = (git rev-parse origin/main).Trim()
$localTag = git tag --list $tag
$remoteTag = git ls-remote --tags origin "refs/tags/$tag"

$problems = @()
if ($dirty) { $problems += '작업 트리에 커밋하지 않은 변경이 있습니다.' }
if ($branch -ne 'main') { $problems += "현재 브랜치가 main 이 아닙니다($branch)." }
if ($head -ne $originMain) { $problems += 'HEAD 가 origin/main 과 다릅니다(푸시·머지 또는 pull 필요).' }
if ($localTag -or $remoteTag) { $problems += "태그 $tag 가 이미 있습니다. ModInfo.Version 과 CHANGELOG 를 올리세요." }
if ($problems) {
    if ($Publish) { throw ("게시할 수 없습니다:`n - " + ($problems -join "`n - ")) }
    $problems | ForEach-Object { Write-Warning $_ }
} else { Write-Host "main = origin/main ($($head.Substring(0, 7))), 태그 $tag 없음" }

$ciLine = 'CI 확인 안 함(게시 안 함)'
if ($Publish) {
    if (-not (Get-Command gh -ErrorAction SilentlyContinue)) { throw 'gh(GitHub CLI)가 없습니다. https://cli.github.com 설치 후 gh auth login.' }
    Invoke-Native 'gh auth status' gh @('auth', 'status')
    if ($SkipCiCheck) { $ciLine = 'CI 확인 생략(-SkipCiCheck)' }
    else {
        # CHECKLIST 4절: CI 는 만든 날부터 빨간불이었는데 릴리즈 4개가 그대로 나갔습니다(2026-11-23).
        $runs = gh run list --repo $repo --commit $head --workflow 'Logic Tests' --json status,conclusion,url | ConvertFrom-Json
        $done = @($runs | Where-Object { $_.status -eq 'completed' })
        if (-not $done) { throw "HEAD($($head.Substring(0, 7)))의 CI(Logic Tests) 완료 기록이 없습니다. 끝날 때까지 기다리거나 -SkipCiCheck." }
        if (@($done | Where-Object { $_.conclusion -ne 'success' })) { throw "HEAD 의 CI 가 실패했습니다: $($done[0].url)" }
        $ciLine = "CI(Logic Tests) 통과: $($done[0].url)"
        Write-Host $ciLine
    }
}

# ---------------------------------------------------------------- 3. 빌드·테스트
$gameArgs = @()
if ($GamePath) { $gameArgs = @("-p:GamePath=$GamePath") }

Step 'Release 빌드 (게임 Mods 폴더로 복사하지 않음)'
$buildOut = & dotnet (@('build', 'muse dash test\muse dash test.csproj', '-c', 'Release', '-p:SkipDeploy=true', '-nologo') + $gameArgs) 2>&1 | ForEach-Object { "$_" }
$buildExit = $LASTEXITCODE
$buildOut | ForEach-Object { Write-Host $_ }
if ($buildExit -ne 0) { throw "빌드 실패 (종료 코드 $buildExit)" }
$warnLine = $buildOut | Where-Object { $_ -match '^\s*\d+\s+(Warning|경고)' } | Select-Object -Last 1
$dll = Get-ChildItem (Join-Path $root 'muse dash test\bin\Release') -Recurse -Filter $dllName | Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (-not $dll) { throw "DLL 없음: muse dash test\bin\Release\**\$dllName" }
$fileVer = $dll.VersionInfo.FileVersion
if ($fileVer -and -not $fileVer.StartsWith($version)) { throw "빌드된 DLL 버전($fileVer)이 $version 과 다릅니다." }

Step '로직 테스트'
$testOut = & dotnet run --project 'muse dash test.LogicTests\muse dash test.LogicTests.csproj' -c Release 2>&1 | ForEach-Object { "$_" }
$testExit = $LASTEXITCODE
$testOut | Select-Object -Last 5 | ForEach-Object { Write-Host $_ }
if ($testExit -ne 0) { throw "로직 테스트 실패 (종료 코드 $testExit)" }
$testLine = $testOut | Where-Object { $_ -match '^총 \d+개 테스트' } | Select-Object -Last 1

# ---------------------------------------------------------------- 4. 산출물
Step '산출물'
$dist = Join-Path $root 'dist'
New-Item -ItemType Directory -Force $dist | Out-Null
$distDll = Join-Path $dist $dllName
Copy-Item $dll.FullName $distDll -Force
$dllHash = (Get-FileHash $distDll -Algorithm SHA256).Hash.ToLowerInvariant()
Write-Host "생성: $distDll"
Write-Host "SHA256 $dllHash"

$bt = [char]96
$notes = @"
$(if ($changelogBody) { $changelogBody } else { "$tag" })

## 설치
${bt}$dllName${bt}을 게임의 ${bt}Mods${bt} 폴더에 덮어씁니다 (Muse Dash 6.7.0, MelonLoader 0.7.3 기준). 게임 실행 중에는 DLL이 잠겨 있으니 종료 후 복사하세요.

## 릴리즈 스크립트 검증
- Release 빌드 성공$(if ($warnLine) { " ($($warnLine.Trim()))" })
- 로직 테스트$(if ($testLine) { ": $($testLine.Trim())" } else { " 통과" })
- $ciLine
- SHA256 (${bt}$dllName${bt}): ${bt}$dllHash${bt}

이전 릴리즈 이력은 [CHANGELOG.md](https://github.com/$repo/blob/main/CHANGELOG.md)와 GitHub 릴리즈 목록을 보십시오.
"@
$notesFile = Join-Path $dist "notes-$tag.md"
Write-Utf8NoBom $notesFile $notes
Write-Host "릴리즈 노트 초안: $notesFile"

if (-not $Publish) {
    Write-Host "`n게시하지 않았습니다. 태그·릴리즈까지 하려면: .\tools\release.ps1 -Publish" -ForegroundColor Yellow
    return
}

# ---------------------------------------------------------------- 5. 태그·릴리즈
Step "태그 $tag 생성·푸시"
Invoke-Native 'git tag' git @('tag', '-a', $tag, '-m', "muse-dash-custom-chart $tag")
try {
    Invoke-Native 'git push tag' git @('push', 'origin', "refs/tags/$tag")
} catch {
    git tag -d $tag | Out-Null
    throw "태그 푸시 실패, 로컬 태그를 지웠습니다: $_"
}

Step 'GitHub 릴리즈 생성'
$title = if ($changelog -match "(?m)^## \[$([regex]::Escape($version))\][ \t]*-[ \t]*(?<d>[^\r\n]+)") { "$tag - $($Matches['d'].Trim())" } else { $tag }
try {
    Invoke-Native 'gh release create' gh @('release', 'create', $tag, $distDll, '--repo', $repo, '--verify-tag', '--title', $title, '--notes-file', $notesFile)
} catch {
    Write-Warning "태그 $tag 는 푸시됐지만 릴리즈 생성이 실패했습니다. 원인을 고친 뒤 다음으로 다시 만드세요:"
    Write-Warning "  gh release create $tag `"$distDll`" --repo $repo --verify-tag --title `"$title`" --notes-file `"$notesFile`""
    throw
}
Write-Host "`n릴리즈 게시 완료: https://github.com/$repo/releases/tag/$tag" -ForegroundColor Green
