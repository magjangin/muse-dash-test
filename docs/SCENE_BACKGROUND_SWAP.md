# 배경만 바뀌고 노트는 그대로: 씬 배경 스왑 원리

이 문서는 **"구간마다 배경(scene)은 갈아끼우되, 노트 모양·정체는 BMS 원본 그대로 유지"** 하는 동작이
어떻게 구현되어 있는지를 설명합니다. 직접 로그를 찍어가며 실측으로 알아낸 내용이라, 다시 봐도 바로
이해되도록 "왜 이렇게 했는가"까지 적어 둡니다.

관련 파일:

- `muse dash test/Patches/Scene/GameMusicSceneInitPatch.cs` — zz 변형 + `SceneZzTransformTracker`
- `muse dash test/Patches/Scene/GameMusicScenePreLoadEnemyPatch.cs` — 풀 빌드 직후 정체 복구
- `muse dash test/Patches/Scene/SceneFlowPatch.cs` — `ChangeNote` 차단(배경만 통과)
- `muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.cs` — BMS 원본 정체 등록

---

## 1. 문제: 배경과 노트가 한 덩어리로 묶여 있다

뮤즈대시는 하나의 "GameMain" 씬 안에서 돌아가고, `scene_NN` 배경은 콘텐츠로 갈아끼워집니다.
그런데 배경 선택의 열쇠와 노트 프리팹 선택의 열쇠가 **둘 다 노트 `uid`의 앞 2자리(zz = scene 코드)** 에
묶여 있습니다.

```
uid = "071304"
      └┴──────  zz(scene/배경 코드) = "07"
        └┴────  xx
          └┴──  yy
```

| zz가 결정하는 것 | 결과 |
| --- | --- |
| 등장할 배경(`scene_07`) | 배경 그래픽 |
| 노트 프리팹(`prefab_name`이 zz로 시작) | 노트 외형 |

따라서 **zz를 바꾸면 배경도 바뀌지만 노트 모양까지 같이 바뀝니다.** 우리가 원하는 건
"배경만" 바꾸는 것이므로, 이 둘을 떼어내야 합니다.

게임 내부에는 씬 전환을 담당하는 두 경로가 있습니다.

- `SceneChangeController.ChangeScene(int)` — **배경**을 바꾼다
- `SceneChangeController.ChangeNote(int)` — 그 씬에 맞는 **노트 세트**로 교체한다

즉 노트가 따라 바뀌는 주범은 `ChangeNote`입니다.

---

## 2. 큰 그림: 막고(block) + 바꿨다 되돌리기(transform→restore)

원리는 두 축으로 나뉩니다.

1. **`ChangeNote`를 막는다** → 씬이 전환돼도 게임이 노트 세트를 갈아끼우지 못하게 한다.
2. **zz를 잠깐만 배경용으로 바꿨다가, 노트 풀이 만들어진 직후 원래대로 되돌린다**
   → 배경 등록은 매니페스트 배경 zz로 일어나지만, 노트 데이터는 BMS 원본 정체로 복구된다.

`objId`(노트 식별자)를 정체의 열쇠로 삼아, 리스트가 재정렬·복제되어도 추적이 끊기지 않게 한 것이 핵심입니다.

---

## 3. ChangeNote 차단 — 노트 세트 교체 막기

`muse dash test/Patches/Scene/SceneFlowPatch.cs`

```csharp
[HarmonyPatch(typeof(SceneChangeController), "ChangeNote", new[] { typeof(int) })]
public class SceneChangeController_ChangeNote_Patch
{
    // false 반환 → 원본 ChangeNote 미실행 = 노트 세트 교체 차단
    public static bool Prefix(SceneChangeController __instance, ref int sceneInfo)
    {
        return false;
    }
}
```

`Prefix`가 `false`를 반환하면 Harmony가 원본 메서드를 실행하지 않습니다. 반면 `ChangeScene`은
손대지 않으므로 **배경 전환은 정상적으로 통과**합니다. 이 하나로 "씬이 바뀌어도 노트 프리팹 세트는
그대로"가 성립합니다.

---

## 4. zz 변형 → 복구 2단 구조

`ChangeNote`만 막으면 "전환 시점"의 교체는 막지만, **초기 노트 풀 자체가 어떤 zz로 빌드되는가**는
여전히 문제로 남습니다. 풀은 `musicList`의 zz를 보고 만들어지기 때문입니다. 그래서 zz를 다음 타이밍에
맞춰 잠깐 바꿨다가 되돌립니다.

### 4-1. (주입 직후) BMS 원본 정체 박제

`DBStageInfoExperimentChart.cs` — BMS 노트를 `musicList`에 주입한 직후:

```csharp
SceneZzTransformTracker.RegisterBmsOriginalIdentities(musicList, 1);
```

각 노트의 **진짜 원본**(uid / mirror_uid / noteUid / scene / prefab_name / configData.note_uid)을
`objId`를 키로 `BmsOriginalsByObjId`에 저장합니다. 이게 나중에 "되돌릴 목적지"가 됩니다.

### 4-2. (InitTimer) zz를 배경용으로 변형

`GameMusicSceneInitPatch.cs`의 `TransformSceneSegments` — 풀이 빌드되기 전:

- 매니페스트에서 가져온 배경 zz(`activeRenderZz`, 없으면 `07`)로 각 구간 노트의 zz를 바꾼다.
- 단, 그 노트에 **BMS 원본 zz가 등록되어 있으면 그 zz를 우선**한다.
  (`TryGetBmsOriginalUid` — 커밋 `9d27d0a` "Prefer original BMS scene prefix for preload")

```csharp
string renderZz = activeRenderZz;
if (SceneZzTransformTracker.TryGetBmsOriginalUid(note.objId, out string bmsOriginalUid))
    renderZz = bmsOriginalUid.Substring(0, 2);   // BMS 원본 배경을 우선

// uid / mirror_uid / scene / prefab_name / noteUid / configData.note_uid 를 renderZz 기준으로 변형
SceneZzTransformTracker.Record(note, newUid, renderPrefabName);  // 원복용 매핑 저장
```

`Record`는 "원본 ↔ 렌더(변형)값" 쌍을 함께 보관합니다. 복구 단계에서 이 매핑으로 되돌립니다.

### 4-3. (PreLoadEnemy 직후) 원본 정체로 복구

`GameMusicScenePreLoadEnemyPatch.cs`의 `Postfix` — 노트 오브젝트 풀(`preloads`/`objCtrls`)이
만들어진 **직후**:

```csharp
SceneZzTransformTracker.RestoreIdentities(db.musicList);        // musicList 데이터 복구
SceneZzTransformTracker.RestoreRuntimeObjects(__instance);      // 이미 생성된 런타임 객체 복구
```

- `RestoreIdentities` — `objId`를 키로 `musicList`의 각 노트를 원본 uid/scene/prefab으로 되돌린다.
- `RestoreRuntimeObjects` — 풀에 이미 들어간 런타임 객체 그래프를 리플렉션으로 2단계까지 파고들어,
  변형된 uid 문자열·noteUid 스칼라까지 원본값으로 치환한다.

이 두 단계 덕분에 **배경은 변형된 zz로 등록되어 바뀌지만, 노트의 실제 데이터는 BMS 원본**으로
돌아옵니다.

---

## 5. 타임라인 한눈에 보기

| 순서 | 시점 / 후킹 | 파일 | 하는 일 |
| --- | --- | --- | --- |
| 1 | 주입 직후 | `DBStageInfoExperimentChart.cs:64` | `RegisterBmsOriginalIdentities` — 원본 정체를 `objId`로 박제 |
| 2 | `InitTimer` Prefix | `GameMusicSceneInitPatch.cs` | `TransformSceneSegments` — zz를 배경용으로 변형(+ BMS 원본 zz 우선) |
| 3 | (게임) `PreLoadEnemy` | — | 변형된 zz로 노트 풀/배경 빌드 |
| 4 | `PreLoadEnemy` Postfix | `GameMusicScenePreLoadEnemyPatch.cs` | `RestoreIdentities` + `RestoreRuntimeObjects` — 노트 데이터 원복 |
| 5 | (게임) `ChangeScene` | `SceneFlowPatch.cs` | 배경만 전환(통과) |
| 6 | (게임) `ChangeNote` | `SceneFlowPatch.cs` | **차단** — 노트 세트 교체 안 함 |

---

## 6. 왜 `objId`가 정체의 열쇠인가

복구 단계에서 노트를 식별할 때 uid나 인덱스를 쓰면 안 됩니다. 이유:

- **uid**: 변형 단계에서 우리가 직접 바꾼 값이라 식별자로 쓸 수 없다.
- **인덱스**: `ApplyBmsDoubleState` / `SortBmsNotesByShowTick` 등으로 리스트가 재정렬되면 깨진다.

`objId`는 변형·정렬·복제를 거쳐도 노트마다 고정으로 따라다니므로, "이 노트의 원본이 무엇이었는가"를
끝까지 추적할 수 있는 유일한 안정적 키입니다. 그래서 등록/복구 모두 `objId`를 사용합니다.
(커밋 `acfe956` "Preserve BMS note identity across scene preload")

---

## 7. 디버깅 팁

- `SceneZzTransformTracker`의 로그는 `zz분포`(앞 2자리 히스토그램)로 찍힙니다. 변형 전후 zz 분포가
  의도대로 바뀌고 복구 후 원본 분포로 돌아오는지 확인하세요.
- 배경은 바뀌는데 노트까지 따라 바뀐다면 → `RestoreIdentities`/`RestoreRuntimeObjects`의
  `restored` 카운트가 0이 아닌지, `objId` 매칭이 되는지 확인.
- 노트는 유지되는데 배경이 안 바뀐다면 → `ChangeScene` 경로와 `activeRenderZz`(매니페스트 배경 zz)
  해석(`ResolveInitialRenderZz`)을 확인.
- 보라색(존재하지 않는 `scene_00`) 화면이 뜨면 → 씬 전환 노트가 `scene_00`을 등록하지 않도록
  처리한 부분(`DBStageInfoExperimentChart.Bms.cs`의 SceneToggle 분기)을 확인.

---

## 8. 지향해야 할 결합도 분리 (Decoupled Design Guideline)

현재 구현은 배경(Scene)을 갈아끼우기 위해 게임 데이터(`musicList`)를 직접 변형한 뒤 오브젝트 풀 로드 후 원복하는 **변형→복구(Modify -> Restore)** 차선책을 사용하고 있습니다. 이는 원본 데이터 상태를 인플레이스로 오염시킬 수 있어 잠재적인 상태 오작동 위험을 내포합니다.

가장 이상적인 결합도 분리(Decoupling) 방향은 **데이터를 절대 변형하지 않는 구조**로 설계하는 것입니다.

### 1. 배경 결정 로직 후킹 분리
- 게임 엔진이 현재 실행 중인 스테이지의 씬 배경 리소스를 결정할 때 참조하는 멤버(예: `StageInfo` 혹은 `ActiveScene` 필드)를 파악합니다.
- 배경 전환 지점(예: `SceneChangeController.ChangeScene` 호출 시점 또는 리소스를 매핑하여 실제 로딩하는 메소드)을 타깃팅하여, 게임 데이터 `musicList`를 건드리지 않고 **우리가 주입할 커스텀 배경 번호만 가로채서 주입**합니다.

### 2. 프리팹 맵핑 분리
- 씬 코드(zz)에 따라 노트를 인스턴스화할 때, 노트 생성자가 참조하는 프리팹 매핑(UID -> Prefab 이름) 지점을 하모니 패치로 우회합니다.
- `musicList`의 노트 UID가 원본 BMS 그대로 유지된 상태에서, 게임이 프리팹을 찾으려 할 때만 후킹하여 임시 렌더링용 zz가 포함된 프리팹 이름을 반환하도록 구현합니다.

이러한 분리가 적용된다면 데이터 원본의 순수성이 유지되므로 더 이상 `RestoreIdentities`나 리플렉션을 사용한 복잡한 `RestoreRuntimeObjects` 재귀 탐색 로직이 필요하지 않게 되며, 모드의 견고함과 안정성이 크게 향상될 것입니다.
