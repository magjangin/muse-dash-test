# 성능 검토 — 2026-09-20

검토 기준: `495597d321ddb3999a78990acf1f3d9f95612dfc` (`origin/main`과 동일).

## 범위와 결론

Git 추적 파일 197개(C# 139개)를 대상으로 파일 목록, 실행 진입점, 반복문, 파일 I/O,
Unity 객체 검색, 리플렉션, 캐시와 진단 호출을 전수 검색하고 주요 호출 경로를 상세 검토했다.
문서·도구·테스트는 런타임 코드와 구분했다. 전체 파일 목록은
[검토 범위](PERFORMANCE_REVIEW_2026-09-20_FILES.md)에 기록했다.
무시된 빌드 산출물, 로컬 디컴파일 자료, 게임 엔진 전체는 전수 검토 대상이 아니다.

남아 있는 불필요한 작업은 확인된다. 특히 **출력하지 않는 진단을 계산하는 코드**,
**캐시 조회에 숨은 채보 순회**, **키 설정 실패 시 매 프레임 재탐색**을 먼저 다룰 가치가 있다.
아래 순서는 수정/계측 우선순위이며, 측정된 CPU 점유율 순위가 아니다.
Unity CPU/GPU 프로파일러와 프레임 시간은 이번에 측정하지 않았으므로 실제 끊김 원인을 확정하지 않는다.
이번 변경은 검토 문서뿐이며 런타임 최적화는 적용하지 않았다.

## 우선 검토할 항목

### 1. 고스트 노트 캐시 적중 후에도 임시 진단이 타임라인을 순회한다

- 위치: `Patches/Battle/Mechanics/GhostNoteAlphaHold.cs:190,268-345`.
- `PlayByKey("in")`에서 상태 캐시가 적중해도 `ProbeCachedState`를 호출한다.
  이 함수는 `FindAnimation → ReadMinAlpha → ScanAlpha`로 타임라인과 알파 키를 읽는다.
  `probeLogged.Add`의 중복 로그 방지는 **순회 이후**여서 이미 출력한 애니메이션도 다시 검사한다.
- 세션 전체 `ProbeBudget=300` 한도는 있다. 무한 반복이라고 볼 수 없지만,
  고스트가 등장하는 플레이 구간에 최대 300회 진단 비용이 집중될 수 있다.
  로그 레벨을 Error로 낮춰도 검사 자체는 실행된다.
- 개선 방향: 진단을 명시적인 기본 off 설정으로 분리하고, 일치 여부를 계속 확인할 필요가 없다면
  포인터/애니메이션별 검사 여부를 순회 전에 확인한다. 실제 알파 고정·복원 동작은 유지한다.
- 검증: 고스트 밀집 구간의 검사 횟수, CPU 시간, 할당량을 비교하고 곡 전환 후 알파 복원도 확인한다.

### 2. BMS 캐시 조회가 O(1)이 아니라 O(노트 수 + 메타데이터 수)다

- 위치: `Patches/Hwa/HwaResourceManager.Bms.cs:13-30`,
  `Patches/Diagnostics/HwaChartDiagnostics.cs:9-57`.
- 딕셔너리 조회 뒤 항상 `DescribeBmsChart`를 생성한다. 여기서 WAV 메타데이터를 세고,
  `BuildSwapEvents(chart).Count`를 위해 **모든 노트 순회와 이벤트 목록 생성**을 반복한다.
  호출자가 `out _`로 설명을 버려도 같은 작업이 실행된다.
- 호출 경로: `CustomRecordStore.LoadResult → BelongsToCurrentChart → ChartFingerprint.ForUid`
  및 차트 주입, HitPoint 후보 탐색, 보스 초기화. 지문 해시 캐시가 적중해도 앞선 설명 생성은 남는다.
- 개선 방향: 설명 없는 조회 API와 진단 API를 분리하거나, 불변 차트별 요약을 로드 시 캐시한다.
  BMS 재로드 시에는 새 차트의 요약으로 교체해야 한다.
- 검증: 노트 수를 늘리며 반복 캐시 조회의 시간/할당량을 비교한다. 보스 out/in이 많은 차트도 포함한다.

### 3. 음악 정보 진단은 Verbose를 꺼도 리플렉션과 씬 검색을 수행한다

- 위치: `Patches/UI/Music/PnlMusicDiagnostics.cs:38-88`,
  `PnlMusicDiagnostics.Extraction.cs:12-48`, `PnlMusicDiagnostics.AudioClip.cs:69-124`.
- 제목 적용 이후 `ExtractMusicInfo`와 `LogCompact`를 무조건 실행한다.
  로그 출력만 마지막 `ModLogger.Verbose`에서 막힌다.
- `FirstNonEmpty(FindAudioClipName(...), FindSceneMusicAudioClipName(...), ...)`는 지연 평가가 아니다.
  첫 후보를 찾았어도 나머지 인자가 평가되므로 `GameObject.Find("BGM")`가 실행되고,
  BGM을 못 찾으면 전체 AudioSource 검색까지 진행한다.
- `PnlPreparationPatch`의 OnEnable/RefreshUi와 0.25초·1초 지연 적용이 같은 경로를 반복한다.
- 개선 방향: 필요한 제목 적용을 보존하면서 관찰 전용 추출에 Verbose 게이트를 둔다.
  폴백 탐색은 이전 후보가 실패했을 때만 실행하고, 동일 패널의 중복 지연 작업을 합친다.
- 검증: 준비 화면 진입/난이도 변경을 반복하며 진단 off일 때 씬 검색 횟수가 0인지 확인한다.

### 4. 키 설정을 찾지 못하면 2초 재시도 제한을 우회한다

- 위치: `Patches/UI/Custom/InputOverlay.Render.cs:87-96`, `InputOverlay.cs:78-86`.
- `!keysLoaded`일 때 `checkTimer >= CheckInterval || airKeys.Count == 0`를 검사한다.
  최초 검색이 실패하면 공중 키 목록은 계속 비어 있으므로 매 Repaint마다
  `Resources.FindObjectsOfTypeAll<StandloneCtrlConfig>()`를 다시 호출한다.
- 발생 조건: 키 오버레이가 켜져 있고, 배틀에 진입했으며, 키 설정 객체가 없거나 읽기에 실패한 경우.
  기본 `showOverlay=false` 상태에는 해당하지 않는다.
- 개선 방향: 최초 시도 여부를 별도 관리하고 실패 후에는 시간 조건만으로 재시도한다.
- 검증: 설정 객체가 없는 상태에서 10초 동안 검색 횟수를 집계한다. 정상 상태의 키 표시도 확인한다.

### 5. 같은 곡의 메뉴 BGM 요청을 합치지 않는다

- 위치: `Patches/Hwa/HwaMenuBgmController.cs:22-36,89-189`,
  `Patches/UI/Stage/PnlStagePatch.cs:289`, `PnlPreparationPatch.cs:21`.
- RefreshDiffUI와 준비 화면 진입에서 동일 UID를 요청해도 세대를 증가시키고 새 코루틴을 시작한다.
  `currentLoadingUid`는 저장하지만 동일 곡 중복 요청을 거르는 조건에는 사용하지 않는다.
- 세대 검사는 오래된 결과 적용을 막지만, 파일 탐색은 첫 대기 이전에 실행된다.
  이미 시작된 요청은 `SendWebRequest`가 끝난 뒤 세대를 검사하므로 불필요한 로드가 남을 수 있다.
  완료된 동일 곡을 재요청하면 클립을 다시 로드하고 처음부터 재생한다.
- 개선 방향: 로딩 중/재생 중인 곡과 소스를 확인해 동일 요청을 합친다.
  곡 파일 변경과 A→B→A의 낡은 결과 차단을 유지해야 한다.
- 검증: 동일 곡 난이도 변경, 준비 화면 왕복, 빠른 A→B→A에서 요청 수와 재생 연속성을 비교한다.

### 6. 기록 UI 갱신마다 JSON을 동기적으로 다시 읽고 파싱한다

- 위치: `Core/CustomRecordStore.cs:272-289`, `Patches/UI/Stage/CustomRecordUiPatchHelper.cs:35,77,132`.
- 매번 파일 탐색 → `ReadAllText` → 문자열 분할 파싱 → 지문 확인 → 성공 로그를 실행한다.
  준비 화면 즉시 적용과 지연 적용마다 반복되며, 지문 확인은 항목 2의 전체 채보 순회도 유발한다.
- 개선 방향: 곡 폴더 키·난이도별 기록을 캐시하고 저장/외부 변경/채보 교체 시 무효화한다.
  UID는 폴더 순번이므로 UID만으로 캐시하면 안 된다. 채보 지문 검증도 제거하면 안 된다.
- 검증: 같은 기록 화면 100회 갱신 시 파일 읽기 횟수, 할당량, UI 시간을 비교한다.
  최고 기록 저장, BMS 교체, 폴더 순번 변경 후 잘못된 기록이 붙지 않는지도 확인한다.

### 7. 커버 이미지 캐시는 방문한 곡 수만큼 계속 커진다

- 위치: `Patches/UI/Music/MusicButtonCellPatch.cs:160-211`.
- 첫 표시 때 `ReadAllBytes + LoadImage`로 원본 크기의 PNG를 동기 디코딩한다.
  Sprite/Texture를 정적 딕셔너리에 보관하고 `DontUnloadUnusedAsset`을 설정하지만 퇴출 경로가 없다.
- 큰 커버가 많으면 최초 스크롤 시 끊김과 네이티브/GPU 메모리 증가 후보다.
  예를 들어 RGBA32 2048×2048 한 장의 기본 픽셀 데이터만 16 MiB다.
  실제 사용량은 포맷·밉맵·CPU 복사 여부에 따라 다르므로 이를 실측 메모리로 해석하면 안 된다.
- 개선 방향: 표시 크기에 맞는 썸네일, 읽기 불필요한 텍스처의 CPU 사본 해제,
  표시 중인 Sprite를 보호하는 제한된 캐시와 명시적 Destroy를 검토한다.
- 검증: 고해상도 커버가 많은 곡 목록을 왕복하며 Texture/Sprite 수, 네이티브 메모리, 프레임 시간을 측정한다.

## 추가로 확인할 비용

| 항목 | 코드 근거와 판단 |
| --- | --- |
| AP 효과음의 결과 화면 게이트 | `AllPerfectSound.cs:34-36`은 ActiveTarget 존재만 검사한다. 그런데 `APModPatch.cs:28`은 플레이 중 AddScore에서 이를 채우므로 첫 점수 이후 전역 PlayOneShot 훅에서 clip.name을 읽을 수 있다. 이름을 두 번 읽는 부분도 있다. 실제 결과 상태와 연결된 게이트/클립별 판정 캐시를 검토하되 FC 효과음 발생 시점을 먼저 확인한다. |
| 정확도 관찰 코드 | `APModPatch.cs:102-129`는 GetAccuracy 호출마다 GetTrueAccuracy/GetTrueAccuracyNew와 여러 필드를 읽고 Info 로그를 만든다. 두 getter의 패치와 자체 계산까지 포함하면 커스텀 곡에서 계산이 중복된다. 현재 로그는 9회뿐이라 지속적인 프레임 병목으로 분류하지 않는다. |
| 판정바 off의 잔여 비용 | `JudgmentBar.cs:289`에서 Decimal→문자열→float 변환 후 RegisterHit 내부에서 off를 판단한다. 누적 기록 정리는 이미 있으나 변환 비용은 남는다. 훅 앞단에도 기능 상태 검사를 두는 것을 검토한다. |
| 매 프레임 인스턴스 델리게이트 | `MainMod.cs:181`의 HandleExperimentStageUpdate는 static이 아니다. C# 11의 정적 메서드 그룹 캐시 설명이 이 호출에는 적용되지 않는다. Action 필드 캐시는 작은 개선이며 큰 병목보다 우선하지 않는다. |
| 시작 시 모든 BMS 선로드 | HwaResourceManager가 모든 곡을 동기 파싱한다. 곡 수·채보 크기에 비례하는 시작 지연이다. 실제 로그에 파싱 시간은 있지만 플레이 프레임 지연을 뜻하지 않는다. 지연 로드는 호출자가 로드 완료를 전제하는지 먼저 검토해야 한다. |
| 숙주 곡 이름 검색 | `CustomTagRegistrySupport.BuildAndInjectVirtualSongs → TryFindMusicInfoByQuery`는 직접 UID 조회 실패 시 전체 곡을 다시 정규화·검색한다. 커스텀 곡 수 C와 전체 곡 수 M에 대해 대략 O(C×M). 등록 시의 비용이므로 프레임 루프와 구분한다. |
| 차트 주입의 복제 비용 | AddExperimentNotes/CloneMusicData/MoveNote/ApplyNoteSpec에서 noteData/configData가 반복 복제된다. 롱노트는 0.1초 간격으로 전개되어 비용이 늘어난다. 순수 파서보다 IL2CPP 경계를 넘는 복제·필드 읽기/쓰기를 먼저 계측한다. 값 타입 write-back 계약은 반드시 보존한다. |
| 배틀 미디어 수명 | HwaBattleMediaController의 ResetState는 클립 참조를 지우고 StopMedia는 재생만 멈춘다. 재시작을 반복하며 AudioClip/Material 네이티브 수명을 점검할 가치가 있다. Unity의 실제 회수 시점을 측정하지 않았으므로 누수로 확정하지 않는다. |

## 이미 적용된 개선과 낮은 우선순위

- BmsParser의 시간 계산은 정렬 후 BPM/노트를 단일 순회한다. 기존 O(N×BPM) 문제가 남아 있다고 판단하지 않는다.
- WAV 해석 결과는 RawValue별 캐시이며 특수 노트 매칭에 채널별 재정렬도 없다.
- showTick 정렬 키는 비교 전에 읽어 두므로 비교자마다 IL2CPP Decimal 문자열 변환을 반복하지 않는다.
- TouchInput은 프레임 캐시를 쓰고, 체력바 검색은 성공한 객체를 재사용한다.
- OnGUI는 Repaint만 처리하며 판정 기록 만료 정리도 있다.
- config.txt 감시는 1초 간격이고 배틀 중에는 건너뛴다. 매 프레임 파일을 읽는다고 지적할 수 없다.
- ExperimentHitPointInstaller에는 후보/프리팹 캐시, 0.5초 간격, 최대 40회 제한이 있다.
- GameMusicScene.Run 관찰 덤프는 Verbose 게이트가 있고 SpineActionContract 덤프는 기본 off다.
- ModReflection의 멤버 캐시와 실패 로그 중복 억제, Discord Presence 중복 전송 억제가 이미 있다.
- SignatureDumper와 scripts의 전체 어셈블리 탐색은 개발 도구 실행 시의 비용이다.
  게임 런타임 병목으로 합산하지 않는다. 테스트와 문서도 마찬가지다.

## 확인한 실행 증거와 한계

기존 게임 로그 `H:/muse dash hwa/MelonLoader/Latest.log`를 읽기만 했다.
관찰한 스냅샷은 173,034 bytes, 수정 시각 2026-09-20 19:56:17 KST다.

- GhostNote.Probe 로그 12행: 임시 진단 실행은 확인됨. 중복 로그가 억제되므로 실제 검사 횟수는 더 많을 수 있다.
- APMod.Debug.Accuracy 9행: 매 프레임 실행을 입증하지 않는다.
- 기록 로드 성공 0행: 이 세션은 항목 6의 실행 빈도를 검증하지 못한다.
- 메뉴 BGM 트리거 2행, 주입 완료 1행: 동일 UID 중복 로드는 이 로그만으로 재현되지 않았다.
- BMS 파싱 완료 4행: 그중 386노트/메타데이터 633개에 51ms,
  483노트/메타데이터 633개에 9ms. 초기화·캐시·JIT 등 조건이 통제되지 않았으며
  진단 요약 생성은 파싱 타이머 정지 이후이므로 이 수치에 포함되지 않는다.
- 이 로그의 로드 DLL이 검토 커밋과 완전히 같은지는 검증하지 않았다. 코드 근거와 보조 관찰을 구분한다.

검증 명령 및 결과:

```powershell
dotnet run --project 'muse dash test.LogicTests/muse dash test.LogicTests.csproj' -c Release
# 81개 통과, 실패 0개

dotnet build 'muse dash test/muse dash test.csproj' -c Release `
  '-p:ModsDir=H:\source\repos\muse dash test\muse dash test\bin\audit-mods' `
  -p:UseSharedCompilation=false
# 경고 0개, 오류 0개
```

산출 DLL과 저장소 내부 audit-mods 사본의 SHA-256은 모두
`91EE9D16AE493CBD7FAE254C42837EECBF22044DEAFF42F615CD0B35512E04A4`로 일치했다.
게임의 Mods 폴더로 배포하지 않았다. 테스트는 순수 로직 검증이며 Unity 프레임 성능 검증이 아니다.

CHECKLIST.md를 확인했다. 이번에는 문서만 변경하여 새 게임 실행이나 정상/폴백 배틀 진입은 수행하지 않았다.
후속 최적화에서는 동일 곡·동일 설정의 프레임 시간/할당량을 먼저 기록하고,
정상 BMS 경로와 BMS/앨범 없음 → `0-0` 폴백 → 실제 배틀 진입을 모두 검증해야 한다.
