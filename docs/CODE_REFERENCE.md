# 코드 파일별 레퍼런스

이 문서는 `muse dash test` 프로젝트의 C# 파일을 읽을 때 길잡이로 쓰기 위한 파일별 설명입니다. 코드 안에는 실험용 주석만 짧게 두고, 자세한 설명은 이 문서에 모았습니다.

## 전체 흐름

모드는 MelonLoader가 `MainMod`를 로드하면서 시작됩니다. 실제 실험 로직은 대부분 Harmony 패치로 게임 메서드 호출 전후에 끼어드는 방식입니다.

큰 흐름은 아래와 같습니다.

1. 게임이 곡/스테이지/전투 데이터를 로드합니다.
2. `DBStageInfo.SetRuntimeMusicData` 후킹에서 런타임 노트 리스트를 실험용 리스트로 바꿉니다.
3. 곡 선택/준비 UI가 열릴 때 `PnlStage`와 `PnlPreparation` 패치가 제목, 아티스트, 레벨 디자이너 텍스트를 읽거나 바꿉니다.
4. 보스가 초기화될 때 `Boss.InitBossObject` 패치가 실제 보스 프리팹 이름과 씬 번호를 바꿉니다.
5. 전투 컴포넌트와 DB 패치들은 로그를 통해 실제 런타임 객체 구조를 확인하게 해 줍니다.

## `main.cs`

`main.cs`는 MelonLoader 모드의 진입점입니다.

| 멤버 | 역할 |
| --- | --- |
| `MelonInfo` | 모드 이름, 버전, 작성자를 MelonLoader에 등록합니다. |
| `MelonGame` | 대상 게임을 `PeroPeroGames / MuseDash`로 지정합니다. |
| `MainMod.OnApplicationStart` | 모드 로드 확인 로그를 남깁니다. |
| `MainMod.OnSceneWasLoaded` | Unity 씬이 바뀔 때 씬 이름과 빌드 인덱스를 기록합니다. |
| `MainMod.OnApplicationQuit` | 모드 종료 로그를 남깁니다. |

`OnApplicationStart`는 현재 SDK에서 obsolete 경고가 뜰 수 있습니다. 빌드는 되지만, 나중에 MelonLoader 버전에 맞춰 새 생명주기 메서드로 옮길 수 있습니다.

## `Patches/DBStageInfoPatch.cs`

노트 실험의 중심 파일입니다. `DBStageInfo.SetRuntimeMusicData`가 원본 곡 데이터를 런타임용 `MusicData` 리스트로 만든 뒤, 이 패치가 그 리스트 내용을 실험용 노트로 바꿉니다.

### 핵심 상수

| 이름 | 의미 |
| --- | --- |
| `BaseDt` | 일반 노트의 기본 표시 선행 시간입니다. `showTick = tick - dt` 계산에 쓰입니다. |
| `LongMiddleStep` | 롱노트 middle 조각을 몇 tick 간격으로 만들지 결정합니다. |
| `SourceNoteIndex` | 실험 노트 템플릿으로 복사할 원본 노트 인덱스입니다. |
| `DebugExperimentNotes` | 생성 과정 상세 로그를 켤지 정합니다. |

### `ExperimentNotes`

실험자가 가장 많이 수정하는 배열입니다. 배열 한 줄이 “실험 노트 1종”입니다. `Count`가 2 이상이면 같은 설정의 노트를 반복 생성합니다.

예:

```csharp
new ExperimentNoteSpec
{
    Label = "공중 일반",
    Uid = "051004",
    NoteType = 1,
    Pathway = 1,
    StartTick = 15.0,
    Count = 4,
    Interval = 0.5
},
```

### 주요 메서드

| 메서드 | 역할 |
| --- | --- |
| `Postfix` | `SetRuntimeMusicData` 완료 후 실험 차트 적용을 시작합니다. |
| `ApplyExperimentChart` | 원본 리스트를 복사한 뒤 `[0]` 앵커와 실험 노트만 남기도록 재구성합니다. |
| `AddExperimentNotes` | `ExperimentNoteSpec` 하나를 실제 `MusicData` 하나 이상으로 확장합니다. |
| `AddMovedNote` | 일반 단일 노트를 추가합니다. |
| `AddMulNote` | 샌드백/type 8처럼 길이를 가진 단일 노트를 추가합니다. |
| `AddLongNote` | 롱노트 start, middle, end 시퀀스를 추가합니다. |
| `ApplyNoteSpec` | UID, 타입, 레인, 프리팹, 효과음, 보스 액션, `Scene`, `IbmsId` 값을 노트에 반영합니다. |
| `BuildPrefabName` | UID와 레인으로 `{uid}_{road/air}_{nor/up/down}_1` 형태의 프리팹명을 추정합니다. |
| `IsBossProjectileUid` | `xx=06/07/08`, `yy=01/04` 보스 발사체 UID인지 확인합니다. |
| `MoveNote` | `objId`, `tick`, `dt`, `showTick`, `configData.time`, `configData.length`를 갱신합니다. |
| `GetEffectiveDt` | 직접 지정 Dt, 보스 발사체 기본 0.7, 보스 트리거 0, UID 패턴 순서로 dt를 결정합니다. 보스 발사체는 `BossAction`이 비어 있어도 0.7을 씁니다. |
| `CloneMusicData` | 원본 `MusicData`와 내부 `noteData`, `configData`를 복제합니다. |
| `ResetRuntimeFlags` | 원본에서 복사된 더블/롱노트 상태를 새 노트에 남기지 않도록 초기화합니다. `0004`/type 9 씬 전환 노트는 `doubleIdx=-1`로 보정합니다. |

### 안전하게 읽는 방법

이 파일을 볼 때는 아래 순서로 읽는 편이 쉽습니다.

1. `ExperimentNoteSpec` 필드가 무엇인지 봅니다.
2. `ExperimentNotes`에 현재 어떤 실험이 들어 있는지 봅니다.
3. `ApplyExperimentChart`에서 원본 리스트가 어떻게 바뀌는지 봅니다.
4. 일반 노트면 `AddMovedNote`, 롱노트면 `AddLongNote`, 샌드백이면 `AddMulNote`를 봅니다.
5. 마지막으로 `ApplyNoteSpec`와 `MoveNote`에서 실제 값이 어떻게 바뀌는지 확인합니다.

## `Patches/BossPatch.cs`

보스 관련 런타임 호출을 관찰하고, 실제 보스 프리팹을 바꾸는 파일입니다.

| 패치 클래스 | 후킹 대상 | 역할 |
| --- | --- | --- |
| `Boss_Play_Patch` | `Il2Cpp.Boss.Play` | 보스 액션 키가 재생되는지 확인합니다. |
| `Boss_SetBoss_Patch` | `Il2Cpp.Boss.SetBoss` | 보스 설정 시점을 확인합니다. |
| `Boss_InitBossObject_Patch` | `Il2Cpp.Boss.InitBossObject` | 실제 보스 프리팹 `name`과 `scene`을 바꿉니다. |
| `Boss_SceneBossChange_Patch` | `Il2Cpp.Boss.SceneBossChange` | 보스 씬 전환 `idx`를 기록하고, `SceneBossChangeRules`로 바꿉니다. |

`BossRewriteRules`는 보스 변경 규칙입니다.

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

조건 필드는 아래처럼 동작합니다.

| 필드 | 의미 |
| --- | --- |
| `OrigName = "*"` | 원본 보스 이름을 가리지 않고 모두 매칭합니다. |
| `OrigScene = null` | 원본 씬 번호를 가리지 않습니다. |
| `OrigIsLast = null` | 마지막 보스 여부를 가리지 않습니다. |
| `NewName` | 바꿀 보스 프리팹 이름입니다. |
| `NewScene` | 바꿀 보스 씬 번호입니다. |

보스 액션 트리거 노트는 `DBStageInfoPatch.cs`에서 만들고, 실제 보스 모델은 이 파일에서 바꾼다는 점을 분리해서 보면 됩니다.

`SceneBossChangeRules`는 보스 씬 전환 idx 변경 규칙입니다.

```csharp
new SceneBossChangeRule { OrigIdx = null, NewIdx = 7 },
```

`OrigIdx = null`은 원본 idx를 가리지 않고 모두 매칭한다는 뜻입니다. 특정 idx만 바꾸려면 `OrigIdx = 5`처럼 숫자를 넣습니다.

## `Patches/PnlStagePatch.cs`

곡 선택 화면의 곡 정보 추출과 UI 텍스트 실험을 담당합니다. 파일이 긴 이유는 게임 UI가 화면마다 다른 멤버명, 자식 오브젝트명, 텍스트 컴포넌트를 쓰기 때문입니다.

### 패치 클래스

| 패치 클래스 | 후킹 대상 | 역할 |
| --- | --- | --- |
| `PnlStage_Start_Patch` | `PnlStage.Start` | 곡 선택 패널이 열린 직후 곡 정보를 기록합니다. 지연 로그도 한 번 더 찍습니다. |
| `PnlStage_ChangeMusic_Patch` | `PnlStage.ChangeMusic(int)` | 곡 선택이 바뀐 직후 정보를 기록합니다. |
| `PnlStage_ChangeFinalMusic_Patch` | `PnlStage.ChangeFinalMusic(int)` | 최종 선택 곡 정보가 확정된 뒤 정보를 기록합니다. |

### 실험 상수

| 이름 | 의미 |
| --- | --- |
| `EnableSongTitleExperiment` | 곡 제목/아티스트/디자이너 텍스트 교체를 켤지 정합니다. |
| `ExperimentTitle` | 화면에 쓸 곡 제목입니다. |
| `ExperimentArtist` | 화면에 쓸 아티스트 이름입니다. |
| `ExperimentLevelDesignerLabel` | “레벨 디자이너” 같은 라벨 텍스트입니다. |
| `ExperimentLevelDesignerName` | 실제 레벨 디자이너 이름으로 표시할 값입니다. |
| `ApplySongTitleExperimentGlobally` | 패널 루트뿐 아니라 씬 전체 텍스트까지 더 넓게 바꿀지 정합니다. |

### `PnlMusicUtils`

`PnlMusicUtils`는 반사와 Unity 컴포넌트 검색을 섞어서 정보를 찾습니다.

| 메서드 | 역할 |
| --- | --- |
| `LogMusicInfo` | 패널에서 곡 정보를 추출하고 로그로 출력합니다. |
| `LogPreparationMusicInfo` | 준비 패널 정보를 읽고, 부족하면 살아 있는 `PnlStage`에서 보완합니다. |
| `ApplySongTitleExperiment` | 알려진 멤버명과 자식 텍스트 오브젝트에 실험 문자열을 씁니다. |
| `SetMemberText` | 필드/프로퍼티 이름으로 텍스트 컴포넌트를 찾아 값을 씁니다. |
| `SetChildTextByNames` | 자식 오브젝트 이름 후보를 기준으로 텍스트를 바꿉니다. |
| `ExtractMusicInfo` | 제목, 아티스트, 디자이너, 오디오 클립을 여러 후보에서 추출합니다. |
| `FindAudioClipName` | 패널 멤버에서 음악 클립처럼 보이는 값을 찾습니다. |
| `FindSceneMusicAudioClipName` | 씬의 `AudioSource`에서 재생 중인 음악 클립을 찾습니다. |
| `FillByNamedMembers` | 직접 후보로 못 찾은 값을 필드/프로퍼티 이름 기반으로 보완합니다. |

곡 제목은 대체로 `PnlStage`에서 잘 잡히고, 준비 화면은 `PnlPreparation`과 조합해서 보는 편이 좋습니다.

## `Patches/PnlPreparationPatch.cs`

준비 패널 쪽 곡 정보 로그와 텍스트 실험 적용을 담당합니다.

| 패치 클래스 | 후킹 대상 | 역할 |
| --- | --- | --- |
| `PnlPreparation_Awake_Patch` | `PnlPreparation.Awake` | 준비 패널이 만들어진 직후 곡 정보를 읽습니다. |
| `PnlPreparation_GameStart_Patch` | `PnlPreparation.GameStart` | 게임 시작 직후 준비 패널의 곡 정보를 읽습니다. |
| `PnlPreparation_OnBattleStart_Patch` | `PnlPreparation.OnBattleStart` | 전투 시작 콜백 직후 곡 정보를 읽습니다. |

이 파일 자체는 짧고, 실제 추출 로직은 `PnlStagePatch.cs`의 `PnlMusicUtils`를 재사용합니다.

## `Patches/GameMusicScenePatch.cs`

음악 씬 로드 흐름을 확인하는 작은 로그 패치입니다.

| 패치 클래스 | 후킹 대상 | 역할 |
| --- | --- | --- |
| `GameMusicScene_LoadScene_Patch` | `GameMusicScene.LoadScene` | 로드되는 음악 씬 이름을 기록하고, `LoadSceneRewriteRules`로 바꿉니다. |

현재 기본값은 모든 씬을 `scene_07`로 바꾸는 설정입니다.

```csharp
new LoadSceneRule { OrigSceneName = "*", NewSceneName = "scene_07" },
```

`OrigSceneName = "*"`는 원본 씬 이름을 가리지 않고 모두 매칭한다는 뜻입니다. 특정 씬만 바꾸려면 `OrigSceneName = "scene_05"`처럼 씁니다. 조작을 잠시 끄려면 `EnableLoadSceneRewrite`를 `false`로 바꿉니다.

## `Patches/StageBattleComponentPatch.cs`

전투 컴포넌트가 들고 있는 런타임 음악 데이터 구조를 덤프하는 조사용 파일입니다.

| 패치 클래스 | 후킹 대상 | 역할 |
| --- | --- | --- |
| `StageBattleComponent_LoadMusicData_Patch` | `StageBattleComponent.LoadMusicData` | 전투 컴포넌트의 공개 프로퍼티와 노트 리스트를 덤프합니다. |
| `StageBattleComponent_InitData_Patch` | `StageBattleComponent.InitData` | 전투 데이터 초기화 호출을 기록합니다. |

`DumpStageBattleComponentProperties`는 아래 리스트를 특별히 더 깊게 봅니다.

- `m_MusicTickData`
- `m_SortedMusicTickData`
- `m_TimeNodeOrders`

이 덤프는 로그가 매우 길어질 수 있습니다. 노트 구조를 파악할 때만 켜두고, 일반 실험 중에는 너무 많은 로그가 원인 파악을 방해할 수 있습니다.

## `Patches/UiAndDBPatches.cs`

전투 시작과 스테이지 정보 설정을 관찰하는 보조 패치입니다.

| 패치 클래스 | 후킹 대상 | 역할 |
| --- | --- | --- |
| `PnlBattle_GameStart_Patch` | `PnlBattle.GameStart` | 전투 패널의 게임 시작 호출 전후를 기록합니다. |
| `DBStageInfo_SetStageInfo_Patch` | `DBStageInfo.SetStageInfo` | `StageInfo` 객체의 필드/프로퍼티를 반사로 덤프합니다. |

`DumpStageInfo`는 타입을 정확히 몰라도 필드와 프로퍼티를 전부 읽어 보도록 만들어져 있습니다. `SafeVal`은 문자열, primitive, enumerable을 로그에 적당히 펼쳐서 보여줍니다.

## `Properties/AssemblyInfo.cs`

일반적인 .NET 어셈블리 메타데이터 파일입니다. 현재 프로젝트는 `.csproj`에서 `GenerateAssemblyInfo=false`를 사용하므로 이 파일이 어셈블리 정보를 직접 제공합니다.

실험 로직은 없습니다.

## 읽기 추천 순서

처음 코드를 보는 경우에는 아래 순서를 추천합니다.

1. `main.cs`
2. `DBStageInfoPatch.cs`
3. `NOTE_EXPERIMENTS.md`
4. `BossPatch.cs`
5. `BOSS_EXPERIMENTS.md`
6. `PnlStagePatch.cs`
7. `PnlPreparationPatch.cs`
8. `LOGGING_AND_TROUBLESHOOTING.md`
9. `StageBattleComponentPatch.cs`
10. `UiAndDBPatches.cs`

노트만 바꾸고 싶으면 `DBStageInfoPatch.cs`와 `NOTE_EXPERIMENTS.md`만 봐도 됩니다. 보스가 목적이면 `DBStageInfoPatch.cs`, `BossPatch.cs`, `BOSS_EXPERIMENTS.md`를 같이 봅니다. 곡 제목/아티스트/레벨 디자이너가 목적이면 `PnlStagePatch.cs`와 `PnlPreparationPatch.cs`를 보면 됩니다.
