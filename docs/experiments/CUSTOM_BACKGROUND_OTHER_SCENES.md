# 커스텀 배경(테마) 가능성: scene_10 외 나머지 씬 조사 (조사만, 2026-11-14)

[CUSTOM_BACKGROUND_FEASIBILITY.md](CUSTOM_BACKGROUND_FEASIBILITY.md)(scene_10 기준)와 같은 방법으로 나머지 씬(01~09, 11, touhou_black, 그리고 `_cy`/`_fool`/`dave`/`common` 번들)을 번들 데이터로 확인한 기록입니다. 코드는 아직 없습니다. 로드 경로(`InitSceneObject` 단일 생성 경로)는 위 문서 1절과 모든 씬이 같으므로 되풀이하지 않습니다.

- 방법: UnityPy(`FALLBACK_UNITY_VERSION=2019.4.41f1`)로 `gamescene_*.bundle` 30개를 읽어 `scene*` 루트별 컴포넌트·스프라이트·애니메이션 클립을 집계했습니다. 재실행: `python scripts/scene_survey.py`(결과 JSON은 `%TEMP%\_scene_survey.json`).
- 스프라이트 크기·PPU는 번들 두 개를 한꺼번에 열어야 풀리는 참조가 있어 별도로 읽었습니다(아래 scene_11).
- ISIL·Ghidra로 코드를 새로 본 것은 없습니다. 아래는 모두 **번들 데이터**에서 나온 내용입니다.

## 1. 전 씬 공통으로 확인된 것

| 항목 | 결과 |
| --- | --- |
| 스프라이트 교체 커브(`m_PPtrCurves`) | **전 씬 AnimationClip 전부 0개.** scene_10에서 본 것과 같아, 스프라이트를 바꿔도 애니메이션이 원래 그림으로 되돌리지 않습니다 |
| 스프라이트 PPU | 확인한 모든 스프라이트가 100 (1유닛 = 100px) |
| 그림의 형태 | 배경 바탕은 전부 `SpriteRenderer`. Spine은 03·07·08·12의 움직이는 소품에만 있음(scene_10 문서 1-2절 표와 일치) |
| 스크롤 방식 | **구형**: 01~09는 레이어 부모의 `Animator`가 위치 곡선을 반복(클립 길이가 곧 반복 주기). **신형**: 10·11·12는 `Scrollable`/`OffsetGroup`/`SceneScrollableControl` |
| `_cy` 번들 | 씬 루트·프리팹이 없고 **스프라이트 아틀라스 텍스처 한 장 + Sprite 몇 개**뿐입니다(01: 8개, 02: 5개, 04: 15개 + 클립 1개, 05: 4개 + 크리스마스 아틀라스, 06: 1개). 일반 번들의 같은 씬이 참조하는 추가 아틀라스로 보이며, 어떤 조건에서 쓰이는지는 확인하지 못했습니다 |
| `_fool` 번들 | 씬 번들이 아닌 만우절 연출 오브젝트(`ParGuText`, `CatchCircle` 등, 01_fool은 GameObject 297개)이고 `gamescene_11_fool`은 `Scene11_Aprilfoolsday` 텍스처 한 장입니다 |
| `gamescene_dave` | 데이브 더 다이버 미니게임(`dave_game_main`, `DaveGameAnimationController`, `SkeletonPartsRenderer`). 배경 씬(`scene_NN`)이 아니고 `SpriteRenderer`도 없음 |
| `gamescene_common` | 노트·보스·UI 이펙트 프리팹 모음(`SpineActionController` 등)이라 배경과 무관 |

> 클립의 곡선은 Generic 클립이라 `m_MuscleClip`·`m_ClipBindingConstant`에 들어 있습니다. 위치/회전/크기 개수는 바인딩의 `typeID`·`attribute`로 셌고, 스프라이트 곡선만 `m_PPtrCurves`에 따로 있어 그 길이로 판정했습니다.

## 2. 씬별 요약

| 씬 | SpriteRenderer(고유 스프라이트) | Spine | 최상위 자식 | 씬 전용 스크립트 | 스크롤 클립(반복 주기) |
| --- | --- | --- | --- | --- | --- |
| 01 | 1714 (32) | 0 | `Bg` `Mid` `Near` `Near_air` | `DoublePlaySceneObjControl`, `DefaultEggControl` | `s01_floor` 0.75s, `s01_floor_air` 0.75s, `s01_well`/`_1`~`_5` 8~11s |
| 02 | 4183 (66) | 0 | `Bg` `Far_1` `Far_2` `Mid_2` `Near` `far_bg` `far_rocket` | `SpriteMask` 48개, `DOTweenAnimation`, `DefaultEggControl` | `s02_floor` 18.67s, `s02_far` 12s, `s02_Far_2` 13s, `s02_Mid_2` 5.25s 외 |
| 03 | 220 (49) | **35** | `Bg` `Default` `Far` `FxGhost_special` `FxParticle` | `DefaultEggControl`, `Localization` 34개 | `s03_floor` 0.5s, `s03_closs_shot` 0.45s, `s03_long_shot` 4s, `s03_medium_shot` 15s |
| 04 | 330 (33) | 0 | `Bg` `Far` `Mid` `Near` `rain_sfx` | `DefaultEggControl` | `s04_floor` 2.83s, `s04_far` 3.17s, `s04_mid` 20s |
| 05 / _christmas | 64 (19) / 81 (20) | 0 | `Far` `Mid` `Near` `s05_scene_sfx` | `BACandyEggHandler` 2개(비크리스마스만) | scene_10 문서 6절 참고 |
| 06 | 774 (62) | 0 | `Bg` `Bg_cat` `Bg_cat _Cy_static` `Bg_moutain` `Bg_tree_Touhou` `Far ` `FxParticle` `Light` `Mid` `Near` `far_floor` | `DOTweenAnimation` 9개, `DefaultEggControl` | `s06_floor` 0.8s, `s06_far` 12s, `s06_mid` 20s, `s06_bg_moutain` 21s |
| 07 | 256 (23) | 4 (관람차) | `GameObject` `Near_road` `cloud` `far` `farlight_all` `mid` `par_star` `sky` `stars` | `GameAudioVisualization`, `GCClip` 128개 (`DefaultEggControl` 없음) | `s07_road` 4.08s, `s07_far` 89.9s, `s07_mid` 80s, `s07_cloud` 50s |
| 08 | 821 (68) | 2 (달 `08_moon`) | `Bg` `Far_all` `Ice_all` `Mid_A_0` `Mid_A_all` `Mid_B_1` `Mid_B_all` `Mid_color` `Near_all` `water` | `DefaultEggControl` | `08_road` 18s, `08_near` 12s, `08_far`/`_far_2` 32/30s, `08_ice` 11.3s |
| 09 | 512 (69) | 0 | `Bg` `Far_all` `Mid_A_all` `Mid_B_all` `Mid_color` `Near_all` `par_leaves` | 없음 (`DefaultEggControl`도 없음) | `09_near_floor` 10.5s, `09_far_a` 28s, `09_far_c` 60s |
| 11 | 296 (53) | 0 | `ScrollableGear` `par_star` | scene_10과 같은 크립톤 계열 구성(`CryptonEggControl`, `Scrollable` 8개 등) | 신형 `Scrollable` |
| touhou_black | 3 (2) | 0 | `Touhou_black` | `SceneSubControl`만 | 없음 |

고유 스프라이트 수는 한 장짜리 이름 기준이라, 같은 그림을 여러 번 쓰는 씬은 렌더러 수보다 훨씬 적습니다. 그래서 **스프라이트 이름 → PNG 교체 방식(scene_10 문서 4절 A안)으로는 씬당 20~70장만 그리면 됩니다.**

### 가장 큰 스프라이트(그리기 쉬운 단서, 가로×세로 px)

| 씬 | 큰 스프라이트 상위 3개 |
| --- | --- |
| 01 | `s01_well_1_ba3` 511×271, `s01_well_1_ba4` 511×255, `s01_well_1_ba2` 511×255 |
| 02 | `S02_Station` 584×316, `mid_B5_Arknights` 1618×96, `S02_word` 1198×125 |
| 03 | `S03_Candles` 1858×471, `s03_far` 550×481, `s03_well_2` 250×660 |
| 04 | `s04_building_b4_Touhou` 568×640, `s04_Arknights_5` 664×464, `s04_Arknights_3` 725×356 |
| 05 | `s05_far_tree` 1250×513, `s05_floor` 1530×394, `s05_garbage_can_ba` 1318×428 |
| 05 크리스마스 | `s05_far_tree_christmas` 1250×640, `s05_floor_christmas` 1530×394, `s05_bg_christmas` 666×620 |
| 06 | `far_uuz_tree_Touhou` 716×856, `middle_girl_Touhou` 692×556, `mountain` 460×440 |
| 07 | `tower` 647×1202, `roller_coaster_b` 789×531, `roller_coaster_a` 673×453 |
| 08 | `moon5` 1381×604, `sakura_tree 1` 990×672, `tree` 888×719 |
| 09 | `planet_glow` 923×923, `planet_01` 709×709, `floor_n_02` 1040×263 |
| 12 | `s12_far_c` 1893×719, `s12_middle_BIGb` 1575×729, `s12_far_b` 1565×661 |

(`s10_bg`처럼 16×16 조각을 늘려 쓰는 그림은 이 표에 없습니다. 이 순위는 원본 이미지 크기이고, 화면에서 차지하는 크기는 `scale`까지 봐야 합니다.)

## 3. 씬별로 눈에 띄는 점

- **scene_01**: 바닥(`s01_floor`)과 공중 바닥(`s01_floor_air`)이 각자 0.75초 주기로 따로 돕니다. 바닥 조명은 `s01_floor_light_standby/_act`가 `SpriteRenderer` 색(클래스 212)만 바꿉니다. `DoublePlaySceneObjControl`(이름상 2인 플레이용)은 이 씬에만 있고 동작은 보지 않았습니다.
- **scene_02**: `SpriteMask` 48개와 렌더러 4183개로 가장 무겁습니다. 스프라이트 이름에 `Arknights`·`Touhou`·`cy`가 붙은 변형(`mid_B5_Arknights`, `mid_B5_Touhou`, `s02_mid_h7B_cy`, `mid_F2_tuzi_Arknights`)과 대응 클립(`s02_mid_B5_arknights` 25.9초)이 있습니다. **이름만 보고 어느 곡에서 켜지는지 단정하지 않습니다**(12번을 명일방주로 적었다 틀린 선례, CUSTOM_CHART_GUIDE 5절). 켜짐 조건은 `DefaultEggControl`을 따로 읽어야 압니다.
- **scene_03**: 유일하게 배경에 **Spine이 35개**(`Fire`, `Ghost2` 등, 우물 `Well1/3`·`Well3_special`·`Well3_Wacca`·`Well3_Touhou` 아래)입니다. 우물 변형이 `Default/Mid/` 아래 곡 종류별로 나뉘어 있고 `FxGhost_special`은 따로 있습니다. 바탕(`Bg`·`Far`)은 스프라이트라 교체가 되지만, 우물의 불·유령은 Spine이라 scene_12와 같은 문제(교체 불가, 숨기기나 Spine 스킨 방식 필요)가 있습니다.
- **scene_04**: 고유 스프라이트가 33개로 적고 `s04_Arknights_1~5`, `s04_building_b4_Touhou` 같은 콜라보 변형이 섞여 있습니다. `_cy` 번들에 `s04` 클립 1개와 컨트롤러가 따로 있습니다. 스프라이트 교체에 가장 부담이 적은 씬 후보입니다(Spine 0, 스크롤 클립 3개).
- **scene_05**: 이미 scene_10 문서 6절에서 다뤘습니다. 이 조사로 추가된 것은 크리스마스 판(`scene_05_christmas`)이 **같은 번들에 별도 루트**로 있고 스프라이트가 `_christmas` 접미사로 따로 있다는 것, 비크리스마스 판에만 `BACandyEggHandler`가 있다는 것입니다. 크리스마스 판의 비행 클립은 `s05_long_shot_christmas`(11.17초, 위치 5개)입니다.
- **scene_06**: 클립 `s06_cat_ball_cy`가 위치 19·크기 19·회전 11개로 가장 복잡한 곡선 한 덩어리입니다. 최상위에 `Bg_cat _Cy_static`, `Bg_tree_Touhou`처럼 콜라보 전용 자식이 있고, **`Far `(끝 공백)**처럼 이름에 공백이 붙은 노드가 있어 경로로 찾을 때 주의해야 합니다.
- **scene_07**: `GameAudioVisualization` + `GCClip` 128개(scene_10과 같은 막대 시각화). 스크롤 주기가 매우 길어(`far` 89.9초, `mid` 80초) 이음매가 눈에 안 띄기 쉽습니다. 관람차 Spine 4개(`sky_wheel_1`)가 `mid/roller_coaster_A` 아래에 있습니다. `DefaultEggControl`이 없는 두 씬(07, 09) 중 하나입니다.
- **scene_08**: `Mid_B`·`Mid_A` 클립이 위치 12~13개짜리로 나무·집이 개별 곡선을 가집니다(`08_mid_B` 위치 13, `08_mid_A_1` 위치 12). 스프라이트 이름을 바꿔도 위치 곡선은 그대로라 그림 크기를 크게 바꾸면 어긋납니다. Spine은 달(`08_moon`) 한 종류뿐입니다. 곡 uid로 켜지는 Bad Apple 흑백 연출은 별도 번들 `gamescene_touhou_black`(scene_touhou_black, 스프라이트 3개)로 갑니다.
- **scene_09**: 클립에 대화창(`dialog_change_m/n`, `dialog_recover_m/n`)이 들어 있습니다. 위치가 아니라 **회전 곡선 15~48개**짜리라 이 씬만 배경 안에 대사 연출이 있는 것으로 보입니다(동작은 미확인). `planet_glow`(923×923)·`planet_01`이 큰 바탕입니다.
- **scene_11**: **scene_10의 그림을 그대로 씁니다.** 번들 두 개를 같이 열어 스프라이트를 풀어 보니 고유 스프라이트 53개 중 52개가 `scene_10_miku`와 이름이 같고, `rin_len`과는 29개만 겹칩니다(나머지 1개는 풀리지 않는 참조). 자기 번들에 아틀라스가 없고 스프라이트가 scene_10 번들을 가리키기 때문에(자기 번들에는 AnimationClip도 0개라 애니메이션도 scene_10 쪽을 쓸 것으로 보이나 참조를 직접 확인하지는 않음), `gamescene_11` 번들만 열면 스프라이트 이름이 하나도 안 풀립니다. 그래서 CUSTOM_CHART_GUIDE 5절의 "번들이 작다(370KB)"는 이 때문이고, 앞서 "LEN 무대" 추정이 근거가 약하다고 한 판단도 맞습니다(스프라이트는 미쿠판 기준). **교체 방식을 이름 기준으로 만들면 scene_10 PNG가 scene_11에도 같이 적용됩니다**(의도한 동작인지 정해야 함). 곡 중간 전환은 1~10·12만 된다고 되어 있으므로 11은 곡 전체 씬으로만 쓰입니다.
- **scene_12**: scene_10 문서 5절 참고(변경 없음).
- **touhou_black**: `scene_touhou_black`은 `Touhou_black` 자식 하나에 스프라이트 3개(고유 2개)뿐입니다. `TouhouLogic.ReplaceBadAppleScene`이 이름을 바꿔 부르는 대상입니다. 바꿀 그림이 가장 적은 씬입니다.

## 4. 커스텀 경로에 미치는 영향 (scene_10 문서 4절 기준으로 정리)

| 안 | 가능한 씬 | 비고 |
| --- | --- | --- |
| A. 스프라이트 이름 → PNG 교체 | **전 씬** | 스프라이트 교체 커브가 없으므로 안전. 01·02·06·08은 렌더러 수가 많아 첫 로드 때 `GetComponentsInChildren` 비용이 있을 수 있어 씬당 한 번만 돌리고 캐시해야 함(`InitSceneObject`는 씬 생성 때만 불림) |
| 소품 Spine(03 우물·07 관람차·08 달·12) | 03·07·08·12 | A안으로 안 바뀜. 해당 `SkeletonAnimation` 끄기 또는 Spine 스킨 교체가 필요 |
| B. 스크롤 값 조정 | **10·11·12만** | 01~09는 위치 곡선이 클립에 고정이라 주기·거리 조정 불가. 같은 간격으로 반복되는 그림만 넣어야 이음매가 안 보임 |
| 곡 중간 전환 | 1~10·12 | `InitSceneObject` 한 곳에서 만들어지므로 같은 Postfix로 처리됨 (scene_10 문서 1절) |

**시작점으로 적당한 씬**: 04(Spine 0, 스크롤 클립 3개, 고유 스프라이트 33개) 또는 touhou_black(스프라이트 2개). scene_10에서 만든 방식을 그대로 확장하면 됩니다. 반대로 03은 Spine 35개, 02는 렌더러 4183개라 마지막에 해야 합니다.

## 5. 남은 확인

1. `DefaultEggControl`의 켜짐 조건(콜라보 변형 `Arknights`/`Touhou`/`cy`를 고르는 방식): ISIL로 `DefaultEggControl`과 `SceneEggTag`를 읽어야 압니다. 이번 조사에서는 번들 데이터만 봤습니다.
2. `_cy` 번들이 어떤 조건에서 로드되는지(`LoadFromName` 호출처).
3. scene_09의 `dialog_*` 클립이 어떤 이벤트에서 재생되는지, scene_01의 `DoublePlaySceneObjControl` 동작.
4. scene_11의 sprite 참조 1개(풀리지 않음)가 무엇인지.
5. 실기 검증은 scene_10 문서 8절과 같이 아직 없습니다.

