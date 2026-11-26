# 변경 이력

`tools/release.ps1`이 `## [<버전>]` 항목 본문을 GitHub 릴리즈 노트로 씁니다. 버전은 `muse dash test/Core/ModInfo.cs`의 `ModInfo.Version`과 같아야 합니다.
v0.16.0 이전 이력은 [GitHub 릴리즈 목록](https://github.com/magjangin/muse-dash-test/releases)과 `git log --oneline`을 보십시오.

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
