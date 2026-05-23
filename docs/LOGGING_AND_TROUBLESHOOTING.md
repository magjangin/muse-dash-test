# 로그와 문제 해결 가이드

이 문서는 모드가 출력하는 로그를 어떻게 읽고, 문제가 생겼을 때 어디부터 확인할지 정리한 문서입니다.

## 먼저 보는 로그

실험 중 가장 먼저 확인할 로그는 아래입니다.

```text
모드가 로드되었습니다.
씬이 로드되었습니다: ...
DBStageInfo.SetRuntimeMusicData 호출됨: ...
실험 노트 추가: ...
실험 차트 적용 완료: ...
PnlStage.ChangeFinalMusic: 곡 이름=..., 음악 클립=..., 아티스트 이름=...
PnlPreparation.GameStart: 곡 이름=..., 음악 클립=..., 아티스트 이름=...
Il2Cpp.Boss.InitBossObject: 변경 적용 -> name=..., scene=...
```

이 로그들이 모두 보이면 모드는 정상적으로 로드되고, 차트/보스/UI 후킹도 대부분 작동 중이라고 볼 수 있습니다.

## 로그별 의미

| 로그 | 의미 |
| --- | --- |
| `모드가 로드되었습니다.` | MelonLoader가 DLL을 정상 로드했습니다. |
| `씬이 로드되었습니다` | Unity 씬 전환이 감지됐습니다. |
| `DBStageInfo.SetRuntimeMusicData 호출됨` | 게임이 런타임 차트 데이터를 만들었습니다. |
| `실험 노트 추가` | `ExperimentNotes`의 항목이 실제 `MusicData`로 들어갔습니다. |
| `실험 차트 적용 완료` | 원본 리스트를 실험 리스트로 재구성했습니다. |
| `PnlStage.ChangeMusic` | 곡 선택이 바뀐 직후입니다. 클립은 아직 이전 값일 수 있습니다. |
| `PnlStage.ChangeFinalMusic` | 최종 곡 변경 후라 곡 정보가 더 믿을 만합니다. |
| `PnlPreparation.GameStart` | 준비 화면에서 게임 시작 직후입니다. |
| `Boss.InitBossObject 호출` | 실제 보스 오브젝트 초기화가 시작됐습니다. |
| `Boss.InitBossObject: 변경 적용` | `BossRewriteRules`가 매칭되어 보스 이름/씬이 바뀌었습니다. |

## 노트가 안 보일 때

### 1. `실험 노트 추가`가 찍히는지 확인

찍히지 않으면 `DBStageInfo.SetRuntimeMusicData`가 아직 호출되지 않았거나, `ExperimentNotes` 배열 문법에 문제가 있을 수 있습니다.

봐야 할 위치:

```text
muse dash test/Patches/DBStageInfoPatch.cs
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

`DBStageInfoPatch.cs`의 `ExperimentNotes`에 `BossAction="in"`이 필요합니다.

예:

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

`BossPatch.cs`에서 `BossRewriteRules`가 매칭되어야 합니다.

로그:

```text
Il2Cpp.Boss.InitBossObject 호출: name=..., scene=..., isLast=...
Il2Cpp.Boss.InitBossObject: 변경 적용 -> name=0601_boss, scene=6
```

`변경 적용` 로그가 없으면 조건이 안 맞은 것입니다.

확인할 점:

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

곡 정보 실험은 `PnlStagePatch.cs`의 상수에서 켭니다.

```csharp
private const bool EnableSongTitleExperiment = true;
```

확인할 점:

- `PnlStage.Start` 로그가 찍히는지
- `PnlStage.ChangeFinalMusic` 로그가 찍히는지
- `PnlPreparation.GameStart` 로그가 찍히는지
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

`PnlStage.ChangeMusic` 직후에는 클립이 이전 곡 또는 메뉴 BGM처럼 보일 수 있습니다. 클립 이름은 아래 로그를 더 신뢰합니다.

- `PnlStage.ChangeFinalMusic`
- `PnlPreparation.GameStart`
- `PnlPreparation.OnBattleStart`

오디오 클립 탐색은 두 경로를 사용합니다.

1. 패널 인스턴스의 필드/프로퍼티에서 `AudioClip` 또는 `musicClip`, `demoMusic`, `bgm`, `audio` 이름 찾기
2. 씬의 `AudioSource`에서 재생 중이거나 음악처럼 보이는 클립 찾기

클릭음이나 효과음은 `click`, `sfx`, `button` 이름을 기준으로 제외합니다.

## 로그가 너무 많을 때

아래 기능들은 로그가 길어질 수 있습니다.

| 위치 | 이유 |
| --- | --- |
| `DebugExperimentNotes = true` | 노트 생성 전후 상태를 자세히 출력합니다. |
| `DumpMusicList` | 원본/실험 노트 내부 필드를 덤프합니다. |
| `DumpStageBattleComponentProperties` | 전투 컴포넌트의 공개 프로퍼티와 리스트를 깊게 덤프합니다. |
| `DumpStageInfo` | StageInfo 필드와 프로퍼티를 모두 덤프합니다. |

실험값이 어느 정도 안정되면 상세 덤프는 끄거나 호출을 주석 처리하는 편이 로그 읽기가 쉬워집니다.

## 빌드가 실패할 때

### NuGet.Config 접근 오류

샌드박스 환경에서는 아래처럼 사용자 NuGet 설정 접근이 막힐 수 있습니다.

```text
Access to the path 'C:\Users\...\AppData\Roaming\NuGet\NuGet.Config' is denied.
```

이 경우 일반 PowerShell/터미널에서 빌드하거나, 접근 권한이 있는 환경에서 실행해야 합니다.

### 참조 DLL 오류

`.csproj`는 Muse Dash 설치 폴더의 DLL을 직접 참조합니다.

```text
..\..\..\..\steam\steamapps\common\Muse Dash\MelonLoader\...
```

빌드가 참조 DLL을 못 찾으면 아래를 확인합니다.

- Muse Dash 설치 경로가 실제로 `H:\steam\steamapps\common\Muse Dash`인지
- MelonLoader가 설치되어 있는지
- `MelonLoader\net6`와 `MelonLoader\Il2CppAssemblies` 폴더가 있는지
- `.csproj`의 `HintPath`가 현재 설치 경로와 맞는지

## 변경 후 기본 확인 순서

1. `dotnet build "muse dash test\muse dash test.csproj"`로 빌드합니다.
2. DLL을 Muse Dash `Mods` 폴더에 반영합니다.
3. 게임 실행 후 `모드가 로드되었습니다.` 로그를 확인합니다.
4. 곡 선택 화면에서 `PnlStage` 로그를 확인합니다.
5. 게임 시작 후 `DBStageInfo.SetRuntimeMusicData`와 `실험 차트 적용 완료`를 확인합니다.
6. 노트 실험이면 `실험 노트 추가` 로그의 UID/type/pathway/prefab을 확인합니다.
7. 보스 실험이면 `Boss.InitBossObject: 변경 적용` 로그를 확인합니다.
