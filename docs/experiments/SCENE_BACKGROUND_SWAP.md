# 배경만 바뀌고 노트는 그대로: 씬 배경 스왑 원리 (결합도 분리 아키텍처)

이 문서는 **"구간마다 배경(scene)은 갈아끼우되, 노트 모양·정체는 BMS 원본 그대로 유지"** 하는 동작이
어떻게 구현되어 있는지를 설명합니다.

관련 파일:

- `muse dash test/Patches/Scene/GameMusicScenePatch.cs` — 초기 배경 씬(`LoadScene`) 리다이렉션
- `muse dash test/Patches/Scene/GameMusicSceneInitPatch.cs` — `StageBattleComponent.IsIgnoreCurrentNoteChecking` 및 `GameMusicScene.IsIgnoreCurrentNoteChecking` 후킹(노트 정체 덮어쓰기 원천 차단) + 장시간 곡 `InitTimer` 확장
- `muse dash test/Patches/Scene/GameMusicScenePreLoadEnemyPatch.cs` — 풀 빌드 전후 무변형 정체 진단
- `muse dash test/Patches/Scene/SceneFlowPatch.cs` — 런타임 `ChangeNote` 차단(배경 `ChangeScene`만 통과)
- `muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Bms.cs` — 씬 전환 노트(`0004yy`)의 `ibms_id`(`1O`~`1Y`) 매핑

---

## 1. 문제: 배경과 노트가 한 덩어리로 묶여 있다

뮤즈대시는 하나의 "GameMain" 씬 안에서 돌아가고, `scene_NN` 배경은 콘텐츠로 갈아끼워집니다.
그런데 게임 엔진(`StageBattleComponent.LoadMusicData` 및 `SceneChangeController`)은 씬이 전환될 때
배경만 바꾸는 것이 아니라 **그 구간 이후 등장하는 모든 노트의 `uid`·`prefab_name` 앞 2자리(`zz`)까지 전환된 씬 번호로 덮어쓰도록** 설계되어 있습니다.

게임 내부에는 씬 전환에 관여하는 세 지점이 있습니다.

1. **`StageBattleComponent.LoadMusicData()`**:
   - 채보에 씬 전환 노트(`ibms_id`가 `sceneInfo` 사전에 있는 `1O`~`1Y`)가 있으면 `curSceneChangeType`(`m_Scenes`)에 씬을 등록하고 `isSceneChangeType = true`로 켭니다.
   - 직후 `StageBattleComponent.IsIgnoreCurrentNoteChecking(prefabName)`이 `false`를 반환하면 `GetNoteInfoName(switchSceneId, prefabName)`을 호출해 **이후 모든 노트의 `noteData`(`uid`, `prefab_name`, `mirror_uid`, `noteUid`, `scene`)와 `configData.note_uid`를 전환된 씬 번호(`zz`)로 인플레이스 덮어씁니다.**
   - 또한 두 번째 루프에서 씬 전환 전후 노트의 `configData.scenes` 목록을 채워 넣습니다.
2. **`GameMusicScene.PreLoad(int idx)`**:
   - `configData.scenes`가 채워져 있고 `GameMusicScene.IsIgnoreCurrentNoteChecking(curScene, notePrefabName)`이 `false`이면, 노트마다 씬별 복제 프리팹을 생성합니다.
3. **`SceneChangeController.ChangeScene(int)` / `ChangeNote(int)`**:
   - 인게임에서 씬 전환 노트를 지날 때 `ChangeScene`으로 배경을 바꾸고, `ChangeNote`로 노트 렌더 세트를 교체합니다.

---

## 2. 해결책: 변형 없는 결합도 분리 (Zero-Mutation Decoupling)

과거에는 `StageBattleComponent.LoadMusicData()`가 노트를 덮어쓰는 이유를 몰라, `InitTimer`에서 노트를 다시 변형(`TransformSceneSegments`)했다가 `PreLoadEnemy` 직후 `RestoreIdentities`와 `RestoreRuntimeObjects`로 복구하는 2단 우회 구조(Modify → Restore)를 사용했습니다.

Cpp2IL/ISIL 역공학으로 `LoadMusicData()`와 `PreLoad()`의 내부 분기를 확정한 뒤에는 **데이터를 단 한 번도 변형하지 않는 순수 분리 구조**로 전환했습니다.

### 2-1. 초기 배경 및 씬 전환 배경 등록

- **초기 배경**: `GameMusicScene_LoadScene_Patch`(`GameMusicScenePatch.cs`)가 `hwa info.txt`의 `scene` 설정값을 읽어 `GameMusicScene.LoadScene(ref sceneName)`에 `scene_NN`을 주입합니다.
- **곡 중간 씬 전환(`0004yy`)**: `DBStageInfoExperimentChart.Bms.cs`가 `0004yy` 노트에 `ibms_id`(`1O`~`1Y`)를 부여합니다. `StageBattleComponent.LoadMusicData()`는 이 `ibms_id`를 보고 전환될 배경들을 `curSceneChangeType`에 정상 등록하며, `GameMusicScene.LoadScene`이 해당 배경 오브젝트들을 미리 인스턴스화합니다.

### 2-2. 노트 정체 덮어쓰기 원천 차단 (`IsIgnoreCurrentNoteChecking` 패치)

`GameMusicSceneInitPatch.cs`에서 두 메서드를 Prefix로 후킹하여 커스텀 차트 플레이 중일 때 항상 `true`를 반환하게 합니다.

1. **`StageBattleComponent.IsIgnoreCurrentNoteChecking(string prefabName)` → `true`**
   - `LoadMusicData()`가 배경 목록(`curSceneChangeType`)과 `isSceneChangeType = true`는 그대로 설정하면서도, 개별 노트의 `noteData`/`configData.note_uid` 덮어쓰기와 `configData.scenes` 채우기를 **전부 건너뜁니다.**
2. **`GameMusicScene.IsIgnoreCurrentNoteChecking(int curScene, string notePrefabName)` → `true`**
   - `GameMusicScene.PreLoad(idx)`가 각 노트를 BMS 원본 `prefab_name` 그대로 인스턴스화합니다.

### 2-3. 런타임 노트 세트 교체 차단 (`ChangeNote` 패치)

`SceneFlowPatch.cs`의 `SceneChangeController_ChangeNote_Patch`가 커스텀 차트 플레이 중 `ChangeNote(int)` 호출만 차단하고 `ChangeScene(int)`은 그대로 통과시킵니다.

---

## 3. 타임라인 한눈에 보기

| 순서 | 시점 / 후킹 | 파일 | 하는 일 |
| --- | --- | --- | --- |
| 1 | `SetRuntimeMusicData` | `DBStageInfoExperimentChart.cs` | BMS 원본 `uid`·`prefab_name`으로 `musicList` 생성 (`0004yy`는 `ibms_id` 매핑) |
| 2 | `LoadMusicData` | `GameMusicSceneInitPatch.cs` | `StageBattleComponent.IsIgnoreCurrentNoteChecking` → `true` 반환으로 배경만 등록하고 노트 변형 원천 차단 |
| 3 | `InitTimer` Prefix | `GameMusicSceneInitPatch.cs` | 장시간 커스텀 곡일 때 `FixUpdateTimer` 총 시간(`total`) 확장 및 무변형 `zz` 분포 로깅 |
| 4 | `LoadScene` Prefix | `GameMusicScenePatch.cs` | 매니페스트 초기 배경(`scene_NN`) 리다이렉션 + 전환 배경 프리로드 |
| 5 | `PreLoadEnemy` | `GameMusicSceneInitPatch.cs` | `GameMusicScene.IsIgnoreCurrentNoteChecking` → `true` 반환으로 BMS 원본 프리팹 그대로 노트 풀 생성 |
| 6 | 인게임 씬 전환 | `SceneFlowPatch.cs` | `ChangeScene` 통과(배경 전환), `ChangeNote` 차단(노트 외형 유지) |
