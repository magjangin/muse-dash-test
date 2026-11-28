# 변경 이력

`tools/release.ps1`이 `## [<버전>]` 항목 본문을 GitHub 릴리즈 노트로 씁니다. 버전은 `muse dash test/Core/ModInfo.cs`의 `ModInfo.Version`과 같아야 합니다.
v0.16.0 이전 이력은 [GitHub 릴리즈 목록](https://github.com/magjangin/muse-dash-test/releases)과 `git log --oneline`을 보십시오.

## [0.17.1] - BGA 영상-BGM 싱크 정밀도 대폭 향상·일시정지 복귀 위상 보호

### BGA 비디오 및 오디오 동기화 개선
- **배틀 시작 시점 미세 위상차 제거**: `HwaSyncManager.TryStartVideoAtMusicTime`에서 시작 시간 검사 임계치를 기존 `0.05f`(50ms)에서 `0.001f`(1ms)로 완화했습니다. 배틀 시작 시점에 BGM이 0.01~0.04초 부근을 지나고 있을 때 비디오 시크를 건너뛰어 초반에 최대 40~50ms의 위상차가 형성된 채 출발하던 문제를 원천 차단했습니다.
- **일시정지 복귀(`ResumeMedia`) 위상 보호**: ESC 일시정지 후 배틀 재개 시 `VideoPlayer`와 `AudioSource` 간의 재개 레이턴시 차이로 순간적인 오차 점프가 생기던 문제를 방지하기 위해 `HwaSyncManager.NotifyResume()`을 신설했습니다. 재개 직후 0.5초간 안전 쿨다운을 부여하고 통계를 재설정하여 BGA가 급격히 튀거나 떨리지 않고 부드럽게 동기화 상태로 복귀합니다.
- **동적 드리프트 추종 게인 강화**: 데드밴드를 20ms에서 15ms로 정밀화하고 속도 조절 게인을 `1.0`에서 `1.2`로 향상시켰습니다(오차 50ms 발생 시 즉시 ±6% 재생 속도 조절). BGA 영상 속 오디오 트랙과 BGM 간의 콤 필터링(에코/플랜징) 위상차를 훨씬 빠르고 부드럽게 밀착 수렴시킵니다.

## [0.17.0] - 곡 선택 ALL 태그 목록 0곡 버그 수정·감사 15차 업데이트 내성·병목·도구 전면 보강

### 고친 것
- **곡 선택 화면 진입 직후 ALL 태그 목록 0곡 버그 수정**: 곡 선택 화면에 들어가면 ALL 태그인데도 곡 목록이 0곡으로 비어 있던 문제를 고쳤습니다(태그 패널에서 곡을 눌러야 채워졌음). 원인은 커스텀 태그(`1999`) NRE를 막는 `MusicTagManager_RefreshStageDisplayMusics_Patch`가 게임의 "지금 태그로 갱신"(`-1`)까지 막은 것입니다. 이제 `-1`은 지금 태그가 커스텀(`1999`)일 때만 막습니다(`TagRefreshRules`). 실기 확인(2026-11-27): 진입 직후 803곡 정상 표시, 커스텀 태그 16곡 선택·그 상태로 재진입 시 오류 0건.
- **`EnableAPMod=false` 기록 저장·효과음 연동 누락 수정**: `EnableAPMod`가 꺼져 있을 때 `AddScore`/`IsFullCombo` 훅이 빠져 `VictoryDataCache.ActiveTarget`이 비면서 커스텀 곡 플레이 기록 저장(`CustomRecordStore.SaveResult`)과 올 퍼펙트 효과음 제어가 함께 건너뛰어지던 문제를 고쳤습니다. 이제 `GetAccuracy` 패치가 `EnableAPMod`와 무관하게 `ActiveTarget`을 확보합니다.
- **커스텀 Spine 스킨 JSON 난독화 검사 오류 수정**: `CustomSkinInjector`에서 공백만 있는 `.json` 파일을 읽을 때 `IndexOutOfRangeException`이 나거나, UTF-8 BOM이 붙은 정상 `.json`을 암호화된 파일로 오진해 `[ERROR]` 배너를 띄우던 문제를 고쳤습니다.
- **강제 퍼펙트 설정 스위치 반영 보강**: 배틀 중 `config.txt`의 `강제퍼펙트`가 켜질 때 `ModConfig.EnableForcePerfect` 스위치도 함께 확인하도록 정합성을 맞췄습니다.
- **릴리즈 스크립트(`tools/release.ps1`) PowerShell 5.1 `stderr` 오판 수정**: Windows PowerShell 5.1의 `$ErrorActionPreference = 'Stop'`이 `git push`·`dotnet` 등의 정상 `stderr` 진행 출력을 종료 예외(`NativeCommandError`)로 승격해 태그 푸시 후 실패로 오판하던 문제를 `Invoke-Native`/`Invoke-Captured` 격리로 해결했습니다. 또한 `10` 이상 패치 버전 번호에 대한 `FileVersion` 접두 비교와 `dist/notes-v*.md` 이전 초안 자동 정리를 추가했습니다.

### 업데이트 내성 강화
- **JIT 경계 격리(`NoInlining`) 확대**: `MainMod.EnableMultiTouch`, `TouchInput.ReadInputSystemCore`/`ReadLegacyCore`, `CustomRecordStore.ReadBattleDifficulty`/`ReadSelectedDifficulty`, `CustomSkinInjector.PrimaryMaterialOfCore`, `NoteSkinInjector.TryInjectCore`, `Patch_Inject_BlackGirlBattle.TryInjectCore`, `Patch_SpineActionContract.DumpSupplyCore`, `SelectedSongLocalizationScope.ReadFrameCountCore`를 별도 `NoInlining` 메서드로 분리해 게임 업데이트나 런타임 환경 차이로 멤버가 바뀔 때 상위 폴백·예외 격리가 안전하게 작동하도록 보강했습니다.
- **비활성 기능·Verbose 전용 패치 `Prepare()` 게이트 확대**: `PatchInstaller`에서 `Prepare()` 판정을 사전 JIT 검사보다 먼저 수행하고, `AutoPlayPatch`, `ForcePerfectPatch`, `BossPatch`, `StageBattleComponent_LoadMusicData_Patch`, `JudgmentBar`, `PnlInputMobilePatch`, `DiscordManagerDebugPatch`, `GameMusicScene_Run_Patch`, `SceneFlowPatch`(관찰 전용 3개, `ChangeNote` 제외), `PnlStagePatch`(관찰 전용 7개), `PnlRankHookPatch`(관찰 전용 3개)에 `Prepare()`를 추가해 꺼진 기능이나 기본 로그 수준에서 불필요한 네이티브 트램펄린 자체를 걸지 않습니다.
- **오버로드·파라미터 모호성 차단**: `ModReflection`, `Il2CppWrapperBase`, `InputOverlay`에서 `AmbiguousMatchException`을 방어하고, 주요 `[HarmonyPatch]` 어트리뷰트에 명시적 파라미터 타입 배열을 지정했으며 `PnlVictoryLoggingPatch`/`AudioManager_PlayBgm_Patch`의 동적 타깃에서 Il2CppInterop 합성 `params` 래퍼를 제외했습니다.
- **핫리로드 스레드 안전성 및 파일 삭제 공유**: `HwaResourceManager`의 매니페스트·가상 UID 컬렉션에 락 동기화를 일원화해 백그라운드 BMS 감시 스레드와의 경합을 막고, `AtomicFile`·`ChartFingerprint` 공유 읽기에 `FileShare.Delete`를 추가해 에디터의 원자적 교체 저장과 충돌하지 않게 했습니다.

### 병목 및 메모리 할당 최적화
- **채보 주입 및 배틀 시작 대량 할당 제거**: `DBStageInfoExperimentChart`에서 `DebugExperimentNotes`/Verbose가 꺼져 있을 때도 조립되던 노트당 문자열 보간·IL2CPP 마샬링 수만 건을 게이트 뒤로 옮기고, `SortBmsNotesByShowTick`의 리스트 재구성을 제자리 덮어쓰기로 바꿨으며, `GameMusicScenePreLoadEnemyPatch`에서 배틀 시작마다 최대 6,000회 발생하던 클로저·박싱 할당을 제거했습니다.
- **인게임 프레임·타격 핫패스 경량화**: `GhostNoteAlphaHold`(고스트 보이기 꺼짐 시 조기 반환), `NoteSkinInjector`(스킨 미존재 스켈레톤 포인터 캐시), `OffsetHookPatches`(`Il2CppSystem.Decimal` 캐시), `APModPatch`(포인터 비교로 매 타격 인터롭 비교 제거), `AudioManager_PlayBgm_Patch`(`object[] __args` 박싱 제거), `BmsDtTable`·`ExperimentHitPointInstaller`(`Substring` 제거), `BmsParser`(`#` 비헤더 줄 조기 통과·정규화 무할당 패스트패스), `SongAudioFiles`(음원 없음 결과 캐시), `CustomSkinInjector`(대용량 스킨 JSON 이중 디코딩 제거), `SaveDataManagerPatch`(`TryGetValue` 단일 조회) 등 전역 병목을 해소했습니다.
- **로케일·크로스플랫폼 일관성**: `SignatureDumper`(실수 리터럴 `InvariantCulture` 및 `AssemblyResolve` 해제), `CustomRecordStore.Serialization`, `CustomRecordUiPatchHelper`, `PnlReportCardPatch`, `RankCellHookPatch`, `HwaManifestLoader` 등에 `CultureInfo.InvariantCulture`와 `\`/`/` 공통 경로 분리를 완비했습니다.

### 문서 및 테스트
- `tools/check-code-reference.ps1`이 부분 문자열 대신 정확한 `.cs` 파일명 단위로 대조하도록 강화하고 `CODE_REFERENCE.md` 누락 항목·`LOGGING_AND_TROUBLESHOOTING.md`·`OFFLINE_CUSTOM_SANDBOX_GUIDE.md`·`MOD_SYSTEM_BLUEPRINT.md`·`README.md`를 최신 코드 기준으로 정비했습니다. CI 워크플로에 `fix/*` 브랜치 트리거를 추가하고 순수 로직 단위 테스트를 222개로 확장했습니다.

## [0.16.9] - 감사 14차·보스 전환 맵 2중 조회 해소·고스트 판정 힙 할당 방지·임시 채보 필터 강화

게임 동작을 바꾸는 기능 추가는 없습니다.

### 병목 및 할당 최적화
- `BmsWavParser`: `xxyy` 보스 전환 노트 판정 시 `XxyyTransitionMap.ContainsKey` 호출 후 `ApplyBossTransitionFromXxyy` 내부에서 다시 `XxyyTransitionMap.TryGetValue`를 호출하던 2중 딕셔너리 조회를 제거했습니다. `ApplyBossTransitionFromXxyy`를 직접 호출하고 결과 상태로 UID 해석 여부를 판별하여 핫패스 룩업을 절반으로 줄였습니다.
- `GameMusicSceneInitPatch`: 씬 노트 로드 시 고스트 노트(`xx=17`) 검사 경로에서 `uid.Substring(2, 2) != "17"` 호출로 인해 매 노트마다 발생하던 `string` 힙 할당을 제거했습니다. `ScenePatchHelpers.IsGhostNoteUid`(`uid[2] == '1' && uid[3] == '7'`)를 도입하여 무할당 인덱스 검사로 최적화했습니다.
- `CustomTagRegistrySupport`: 언어 맵의 기본 이름 조회 시 `languages.ContainsKey("English") ? languages["English"] : ...`의 2중 조회를 `languages.TryGetValue("English", out string eng)` 단일 조회로 단축했습니다.

### 채보 파일 필터링 안정성
- `BmsFileNames.IsTempFile`: 에디터나 다운로더/백업 툴이 남길 수 있는 `.tmp` 및 `.temp` 확장자 파일(`chart.bms.tmp`, `chart.bms.temp`, `*.TMP`)을 임시 파일로 명확히 감지하도록 필터를 보강했습니다.

## [0.16.8] - 감사 13차·공유 읽기 안정화·리스트 할당 최적화·로케일 보강

게임 동작을 바꾸는 기능 추가는 없습니다.

### 파일 입출력 안정화 (공유 잠금 위반 방지)
- `AtomicFile.ReadAllBytesShared` 및 `AtomicFile.ReadAllTextShared`(`FileShare.ReadWrite`)를 신설하고 모든 런타임 파일 읽기 경로에 일괄 적용했습니다.
  - 적용 대상: `TextFileDecoder`, `CustomRecordStore.Serialization`, `CoverImageManager`(`cover.png`), `AlbumTagTogglePatch`(`tag_icon.png`), `InputOverlay.Config`(`config.txt`), `CustomSkinInjector`(`char/note *.atlas`, `*.png`, `*.json`).
  - 외부 텍스트 에디터(메모장, VS Code 등), Spine 에디터, 그래픽 툴, 클라우드 동기화 또는 백업 프로그램이 쓰기 핸들을 쥐고 있어도 공유 위반 예외(`IOException`) 없이 안전하게 로드됩니다.

### 병목 및 할당 최적화
- `CustomTagRegistry.RegisterAll`: 가상 앨범 및 곡 등록 시 마샬링된 Il2Cpp 문자열 리스트(`displayMusicList`)를 `info.SetTagUids`에서 재사용하도록 개선했습니다. 매 실행 및 태그 갱신마다 불필요하게 4회씩 발생하던 무거운 네이티브 리스트 할당을 1회 줄였습니다.
- `BmsNoteMatcher`: 차트 채널별 노트 분류 시 `notesByChannel.ContainsKey` 후 인덱서 접근(`notesByChannel[...]`)을 수행하던 2중 딕셔너리 조회를 `TryGetValue` 1회 조회로 단축했습니다.
- `CoverImageManager`: `MusicCellImageDiagnostics`의 `loggedUids` `HashSet`에 `StringComparer.Ordinal`을 지정하여 불필요한 문화권 가중치 비교 연산을 제거했습니다.
- `CustomContentIds.IsVirtualSong`: 가상 곡 식별 비교를 `!string.Equals(uid, AlbumUid, StringComparison.Ordinal)`로 최적화했습니다.
- `AlbumTagTogglePatch`: 태그 UID 판정 시 `string.Equals(..., StringComparison.Ordinal)`을 적용했습니다.

### 로케일·문화권 일관성
- `DiscordPresenceManager`: 플레이 결과 리포트 시 정확도 포맷(`{displayAcc:F2}%`)에 `CultureInfo.InvariantCulture`를 적용하여 쉼표 표기 OS 환경에서도 온점 표기(`0.00%`)로 일관되게 전송되도록 보강했습니다.
- `JudgmentBar`: 판정 오프셋 표시(`{offsetMs:F0} ms`)에 `CultureInfo.InvariantCulture`를 적용했습니다.

### 테스트
- `AtomicFileTests`에 파일이 `FileAccess.ReadWrite, FileShare.ReadWrite`로 열려 있는 동시성 상황에서의 바이트/텍스트 공유 읽기 검증 테스트 2종을 추가했습니다 (총 211개 로직 테스트 통과).

## [0.16.7] - 감사 12차·기록 조회 병목 해소·아카이브 구버전 호환·BMS 파서 내성

게임 동작을 바꾸는 기능 추가는 없습니다.

### 병목 해소
- 곡 선택/준비 화면에서 슬롯마다 불리는 `CustomRecordStore.LoadResult` 안에서 `ChartFingerprint.ForUid`를 최대 4회 중복 호출하던 것을 1회 계산 후 전달하도록 개선했습니다. 동일 파일에 대한 불필요한 `FileInfo` 파일시스템 메타데이터 질의가 제거되었습니다.
- `TextFileDecoder.ReadAllText`가 `MemoryStream` 버퍼 복사를 거치던 것을 파일 크기 기반 직접 배열 할당 읽기로 전환해 대형 채보 읽기 시 버퍼 재할당 및 GC 압박을 줄였습니다.

### 업데이트 내성·안정성
- 채보 지문 보관소(`RecordArchive`) 복원 시 폴더 키 보관본이 없을 경우 레거시 `uid` 키 보관본(`record/archive/{uid}_{diff}.{fingerprint}.json`)도 확인하도록 보강했습니다. 폴더 명칭 도입 이전이나 과도기에 보관된 옛 기록도 안전하게 복원됩니다.
- 오프라인 플래그(`OFFLINE_SANDBOX.txt`) 및 기록 파일(`record/*.json`) 읽기를 공유 읽기 모드(`FileShare.ReadWrite`)로 전환하여 외부 에디터나 클라우드 동기화 툴이 파일을 열고 있을 때 발생할 수 있는 파일 잠금 충돌(`IOException`)을 방지했습니다. `OFFLINE_SANDBOX.txt` 파싱에 `StringComparison.Ordinal`을 적용했습니다.
- `AtomicFile.WriteAllText`의 `finally` 정리 과정에서 임시 파일(`.tmp`) 삭제 실패 시 원본 예외나 성공 상태가 덮어써지지 않도록 안전하게 격리했습니다.
- BMS 파서(`BmsParser.Lexer`)가 `#WAV` 접두를 가진 비-WAV 헤더(예: `#WAVETABLE` 등)를 만났을 때 셀 폭을 비정상적으로 확장하여 마디 파싱을 망가뜨리던 문제를 해결했습니다. 이제 표준 Base36 규격(2 또는 3자리 영숫자)만 WAV 키로 인식합니다.
- `BmsWavParser`에서 `_dt`가 있는 노트 파싱 시 `NormalizePrefabName`을 불필요하게 2회 중복 호출하던 것을 1회로 정리했습니다.

### 로케일·로그 최적화
- `CustomRecordUiPatchHelper`의 점수(`FormatScore`, 천 단위 구분 `N0`) 및 정확도(`FormatAccuracy`, `0.00%`) 포맷에 `CultureInfo.InvariantCulture`를 적용하여 유럽 등 쉼표 소수점 OS 환경에서도 일관된 UI 문자열을 보장합니다.
- 타이틀 및 메뉴 전환 시 반복 호출되던 `DisableUpdateNoticePatch` 버전 체크 훅의 로그 레벨을 `Msg`에서 `Verbose`로 하향하여 콘솔을 깔끔하게 유지합니다.

## [0.16.6] - 감사 11차·채보별 기록 보관·BMS/info.txt 인코딩 판별

### 기록
- 곡 폴더의 BMS를 갈아끼운 뒤 한 판 하면, 덮기 직전의 옛 채보 기록을 `record/archive/{곡 폴더}_{난이도}.{채보 지문}.json`으로 보관합니다. 예전에는 처음 덮을 때의 원본 하나만 `.bak`으로 남아, A → B → C로 바꾸면 B의 최고 기록이 사라졌습니다.
- 채보를 예전 것으로 되돌리면 그 채보의 보관 기록을 찾아 결과창·기록 카드·곡 선택 화면에 다시 보여주고, 다음 판부터 그 기록에 이어서 최고 기록·플레이 횟수를 쌓습니다. 예전에는 되돌려도 "다른 채보의 기록이라 표시하지 않습니다"로 빈 기록부터 시작했습니다.

### 인코딩
- BMS 채보와 곡 폴더의 `info.txt`를 UTF-8로만 읽던 것을, BOM(UTF-8/UTF-16) → UTF-8 → CP949 → Shift-JIS 순으로 가려 읽습니다(`TextFileDecoder`). 메모장에서 'ANSI'로 저장한 `info.txt`의 한글 키(`난이도N bms`, `크리스마스 …`, `dt를 권장 표로 고치기`)가 깨져 조용히 무시되거나, CP949·Shift-JIS BMS의 `#TITLE`·`#WAV` 파일 이름이 깨질 수 있었습니다. 올바른 UTF-8 파일은 예전과 똑같이 읽힙니다. UTF-8이 아닌 `info.txt`를 읽으면 로그에 한 줄 남깁니다.

## [0.16.5] - 감사 10차·MelonLoader 버전 경고·문화권 무관 문자열 비교

게임 동작을 바꾸는 기능 추가는 없습니다.

### 업데이트 내성
- 시작할 때 MelonLoader 버전을 검증 버전(0.7.3)과 비교해 다르면 `[GameVersion]` 경고 한 줄을 남깁니다. 로더가 바뀌면 Il2Cpp 래퍼 어셈블리가 다시 만들어지는데, 게임 버전·`GameAssembly.dll` 크기 검사로는 이 경우가 드러나지 않았습니다. 버전은 `MelonLoader.dll`의 어셈블리 버전으로 읽어 로더 내부 API에 기대지 않습니다. 기능은 끄지 않습니다.
- `config.txt`의 키·참/거짓 값 해석과 메뉴 BGM 소스 이름 매칭이 실행 PC의 문화권(`ToLower()`)을 따르던 것을 문화권 무관(`ToLowerInvariant()`)으로 바꿨습니다. 터키어 등 대소문자 규칙이 다른 로케일에서 `I`가 다른 글자로 바뀌어 매칭이 어긋날 수 있었습니다. `config.txt`에 쓰는 `true`/`false`도 같은 방식으로 씁니다.

### 병목
- 노트 uid 접두 판정(`uid.StartsWith("0002")` 등, 채보 주입 때 노트마다 호출)과 그 밖의 리터럴 접두 비교 26곳을 서수 비교(`StringComparison.Ordinal`)로 바꿨습니다. 기본 `StartsWith(string)`은 문화권 비교라 ICU를 거쳐 느리고, 로케일에 따라 결과가 달라질 수 있습니다.

## [0.16.4] - 감사 9차·BMS 감시 오류 복구·키음 계측 비용·코드 레퍼런스 전수화

게임 동작을 바꾸는 기능 추가는 없습니다.

### 업데이트 내성·안정성
- 채보 실시간 감시(`HwaResourceManager.Watcher`)가 감시자 오류를 받지 않아, 에디터가 여러 파일을 한꺼번에 저장하거나 곡 폴더를 복사해 감시 버퍼가 넘치면 그 사이 채보 변경이 로그 없이 반영되지 않던 문제를 고쳤습니다. 이제 오류를 경고로 남기고 모든 곡을 다시 읽도록 예약하며, 감시가 멈췄으면 다시 겁니다. 감시 버퍼도 8KB → 64KB로 늘렸습니다.
- 재로드 타이머 콜백이 앞 콜백이 끝나기 전에 다른 스레드에서 겹쳐 돌 수 있어, 같은 곡을 두 번 동시에 읽고 늦게 끝난 쪽(옛 내용일 수 있음)이 캐시를 덮을 수 있었습니다. 도는 중에 온 예약은 콜백이 끝난 뒤 한 번 더 돌게 미룹니다.

### 병목
- 상시 켜진 키음 계측(`KeysoundProbe`)이 키음마다 `clip.name`(IL2CPP 문자열 복사)을 읽던 것을 클립별 한 번으로 줄였습니다(배틀마다 캐시 비움, `AllPerfectSound`와 같은 방식).
- 게임 BGM 소스의 `clip` 대입 훅이 로그에만 쓰는 클립 이름을 Verbose가 꺼져 있어도 읽던 것을 Verbose일 때만 읽습니다.

### 문서
- `docs/architecture/CODE_REFERENCE.md`에 '12. 전체 파일 지도'를 추가했습니다. 모드 소스 161개 중 73개가 문서에 한 번도 나오지 않았습니다.
- `tools/check-code-reference.ps1`을 추가해 CI(`Logic Tests`)와 `tools/release.ps1`이 모든 소스 파일이 레퍼런스 문서에 있는지 확인합니다. 새 파일을 문서에 안 적으면 실패합니다.
- `ModConfig` 설명의 토글 개수(13 → 14)를 바로잡았습니다.

## [0.16.3] - 감사 8차·패치 Prepare 일관화·핫패스 비용·로케일 수정

게임 동작을 바꾸는 기능 추가는 없습니다. (`v0.16.2`는 별도 태그·릴리즈를 올리지 않았으므로, 아래 `0.16.2` 항목도 이번 `v0.16.3` 배포에 함께 포함됩니다.)

### 업데이트 내성
- `PatchInstaller`가 패치 클래스의 `Prepare()` / `[HarmonyPrepare]`를 일반 설치(`ApplyAll`)와 부분 복구(`ApplySurvivingMethodPatches`) 양쪽에서 일관되게 검사하도록 보강했습니다. `Prepare()`가 `false`를 돌려주거나 예외를 던진 패치는 대상 검사·적용·집계에서 안전하게 제외됩니다.

### 병목
- 매 노트 타격마다 불리는 `AbstractFeverManager.AddFever` 훅에서 `Verbose` 로그가 꺼져 있으면 `DateTime.UtcNow` 시계 조회를 건너뛰고 Postfix를 즉시 반환하도록 줄였습니다.
- 진단 전용 훅 `PnlMenu_OnEnable_DebugPatch`에 `Prepare()`를 붙여 `Verbose`가 꺼져 있을 때는 훅 자체를 걸지 않습니다.
- `CollabEndTimeDumpPatch`가 `Verbose`가 꺼져 있을 때 `AlbumJsonIndexDefine`의 정적 프로퍼티 60여 개를 리플렉션으로 읽던 시작 시 비용을 제거했습니다.
- `MouseTouchBridgePatch`(매 프레임 불리는 입력·커서 훅 8개)에 `Prepare() => ModConfig.EnableMobileTouch`를 붙여 `EnableMobileTouch=false`일 때 훅 자체를 걸지 않도록 했습니다.
- `SpineActionController.PlayByKey` 핫패스(`GhostNoteAlphaHold`, `NoteAnimationSyncPatch`)와 `KeysoundProbe.OnUnityPlayOneShot`의 null 비교를 관리 비교(`is null` / `is not null`)로 바꿨습니다.

### 버그 수정
- `InputOverlay.WriteConfigFile`이 실수 설정값(`판정바틱유지시간` 등)을 시스템 로케일로 쓰던 문제를 `CultureInfo.InvariantCulture`로 고쳤습니다(쉼표 소수점 OS에서 `1,2`로 저장돼 `TryParseFloat`가 못 읽던 왕복 불일치 해소).
- `GhostNoteAlphaHold`의 복사·붙여넣기 예외 로그 라벨을 실제 메서드 이름으로 바로잡았습니다.

### 문서
- `README.md`, `docs/architecture/AUDIT_2026-11-14.md`, `AGENTS.md`를 최신 기능·릴리즈 자동화(`tools/release.ps1`·`CHANGELOG.md`)·감사 8차 결과에 맞게 갱신했습니다.

## [0.16.2] - 감사 7차·설정 핫리로드 누락 수정·노트 경로 비용

게임 동작을 바꾸는 기능 추가는 없습니다.

### 버그 수정
- `config.txt`를 메모장 등이 저장하는 도중(파일이 비었거나 잠긴 순간)에 읽으면 그 변경이 다음 저장 때까지 반영되지 않던 문제를 고쳤습니다. 이제 읽기에 성공했을 때만 수정 시각을 기록하고, 같은 변경을 1초 간격으로 최대 5번 다시 읽습니다.

### 병목
- 스킨 이름 진단 훅(`SpineActionController.Awake`, 노트·캐릭터가 생길 때마다 호출)을 `EnableSpineSkin`과 Verbose 로그가 둘 다 켜진 실행에서만 겁니다. 예전에는 본문이 바로 돌아가도 노트마다 훅 호출 비용이 들었습니다.
- 모든 효과음마다 불리는 `AudioSource.PlayOneShot` 훅에서 Unity 비교(IL2CPP 호출) 대신 관리 비교를 씁니다.

### 내부
- 기기 감지·기능 토글 로드를 패치 설치보다 먼저 합니다. 패치의 `Prepare()`가 설정을 볼 수 있고, UMPC에서는 패치 설치 로그부터 억제됩니다.
- README에 v0.13~v0.16 기능 요약을 추가했습니다.

## [0.16.1] - 감사 6차·업데이트 내성·릴리즈 자동화

게임 동작을 바꾸는 기능 추가는 없습니다. 정상 경로의 결과는 그대로이고, 업데이트·예외 상황의 복구와 매 프레임 비용을 줄였습니다.

### 업데이트 내성
- 패치 클래스 하나가 적용 중 실패해도(게임 업데이트로 원본 파라미터 이름이 바뀐 경우 등) 이미 걸린 일부를 걷어낸 뒤 메서드별로 다시 걸어, 실패한 메서드만 빠지게 했습니다(`PatchInstaller`).
- `#WAV` 경로의 폴더 접두를 `\`·`/` 둘 다 직접 잘라, Windows에서 만든 채보를 Linux 계열 런타임(Android 포함)에서도 UID로 읽습니다.
- 키음 계측의 타격 연출 깊이가 원본 예외로 고착되면 다음 프레임에 스스로 풀립니다.

### 병목
- 공식 곡 배틀·메뉴에서 BGA 싱크가 프레임마다 IL2CPP 조회를 하지 않습니다.
- `캐릭터 커스텀 스킨=0`이면 노트가 생길 때마다 오브젝트 이름을 읽지 않습니다.
- 배틀 밖(메뉴·로딩)에서는 키음 계측의 F8 키 확인(`Input.GetKeyDown`)을 하지 않습니다.

### 견고성·로그
- 음원 없는 곡에서 채보 길이를 처음 0으로 읽으면 진행바가 멈춰 있을 수 있던 것을 1초 뒤 다시 구하도록 고쳤습니다.
- 30초마다 나오던 `[Sync.BGA]` 오차 요약을 Verbose로 내렸습니다.

### 개발
- CI(Logic Tests)를 Windows 러너로 옮겨 처음으로 초록불이 되었습니다(11-18 도입 뒤 줄곧 실패).
- `tools/release.ps1`: 버전·CHANGELOG·git·CI 확인 → Release 빌드 → 로직 테스트 → 태그·GitHub 릴리즈를 한 번에 합니다. `-p:SkipDeploy=true`로 Mods 폴더 복사를 건너뛸 수 있습니다.
