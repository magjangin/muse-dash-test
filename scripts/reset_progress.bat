@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul

echo.
echo ========================================
echo  Muse Dash 진행도 초기화 도구
echo ========================================
echo.
echo [경고] 이 스크립트는 계정 진행도(클리어 기록, 최고점수, 언락 상태)를
echo        백업 후 완전히 삭제합니다.
echo.
echo [중요] Steam 클라우드 동기화가 켜져 있으면 삭제해도 곧바로 복원됩니다.
echo        먼저 Steam 라이브러리 -^> Muse Dash 우클릭 -^> 속성 -^> 업데이트 탭에서
echo        "이 게임에 대해 Steam Cloud 동기화 사용"을 꺼주세요.
echo.
set /p CONFIRM="클라우드 동기화를 끄셨고, 진행도를 초기화하시겠습니까? (Y/N): "
if /I not "!CONFIRM!"=="Y" (
    echo 취소되었습니다.
    pause
    exit /b 0
)

:: Steam이 켜져 있으면 .sav 동기화 중 충돌이 날 수 있으니 안내
tasklist /FI "IMAGENAME eq musedash.exe" 2>nul | find /I "musedash.exe" >nul
if not errorlevel 1 (
    echo [ERROR] Muse Dash가 실행 중입니다. 게임을 종료한 뒤 다시 실행해주세요.
    pause
    exit /b 1
)

set "BACKUP_DIR=%~dp0reset_progress_backup"
if not exist "!BACKUP_DIR!" mkdir "!BACKUP_DIR!"

rem %date%는 로캘마다 형식이 달라 자르는 방식이 깨집니다. 날짜·시간은 PowerShell로 얻습니다.
for /f %%T in ('powershell -NoProfile -Command "Get-Date -Format yyyyMMdd_HHmmss"') do set "TIMESTAMP=%%T"
if not defined TIMESTAMP set "TIMESTAMP=%RANDOM%%RANDOM%"

echo.
echo [INFO] 백업 폴더: !BACKUP_DIR!
echo.

:: 1. .sav 파일 백업 (여기서는 지우지 않습니다. 모든 백업이 끝난 뒤에 3단계에서 지웁니다)
set "SAV_DIR=%LOCALAPPDATA%\Steam\MuseDash"
set "FOUND_SAV="
set "SAV_COUNT=0"
if exist "!SAV_DIR!" (
    for %%F in ("!SAV_DIR!\*MuseDashSaves.sav") do (
        set /a SAV_COUNT+=1
        set "FOUND_SAV=%%F"
    )
)

:: 세이브가 여러 개면 어느 계정의 진행도인지 알 수 없으므로 아무것도 지우지 않고 중단합니다.
if !SAV_COUNT! GTR 1 (
    echo [ERROR] 세이브 파일이 여러 개 있어 어느 것을 지울지 정할 수 없습니다. 아무것도 삭제하지 않고 중단합니다.
    for %%F in ("!SAV_DIR!\*MuseDashSaves.sav") do echo   - %%F
    pause
    exit /b 1
)

if defined FOUND_SAV (
    echo [INFO] 세이브 파일 발견: !FOUND_SAV!
    copy /Y "!FOUND_SAV!" "!BACKUP_DIR!\MuseDashSaves_!TIMESTAMP!.sav" >nul
    if errorlevel 1 (
        echo [ERROR] 세이브 파일 백업에 실패했습니다. 아무것도 삭제하지 않고 중단합니다.
        pause
        exit /b 1
    )
    echo [INFO] 백업 완료 -^> !BACKUP_DIR!\MuseDashSaves_!TIMESTAMP!.sav
) else (
    echo [WARN] 세이브 파일을 찾지 못했습니다: !SAV_DIR!\*MuseDashSaves.sav
)

:: 2. 레지스트리 백업 (키가 있을 때만 내보냅니다. 내보내기가 실패하면 아무것도 지우지 않고 중단합니다)
echo.
echo [INFO] 레지스트리 백업 중...
reg query "HKCU\Software\PeroPeroGames\Muse Dash" >nul 2>&1
if not errorlevel 1 (
    reg export "HKCU\Software\PeroPeroGames\Muse Dash" "!BACKUP_DIR!\Registry_MuseDash_space_!TIMESTAMP!.reg" /y >nul 2>&1
    if errorlevel 1 (
        echo [ERROR] 레지스트리 백업 실패 - Muse Dash 키. 아무것도 삭제하지 않고 중단합니다.
        pause
        exit /b 1
    )
)

reg query "HKCU\Software\PeroPeroGames\MuseDash" >nul 2>&1
if not errorlevel 1 (
    reg export "HKCU\Software\PeroPeroGames\MuseDash" "!BACKUP_DIR!\Registry_MuseDash_nospace_!TIMESTAMP!.reg" /y >nul 2>&1
    if errorlevel 1 (
        echo [ERROR] 레지스트리 백업 실패 - MuseDash 키. 아무것도 삭제하지 않고 중단합니다.
        pause
        exit /b 1
    )
)

:: 3. 모든 백업이 확인된 뒤에만 삭제합니다
if defined FOUND_SAV (
    del /F /Q "!FOUND_SAV!"
    if exist "!FOUND_SAV!" (
        echo [ERROR] 세이브 파일을 지우지 못했습니다. 백업은 남아 있습니다: !FOUND_SAV!
    ) else (
        echo [INFO] 세이브 파일 삭제 완료.
    )
)

reg delete "HKCU\Software\PeroPeroGames\Muse Dash" /f >nul 2>&1
reg delete "HKCU\Software\PeroPeroGames\MuseDash" /f >nul 2>&1
echo [INFO] 레지스트리 삭제 완료.

echo.
echo ========================================
echo [SUCCESS] 진행도 초기화 완료
echo ========================================
echo.
echo 백업 파일은 다음 폴더에 보관되어 있습니다:
echo   !BACKUP_DIR!
echo.
echo 복구하려면:
echo   1. .sav 백업 파일을 "!SAV_DIR!" 폴더에 원래 이름으로 복사
echo   2. .reg 백업 파일을 더블클릭해서 레지스트리 복원
echo.
echo 이제 게임을 실행해 신규 계정처럼 시작되는지 확인하세요.
echo 확인 후 Steam 클라우드 동기화를 다시 켜면 새 빈 세이브가 업로드됩니다.
echo.
pause
