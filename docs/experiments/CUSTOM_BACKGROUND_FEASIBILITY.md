# 커스텀 배경(테마) 가능성 조사: scene_10 기준 (조사만, 2026-11-12)

노트·캐릭터 스킨 다음 단계로, 전투 배경을 사용자 그림으로 바꿀 수 있는지 Cpp2IL ISIL과 게임 번들(UnityPy)로 확인한 기록입니다. 대상은 **scene_10(크립톤/보컬로이드 무대)**이고, 비교용으로 구형 씬 scene_05를 짧게 덧붙였습니다. 코드는 아직 없습니다.

- ISIL: `Cpp2IL.exe --game-path "H:/muse dash hwa" --output-as isil` (Muse Dash 6.7.0)
- 번들: `MuseDash_Data/StreamingAssets/aa/StandaloneWindows64/gamescene_10_assets_all_*.bundle` (Addressables)

## 1. 배경을 불러오는 경로 (ISIL, 모든 씬 공통)

```
GameMusicScene.LoadScene(sceneName)
 ├─ SceneFestival(sceneName)                 ← 크리스마스 이름 치환 (ChristmasFestivalPatch)
 ├─ InitSceneObject(name, isActive=true)     ← 처음 배경
 ├─ Boss.SetBoss / SetGameBrightnessColor / PreLoadEnemy / PreLoadExtraObjs
 └─ 곡 중간 전환 목록마다: String.Format("scene_{0:00}") → SceneFestival → InitSceneObject(name, false)

GameMusicScene.InitSceneObject(name, isActive)
 ├─ TouhouLogic.ReplaceBadAppleScene(name)   ← 게임 스스로 씬을 통째로 바꾸는 선례 (scene_touhou_black)
 ├─ StageBattleComponent.CreatePrefabAndCache(name)
 │    = 캐시 사전 조회 → 없으면 ResourcesManager.instance.LoadFromName<GameObject>(name) (Addressables) → Object.Instantiate
 ├─ "(Clone)" 제거, 부모 붙이기
 ├─ SceneSubControl 찾기 → SceneSubControl.Init() → SceneSubCtrls 사전에 등록
 └─ SetActive(isActive)

SceneChangeController.ChangeScene(sceneInfo)   ← 곡 중간 씬 전환 노트(0004yy)
 └─ 이름 "scene_NN"(+ "_christmas")으로 이미 만든 오브젝트를 찾아 SetActive, Boss.SceneBossChange
SceneChangeController.SceneAnimationReset → Animator.Rebind + SceneSubControl.ResetScrollProgress
```

**모든 배경 오브젝트(처음 씬 + 곡 중간 전환 씬)는 `InitSceneObject` 한 곳에서 만들어집니다.** 여기 Postfix 하나로 전부 손댈 수 있고, `ChangeScene`은 이미 만든 오브젝트를 켜고 끄기만 합니다.

`SceneSubControl.Init`은 씬 아래의 `Animator`·`SkeletonAnimation`·`ParticleSystem`·`DOTweenAnimation`·`SceneScrollableControl`을 모아 일시정지·"배경 정지" 설정(`SetFreeze`)·보스 등장 이벤트(`Battle/OnBossIn`/`OnBossOut`)에 씁니다. 원래 자리에 그림만 바꿔 넣으면 이 처리가 그대로 따라옵니다.

## 2. scene_10의 특이점: 빈 껍데기 + 변형 2개

번들 안의 `scene_10` 프리팹은 **자식이 없고 `CryptonSceneHandler`만 붙은 껍데기**입니다. 실제 무대는 두 변형에 있습니다.

| 프리팹 | 고르는 조건 |
| --- | --- |
| `scene_10_miku` | 기본 |
| `scene_10_rin_len` | `DBMusicTagDefine.IsRinLenEggSong(uid)`가 참 (선택 곡 uid가 린·렌 이스터에그 목록에 있음) |

`CryptonSceneHandler.Awake`가 위 조건으로 한 변형을 골라 `InstantiateSceneObject`로 껍데기 아래(`localPosition = 0`)에 복제합니다. `Awake`는 `Instantiate` 안에서 돌므로 **`InitSceneObject` Postfix 시점에는 변형이 이미 자식으로 붙어 있습니다.** 커스텀 곡은 숙주 곡 uid로 판정되므로 보통 `miku`이고, 고정하려면 `IsRinLenEggSong` Prefix로 정할 수 있습니다.

## 3. scene_10_miku 구조

```
scene_10_miku (SceneSubControl, SceneScrollableControl[m_DefaultStepPerSecond=8], ScrollableGear, OffsetGroup, CryptonEggControl)
└─ ScrollableGear
   ├─ Near            야광봉(LightStick, LightStick2) — 각각 Scrollable
   ├─ Floor           s10_track, s10_road_dark, s10_track_line, s10_shadow_white, s10_speaker_box_small
   ├─ Mid1            보컬로이드(s10_miku/kaito/meiko/rin/len), 스포트라이트, 대형·중형 스피커
   ├─ Mid2            무대(s10_stage, s10_stage_mark, s10_stage_miku_1/2), 깃발, 큐브, 신시사이저, 음표, 기타 (SpriteMask 있음)
   ├─ Mid2_lightStick 뒤쪽 야광봉
   ├─ Mid3            별 s10_star × 6
   ├─ Far             AudioClips: GameAudioVisualization, 막대 128개
   └─ Bg              s10_bg 한 장 (스크롤 없음)
└─ par_star (ParticleSystem)
```

배경 그림은 **전부 `SpriteRenderer`**입니다. Spine(`SkeletonAnimation`)은 노트·보스에만 있습니다.

> **검증 (2026-11-12):** `gamescene_10_assets_all`과 `gamescene_10_fool_assets_all` 번들의 Spine 컴포넌트 전체를 붙은 오브젝트의 전체 경로로 뽑았습니다. 루트가 전부 노트 프리팹(`10xxyy`)이거나 보스(`1001`)였고, **`scene_10*` 아래에는 0개**였습니다. `CryptonEggControl`/`SceneEggAbstractController`도 런타임에 다른 에셋을 불러오지 않습니다(ISIL에 `LoadFromName`·`Instantiate` 없음). 보컬로이드 캐릭터(`s10_miku`·`s10_kaito`·`s10_rin` 등)는 한 장짜리 스프라이트이고, 부모 `Animator`(`Miku`, `Kaito`, `kagamine`)가 위치·크기만 흔듭니다. 같은 번들 사본(`H:\유튜브 곡 모음\…\gamescene\`, SHA1이 게임 폴더와 같음)에서 스프라이트를 PNG로 뽑아 확인했습니다. `s10_bg`는 16×16짜리 단색 조각을 늘려 쓰는 그림이라, 바꿀 때는 크기(스케일)를 함께 맞춰야 합니다. `rin_len` 변형은 같은 뼈대에 스프라이트만 `_rin`·`_rin_len` 판(`s10_track_rin_len`, `s10_stage_rin`, `s10_star_rin`, 야광봉 Y/O 색 등)으로 바꾼 구성입니다.

### 3-1. 스크롤: 신형 `Scrollable` 방식 (구형 Animator 방식이 아님)

`SceneScrollableControl.Update` → `OnUpdate(tick)`이 레이어마다 다음 값만큼 이동량을 늘립니다(ISIL).

```
이동량 += speedMul × m_DefaultStepPerSecond(8) × deltaTime × Scrollable.m_SpeedFactor
진행도 = Repeat(이동량 / m_ScrollLength)   → OffsetGroup이 m_Angle(-180 = 왼쪽) 방향으로 위치를 옮김
```

`m_IsGroup = 1`인 레이어는 `Scrollable.Start`에서 자식들을 각자 `m_ScrollLength` 안에서 돌도록 배치합니다(`m_Anchor`가 기준점). 아래 값은 번들에 직렬화된 값입니다(miku 변형).

| 레이어 | m_ScrollLength (반복 간격) | m_SpeedFactor | 속도 (유닛/초 = 8 × factor) |
| --- | --- | --- | --- |
| Near/LightStick | 15.4 | 0.8 | 6.4 |
| Near/LightStick2 | 15.4 | 0.7 | 5.6 |
| Floor | 12.8 | 0.65 | 5.2 |
| Mid1 | 73.2 | 0.4 | 3.2 |
| Mid2 / Mid2_lightStick / Mid3 | 54.9 | 0.3 | 2.4 |
| Far | 44.8 | 0.2 | 1.6 |
| Bg | (Scrollable 없음, 고정) | — | 0 |

이 값들은 직렬화 필드라 **런타임에 길이·속도를 바꿀 수 있습니다**(구형 씬은 Animator 클립 안에 박혀 있어 못 바꿈).

### 3-2. 애니메이션이 스프라이트를 덮어쓰는지

scene_10 번들의 AnimationClip 20개를 모두 봤습니다. **스프라이트(PPtr) 커브가 하나도 없습니다.** 클립이 건드리는 것은 Transform 위치·회전·크기, `SpriteRenderer` 색, GameObject 켜기/끄기뿐입니다. 그래서 스프라이트를 바꿔도 `Animator.Rebind`(씬 리셋)나 재생 중 애니메이션이 원래 그림으로 되돌리지 않습니다. 색을 바꾸는 클립(`lightstick_red_in/out`, `MikuScreen`, `RinLenScreen`)은 새 그림에도 그대로 색을 입힙니다.

### 3-3. 오디오 시각화

`Far/AudioClips`의 `GameAudioVisualization`은 `FixedUpdate`에서 오디오 스펙트럼(`GetSpectrumAudiosource` → `MakeFrequencyBands` → `BandBuffer` → `CreateAudioBands`)으로 막대 128개를 움직입니다. 커스텀 곡 오디오에도 반응하는지는 게임이 듣는 AudioSource에 달렸으므로 실기에서 확인해야 합니다.

## 4. 구현 경로 후보 (scene_10)

### A. 스프라이트 교체 (권장, 위험 낮음)

`GameMusicScene.InitSceneObject` Postfix에서 `scene_10` 아래 변형을 찾아 `GetComponentsInChildren<SpriteRenderer>(true)`를 돌며 **원본 스프라이트 이름 → PNG로 만든 `Sprite`**로 바꿉니다.

- 폴더는 노트 스킨과 같은 규칙: `skin test/scene/scene_10/{원본 스프라이트 이름}.png` (변형별로 다르면 `scene_10_miku/`·`scene_10_rin_len/`). 없는 파일은 원본 유지.
- `Sprite.Create`의 pivot은 원본 스프라이트의 `pivot / rect.size`, `pixelsPerUnit`은 원본과 같게 두면 위치·크기가 그대로입니다. 그림 크기를 바꾸고 싶으면 PPU로 맞춥니다.
- 스크롤 이음매: **Floor처럼 한 장이 반복되는 레이어는 그림 폭이 반복 간격(12.8유닛)과 맞아야** 합니다. 무대·캐릭터처럼 띄엄띄엄 놓인 장식은 크기 제약이 없습니다.
- 원본 PNG와 pivot·PPU 표를 뽑아 주는 덤프 스크립트가 있으면 사용자가 그리기 쉽습니다(UnityPy로 가능).
- 대상 곡 판정은 노트 스킨처럼 곡 옵션(`info.txt`)으로 켭니다. 공식 곡에 번지지 않게 프리팹·아틀라스 원본은 건드리지 않고 복제된 인스턴스만 바꿉니다.

### B. 레이어 추가·값 조정 (중간)

- 원본 장식을 끄고(`SetActive(false)`) 같은 레이어(`Mid1` 등) 아래에 새 `GameObject + SpriteRenderer`를 붙이면 그 레이어의 스크롤을 그대로 탑니다.
- `Scrollable.m_ScrollLength`·`m_SpeedFactor`를 바꿔 반복 간격·시차 속도를 조정할 수 있습니다. 단 `m_IsGroup` 레이어는 `Start`에서 자식 배치를 계산하므로 **`Start` 전(같은 프레임의 Postfix)에 바꿔야** 합니다. 다른 시점이면 `ResetScrollProgress`/재배치 필요 여부를 확인해야 합니다.

### C. 완전 커스텀 씬 (높음)

`CryptonSceneHandler`가 이미 "껍데기 + 런타임 복제" 구조이므로, 같은 방식으로 껍데기 아래에 직접 만든 계층을 붙이는 것이 가장 자연스럽습니다. 필요한 것: `SceneSubControl`(씬 번호는 이름의 숫자에서 읽음), `SceneScrollableControl` + `ScrollableGear` + 레이어별 `Scrollable`/`OffsetGroup`(게임 컴포넌트를 `AddComponent`해 필드 설정), `HitPoints_10`·보스 위치는 숙주 것 사용. A·B가 실기 확인된 뒤 검토합니다.

## 5. 참고: 구형 씬(scene_05)과의 차이

scene_05는 레이어 부모의 `Animator`가 `localPosition.x`를 0 → −(반복 간격)으로 반복하는 구형 방식입니다(예: 바닥 15.30유닛/3.017초, 배경 6.66유닛/7초). 스프라이트 교체(A)는 똑같이 되지만, 반복 간격·속도가 클립에 박혀 있어 B의 값 조정은 안 됩니다. scene_10은 신형 `Scrollable` 방식이라 커스텀 배경의 틀로 더 유리합니다.

## 6. 로비(곡 선택) 배경 테마는 별개

메뉴의 배경 테마(`StageBackgroundDataManager`, `PnlStageBackground*`)는 다른 경로입니다. `LobbyBackgroundLoader.SwitchAndLoadBackground(bgId)` → `LoadBackground(StageBackgroundData)` → `OnLoadBgCompleted(showBg)`(현지화 적용·`SkeletonAnimation.ForceUpdate`·`AdaptScreen`). `OnLoadBgCompleted`에서 `showBg` 안의 그림을 바꾸는 방식이 같은 원리로 가능해 보이나 프리팹 구조는 보지 않았습니다.

## 7. 남은 확인 (실기)

1. scene_10 숙주 곡에서 `InitSceneObject` Postfix로 `s10_bg`(고정)와 `s10_track`(Floor, 스크롤) 하나씩 교체 → 이음매·일시정지·배경 정지 설정·재시작 후 유지 확인.
2. 커스텀 곡에서 `scene_10_miku`/`rin_len` 중 어느 변형이 뜨는지 로그로 확인.
3. `GameAudioVisualization`이 커스텀 곡 오디오에 반응하는지.
4. 곡 중간 씬 전환(`0004yy`)으로 들어오는 scene_10도 같은 Postfix로 바뀌는지.
