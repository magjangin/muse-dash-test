# 저장소 전체 범위 리뷰 — 2026-09-12

검토 기준 커밋: `5b659dea132da4a3e3a554cc065d18d44be9b58f`.
시작 전에 `git fetch origin`을 실행했다. `main`은 `origin/main`보다 2커밋 앞서고 뒤처짐은 없었으며 작업 트리는 깨끗했다.

## 범위와 검증 수준

- Git 추적 파일 199개를 대상으로 목록·크기·구조를 점검했다. 텍스트 198개에는 C# 141개, Markdown 33개, PowerShell 13개, Python 2개, 배치 3개와 프로젝트·설정 파일이 포함된다. 나머지 1개는 PNG 리소스다.
- 전체 텍스트에 정적 패턴 검사와 문서 링크 검사를 적용했고, 기록/세이브, BMS 파싱·갱신, 차트 주입, 미디어 생명주기, 설정 게이트, 태그 등록, 빌드·초기화 스크립트는 관련 호출부를 상세 대조했다. 모든 줄을 수동 정독하거나 모든 Harmony 훅을 실행했다는 의미는 아니다.
- 파일별 일괄 점검 범위는 [파일 목록](REPOSITORY_REVIEW_2026-09-12_FILES.md)에 남겼다.
- 생성물, `.git` 내부, 로컬 전용 디컴파일 자료는 전체 파일 검사의 대상에서 제외했다. PNG의 디자인 평가는 하지 않았다.
- 소스 파일은 모두 600줄 이하다. 이번 작업은 리뷰이며 게임 동작 코드는 수정하지 않았다.
- `docs/guides/CHECKLIST.md`를 확인했다. 실제 게임의 정상/앨범·BMS 없는 `0-0` 폴백 배틀 진입은 실행하지 않았다. 컴파일 성공으로 IL2CPP 훅의 실행 안전성을 보증할 수 없다.

## 우선 수정할 문제

### 1. [P1] 백업에 실패해도 원본 진행도를 삭제한다

위치: [reset_progress.bat](../../scripts/reset_progress.bat#L53), 53–68행.

`.sav`의 `copy` 결과를 검사하지 않고 바로 `del`을 실행한다. 레지스트리도 `reg export` 두 건의 성공 여부를 확인하지 않고 `reg delete`로 넘어간다. 백업 폴더 쓰기 권한이나 디스크 공간 문제로 백업이 실패하면 원본만 삭제될 수 있다. 화면에는 백업 완료/초기화 성공이 표시된다.

개선: 각각의 백업이 성공하고 산출물이 유효한지 확인한 뒤 해당 원본만 삭제한다. 하나라도 실패하면 삭제 단계 전에 오류 코드로 중단한다. 실제 사용자 세이브/레지스트리 삭제는 검증 목적으로 실행하지 않았다.

### 2. [P1] 표시용으로 바꾼 계정 해금 값을 저장 전에 복원하지 않는다

위치: [PnlStagePatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlStagePatch.cs#L24), 24–28행 및 261–265행; [UnlockAllMasterGuard.cs](../../muse%20dash%20test/Core/UnlockAllMasterGuard.cs#L23); [SaveDataManagerPatch.cs](../../muse%20dash%20test/Patches/Database/Save/SaveDataManagerPatch.cs).

곡 선택 화면 진입/난이도 갱신 시 `Account["IsUnlockAllMaster"]`를 조건 없이 `true`로 바꾼다. 원래 값을 `CaptureOnce`로 보관하지만 `GetRestoreValue()`의 호출부가 없다. 저장 Prefix는 가상 UID 필드와 리스트만 제거하므로 이 계정 필드는 복원하지 않는다. 원래 `false`였던 계정에서 화면을 열고 저장하면 모드의 임시 변경이 원본 저장 데이터에 포함될 위험이 있다.

개선: 가능하면 계정 저장 필드 대신 표시/조회 결과에만 개입한다. 저장 시 복원이 필요하다면 저장 실패·예외까지 포함한 복원 흐름을 설계하고, 정상 플레이로 획득한 해금 상태는 보존해야 한다. 정적 호출 경로로 확인한 문제이며 실제 계정 저장 파일의 변경은 이번에 실험하지 않았다.

### 3. [P1] AP 배너 기능을 끄면 커스텀 플레이 기록도 저장되지 않는다

위치: [APModPatch.VictoryBanner.cs](../../muse%20dash%20test/Patches/Battle/UI/APModPatch.VictoryBanner.cs#L41), 41–52행.

`EnableAPMod=false`이면 `TrySaveCustomRecord()` 전에 반환한다. 전체 소스에서 `CustomRecordStore.SaveResult()`의 호출은 이 경로 하나다. 배너·정확도 오버라이드를 끄려던 사용자는 커스텀 기록도 잃는다. 반면 `APModPatch.cs`의 정확도 Postfix 세 개에는 같은 게이트가 없어 설정의 실제 의미도 일관되지 않는다.

개선: 기록 저장과 배너 렌더링을 분리하고, AP 토글을 꺼도 기록 저장은 유지한다. 커스텀 차트에 필수인 정확도 보정과 선택 기능인 배너의 설정 의미도 분리한다. 토글을 끈 한 판의 기록 생성/누적을 회귀 테스트에 포함한다.

### 4. [P1] 이전 채보의 결과에 수정된 BMS의 지문이 붙는다 — 재현 확인

위치: [CustomRecordStore.cs](../../muse%20dash%20test/Core/CustomRecordStore.cs#L164), 164행; [ChartFingerprint.cs](../../muse%20dash%20test/Core/ChartFingerprint.cs#L54), 54행.

전투에 주입한 채보의 지문을 보관하지 않고 결과 저장 시점에 파일을 다시 해시한다. A를 플레이하는 동안 파일을 B로 수정하면 A의 점수·FC/AP가 B의 지문으로 저장되고 `LoadResult()`의 대조도 통과한다. 워처 갱신 성공 여부와 무관하게 발생한다.

실제 소스를 연결한 격리 프로그램에서 노트 1개짜리 A를 캐시에 로드하고 디스크를 노트 2개짜리 B로 바꾼 뒤 A 결과를 저장했다. 저장 노트 수는 1인데 저장 지문은 B의 `21be2ed59283dcf1`로 나왔으며 정상 로드되었다. A의 지문은 `7e19f054778b85cc`였다.

개선: 파싱에 사용한 동일 바이트의 지문과 곡 키·난이도를 전투 시작 시 고정하고, 저장/병합에서도 그 전투 스냅샷을 사용한다. 끝날 때 파일을 다시 읽어 플레이 대상을 추정하지 않는다.

### 5. [P2] BMS 저장의 마지막 변경을 버려 불완전한 캐시가 남는다 — 재현 확인

위치: [HwaResourceManager.Watcher.cs](../../muse%20dash%20test/Patches/Hwa/HwaResourceManager.Watcher.cs#L86), 86–94행.

현재 코드는 첫 이벤트를 즉시 파싱하고 300ms 안의 후속 이벤트를 버린다. 에디터가 파일을 비운 뒤 내용을 쓰는 저장 방식이면 첫 이벤트에서 빈 채보를 캐시에 넣고 완료 이벤트를 무시한다. 빈 텍스트도 `BmsParser`에서 정상 객체로 반환되므로 재로드 성공으로 취급된다.

파일을 빈 내용으로 저장→이벤트 처리→완성된 노트 2개 저장→즉시 이벤트 처리 순서로 호출했을 때 `disk notes=2, cache notes=0`을 확인했다. OS 이벤트 타이밍을 기다린 테스트가 아니라 실제 핸들러에 해당 순서를 주입한 결정적 재현이다.

개선: 마지막 이벤트 이후 조용해진 시점에 다시 읽는 디바운스로 바꾸고, 저장 중 읽기 실패에 제한적 재시도를 둔다. 파싱 결과를 검증한 다음 캐시를 교체한다.

### 6. [P2] BMS 삭제·이름 변경 후 이전 채보가 계속 사용된다 — 삭제 재현 확인

위치: [HwaResourceManager.Watcher.cs](../../muse%20dash%20test/Patches/Hwa/HwaResourceManager.Watcher.cs#L69), 69–71행 및 140–154행.

삭제 이벤트는 구독하지만 파일이 없어 `newChart == null`이 되면 기존 캐시를 지우지 않는다. 재현 결과 `reload=False, cache notes=2, fingerprint=none`이었다. 다음 전투가 삭제된 채보를 사용할 수 있고 지문과 캐시의 의미도 어긋난다. `Renamed`는 새 경로만 처리하므로 `.bms`를 `.bak`로 바꾸면 확장자 필터에서 반환하고 옛 경로는 무효화되지 않는다.

개선: 명시적인 삭제/이동과 일시적 읽기 실패를 구분한다. 삭제·이동은 옛 경로의 곡 캐시를 무효화하고, 이름 변경은 양쪽 경로를 처리한다. 파일이 실제로 없는 폴백 전투도 검증해야 한다.

### 7. [P2] 0마디의 BPM 변경을 기본 BPM이 덮어쓴다 — 재현 확인

위치: [BmsParser.cs](../../muse%20dash%20test/Bms/BmsParser.cs#L148), 148–154행.

동일 tick의 BPM 이벤트를 `Source` 문자열로 정렬한다. `BPM01`이 `default`보다 앞에 와서 0마디 변경을 먼저 적용한 뒤 기본 BPM을 다시 적용한다.

입력 `#BPM 120`, `#BPM01:240`, `#00008:01`, `#00113:01`에서 첫 노트는 1초여야 하지만 2초로 계산되었다. 기본 BPM을 초기 상태로 분리하거나 같은 시각 이벤트의 우선순위를 명시해야 한다.

### 8. [P2] BPM 0을 받아 무한대 노트 시간을 만든다 — 재현 확인

위치: [BmsParser.Lexer.cs](../../muse%20dash%20test/Bms/BmsParser.Lexer.cs#L17), 17–26행; [BmsParser.cs](../../muse%20dash%20test/Bms/BmsParser.cs#L164), 164행.

`#BPM 0`이 유효한 헤더로 처리된다. 기본 BPM까지 0이면 `60f / currentBpm`이 무한대가 된다. `#BPM 0\n#00113:01`에서 `note.Time=Infinity`를 확인했다. 이 값은 후속 Decimal 변환/주입이 사용할 수 있는 시간 값이 아니다.

개선: 헤더·별칭 BPM을 양수이면서 유한한 값으로 검증하고, 잘못된 입력은 명시적으로 거부하거나 정해진 기본값으로 처리한다. 원본 `musicList.Clear()` 전에 채보 변환·검증을 완료하면 입력 오류가 전투 데이터를 부분적으로 지우는 것도 방지할 수 있다.

### 9. [P2] 미디어 주입 대상과 일시정지·싱크 보정 대상이 다르다

위치: [HwaBattleMediaController.cs](../../muse%20dash%20test/Patches/Battle/UI/HwaBattleMediaController.cs#L112), 112행 및 256–291행; [HwaBattleMediaController.Lifecycle.cs](../../muse%20dash%20test/Patches/Battle/UI/HwaBattleMediaController.Lifecycle.cs#L36), 36행/77행; [HwaSyncManager.cs](../../muse%20dash%20test/Patches/Hwa/HwaSyncManager.cs#L46), 46행.

주입은 기존 AudioSource를 우선 재사용하고 그것도 없을 때만 `HwaBattleBgmSource`를 만든다. 반면 Pause/Resume와 싱크 보정은 그 이름의 오브젝트만 찾는다. 재사용 경로에서는 모드가 실제 바꾼 소스를 제어하지 못한다. 원본 게임이 해당 소스까지 대신 제어하는지는 런타임 확인이 필요하다.

개선: 주입 시 확보한 AudioSource 참조를 모든 미디어 제어에서 공유한다. 비디오는 길이가 음악과 다른 경우 `progressRatio * video.length`로 환산하면 같은 실제 경과 시간이 되지 않으므로 공통 전투 시간을 기준으로 검증한다.

### 10. [P2] 배틀 종료 후 진행 중이던 오디오 로드가 재생을 시작할 수 있다

위치: [HwaBattleMediaController.cs](../../muse%20dash%20test/Patches/Battle/UI/HwaBattleMediaController.cs#L153), 153–192행; [HwaBattleMediaController.Lifecycle.cs](../../muse%20dash%20test/Patches/Battle/UI/HwaBattleMediaController.Lifecycle.cs#L94).

`SendWebRequest()`를 기다린 뒤 전투/요청 세대의 유효성을 확인하지 않고 `targetSource.Play()`를 호출한다. 로딩 중 Exit/Restart/StopMedia가 먼저 실행되면 정지 이후 옛 요청이 완료되어 다시 재생하거나 다음 곡의 소스를 덮을 수 있다. 메뉴 BGM에는 이미 `monitorGeneration` 방식의 방어가 있으나 배틀 로더에는 없다.

개선: 전투별 세대 번호/취소 상태를 만들고 종료 시 무효화한다. 로드 완료 후 대상 생존과 세대를 다시 확인하고 낡은 요청의 자원을 정리한다. 실제 게임의 느린 로드→즉시 나가기 시나리오는 추가 실행 검증 대상이다.

### 11. [P2] 배포 실패에도 성공 메시지와 종료 코드 0을 반환한다 — 재현 확인

위치: [muse dash test.csproj](../../muse%20dash%20test/muse%20dash%20test.csproj#L47), 47–52행.

`Copy`는 `ContinueOnError="WarnAndContinue"`인데 성공 메시지는 무조건 출력된다. 작업 폴더의 일반 파일을 `ModsDir`로 지정해 배포 타깃만 실행했을 때 MSB3021 복사 실패 경고와 `Successfully deployed`가 함께 나오고 종료 코드는 0이었다. 체크리스트에 알려진 사고이지만 코드 자체에는 남아 있다.

개선: 배포 타깃의 실패를 오류로 전달하고 성공한 경우에만 성공 메시지를 출력한다. 검토/CI 빌드에서 배포를 끌 수 있는 명시적 속성도 추가하면 좋다. `build.bat`의 별도 복사 검증만으로 직접 `dotnet build` 경로를 보호할 수는 없다.

### 12. [P2] Discord 기능을 꺼도 직접 갱신과 Prefix 변조가 남는다

위치: [CustomPlaySession.cs](../../muse%20dash%20test/Core/CustomPlaySession.cs#L45), 45–55행; [DiscordManagerDebugPatch.cs](../../muse%20dash%20test/Patches/Diagnostics/DiscordManagerDebugPatch.cs#L25), 25행.

관리자의 `SendPresence()`는 토글을 검사하지만 세션 코드가 그 뒤 게임 `SetUpdateActivity()`를 직접 호출한다. 해당 메서드의 Prefix도 토글 없이 상태를 변경한다. 따라서 `EnableDiscordRPC=false`가 모드의 개입 중단을 보장하지 않는다.

개선: 발신 경로를 하나로 모으고 Prefix에도 같은 게이트를 적용한다. 게임 자체의 Discord 기능을 끄는 것과 모드의 추가 기능을 끄는 것은 구분한다. 외부 Discord 수신은 이번에 확인하지 않았다.

## 추가 개선점과 확인이 필요한 부분

- **기록 저장 내구성**: `CustomRecordStore.cs:172`의 직접 덮어쓰기는 중간 실패 시 이전 최고 기록까지 손상시킬 수 있다. 같은 폴더의 임시 파일→완료 후 교체와 백업을 고려한다. 수동 JSON 파서는 줄 배치에 의존하므로 구조화 파서와 필드 검증을 쓰는 편이 낫다.
- **미디어 해제 설명과 구현 불일치**: `HwaBattleMediaController.Lifecycle.cs:130`은 다음 주입/ResetState에서 클립을 파괴한다고 설명하지만 실제 ResetState는 참조만 비우고 `previousInjected`도 사용하지 않는다. Unity의 자동 해제까지 포함한 실제 메모리 증가는 미측정이다. 과거 사고 때문에 승리 이벤트 안에서 즉시 Destroy하면 안 되므로, 참조 해제와 안전한 지연 정리 시점을 계측해야 한다.
- **IL2CPP 복제 실패 처리**: `CustomTagRegistrySupport.TryCloneMusicInfo`는 하위 객체 복제/재대입 실패에도 진행한다. 이후 공유 `MusicExInfo`를 수정할 가능성이 있으므로 복제와 setter 성공을 확인하고 실패 시 그 가상 곡만 건너뛰는 것이 안전하다. 현재 게임에서 실패하는지는 미확인이다.
- **노트 수 상한**: `DBStageInfoExperimentChart.cs:65`의 short 상한 검사는 노트를 모두 생성한 뒤 로그만 남긴다. 확장될 롱노트 수를 사전에 계산하고 초과 채보를 주입 전에 거부해야 한다.
- **타이밍 정밀도**: `DBStageInfoExperimentChart.Bms.cs:35`와 128행은 노트 시간을 소수 둘째 자리로 먼저 반올림한다. 뒤에서 셋째 자리로 정규화해도 손실된 최대 약 5ms는 복원되지 않는다. 판정 시간과 표시 배치 정밀도를 분리하고 인접한 두 시각의 더블 노트 오인 여부를 검사한다.
- **로그 비용**: `ModLogger`가 문자열을 받은 뒤 레벨을 검사하므로 보간식 내부의 리플렉션/IL2CPP 접근은 로그를 꺼도 실행된다. 특히 `APModPatch.cs:102`의 추가 정확도 조회와 상세 진단 계산은 호출부에서 레벨로 감싸는 편이 낫다. 실제 프레임 비용은 미측정이다.
- **보조 도구**: `SignatureDumper/Program.cs`는 덤프 예외를 출력해도 최종 0을 반환하고 기존 출력 폴더를 먼저 삭제한다. 임시 디렉터리에 성공적으로 덤프한 뒤 교체하고 실패를 종료 코드에 반영하면 자동화가 오류를 감지할 수 있다.
- **오래된 스크립트/문서**: `_refactor_apply.py`는 현재 없는 `Patches/UI/Common/...` 경로를 사용한다. `CODE_REFERENCE.md:119`와 `NOTE_COLOR_TINTING.md:4`는 삭제된 `NoteColorDiagnosticsPatch.cs`를 링크한다. 모바일 가이드의 `file:///h:/...` 링크 8개는 다른 클론에서 이식성이 없다. README의 UID 1998/곡 1999-0 및 `record/{uid}` 설명도 현재 상수·폴더 키 방식에 맞춰야 한다. 실험 기록은 당시 버전과 현재 적용 여부를 구분해 보존하는 편이 좋다.

## 실행 결과

| 검증 | 결과 |
| --- | --- |
| 기존 LogicTests | 80개 통과, 실패 0 |
| 모드 Debug 빌드 | 경고 0, 오류 0 |
| SignatureDumper Debug 빌드 | net472/net8.0 모두 경고 0, 오류 0 |
| 검토용 배포 DLL | `artifacts/review-mods`와 빌드 DLL의 SHA256 일치 |
| BMS·기록 격리 재현 | 위 4–8번의 5가지 이상 동작 확인 |
| 배포 실패 재현 | MSB3021 경고 후 성공 메시지/종료 코드 0 확인 |
| 실제 게임 전투/Harmony 실행 | 미실행 |

검토 빌드 SHA256: `5925F082FB6C26472829AB61288FC544F0EB9C467E7A380A836DB29F9F7BAC18`.

게임 `H:/muse dash hwa/Mods`에는 기존 모드 DLL 1개(469,504바이트)가 있었으며 SHA256은 `22940876B6D218F9FF5963175A9704913EF554BE317AF388520CEE1FC5B0FE4D`였다. 이번 검토 빌드는 게임 폴더에 배포하지 않았다. 기존 DLL과 검토 빌드의 해시 차이만으로 설치본이 잘못됐다고 판단하지 않는다. `Latest.log`는 9월 6일 파일이어서 이번 실행의 증거로 사용하지 않았다.

로컬 재현 프로그램은 Git에서 제외되는 `scratch/review/ReviewProbe.csproj`에 남겼다. 실제 BMS/리소스 매니저/기록 소스를 링크하고 게임·로깅 의존성만 스텁으로 대체했다. 파일 변경은 프로그램의 출력 디렉터리 아래 새 GUID 폴더로 제한했다.

```powershell
dotnet run --project 'muse dash test.LogicTests/muse dash test.LogicTests.csproj' -c Debug
dotnet run --project scratch/review/ReviewProbe.csproj
dotnet build SignatureDumper/SignatureDumper.csproj -c Debug
```

권장 순서: 백업/원본 저장 보호 → AP 토글·기록 지문 → BMS 갱신·BPM → 미디어 생명주기 → 배포 실패 처리 → 문서·진단 정리. 각 수정 후 정상 및 `0-0` 폴백을 실제 배틀까지 확인한다.
