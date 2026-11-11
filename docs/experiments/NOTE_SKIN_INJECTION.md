# 노트 스킨 주입 가능성 조사

> 조사만 한 문서입니다(2026-11-11). 코드는 바꾸지 않았고 인게임 실측도 아직 없습니다.
> 근거는 ISIL(`GameMusicScene.PreloadGameObject`, `SpineActionController.Init`)과
> `gamescene_05` 번들을 UnityPy로 연 결과입니다. 다른 씬 번들과 공통 번들(하트·음표 등)은 열어 보지 않았습니다.

## 결론

**된다고 봅니다.** 게임이 이미 같은 일을 합니다. `SpineActionController.Init` 안에서 특정 조건이면
노트의 `SkeletonDataAsset`을 다른 것으로 바꿉니다(`DoctorSkInit`의 `0613_road_rd_SkeletonData`,
곡 `66-8`의 `1000_hp_SkeletonData`). 캐릭터 스킨 주입(`Spine/Patch_Inject_BlackGirlBattle.cs`)도
같은 `Init` Postfix에서 돕니다. 다만 지금 주입기를 그대로 노트에 쓰면 아래 4가지가 틀립니다.

| # | 지금 캐릭터 주입기 | 노트에 필요한 것 | 근거 |
| --- | --- | --- | --- |
| 1 | 오브젝트 이름 완전 일치(`black_girl_battle(Clone)`) | 노트 데이터의 `prefab_name`으로 고르기 | 노트 오브젝트 이름이 `{프리팹}(Clone){objId}`로 바뀜 |
| 2 | `scale` 0.01 고정 | 원래 에셋의 `scale`을 따르기 (scene 05 노트는 **0.005**) | 번들의 `*_SkeletonData.scale` |
| 3 | JSON만 읽음 | 그대로 JSON (게임 원본은 바이너리 `.skel`) | 번들의 `TextAsset` 이름·헤더 |
| 4 | 파일 세트 5개가 코드에 고정 | 곡 폴더나 설정에서 대상 프리팹을 정하기 | `CustomSkinInjector.KnownBaseNames` |

## 1. 노트가 만들어지는 길

`GameMusicScene.PreLoad` → `PreloadGameObject(preObj, md, curScene, hasSceneChange)`가 노트마다 하나씩 만듭니다(ISIL 확인).

1. 프리팹을 `Instantiate`합니다.
2. 이름을 `원래 이름 + objId`로 바꿉니다(`Object.get_name` → `String.Concat(name, (Int16)…)` → `set_name`, ISIL 268~317행).
   예: `051001_road_nor_1(Clone)` 뒤에 숫자가 붙습니다. **그래서 이름 완전 일치로는 노트를 고를 수 없습니다.**
   [CHECKLIST.md](../guides/CHECKLIST.md)의 "이름 부분 일치로 고르지 말 것"도 있으니,
   `SpineActionController.m_MusicData`(Init 안에서 `StageBattleComponent.GetMusicDataByIdx`로 채움)의 `noteData.prefab_name`/`uid`로 고릅니다.
   `m_MusicData`가 빈 컨트롤러가 있다는 기록이 있으므로(`GhostNoteIdentity`), 이름 앞 6자리 uid를 보조로 씁니다.
3. `NodeInitController.InitWithoutRun` → `SpineActionController.Init(idx, curScene)`(ISIL 456행).

노트는 풀에서 꺼내 다시 쓰지 않고 노트마다 인스턴스가 따로 있습니다. 그래서 `Init` Postfix에서 한 번 바꾸면 그 노트는 끝까지 그 스킨입니다.
곡 중간 씬 전환의 씬별 복제(`PreloadTmpGameObject`)는 모드가 `IsIgnoreCurrentNoteChecking`으로 막고 있어 커스텀 곡에서는 생기지 않습니다([SCENE_BACKGROUND_SWAP.md](SCENE_BACKGROUND_SWAP.md)).

### `Init`이 스켈레톤에서 미리 잡아 두는 것

- `m_SkeletonAnimation`(@0x38): 컴포넌트 참조라 스켈레톤을 바꿔도 그대로 유효합니다.
- `m_Bones`(@0x40, `Dictionary<string, Bone>`): `InitSkField(ska)`가 채웁니다. `Initialize(true)`로 스켈레톤을 다시 만들면
  **옛 Bone 객체를 가리키게 됩니다.** `get_bones`를 부르는 곳은 ISIL 전체에서 찾지 못했지만, 안전하게 하려면 주입 뒤 `InitSkField(ska)`를 다시 부릅니다(미확인).
- 잡기 노트는 자식 `Catch`의 `BoneFollower.SkeletonRenderer`를 `PreloadGameObject`에서 연결합니다. 본 이름이 다르면 따라가지 못할 수 있습니다.

## 2. 커스텀 스켈레톤이 지켜야 할 애니메이션 이름 (액션 계약)

게임은 애니메이션 이름을 직접 부르지 않습니다. 프리팹의 `SpineActionController.actionData`에 적힌 **키 → 애니메이션 이름** 표를 거칩니다.
아래는 `gamescene_05` 번들의 프리팹에서 읽은 값입니다. 커스텀 스켈레톤에 **오른쪽 이름의 애니메이션이 없으면 그 동작에서 노트가 멈추거나 안 보입니다.**

| 프리팹 | `in` | `note_out_g` | `note_out_p` | 그 밖 |
| --- | --- | --- | --- | --- |
| `051001_road_nor_1` (속도 1) | `in_nor_44` | `out_g` | `out_p` | |
| `051002_road_nor_2` (속도 2) | `in_nor_38` | `out_g` | `out_p` | |
| `051003_road_nor_3` (속도 3) | `in_nor_33` | `out_g` | `out_p` | |
| `051004_air_nor_1` | `in_nor_44` | `out_g` | `out_p`(무작위 순서 플래그 켜짐) | |
| `051007_road_up_1` | `in_up_30` | `out_g` | `out_p` | |
| `050401_nor_1` (연타) | `in_nor_44` | `out_g` | `out_p` | `note_multihit_hurt` → `hurt_1`·`hurt_2`·`hurt_3` 무작위, `note_multi_alive` → `out_nor_44` |
| `050404_down_1` (연타) | `in_down_27` | `out_g` | `out_p` | 위와 같음 |
| `050301_road_nor_1` (톱니) | `in_nor_44` | | | `note_destroy` → `destroy` |
| `050201_road_nor_1` (롱노트 머리) | (빈 목록) | | | Spine이 아님. 아래 3절 |

- `in_nor_44/38/33`의 숫자는 등장 애니메이션 길이(프레임)입니다. 속도 1·2·3 프리팹이 각각 하나씩 씁니다([NOTE_SPEED_DT.md](NOTE_SPEED_DT.md)). **채보에 쓰는 속도 수만큼 애니메이션을 만들어야 합니다.**
- 원본 `0510_road.skel`에는 `in_up_23/26/30`, `in_down_23/26/30`, `in_nor_44_hint`, `standby`도 들어 있습니다. 한 스켈레톤을 여러 프리팹이 같이 씁니다.
- 고스트 노트(`xx=17`)는 `in_nor_44`의 `ColorTimeline`으로 투명해집니다. 모드의 알파 고정([GHOST_NOTE_ALPHA_HOLD.md](GHOST_NOTE_ALPHA_HOLD.md))은 스켈레톤 데이터 포인터를 캐시 키로 쓰므로 커스텀 스켈레톤에도 따로 적용될 것으로 봅니다(미확인).
- 이 표는 씬 05만 본 것입니다. 다른 씬은 숫자(`in_down_27` 등)가 다를 수 있으니 대상 프리팹을 직접 읽어야 합니다.

## 3. 원본 에셋의 형태 (`gamescene_05` 번들)

| 항목 | 값 |
| --- | --- |
| Spine 버전 | `3.8.99` (`.skel` 헤더). 모드의 캐릭터 스킨 JSON도 `3.8.99` |
| 스켈레톤 형식 | **바이너리 `.skel`** (`0510_road.skel` 31KB 등) |
| `SkeletonDataAsset.scale` | **0.005** (`0503`, `0503_air`, `0510_road` 모두). 캐릭터 주입기는 0.01 고정 |
| 아틀라스 | 일반 몬스터 노트는 씬 공용 아틀라스 하나(`s05_atlas.atlas`, 2048×512)를 같이 씀. 톱니(`0503`)·롱노트 몸통(`0509_*`)은 따로 |
| 롱노트 (`0502`) | Spine이 아니라 텍스처(`0502_road_body`, `0502_road_note_up` …)와 `0502_road_renderer` 프리팹 |
| 크리스마스 | 보스만 별도 스켈레톤(`0501_christmas.skel`). 노트는 공용 |

여기서 나오는 결론:

- **커스텀 노트 하나는 자기 png·atlas·json 세트를 따로 가집니다.** 공용 아틀라스를 고칠 필요는 없지만, 머티리얼이 달라져 그 노트만 드로우콜이 늘어납니다.
- `scale`은 상수로 두지 말고 **바꾸기 전 원래 에셋의 `scale`을 읽어** 새 에셋 생성(`SkeletonDataAsset.CreateRuntimeInstance(…, scale)`)에 넘깁니다. 그러면 씬마다 다른 값도 따라갑니다.
- 바이너리 `.skel`을 런타임에 읽으려면 바이트 `TextAsset`이 필요한데, Unity 2019.4에는 `byte[]` 생성자가 없습니다. **커스텀 스킨은 Spine 3.8 에디터에서 JSON으로 내보내는 것을 전제로 합니다.** 원본을 바탕으로 고치려면 에디터로 `.skel`을 가져와야 합니다(가져오기 가능 여부는 미확인).
- 롱노트는 이 방식으로 바꿀 수 없습니다. 텍스처를 바꾸는 별도 경로가 필요합니다.

## 4. 구현 스케치 (아직 코드 없음)

1. `InjectHelper.TryInject`에 노트 경로 추가: `m_MusicData.noteData.prefab_name`(없으면 이름 앞부분) → 곡 폴더의 `note skin/{prefab_name 또는 uid}/` 세트를 찾습니다.
   커스텀 곡(`CustomPlaySession.Current.ShouldApplyExperimentChart`)에서만 켭니다. 공식 곡은 건드리지 않습니다.
2. `CustomSkinInjector.GetOrBuild`에 `scale` 인자를 받게 하고, 캐시 키를 `(세트 경로, scale)`로 합니다.
3. 주입 직후 계약 검사: 그 프리팹의 `actionData`에 있는 애니메이션 이름이 새 스켈레톤에 다 있는지 보고, 빠진 이름은 경고로 남기고 **주입을 건너뜁니다**(멈춘 노트보다 원본 노트가 낫습니다).
4. `ska.Initialize(true)` 뒤 `InitSkField(ska)` 재호출.
5. `Initialize`는 노트마다 불리므로(곡 로딩 중 수백~수천 번), 로딩 시간을 재서 기록합니다. `GetOrBuild`는 세트당 한 번만 만듭니다.

## 5. 실측으로 확인할 것

- [ ] `Init` Postfix 시점에 `m_MusicData.noteData.prefab_name`이 채워져 있는가
- [ ] scale 0.005로 만든 JSON 스켈레톤이 원본과 같은 크기·위치로 나오는가
- [ ] `in`/`out_p`/`out_g` 재생, 연타 `hurt_*`, 톱니 `destroy`가 정상인가
- [ ] 프레임 드랍 보정(`NoteAnimationSyncPatch`의 `currentAnimationTime` 조정)이 커스텀 스켈레톤에서도 맞는가
- [ ] 노트 1000개 이상 곡의 로딩 시간 차이
- [ ] 재시작(GameRestart) 뒤에도 스킨이 유지되는가 (`OnControllerStart` 경로)
- [ ] 폴백 경로(`0-0`)와 공식 곡에서 아무것도 바뀌지 않는가
