# 로그와 문제 해결 가이드

이 문서는 모드가 출력하는 로그를 어떻게 읽고, 문제가 생겼을 때 어디부터 확인할지 정리한 문서입니다.

## 먼저 보는 로그

실험 중 가장 먼저 확인할 로그는 아래입니다.

```text
[PatchInstaller] 패치 클래스 N개를 적용했습니다.
[PatchHealth] 패치 대상 N개 전부 정상 해석되었습니다.
모드가 로드되었습니다.
씬이 로드되었습니다: ...
DBStageInfo.SetRuntimeMusicData 호출됨: activeUid=...
[ExperimentChart] 적용 시작: uid=..., reason=...
[ExperimentChart.Bms] BMS 변환 완료: notes=..., specs=..., matchedPairs=...
실험 차트 적용 완료: N개 노트 ...
Il2Cpp.Boss.InitBossObject: ... 적용 -> name=..., scene=...   (보스가 있는 차트일 때)
```

이 로그들이 모두 보이면 모드는 정상적으로 로드되고, 차트/보스 후킹도 대부분 작동 중이라고 볼 수 있습니다. 위 줄은 모두 기본 로그 수준(Info)에서 나옵니다. **`실험 노트 추가:`와 `PnlPreparation.GameStart: 곡 이름=…` 같은 줄은 `LogLevel = "Verbose"`일 때만** 나오므로(아래 "로그가 너무 많을 때" 참고), 안 보인다고 실패로 단정하지 마세요. UMPC로 감지된 기기에서는 `LogLevel = "Auto"`가 `Error`라 위 줄이 거의 안 보입니다.

## 로그별 의미

| 로그 | 수준 | 의미 |
| --- | --- | --- |
| `[PatchInstaller] … 패치를 걸지 않았습니다` | Warning | 게임 업데이트로 그 패치 본문이 현재 게임에서 컴파일되지 않습니다. 뒤의 예외 메시지가 사라진 멤버 이름입니다. |
| `모드가 로드되었습니다.` | Info | MelonLoader가 DLL을 정상 로드했습니다. |
| `씬이 로드되었습니다` | Info | Unity 씬 전환이 감지됐습니다. |
| `DBStageInfo.SetRuntimeMusicData 호출됨` | Info | 게임이 런타임 차트 데이터를 만들었습니다(공식곡도 찍힙니다). |
| `[ExperimentChart] 적용 시작` / `적용 건너뜀` | Info | 커스텀 차트를 적용하는 판인지, 순정 판인지 결정된 결과와 사유입니다. |
| `실험 노트 추가` | **Verbose** | 노트 하나가 실제 `MusicData`로 들어갔습니다(노트마다 한 줄이라 Verbose에만 남깁니다). |
| `실험 차트 적용 완료` | Info | 원본 리스트를 실험 리스트로 재구성했습니다. |
| `PnlPreparation.OnEnable: 곡 이름=…` 등 (`LogCompact`) | **Verbose** | 준비 화면에서 읽은 곡 정보입니다. 예전 문서의 `PnlStage.ChangeFinalMusic` 로그는 지금 없습니다. |
| `Boss.InitBossObject 호출` | Info | 실제 보스 오브젝트 초기화가 시작됐습니다. |
| `Boss.InitBossObject: … 동적 보스 매핑 적용` | Info | BMS의 첫 `in` 노트로 보스 이름/씬이 정해졌습니다. |
| `Boss.InitBossObject: Static 폴백 변경 적용` | Info | BMS에 `in` 노트가 없어 `BossRewriteRules`가 매칭됐습니다. |
| `Boss.InitBossObject: 변경 건너뜀` | Info | 커스텀 차트를 적용하지 않는 판(공식곡 등)이라 손대지 않았습니다. |

## 노트가 안 보일 때

### 1. `실험 노트 추가`가 찍히는지 확인

먼저 `LogLevel = "Verbose"`로 바꿔야 이 줄이 나옵니다. 그래도 찍히지 않으면 `DBStageInfo.SetRuntimeMusicData`가 아직 호출되지 않았거나, 그 곡에 커스텀 차트가 적용되지 않은 판(`[ExperimentChart] 적용 건너뜀`)이거나, `ExperimentNotes` 배열 문법에 문제가 있을 수 있습니다. (`ExperimentNotes`는 그 곡에 BMS가 없을 때만 쓰입니다. BMS가 있는 곡은 `[ExperimentChart.Bms] BMS 변환 완료` 줄을 보세요.)

봐야 할 위치:

```text
muse dash test/Patches/Database/Stage/DBStageInfoPatch.cs
```

확인할 값:

- `ExperimentNotes` 배열에 원하는 줄이 들어 있는지
- 줄 끝 쉼표가 빠지지 않았는지
- `StartTick`이 플레이 구간 안에 있는지
- `SourceNoteIndex`가 원본 리스트 범위 안인지

### 2. 로그의 `uid`, `type`, `pathway`, `prefab` 확인

예:

```text
실험 노트 추가: 공중 일반 #1/1, objId=1, tick=15, dt=1.47, showTick=13.53, speed=5, uid=051004, type=1, pathway=1, prefab=051004_air_nor_1
```

확인할 점:

| 값 | 체크 |
| --- | --- |
| `uid` | 원하는 UID인지 |
| `type` | 노트 종류와 맞는지 |
| `pathway` | 지상/공중이 맞는지 |
| `prefab` | 실제 리소스명과 맞는지 |
| `dt` | 너무 크거나 이상하게 0이 아닌지 |
| `showTick` | `tick - dt`가 의도한 값인지 |

### 3. UID만 바꿨는지 확인

UID만 바꾸면 외형이나 로직이 원본 노트에 끌려갈 수 있습니다. 일반적으로 아래 값은 같이 맞추는 편이 안전합니다.

- `Uid`
- `NoteType`
- `Pathway`
- 필요하면 `PrefabName`
- 하트/음표면 `KeyAudio`
- 롱/샌드백이면 `Length`

## 롱노트가 이상할 때

롱노트는 단일 노트가 아니라 start, middle, end 여러 행입니다. `IsLong=true`를 쓰지 않고 `NoteType=3`만 지정하면 구조가 부족할 수 있습니다.

정상 예:

```csharp
new ExperimentNoteSpec
{
    Label = "롱 테스트",
    Uid = "050201",
    NoteType = 3,
    Pathway = 0,
    IsLong = true,
    StartTick = 25.0,
    Length = 2.0
},
```

확인할 점:

- `IsLong=true`인지
- `Length`가 0보다 큰지
- `StartTick + Length`가 곡의 정상 플레이 구간 안인지
- 로그에 start/end가 같이 찍히는지

## 샌드백/type 8이 이상할 때

샌드백은 롱노트와 달리 단일 슬롯에 길이를 가진 구조로 봅니다. `IsLong=true`가 아니라 `IsMul=true`를 씁니다.

정상 예:

```csharp
new ExperimentNoteSpec
{
    Label = "샌드백",
    Uid = "020401",
    NoteType = 8,
    Pathway = 0,
    IsMul = true,
    StartTick = 32.0,
    Length = 1.2
},
```

## 보스가 안 나올 때

보스 문제는 두 단계로 나눠 봅니다.

### 1. 보스 액션 트리거가 들어갔는지

BMS 차트를 쓰는 곡은 등장 노트(WAV UID의 뒤 4자리 `0101`, 예: `010101_…wav`)가 차트에 있어야 하고, BMS가 없는 곡은 `DBStageInfoPatch.cs` (`Patches/Database/Stage/`)의 `ExperimentNotes`에 `BossAction="in"`이 필요합니다.

`ExperimentNotes` 예:

```csharp
new ExperimentNoteSpec
{
    Label = "보스 등장",
    Uid = "050101",
    NoteType = 0,
    Pathway = 0,
    BossAction = "in",
    StartTick = 15.0
},
```

로그에서 봐야 할 값:

```text
prefab=empty_000
type=0
dt=0
showTick=15
```

`empty_000`은 보스 모델이 아니라 액션 트리거입니다. 화면에 노트처럼 안 보이는 것이 정상입니다.

### 2. 실제 보스 프리팹이 바뀌었는지

보스 이름/씬은 (1) BMS 차트의 첫 `in` 노트에서 정해지고, 그런 노트가 없을 때만 (2) `BossPatch.cs` (`Patches/Battle/Mechanics/`)의 `BossRewriteRules`가 매칭됩니다.

로그:

```text
Il2Cpp.Boss.InitBossObject 호출: name=..., scene=..., isLast=..., instance=...
Il2Cpp.Boss.InitBossObject: BMS 첫 'in' 노트를 통한 동적 보스 매핑 적용 -> name=0601_boss, scene=6
Il2Cpp.Boss.InitBossObject: Static 폴백 변경 적용 -> name=0701_boss, scene=7
```

`… 적용` 로그가 없고 `변경 건너뜀`만 있으면 커스텀 차트가 적용되지 않은 판입니다. 둘 다 없으면 규칙 조건이 안 맞은 것입니다.

확인할 점 (`BossRewriteRules`를 쓰는 경우):

- `OrigName`이 너무 좁게 잡혀 있지 않은지
- `OrigScene`이 현재 씬과 다른 값인지
- `OrigIsLast`가 실제 값과 다른지
- `NewName`, `NewScene` 조합이 실제 존재하는 보스인지

처음 실험할 때는 조건을 넓게 두는 편이 좋습니다.

```csharp
new BossRule
{
    OrigName = "*",
    OrigScene = null,
    OrigIsLast = null,
    NewName = "0601_boss",
    NewScene = 6
},
```

## 곡 제목/아티스트가 안 바뀔 때

곡 제목/아티스트/레벨 디자이너 표시는 `hwa` 곡 폴더의 `info.txt`(매니페스트) 값이 우선이고, `PnlMusicOverride.cs` (`Patches/UI/Music/`)가 화면 텍스트에 그 값을 씁니다. 제목을 바꾸려면 코드가 아니라 `info.txt`의 `커스텀곡제목`/`커스텀아티스트`/`레벨디자이너`를 고치세요. 매니페스트 조회가 모두 실패했을 때만 `PnlMusicOverride`의 폴백 상수(`ExperimentTitle = "Custom Chart"` 등)가 표시됩니다. (`EnableSongTitleExperiment`는 항상 `true`인 상수라 끄는 스위치가 아닙니다.)

확인할 점:

- `LogLevel = "Verbose"`에서 `[SongTitleOverride] 화면 텍스트 적용(WRITE)` 로그가 찍히는지
- `info.txt`가 읽혔는지(`[HwaResourceManager] manifest 파싱 완료` 로그와 알 수 없는 키 경고)
- 텍스트가 다른 자식 오브젝트명으로 숨어 있는지

곡 선택 화면과 준비 화면은 서로 다른 UI 구조를 쓸 수 있습니다. 한 화면에서는 바뀌고 다른 화면에서는 안 바뀌면 그 화면의 텍스트 오브젝트 후보 이름을 추가해야 할 수 있습니다.

후보 이름 배열:

```csharp
TitleTextObjectNames
ArtistTextObjectNames
LevelDesignerLabelTextObjectNames
LevelDesignerNameTextObjectNames
```

## 음악 클립 이름이 이상할 때

`PnlStage.ChangeMusic` 직후에는 클립이 이전 곡 또는 메뉴 BGM처럼 보일 수 있습니다. 클립 이름은 준비 화면 쪽 로그(`LogLevel = "Verbose"`에서만 출력)를 더 신뢰합니다.

- `PnlPreparation.GameStart: 곡 이름=…, 음악 클립=…`
- `PnlPreparation.OnBattleStart: …`
- `PnlPreparation.OnEnable: …` / `…Delay`(0.25초·1초 뒤 재시도)

(예전 문서가 안내하던 `PnlStage.ChangeFinalMusic` 로그는 지금 코드에 없습니다.)

오디오 클립 탐색은 두 경로를 사용합니다.

1. 패널 인스턴스의 필드/프로퍼티에서 `AudioClip` 또는 `musicClip`, `demoMusic`, `bgm`, `audio` 이름 찾기
2. 씬의 `AudioSource`에서 재생 중이거나 음악처럼 보이는 클립 찾기

클릭음이나 효과음은 `click`, `sfx`, `button` 이름을 기준으로 제외합니다.

## 정확도 계산 및 All Perfect 배너가 이상할 때

올 퍼펙트(All Perfect) 판정이나 결과 화면의 골드 배너 연출이 동작하지 않는 경우 아래 로그와 설정을 점검하십시오.

> [!WARNING]
> **`EnableAPMod = false`(MelonPreferences)이면 결과 화면 훅(`PnlVictory2dManager.OnShowVictory`)이 첫 줄에서 통째로 끝납니다.** AP 배너뿐 아니라 **커스텀 곡 기록 저장(`record/*.json`)과 결과 화면 진입 시 미디어 정지도 그 훅 안에 있어서 함께 건너뜁니다**(현재 동작). 또 `AddScore`/`IsFullCombo` 훅의 `TaskStageTarget` 캐싱도 꺼집니다. 커스텀 곡 기록이 저장되지 않는다면 먼저 이 값이 `true`인지 확인하세요. 반대로 정확도 오버라이드(`GetAccuracy`/`GetTrueAccuracy`/`GetTrueAccuracyNew` 패치)는 이 설정과 무관하게 커스텀 차트를 적용하는 판이면 항상 동작합니다.

### 1. 정확도 디버그 로그 확인
이 로그는 `GetAccuracy`가 불릴 때마다 찍히므로 **`LogLevel = "Verbose"`일 때만** 나옵니다(아래 “수동 로그 레벨 설정” 절 참고). Verbose로 바꾼 뒤 플레이가 끝날 때 콘솔에 아래와 같은 로그가 남는지 확인합니다.
```text
[APMod.Debug.Accuracy] m_MusicCount=0, m_PerfectResult=351, m_GreatResult=0, m_MissResult=0, m_CoolResult=0, m_HitCount=351, m_LongPressCount=10, m_LongPressHitCount=10, m_EnergyCount=0, GetAccuracy()=0.854000, GetTrueAccuracy()=0.914062, GetTrueAccuracyNew()=0.854015
```
* **확인할 점**:
  * `m_GreatResult`와 `m_MissResult`가 모두 0이어야 All Perfect 조건이 만족됩니다.
  * 커스텀 차트의 실제 노트 종류별 집계가 올바르게 작동했는지 `[APMod.Accuracy] Custom chart note counts: Standard=..., Gears=..., Hearts=..., BlueNotes=...` 로그를 통해 교차 검증합니다.

### 2. HUD 폰트 캐싱 상태 점검
올 퍼펙트 달성 시 골드 배너를 그리기 위해 인게임 메인 점수 폰트(`LuckiestGuy-Regular_150_115`)를 동적으로 캐싱해야 합니다.
```text
[APMod.Debug.Font] HUD 폰트 캐싱 시도 시작 - ...
[APMod.Debug.Font] 일반 폰트 획득 완료: 'LuckiestGuy-Regular_150_115'
```
* **문제 상황**: 폰트 캐싱에 실패하여 순정 `"FULL COMBO"` 배너가 그대로 나오거나 골드 텍스트의 외곽선/스타일이 깨지는 경우.
* **해결 방법**: `PnlBattle.instance.currentComps.scoreValue` 객체가 인게임 내에서 활성화되기 전에 조기 쿼리가 들어갔는지 확인하고, 폰트 획득 로그가 한 번이라도 제대로 출력되었는지 점검하십시오.

## 로그가 너무 많을 때 (로그 레벨 제어 및 UMPC 최적화)

v0.10.0부터 **하드웨어 자동 감지(`DeviceDetector`)**와 **동적 로그 레벨 제어(`ModLogger`)** 시스템이 탑재되었습니다.

### 1. UMPC(핸드헬드) 환경에서의 자동 음소거 (Auto Mode)
* **배경**: ROG Ally, Steam Deck, Legion Go 등 UMPC 기기는 저전력(TDP 15~25W) 및 공유 메모리 구조를 가집니다. 게임 루프 또는 차트 파싱(롱노트·샌드백 매칭 수백 건) 시 콘솔 문자열 렌더링 및 디스크 파일 I/O로 인한 순간 끊김(Stuttering/프레임 드랍)이 발생할 수 있습니다.
* **동작**: 시작 시 UMPC가 감지되면 로그 레벨이 **`Error`**(치명적 오류만)로 자동 강하됩니다.
* **단일 창구 (`ModLogger`)**: 모드 내부의 모든 로그는 `ModLogger.Msg/Warning/Verbose/Error`를 거치며, 레벨에 미달하면 콘솔 렌더링·파일 I/O가 일어나지 않습니다. 일반 플레이 중에는 로그가 조용하게 유지됩니다.
* **문자열 조립도 생략**: `ModLogger.Msg/Warning/Verbose($"...")`처럼 보간 문자열로 부르면 전용 핸들러(`ModLogMessageHandler`)가 받아서, 레벨이 꺼져 있을 때는 문자열을 만들지 않고 `{...}` 안의 식도 평가하지 않습니다. 그러니 로그 구멍 안에 “로그를 안 찍어도 꼭 실행돼야 하는 일”을 넣지 마십시오. (예전에는 레벨 판정 전에 문자열이 먼저 완성돼, 기본 설정에서도 Verbose 문자열 125곳이 매번 만들어졌다 버려졌습니다.)
* **참고**: v0.10.0에는 `MelonLogger`를 Harmony로 전역 가로채는 방식이 있었지만, MelonLoader 0.7.3에서 관리 메서드 디투어가 NRE로 실패해 음소거가 아예 걸리지 않았습니다. 시작 로그에 `Failed to HarmonyInit PatchAll: muse_dash_test.MelonLoggerInterceptor`가 보인다면 그 버전입니다. 현재는 제거되었습니다.

### 2. 수동 로그 레벨 설정 (`MelonPreferences.cfg`)
`UserData/MelonPreferences.cfg` 파일의 `[muse-dash-custom-chart-features]` 섹션에서 `LogLevel` 키를 직접 변경할 수 있습니다:

```toml
[muse-dash-custom-chart-features]
LogLevel = "Auto" # "Auto", "Silent", "Error", "Warning", "Info", "Verbose"
```

| 값 | 설명 | 출력 대상 |
|---|---|---|
| **`Auto`** (기본값) | 기기 자동 판별 | UMPC는 `Error`, 일반 PC는 `Info` (또는 `VerboseLog=true` 시 `Verbose`) |
| **`Silent`** | 완전 음소거 | 에러/경고/메시지 포함 전체 차단 |
| **`Error`** | 오류 전용 | 치명적인 예외 및 크래시 오류만 출력 |
| **`Warning`** | 경고 + 오류 | 경고 및 에러만 출력 |
| **`Info`** | 일반 정보 | 기본 상태 안내 및 상태 변경 메시지 포함 |
| **`Verbose`** | 상세 진단 | 내부 디버깅용 상세 로그까지 모두 출력 |

### 3. 디버그 덤프 플래그
아래는 코드에서 켜고 끄는 개발용 진단 스위치입니다. 일반 플레이 시에는 기본값 그대로 두세요.

| 위치 | 기본값 | 설명 |
| --- | --- | --- |
| `DBStageInfo_SetRuntimeMusicData_Patch.DebugExperimentNotes` (`DBStageInfoPatch.cs`) | `false` | `true`로 바꾸고 다시 빌드하면 노트 생성 전후 상태를 Info로 자세히 출력하고 호출 스택까지 남깁니다. |
| `SpineActionContract.DumpEnabled` (`Spine/Patch_SpineActionContract.cs`) | `false` | `true`로 바꾸고 다시 빌드하면 배틀 캐릭터의 액션·애니메이션 목록을 `spine contract/` 폴더에 txt로 덤프합니다. 꺼져 있으면 폴더도 만들지 않습니다. |
| `GameMusicScene_InitTimer_Patch` / `GameMusicScene_PreLoadEnemy_Patch`의 `EnableDebugLogs` | `true`(하드코딩) | 씬 변형·풀 빌드 진단 로그(Info)입니다. 로그 수준으로 끌 수 없습니다. |

> `DumpMusicList`는 스위치가 아니라 함수입니다. 공식곡을 로드할 때마다 원본 차트의 미등록 노트를 훑어 `[OfficialSceneContext]` 로그를 Info로 남깁니다(README의 "특수 기믹 곡 스캔" 조사용). 이전 문서가 안내하던 `DumpStageBattleComponentProperties`/`DumpStageInfo`는 코드에서 제거되었습니다. 노트 상세 덤프(`StageBattleMusicDataDump`)는 호출하는 곳 없이 남아 있다가 지웠습니다. `StageBattleComponent`를 리플렉션으로 전부 훑는 진단이라 [CHECKLIST.md](CHECKLIST.md)의 "깊게 훑는 진단" 항목에 걸리기도 합니다. 꼭 필요하면 커밋 `e896584`의 `StageBattleComponentPatch.cs`에서 되살리되, 기본값 off로 두십시오.

## 빌드가 실패할 때

### NuGet.Config 접근 오류

샌드박스 환경에서는 아래처럼 사용자 NuGet 설정 접근이 막힐 수 있습니다.

```text
Access to the path 'C:\Users\...\AppData\Roaming\NuGet\NuGet.Config' is denied.
```

이 경우 일반 PowerShell/터미널에서 빌드하거나, 접근 권한이 있는 환경에서 실행해야 합니다.

### 참조 DLL 오류

`.csproj`는 게임 설치 폴더(`$(GamePath)`)의 DLL을 와일드카드로 직접 참조합니다(`HintPath`는 쓰지 않습니다).

```text
$(GamePath)\MelonLoader\net6\*.dll
$(GamePath)\MelonLoader\Il2CppAssemblies\*.dll
```

`GamePath`의 기본값은 작성자 환경의 `H:\muse dash hwa`입니다. 다른 위치에 게임이 있으면 알려 줘야 합니다.

```text
dotnet build "muse dash test\muse dash test.csproj" -p:GamePath="D:\Steam\steamapps\common\Muse Dash"
set GAME_PATH=D:\Steam\steamapps\common\Muse Dash    ← build.bat용 (실행 전에 설정)
```

빌드가 참조 DLL을 못 찾으면 아래를 확인합니다.

- `GamePath`(또는 `GAME_PATH`)가 실제 게임 설치 경로인지
- MelonLoader가 설치되어 있고, 게임을 한 번 실행해 `Il2CppAssemblies`가 생성되었는지
- `MelonLoader\net6`와 `MelonLoader\Il2CppAssemblies` 폴더가 있는지

빌드가 끝나면 csproj의 `DeployToMods` 타깃이 DLL을 `$(GamePath)\Mods`로 복사합니다. **복사에 실패해도 `[AutoDeploy] Successfully deployed` 메시지는 찍히므로**(`ContinueOnError="WarnAndContinue"`), `Mods` 폴더에 실제로 파일이 있는지 눈으로 확인하세요.

## 변경 후 기본 확인 순서

1. `dotnet build "muse dash test\muse dash test.csproj"`로 빌드합니다.
2. DLL을 Muse Dash `Mods` 폴더에 반영합니다.
3. 게임 실행 후 `모드가 로드되었습니다.` 로그를 확인합니다.
4. 곡 선택 화면에서 `PnlStage` 로그를 확인합니다.
5. 게임 시작 후 `DBStageInfo.SetRuntimeMusicData`와 `실험 차트 적용 완료`를 확인합니다.
6. 노트 실험이면 `LogLevel = "Verbose"`로 `실험 노트 추가` 로그의 UID/type/pathway/prefab을 확인합니다.
7. 보스 실험이면 `Boss.InitBossObject: … 적용` 로그(`동적 보스 매핑 적용` 또는 `Static 폴백 변경 적용`)를 확인합니다.
8. **폴백 경로도 밟아 봅니다.** 정상(BMS 있는 곡)만 확인하면 폴백은 아무도 안 밟습니다. 곡 폴더를 비우거나 BMS 없는 슬롯을 골라 배틀에 실제로 들어가 봅니다([CHECKLIST.md](CHECKLIST.md)).

## 모드 탓이 아닌 것으로 보이는 에러

콘솔에 빨간 줄이 떠도 모드 코드가 원인이 아닐 수 있습니다. 아래는 추적해 보고 게임 쪽 문제로 결론 낸(또는 그렇게 보고 보류한) 에러입니다.
같은 에러를 다시 보면 여기부터 확인하세요.

### 괴도 린 + Bad Apple: `GirlActionController.GhostDisappear` NRE (보류, 2026-09-27)

**증상** — 플레이 중 콘솔에 아래 에러가 한 번 찍힙니다.

```text
[ERROR] [Il2CppInterop] During invoking native->managed trampoline
System.NullReferenceException: Object reference not set to an instance of an object.
  at DG.Tweening.ShortcutExtensions.DOFloat (UnityEngine.Material target, ...)
  at GirlActionController.GhostDisappear (System.Single dt)
  at GirlActionController.GhostAttack (...)
  at GirlActionController.AttackQuick (...)
  at AttacksController.PlayAttackAnim / ShowAttackEffect / ShowAttack (...)
  at GameLogic.GameTouchPlay.TouchResult (...)
```

**조건** — 세 가지가 모두 맞을 때만 납니다.

- 캐릭터: 괴도 린("두 얼굴의 괴도", 배틀 오브젝트 `thief_girl_battle(Clone)`)
- 곡: Bad Apple!! feat. Nomico (`42-0`)
- 입력: 홀드를 누른 채 반대쪽 단노트를 칠 때. 본체가 홀드에 묶여 있어 분신이 대신 치는 순간입니다(`GhostAttack`).
  홀드 + 단노트는 플레이하면서 눈으로 확인한 것이고, 로그에는 어느 노트에서 났는지 찍히지 않습니다.
  첫 에러 시점이 곡 시작 후 29초로 이르게 난 판도 있고 74~75초에 난 판도 있었던 것도 이 때문으로 봅니다.

**실측** (2026-09-27, 게임 6.7.0)

| 캐릭터 | 곡 | 모드 설정 | 결과 |
| --- | --- | --- | --- |
| 괴도 린 | Bad Apple | 평소 설정(강제퍼펙트 켬) | 에러 |
| 괴도 린 | Bad Apple | 강제퍼펙트 끔 | 에러 |
| 괴도 린 | Bad Apple | 모든 기능 끔(`MelonPreferences.cfg`의 `Enable*` 전부 + `config.txt` 토글) | 에러 |
| 괴도 린 | Bad Apple | 평소 설정 + 아래 3번의 진단 가드를 넣은 빌드 | 에러 8번, **결과 화면까지 클리어** |
| 괴도 린 | 0-40(클리어), 74-4, 48-8 | 평소 설정 | 에러 없음 |
| `black_girl` | Bad Apple | 평소 설정(스킨 주입 포함) | 클리어, 에러 없음 |

**크래시가 아닙니다.** 게임이 스스로 꺼진 적은 없습니다. 앞의 세 판은 빨간 줄을 보고 일시정지한 뒤 직접 종료했습니다
(`Player.log`에 `PnlBattle.OnPauseClicked` 다음 정상 종료 절차가 남아 있습니다).
끝까지 친 판에서는 홀드 + 단노트마다 에러가 났지만(8번, 3~4초 간격) 곡은 멈추지 않고 결과 화면까지 갔습니다.
분신이 화면에서 제대로 사라지는지는 확인하지 않았습니다.

**게임 쪽으로 보는 이유와 한계**

- 예외가 난 곳은 게임 코드이고, 모드에는 괴도 린을 따로 다루는 코드가 없습니다.
- 모든 기능을 꺼도 났습니다. 다만 기능을 꺼도 `PatchInstaller`가 Harmony 패치는 전부 겁니다. 그래서 `TouchResult` 훅은 남아 있었고,
  빨간 줄은 그 훅을 지나가는 예외를 Il2CppInterop이 찍은 것입니다. **모드 DLL을 뺀 순정 상태로는 확인하지 않았으므로 확정은 아닙니다.**

**게임 코드 단서** (`Decompiled/`)

- 분신 페이드용 머티리얼 배열 `GirlActionController.ghostMtrl`이 비어 있습니다(실측). 괴도 린은 이 배열이 **길이 1이고 그 한 칸이 비어** 있습니다.
  진단 가드 로그: `[GhostGuard] 분신 머티리얼 빈 칸 1/1개 (목록=[0]=(빈 칸)), obj=thief_girl_battle(Clone), uid=42-0`.
  `DOFloat`가 맨 먼저 하는 일이 `target` 머티리얼 접근이라 여기서 NRE가 납니다.
- Bad Apple 전용 로직이 따로 있습니다. 롱노트는 `TouhouLogic.ReplaceBadAppleLongPress(ref MusicData)`와 `SetLongCatchColor`가 처리하고,
  실루엣 교체는 `GameMainSpecialLogic.BadAppleLogic`, `ReplaceTools`/`ReplaceSpine`/`ReplaceSkeletonCustomMaterials`가 합니다.
  캐릭터마다 `CharacterEnterConfig.isReplaceBadapple` 플래그도 있습니다.

**다시 볼 때 할 일**

1. `Mods/muse-dash-custom-chart.dll`을 잠깐 빼고 같은 조건으로 플레이합니다. 모드가 없으면 콘솔 빨간 줄은 안 뜨지만,
   에러가 났다면 `%USERPROFILE%\AppData\LocalLow\PeroPeroGames\MuseDash\Player.log`에 `GhostDisappear`가 남습니다. 찍히면 순정 버그로 확정입니다.
2. 분신이 사라지지 않고 남는 등 화면에 이상이 있는지 봅니다. 없으면 실제 피해는 콘솔의 빨간 줄뿐입니다.
3. 모드에서 막기로 한다면, **"같은 배열의 살아 있는 머티리얼로 빈 칸 채우기" 안은 통하지 않습니다.**
   `GhostDisappear` Prefix로 시험했고, 훅은 제대로 걸렸습니다(스택에 `DMD<Il2Cpp.GirlActionController::GhostDisappear>`).
   하지만 괴도 린은 유일한 칸이 비어 있어 채울 머티리얼이 없었습니다. 이 가드는 커밋하지 않고 버렸습니다.
   남은 선택지와 각각의 위험은 이렇습니다.
   - Finalizer로 예외를 삼키기: 빨간 줄은 사라집니다. 하지만 `GhostDisappear`의 나머지(트윈 시퀀스, 완료 콜백 `_GhostDisappear_b__50_0`)가
     실행되지 않아 분신이 남을 수 있습니다.
   - 분신 렌더러의 현재 머티리얼로 채우기: 그 머티리얼이 본체와 공유하는 아틀라스 머티리얼이면 본체까지 같이 페이드될 수 있습니다.
   `GhostDisappear`는 public·non-virtual이라 [체크리스트](CHECKLIST.md)의 IL2CPP 패치 함정 대상은 아닙니다.

당장 피하려면 Bad Apple만 다른 캐릭터로 플레이하면 됩니다.

## 세이브 데이터 위치와 진행도 초기화

Muse Dash의 실제 계정 진행도는 두 곳에 나뉘어 저장됩니다.

| 위치 | 내용 | 비고 |
| --- | --- | --- |
| `HKCU\Software\PeroPeroGames\Muse Dash` (및 `MuseDash` 키) | 해상도, 언어, 그래픽 옵션 등 Unity `PlayerPrefs` 설정값 | `Account_h362646116` 등의 값도 있지만 실제 클리어 진행도의 최종 소스는 아님 |
| `%LOCALAPPDATA%\Steam\MuseDash\<steamID>MuseDashSaves.sav` | 실제 계정 진행도(클리어 기록, 최고점수, 언락 상태 등) | **Steam Cloud로 동기화됨** |

`SaveDataManagerPatch.cs`의 `DataManager.Save()` 패치에서 `Application.persistentDataPath`를 로그로 찍어보면 `C:\Users\...\AppData\LocalLow\PeroPeroGames\MuseDash`가 나오지만, 여긴 로그/캐시만 있고 실제 세이브 파일은 없다 — 진짜 세이브는 위 표의 `.sav` 경로에 있다.

### 진행도를 완전히 초기화하려면

1. **Steam 클라이언트에서 먼저 클라우드 동기화를 끈다.** 라이브러리 → Muse Dash 우클릭 → 속성 → 업데이트 탭 → "이 게임에 대해 Steam Cloud 동기화 사용" 체크 해제.
   - 이 단계를 빼고 로컬 `.sav`나 레지스트리만 지우면, 게임을 켜는 순간 Steam Cloud가 즉시 다시 내려받아 복원시켜버린다.
2. `%LOCALAPPDATA%\Steam\MuseDash\<steamID>MuseDashSaves.sav` 파일을 삭제(또는 백업 후 삭제)한다.
3. (선택) `HKCU\Software\PeroPeroGames\Muse Dash`, `MuseDash` 레지스트리 키도 삭제한다.
4. 게임을 실행해 신규 계정처럼 시작되는지 확인한다.
5. 확인 후 다시 클라우드 동기화를 켜면, 새로 생성된 빈 세이브가 클라우드에 업로드되어 덮어써진다.

복구할 일이 생길 수 있으니 삭제 전에는 항상 `.sav`와 레지스트리 키를 백업(`reg export`, 파일 복사)해두는 것을 권장한다.
