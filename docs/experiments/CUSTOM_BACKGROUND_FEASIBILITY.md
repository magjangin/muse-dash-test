# 커스텀 배경(테마) 가능성 조사 (조사만, 2026-11-12)

노트·캐릭터 스킨 다음 단계로, 전투 배경(`scene_NN`)을 사용자 그림으로 바꿀 수 있는지 Cpp2IL ISIL과 게임 번들(UnityPy)로 확인한 기록입니다. 코드는 아직 없습니다.

- ISIL: `Cpp2IL.exe --game-path "H:/muse dash hwa" --output-as isil` (Muse Dash 6.7.0)
- 번들: `MuseDash_Data/StreamingAssets/aa/StandaloneWindows64/gamescene_05_assets_all_*.bundle` (Addressables)

## 1. 배경을 불러오는 경로 (ISIL)

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
 ├─ go.name에서 "(Clone)" 제거, 부모 붙이기
 ├─ GetComponent<SceneSubControl>() → SceneSubControl.Init() → SceneSubCtrls 사전에 등록
 └─ SetActive(isActive)  (프리팹이 없으면 "Scene … has no prefabs." 로그)

SceneChangeController.ChangeScene(sceneInfo)   ← 곡 중간 씬 전환 노트(0004yy)
 └─ "scene_" + "{0:00}" (+ "_christmas")로 이미 만든 오브젝트를 찾아 SetActive, Boss.SceneBossChange
SceneChangeController.SceneAnimationReset → Animator.Rebind + SceneSubControl.ResetScrollProgress
```

결론: **모든 배경 오브젝트(처음 씬 + 곡 중간 전환 씬)는 `InitSceneObject` 한 곳에서 만들어집니다.** 여기 Postfix 하나로 전부 손댈 수 있고, `ChangeScene`은 이미 만든 오브젝트를 켜고 끄기만 합니다.

`SceneSubControl.Init`/`UpdateAllData`는 씬 아래의 `Animator`, `SkeletonAnimation`, `ParticleSystem`, `DOTweenAnimation`, `SceneScrollableControl`을 `GetComponentsInChildren`으로 모아 일시정지(`SetPause`)·정지(`SetFreeze`, 설정의 "배경 정지")·보스 등장 이벤트(`Battle/OnBossIn`/`OnBossOut`)에 씁니다. 그래서 **바꿔 넣은 그림도 같은 컴포넌트 아래 두면 일시정지·배경 정지가 그대로 적용**됩니다. `Init`이 끝난 뒤 새로 만든 Animator는 이 목록에 들어가지 않습니다(직접 만든 스크롤러는 정지 처리를 따로 해야 함).

## 2. 배경 프리팹의 실제 구조 (scene_05)

```
scene_05 (SceneSubControl, DefaultEggControl)   pos (-6.67, -3.75)
├─ Mid
│  └─ Tree      Animator[s05_tree]          자식 SpriteRenderer s05_tree_2..5 (드문드문 배치)
├─ Near
│  ├─ Floor     Animator[s05_floor_a]       자식 SpriteRenderer s05_floor × 4  (간격 15.30)
│  └─ Water     Animator[s05_floor_c]       자식 SpriteRenderer s05_water × 10 (간격 2.50)
├─ Far
│  ├─ Mountain  Animator[s05_long_shot_1]   s05_mountain_1/3
│  ├─ FarTree   Animator[s05_long_shot_2]   s05_far_tree × 4 (간격 12.5)
│  └─ Bg        Animator[s05_long_shot_3]   s05_bg × 5 (간격 6.66)
└─ s05_scene_sfx (ParticleSystem)
```

- 그림은 전부 **`SpriteRenderer` + 스프라이트 아틀라스**(`SpriteAtlasTexture-scene_05-2048x2048`)입니다. 배경 레이어에는 Spine이 없습니다(번들의 `SkeletonAnimation` 150개는 노트·보스).
- **스크롤은 레이어 부모의 `Animator`가 `localPosition.x`를 0 → −(반복 간격)으로 무한 반복**하는 방식입니다(클립 스트림 디코드):

| 클립 | 레이어 | 반복 간격 (유닛) | 한 바퀴 (초) | 속도 (유닛/초) |
| --- | --- | --- | --- | --- |
| `s05_floor_a` | Near/Floor | 15.30 | 3.017 | 5.07 |
| `s05_floor_c` | Near/Water | 5.00 | 0.750 | 6.67 (y도 흔들림) |
| `s05_tree` | Mid/Tree | 31.85 | 7.867 | 4.05 |
| `s05_long_shot_1` | Far/Mountain | 15.30 | 5.583 | 2.74 |
| `s05_long_shot_2` | Far/FarTree | 12.50 | 10.0 | 1.25 |
| `s05_long_shot_3` | Far/Bg | 6.66 | 7.0 | 0.95 |

- 신형 씬은 Animator 대신 `SceneScrollableControl`(→ `ScrollableGear.m_Scrollables[]` → `Scrollable.m_ScrollLength / m_SpeedFactor / m_Angle`)로 스크롤합니다(`Init`의 "This Scene Have new SceneControl with Scrollable" 로그). 이쪽은 길이·속도가 직렬화 필드라 런타임에 바꿀 수 있습니다.
- `SetGameBrightnessColor`는 설정의 밝기 값으로 씬 위 어두운 덮개 `SpriteRenderer.color`만 바꿉니다. 배경 그림 자체는 건드리지 않습니다.

## 3. 구현 경로 후보

### A. 스프라이트 교체 (권장, 위험 낮음)

`GameMusicScene.InitSceneObject` Postfix에서 만든 씬을 찾아 레이어별 `SpriteRenderer.sprite`를 PNG로 만든 `Sprite`로 바꿉니다.

- 스크롤·시차(parallax)·일시정지·배경 정지·씬 전환·`SceneAnimationReset`이 전부 게임 것 그대로 동작합니다. 레이어 클립에 스프라이트(PPtr) 커브가 없어 `Animator.Rebind`가 교체를 되돌리지 않습니다(scene_05 기준, 다른 씬은 확인 필요).
- 조건: 그림 한 장의 폭이 **그 레이어의 반복 간격과 같아야** 이음매 없이 돕니다. `Sprite.Create(tex, rect, pivot, pixelsPerUnit = tex.width / 반복간격)`으로 맞추고, 원본 스프라이트의 pivot·정렬 순서(`sortingOrder`)는 그대로 둡니다.
- 한 레이어 아래에 같은 스프라이트 타일이 여러 개(Floor ×4 등)이므로, **원본 스프라이트 이름 → 새 스프라이트** 사전으로 바꾸면 타일 전부가 한 번에 바뀝니다. 띄엄띄엄 놓인 장식(Tree2..5, Mountain1/3)은 이름별로 따로 바꾸거나 숨깁니다.
- 숙주 씬마다 레이어 구성이 다르므로 사용자는 "어느 씬을 틀로 쓰는지"를 고르고, 그 씬의 스프라이트 이름에 맞춰 그림을 넣습니다. 노트 스킨(`skin test/note/{스켈레톤 이름}/`)과 같은 규칙으로 `skin test/scene/{scene_NN}/{원본 스프라이트 이름}.png`가 자연스럽습니다. 없는 파일은 원본 유지.
- 원본 그림을 뽑아 주는 덤프 도구(번들 → 스프라이트 PNG + 반복 간격 표)를 같이 두면 사용자가 크기를 맞추기 쉽습니다.

### B. 숙주 레이어 아래에 직접 만든 레이어 붙이기 (중간)

원본 스프라이트를 끄고, 레이어 부모(Animator가 움직이는 노드) 아래에 새 `GameObject + SpriteRenderer`를 붙입니다. 부모가 움직이므로 스크롤을 공짜로 얻습니다. 타일 수·간격을 자유롭게 정할 수 있지만 반복 간격은 여전히 숙주 클립이 정합니다.

### C. 완전 커스텀 씬 (높음)

`CreatePrefabAndCache`/`InitSceneObject`를 가로채 숙주 프리팹 대신 직접 만든 계층을 돌려줍니다. 필요한 것: `SceneSubControl` 부착(`Init`이 이름에서 `\d+`로 씬 번호를 읽음), 자체 스크롤러(런타임 AnimatorController 생성은 어려우므로 `Scrollable`/`ScrollableGear`/`SceneScrollableControl`을 붙여 필드 설정, 또는 ClassInjector로 만든 MonoBehaviour), `ChangeScene`이 이름 `scene_NN`으로 찾으므로 이름 유지, 보스 위치(`Boss.SceneBossChange`)·`HitPoints_NN`은 숙주 것 사용. 게임의 Bad Apple(`TouhouLogic.ReplaceBadAppleScene`)이 같은 방식의 선례입니다. A가 실기 확인된 뒤에 검토할 일입니다.

## 4. 로비(곡 선택) 배경 테마는 별개

메뉴의 배경 테마(`StageBackgroundDataManager`, `PnlStageBackground*`)는 전투 배경과 다른 경로입니다. `LobbyBackgroundLoader.SwitchAndLoadBackground(bgId)` → `LoadBackground(StageBackgroundData)`가 프리팹을 비동기로 불러와 `OnLoadBgCompleted(showBg)`에서 현지화 적용·`SkeletonAnimation.ForceUpdate`·화면 비율 맞춤(`AdaptScreen`)을 합니다. 커스텀 로비 배경은 `OnLoadBgCompleted` Prefix/Postfix에서 `showBg` 안의 그림을 바꾸는 방식이 같은 원리로 가능해 보입니다(로비 배경은 Spine을 쓰는 것이 있어 캐릭터 스킨 주입기를 재사용할 수 있음). 이번 조사 범위 밖이라 프리팹 구조는 보지 않았습니다.

## 5. 남은 확인 (실기)

1. `InitSceneObject` Postfix에서 `scene_05`의 `Floor` 타일 스프라이트 하나만 교체 → 스크롤 이음매·일시정지·배경 정지 설정·재시작(`SceneAnimationReset`) 후 유지 확인.
2. 곡 중간 씬 전환(`0004yy`)으로 들어오는 씬도 같은 Postfix로 바뀌는지 확인.
3. 다른 씬(특히 신형 `SceneScrollableControl` 씬)의 레이어 구성·클립에 스프라이트 커브가 있는지 번들로 확인.
4. Addressables 캐시(`CreatePrefabAndCache`의 사전)는 프리팹만 캐시하고 인스턴스는 매번 새로 만들므로, 교체는 인스턴스에만 하고 원본 프리팹·아틀라스는 건드리지 않습니다(공식 곡에 번지지 않게).
