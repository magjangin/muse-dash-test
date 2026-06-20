# Muse Dash 실험 모드 전체 가이드

이 저장소는 Muse Dash의 런타임 데이터를 후킹해서 노트, 보스, 곡 메타데이터를 빠르게 실험하기 위한 모드입니다. 지금 구조는 “원본 데이터를 직접 대량 수정”하는 방식이 아니라, 게임이 런타임에 만든 객체를 복사하거나 UI 컴포넌트를 찾아서 원하는 값으로 덮어쓰는 방식입니다.

## 프로젝트 구조

```text
Muse Dash/
├── Mods/
│   └── muse dash custom chart.dll  # 빌드 완료된 모드 DLL
└── hwa/                            # 커스텀 리소스 루트 디렉터리
```

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

핵심 코드는 `muse dash test/Patches` 폴더에 분류되어 있습니다.

| 파일 | 역할 |
| --- | --- |
| `Database/Stage/DBStageInfoPatch.cs` | 원본 차트의 첫 플레이 노트를 복사해서 원하는 노트로 바꾼 뒤 주입 |
| `Battle/Mechanics/BossPatch.cs` | `Boss.InitBossObject`를 후킹해서 실제 화면에 나오는 보스 프리팹 변경 |
| `UI/Stage/Selection/PnlStagePatch.cs` | 곡 선택 화면의 곡 제목, 아티스트, 레벨 디자이너 UI 텍스트 실험 및 곡 정보 로그 |
| `UI/Stage/Preparation/PnlPreparationPatch.cs` | 준비 패널의 `Awake`, `GameStart`, `OnBattleStart`에서 곡 정보 실험 적용 |
| `Scene/GameMusicScenePatch.cs` | 음악 씬 로드 로그 확인 |
| `Battle/UI/StageBattleComponentPatch.cs` | 런타임 음악 데이터/노트 데이터 덤프 |
| `UI/Music/PnlMusicTagPatch.cs` | UI/DB 관련 보조 및 태그 정렬 후킹 |
| `MainMod.cs` | MelonLoader 진입점 |

각 파일의 주요 클래스와 메서드 단위 설명은 [코드 파일별 레퍼런스](CODE_REFERENCE.md)에 따로 정리했습니다.

## 빠른 수정 위치

노트 실험은 `Database/Stage/DBStageInfoPatch.cs`의 `ExperimentNotes` 배열을 수정합니다.

```csharp
private static readonly ExperimentNoteSpec[] ExperimentNotes =
{
    new ExperimentNoteSpec { Label = "지상 일반 노트", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0, Speed = 5 },
};
```

곡 제목/아티스트/레벨 디자이너 실험은 `UI/Stage/Selection/PnlStagePatch.cs` 상단의 상수를 수정합니다.

```csharp
private const bool EnableSongTitleExperiment = true;
private const string ExperimentTitle = "화영왕";
private const string ExperimentArtist = "화영왕";
private const string ExperimentLevelDesignerLabel = "레벨 디자이너";
private const string ExperimentLevelDesignerName = "화영왕";
```

실제 보스 프리팹은 `Battle/Mechanics/BossPatch.cs`의 `BossRewriteRules` 배열에서 바꿉니다.

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

- `Database/Stage/DBStageInfoPatch.cs`: 보스 액션 트리거 노트 생성
- `Battle/Mechanics/BossPatch.cs`: 실제 화면에 나오는 보스 프리팹 변경

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
## ALL PERFECT (AP) 배너 커스텀 및 연출 제어

곡 플레이 완료 직후, 결과 카드(`PnlVictory`)가 뜨기 전에 화면을 장식하는 일시적인 **"FULL COMBO!"** 연출 영역을 가로채어, 판정 결과에 따라 찬란한 황금빛의 **"ALL PERFECT!"** 전용 텍스트 배너로 교체 적용하는 기믹입니다.

### 🌟 작동 메커니즘 및 특징

1. **실시간 판정 필터링**:
   * 인게임 플레이 도중 노트를 처리할 때 실시간으로 호출되는 `TaskStageTarget.AddScore` 메서드를 활용하여, 활성화된 판정 레코드(`TaskStageTarget`) 인스턴스를 무스레드 차단 방식으로 정적 캐시에 확보합니다.
   * 결과 화면 진입점(`PnlVictory2dManager.OnShowVictory`)이 발동될 때, 캐싱된 인스턴스로부터 **Great수 = 0, Miss수 = 0, FullCombo여부 = True (정밀 정확도 100%)** 조건을 엄격하게 검증합니다.

2. **HUD 메인 시그니처 폰트 동적 추출 (Dynamic Font Caching)**:
   * 뮤즈 대시의 가장 아름다운 비주얼 아이덴티티 중 하나인 만화풍 서체(`LuckiestGuy-Regular`)는 단순 설정 필드나 static 필드로 노출되지 않습니다.
   * 이를 해결하기 위해 플레이 중인 배틀 HUD 컴포넌트(`PnlBattle.instance.currentComps.scoreValue`)를 런타임 추적하여, 해당 스코어 텍스트 오브젝트가 바라보고 있는 프리미엄 시그니처 폰트 리소스를 메모리 상에서 안전하게 가로채어(`VictoryDataCache.PremiumFont`) 결과 화면 배너 구조로 정밀 이식합니다.

3. **고해상도 커스텀 텍스트 및 아웃라인 스타일링**:
   * 오리지널 풀콤보 배너는 `"F-U-L-L C-O-M-B-O"` 라는 낱개 이미지 글자 오브젝트 11개(`ImgF`, `ImgU`, `ImgL` 등)로 구성되어 있습니다.
   * 올 퍼펙트 달성이 확인되면 이 11개의 오리지널 이미지 오브젝트들을 일괄 `SetActive(false)` 처리하여 시야에서 지우고, 동일 트랜스폼 하위에 `"CustomAPText"` 라는 가상의 Unity GameObject를 동적 생성합니다.
   * 여기에 아래와 같이 네이티브와 혼동할 정도의 정교한 하이퀄리티 UI 스타일러를 적용합니다:
     * **텍스트 문구**: `"ALL PERFECT !"` (폰트 크기 `110`, 정중앙 정렬)
     * **조화로운 색상**: 뮤즈 대시 고유의 하이라이트 노란색과 매칭되는 찬란한 골드 옐로우 (`Color(1f, 0.85f, 0f)`)
     * **3D 입체 섀도우 효과**: `Shadow` 컴포넌트를 동적으로 부착해 우하단 6px 공간으로 불투명도 80%의 진한 그림자 투영 (`new Vector2(6f, -6f)`)
     * **볼륨감 있는 외곽선**: `Outline` 컴포넌트를 부착하여 4px 두께의 불투명한 칠흑색 테두리를 둘러 팝아트풍의 볼드한 서체 느낌을 극대화 (`new Vector2(4f, -4f)`)

4. **투명한 폴백 및 리셋 구조**:
   * 올 퍼펙트 달성에 실패(Great가 1개 이상 발생했거나 Miss가 존재)하여 일반 풀콤보 상태이거나 클리어 실패 상태일 때는 기존에 숨겨두었던 오리지널 이미지 알파벳들을 일괄 다시 활성화(`SetActive(true)`)하고 커스텀 AP 텍스트를 숨겨서 게임의 원본 로직 및 비주얼 흐름을 원천 왜곡하지 않도록 안정성을 유지합니다.

## 커스텀 태그 아이콘 이미지 동적 주입

곡 선택 화면 좌측에 노출되는 커스텀 태그 카테고리(예: **"실험 모드"** 탭)의 아이콘 이미지를 게임 내장 리소스 대신 **모드 DLL 파일 자체에 바이너리로 완전히 포함(Embedded Resource)시켜 패킹**하고 런타임에 직접 로드하는 고성능 연출 기능입니다. 이 방식을 통해 추가 파일 관리 없이 단 하나의 DLL 파일만으로 안전하고 깨끗하게 동작합니다.

### 🌟 작동 메커니즘 및 구조

1. **모드 프로젝트 내부 패킹 (Embedded Resource)**:
   * 고화질 커스텀 스프라이트 이미지(`tag_icon.png`)는 C# 프로젝트 폴더 내의 `Resources/` 디렉터리에 상주합니다.
   * [muse dash test.csproj](file:///h:/source/repos/muse%20dash%20test/muse%20dash%20test/muse%20dash%20test.csproj) 파일에 `<EmbeddedResource>` 태그를 설정하여 빌드 시 이미지 파일 바이너리를 `.dll` 내부의 리소스 파일 시스템에 직접 주입시켰습니다.
     * **임베디드 리소스 경로**: `muse_dash_test.Resources.tag_icon.png`

2. **화면 탭 UI 컴포넌트 감지 (`AlbumTagToggle_Init_Patch`)**:
   * 인게임의 모든 태그 카테고리 버튼(탭 셀)들이 인스턴스화되고 내부 설정이 초기화되는 `AlbumTagToggle.Init` 시점을 Harmony Postfix로 안정적으로 가로챕니다.
   * 해당 컴포넌트 인스턴스의 `tagInfo` 속성이 우리가 주입한 가상 태그 UID(`tag-muse-dash-test`)를 바라보고 있는지 타입 안전(Type-Safe)하게 실시간 비교 검증하여 개입 대상을 특정합니다.

3. **인메모리 바이너리 해독 및 텍스처 복원**:
   * 감지 성공 시 `ExecutingAssembly.GetManifestResourceStream` 메서드를 호출하여 DLL 내부에 패킹된 `tag_icon.png` 리소스 바이트 스트림을 메모리에 바로 적재합니다.
   * 이후 `UnityEngine.ImageConversion.LoadImage`를 활용해 메모리 상에서 해당 바이너리를 유니티 `Texture2D` 텍스처 객체로 고속 해독 및 캐싱합니다.

4. **UI 속성 다이렉트 오버라이딩**:
   * 유니티 계층 구조 탐색(Find)을 매번 수행하는 무거운 방식 대신, `AlbumTagToggle` 내부의 인게임 컴포넌트 노출 속성인 `m_IconImg`(RawImage) 필드에 직접 접근하여 디코딩된 커스텀 `Texture2D`를 다이렉트로 할당(`__instance.m_IconImg.texture = customTexture`)하여 성공시킵니다.

---


## 빌드와 반영

빌드는 저장소 루트에서 `build.bat`를 실행하거나 직접 닷넷 빌드를 수행합니다.

```powershell
# 개발용 디버그 빌드
.\build.bat

# 배포용 릴리즈 빌드
.\build.bat release
```

빌드 결과 DLL:

```text
muse dash test\bin\Debug\net6.0\muse dash custom chart.dll
```

게임에 반영할 위치:

```text
H:\steam\steamapps\common\Muse Dash\Mods\muse dash custom chart.dll
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
