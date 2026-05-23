# Muse Dash 실험 모드 전체 가이드

이 저장소는 Muse Dash의 런타임 데이터를 후킹해서 노트, 보스, 곡 메타데이터를 빠르게 실험하기 위한 모드입니다. 지금 구조는 “원본 데이터를 직접 대량 수정”하는 방식이 아니라, 게임이 런타임에 만든 객체를 복사하거나 UI 컴포넌트를 찾아서 원하는 값으로 덮어쓰는 방식입니다.

가장 자주 보는 문서는 아래 세 개입니다.

- [노트 실험](NOTE_EXPERIMENTS.md): 일반 노트, 공중 노트, 롱노트, 샌드백, 하트, 음표, 속도, dt 실험
- [보스 실험](BOSS_EXPERIMENTS.md): 보스 액션 트리거와 실제 보스 프리팹 변경
- [코드 파일별 레퍼런스](CODE_REFERENCE.md): 전체 C# 파일의 역할, 주요 클래스/메서드, 읽는 순서
- [로그와 문제 해결](LOGGING_AND_TROUBLESHOOTING.md): 로그 해석, 노트/보스/UI가 안 될 때 확인 순서
- 이 문서: 전체 파일 구조, 빌드, 로그 확인, 곡 제목/아티스트/레벨 디자이너 실험

## 먼저 알아야 할 구조

Muse Dash의 노트 데이터는 보통 “노트 종류별 배열”로 나뉘어 있지 않고, 하나의 `MusicData` 리스트 안에 같이 들어갑니다. 일반 노트, 공중 노트, 롱노트의 start/middle/end, 샌드백, 보스 액션 트리거가 모두 같은 리스트에 섞여 있습니다.

현재 모드가 건드리는 핵심 리스트는 런타임에서 만들어진 `m_MusicTickData` 계열입니다. 게임은 곡별 차트 데이터와 노트 정의 테이블을 읽어서 각 항목의 `configData`와 `noteData`를 채운 뒤, 그 결과를 `MusicData` 리스트로 들고 있습니다.

실험할 때 중요한 판단 기준은 아래 순서입니다.

| 기준 | 의미 |
| --- | --- |
| `noteData.type` | 게임 로직상 어떤 노트인지 결정하는 가장 중요한 값입니다. |
| `noteData.pathway` | 지상/공중 레인입니다. `0`은 지상, `1`은 공중입니다. |
| `noteData.uid` | 노트 리소스 키입니다. 씬, 계열, 방향 정보가 섞여 있습니다. |
| `noteData.prefab_name` | 실제로 어떤 프리팹을 붙일지 결정합니다. UID만 바꾸고 이 값이 원본이면 외형이 그대로일 수 있습니다. |
| `noteData.key_audio` | 하트, 음표 같은 특수 노트는 사운드까지 맞추는 편이 안전합니다. |
| `configData.length` | 롱노트나 샌드백처럼 길이가 있는 노트에서 중요합니다. |
| `tick`, `dt`, `showTick` | 배치 시간과 화면 표시 시작 시간을 결정합니다. |

그래서 원하는 노트를 만들 때는 `Uid` 하나만 바꾸는 방식보다 `Uid`, `NoteType`, `Pathway`, 필요 시 `PrefabName`, `KeyAudio`, `Length`까지 세트로 맞추는 것이 좋습니다.

## 파일 구조

핵심 코드는 `muse dash test/Patches` 폴더에 있습니다.

| 파일 | 역할 |
| --- | --- |
| `DBStageInfoPatch.cs` | 원본 차트의 첫 플레이 노트를 복사해서 원하는 노트로 바꾼 뒤 주입 |
| `BossPatch.cs` | `Boss.InitBossObject`를 후킹해서 실제 화면에 나오는 보스 프리팹 변경 |
| `PnlStagePatch.cs` | 곡 선택 화면의 곡 제목, 아티스트, 레벨 디자이너 UI 텍스트 실험 및 곡 정보 로그 |
| `PnlPreparationPatch.cs` | 준비 패널의 `Awake`, `GameStart`, `OnBattleStart`에서 곡 정보 실험 적용 |
| `GameMusicScenePatch.cs` | 음악 씬 로드 로그 확인 |
| `StageBattleComponentPatch.cs` | 런타임 음악 데이터/노트 데이터 덤프 |
| `UiAndDBPatches.cs` | UI/DB 관련 보조 후킹 |
| `main.cs` | MelonLoader 모드 진입점 |

각 파일의 주요 클래스와 메서드 단위 설명은 [코드 파일별 레퍼런스](CODE_REFERENCE.md)에 따로 정리했습니다.

## 빠른 수정 위치

노트 실험은 `DBStageInfoPatch.cs`의 `ExperimentNotes` 배열을 수정합니다.

```csharp
private static readonly ExperimentNoteSpec[] ExperimentNotes =
{
    new ExperimentNoteSpec { Label = "지상 일반 노트", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0, Speed = 5 },
};
```

곡 제목/아티스트/레벨 디자이너 실험은 `PnlStagePatch.cs` 상단의 상수를 수정합니다.

```csharp
private const bool EnableSongTitleExperiment = true;
private const string ExperimentTitle = "화영왕";
private const string ExperimentArtist = "화영왕";
private const string ExperimentLevelDesignerLabel = "레벨 디자이너";
private const string ExperimentLevelDesignerName = "화영왕";
```

실제 보스 프리팹은 `BossPatch.cs`의 `BossRewriteRules` 배열에서 바꿉니다.

```csharp
private static readonly BossRule[] BossRewriteRules = new[]
{
    new BossRule { OrigName = "*", OrigScene = null, OrigIsLast = null, NewName = "0401_boss", NewScene = 4 },
};
```

## 노트 실험 흐름

`DBStageInfo.SetRuntimeMusicData`가 호출된 뒤 원본 음악 데이터 배열을 받습니다. 모드는 원본 `[0]` 슬롯은 그대로 복사해서 유지하고, 원본 첫 플레이 노트인 `SourceNoteIndex = 1`을 베이스로 복사합니다. 그 복사본에 `ExperimentNoteSpec` 값을 덮어써서 새 노트를 만듭니다.

이 방식의 장점은 원본 배열 안에서 같은 타입의 노트를 찾지 않아도 된다는 점입니다. 일반 노트 하나를 베이스로 잡고, `Uid`, `NoteType`, `Pathway`, `PrefabName`, `KeyAudio`, `BossAction`, `Speed`, `Dt`를 덮어씁니다.

자동으로 처리되는 값은 아래와 같습니다.

- `objId`와 `configData.id`는 리스트 순서에 맞춰 자동 설정
- `tick`과 `configData.time` 동기화
- `dt`와 `showTick` 계산
- `Speed` 지정 시 `noteData.speed` 덮어쓰기
- `Dt` 지정 시 `showTick = tick - Dt` 계산
- `configData.note_uid`와 `noteData.uid` 동기화
- `Scene`을 비우면 `noteData.scene`은 UID 앞 두 자리 기준으로 `scene_XX` 설정
- `Scene`을 지정하면 `noteData.scene`을 직접 덮어쓰기. 예: `Scene = "scene_00"`
- 씬 전환 노트(`0004xx`)는 `IbmsId`가 `sceneInfo` 매핑 키와 맞아야 동작. 예: `000401`은 `IbmsId = "1O"`
- `noteData.noteUid`는 UID 숫자값으로 설정
- `PrefabName`을 비워두면 `{uid}_{road/air}_{nor/up/down}_1` 형식으로 자동 생성
- `NoteType=0` 보스 액션 트리거는 기본적으로 `dt=0`, `showTick=tick`, `prefab_name=empty_000`
- 보스 발사체(`xx=06/07/08`, `yy=01/04`)는 `NoteType=1` 일반 노트이며 `BossAction`이 있어도 일반 노트 프리팹과 `dt=0.7`을 사용

### 왜 in-place 수정인가

처음에는 새 리스트를 만들어 반환하는 방식도 생각할 수 있지만, 게임 내부에서 같은 리스트 참조를 여러 곳이 들고 있을 가능성이 있습니다. 새 리스트를 반환하면 어떤 코드는 새 리스트를 보고, 어떤 코드는 원본 리스트를 계속 볼 수 있습니다. 그러면 로그에는 실험 노트가 있는데 화면에는 원본 차트가 나오거나, 노트가 안 보이는 문제가 생길 수 있습니다.

그래서 현재 코드는 리스트 객체 자체는 유지하고, 안의 내용을 `Clear` 후 다시 채우는 in-place 방식을 씁니다. 이렇게 하면 같은 리스트 참조를 들고 있는 게임 내부 코드들이 모두 수정된 내용을 보게 됩니다.

### `[0]` 슬롯을 유지하는 이유

실험 중 `[0]` 슬롯을 바꾸거나 없애면 스폰/표시가 불안정해질 수 있었습니다. 그래서 현재 코드는 `[0]`은 원본 그대로 두고, 실제 실험 노트는 그 뒤에 추가합니다. 일반적으로 `ExperimentNotes`에 1개를 넣으면 최종 리스트는 `[0]` 원본 + 실험 노트 1개가 됩니다.

## 곡 메타데이터 실험 흐름

곡 제목과 아티스트는 `PnlStage` 쪽 UI 텍스트에서 잘 잡힙니다. 준비 화면에서는 `PnlPreparation.Awake`, `PnlPreparation.GameStart`, `PnlPreparation.OnBattleStart`에서 다시 적용합니다. 준비 패널은 텍스트가 `UnityEngine.UI.Text`로 바로 노출되는 경우도 있고, `LongSongNameController`나 마스크 오브젝트 아래에 숨어 있는 경우도 있어서 자식 컴포넌트까지 내려가서 수정합니다.

레벨 디자이너는 두 값으로 나눠 실험합니다.

- `ExperimentLevelDesignerLabel`: 화면의 라벨. 예: `레벨 디자이너`
- `ExperimentLevelDesignerName`: 실제 이름. 예: `화영왕`

로그는 아래처럼 나옵니다.

```text
PnlPreparation.GameStart: 곡 이름=화영왕, 음악 클립=iyaiya_demo, 아티스트 이름=화영왕, 레벨 디자이너=레벨 디자이너, 실제 이름=화영왕
```

`PnlStage.ChangeMusic`는 오디오 클립이 이전 곡이나 메뉴 BGM으로 보일 수 있습니다. 클립까지 정확히 보려면 `PnlStage.ChangeFinalMusic` 또는 `PnlPreparation` 쪽 로그를 더 신뢰하는 편이 좋습니다.

## 보스 실험 흐름

보스 실험은 기본적으로 두 단계로 나뉩니다.

- `DBStageInfoPatch.cs`: 보스 액션 트리거 노트 생성
- `BossPatch.cs`: 실제 화면에 나오는 보스 프리팹 변경

보스 액션 트리거는 `prefab_name=empty_000`을 사용합니다. 이건 보스 모델이 아니라 빈 트리거입니다. 실제 보이는 보스는 `Boss.InitBossObject(name, scene, isLast)`의 `name`과 `scene`에서 결정됩니다.

### 🌟 실시간 보스 교체 (Dynamic Boss Swap)

스테이지 연출 도중 하나의 보스가 퇴장한 뒤, **완전히 다른 종류의 보스를 실시간으로 교환하여 등장**시키는 연출을 지원합니다.

1. **`swap:[보스명]:[씬번호]` 액션 사용**:
   차트의 보스 액션(`BossAction`)에 `swap:보스이름:씬번호` (예: `swap:0401_boss:4`)를 설정하면 모드가 이를 감지해 런타임에 보스 오브젝트를 새로 빌드합니다.
2. **`out` 퇴장 상태 자동 극복**:
   이전 보스가 `out` 액션으로 완전 퇴장하면 유니티 게임 오브젝트가 비활성화(`gameObject.SetActive(false)`) 처리됩니다. 모드는 `swap:` 키워드가 실행되는 시점에 오브젝트와 상위 부모 트랜스폼을 감지하여 안전하게 강제 부활(`SetActive(true)`) 및 교체 작업을 처리합니다.

```csharp
// 실시간 보스 교체 구성 예시
new ExperimentNoteSpec { Label = "보스1 등장", Uid = "050101", NoteType = 0, Pathway = 0, StartTick = 15.0, BossAction = "in" },
new ExperimentNoteSpec { Label = "보스1 퇴장", Uid = "050102", NoteType = 0, Pathway = 0, StartTick = 22.0, BossAction = "out" },
new ExperimentNoteSpec { Label = "보스2 교체", Uid = "050101", NoteType = 0, Pathway = 0, StartTick = 24.0, BossAction = "swap:0401_boss:4" }, // 🌟 24초에 핑크보스로 실시간 교환!
```

보스 발사체는 별도입니다. `xx=06/07/08`, `yy=01/04` 계열은 빈 트리거가 아니라 `NoteType=1` 일반 노트처럼 생성합니다. 보스 액션 없이 발사체만 만들 때는 `BossAction`을 비우거나 생략합니다.

```csharp
new ExperimentNoteSpec { Label = "보스 발사체만 1개", Uid = "070601", NoteType = 1, Pathway = 0, StartTick = 20.0, BossAction = "" },
new ExperimentNoteSpec { Label = "보스 단타 노트 1개", Uid = "070601", NoteType = 1, Pathway = 0, StartTick = 20.0, PrefabName = "070601_road_nor_1", BossAction = "boss_far_atk_1_R", Dt = 0.7 },
new ExperimentNoteSpec { Label = "보스 단타 노트 1개", Uid = "070701", NoteType = 1, Pathway = 0, StartTick = 20.0, BossAction = "boss_far_atk_2", Dt = 0.7 },
```

확인된 발사체 액션은 `**0601`이 `boss_far_atk_1_R`, `**0604`가 `boss_far_atk_1_L`, `**0701`이 `boss_far_atk_2`입니다.

현재 확인한 실제 보스 프리팹은 아래 값입니다.

```csharp
NewName = "0401_boss";
NewScene = 4;
```

## 빌드와 반영

빌드는 저장소 루트에서 실행합니다.

```powershell
dotnet build "muse dash test\muse dash test.csproj" --no-restore
```

빌드 결과 DLL:

```text
muse dash test\bin\Debug\net6.0\muse_dash_test.dll
```

게임에 반영할 위치:

```text
H:\steam\steamapps\common\Muse Dash\Mods\muse_dash_test.dll
```

## 로그 확인

자주 보는 로그는 아래입니다.

```text
실험 노트 추가: ...
실험 차트 적용 완료: ...
PnlStage.ChangeFinalMusic: 곡 이름=..., 음악 클립=..., 아티스트 이름=...
PnlPreparation.GameStart: 곡 이름=..., 음악 클립=..., 아티스트 이름=..., 레벨 디자이너=..., 실제 이름=...
Il2Cpp.Boss.InitBossObject: 변경 적용 -> name=..., scene=...
```

노트가 로그에는 추가됐는데 화면에 안 보이면 UID와 프리팹명이 실제 리소스에 있는지 먼저 봅니다. 보스가 안 나오면 보스 액션 `in`이 들어갔는지, 그리고 `Boss.InitBossObject` 변경 로그가 찍혔는지 봅니다. 곡 메타데이터가 일부만 바뀌면 해당 화면의 텍스트가 다른 컴포넌트나 마스크 아래에 있을 가능성이 큽니다.
