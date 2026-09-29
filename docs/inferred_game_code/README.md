# Muse Dash IL2CPP Inferred Game Code Method Body Reconstruction

이 디렉터리는 *Muse Dash*(IL2CPP) 게임 클래스가 **어떤 역할을 하는지**를 모드 패치([muse dash test/Patches](../../muse%20dash%20test/Patches))와 로그에서 거꾸로 짐작해 적은 문서 모음입니다.

> [!WARNING]
> **`CollabExpiration_Reconstruction.md`를 뺀 나머지 5개 문서의 메서드 본문은 실측이 아니라 추측입니다.**
> 게임 본문은 IL2CPP라 볼 수 없고(`SignatureDumper`로 뜬 `Decompiled/`에는 시그니처만 있습니다), 이 문서들의 `[추측된 메서드 바디]` 코드는 클래스 이름과 패치 위치에서 지어낸 의사코드입니다. 점수(300/150), 체력 차감량(20/40), 저장 파일 경로, 호출하는 다른 클래스 이름 같은 **구체적인 값은 근거가 없습니다.** 이 값을 코드나 다른 문서의 근거로 쓰지 마세요.
> 실제 값은 이 저장소의 코드(`muse dash test/`), 실측 로그, [MD2_TAG_RETARGET_MAP.md](../muse-dash-2/MD2_TAG_RETARGET_MAP.md)의 6.7.0 대조표, 그리고 [CHECKLIST.md](../guides/CHECKLIST.md)를 기준으로 하세요.
> 이 문서들과 실제 확인된 사실이 어긋난 곳은 각 문서 안에 "정정" 표시로 고쳐 두었습니다.

## 📌 왜 이런 문서가 있는가
`Decompiled/` 폴더 내 IL2CPP Interop C# 파일은 MelonLoader Interop 생성기로 만들어진 더미 스텁이라 주요 실행 메서드의 바디가 비어 있거나 `throw new NullReferenceException()`으로 처리되어 있습니다. 그래서 초기에는 모드가 게임 내부의 어디를 건드리는지 파악하려고 **"이 메서드는 대략 이런 일을 할 것이다"를 의사코드로 적어 두었습니다.** 지금 기준으로는 "게임이 실제로 이렇게 동작한다"는 근거가 아니라 **읽기 순서를 잡기 위한 스케치**로만 쓰세요.

---

## 🗂 문서 목록 및 매핑 표

| 문서 파일 | 성격 | 대상 원본 클래스 | 문서에 실제로 들어 있는 메서드 |
| :--- | :--- | :--- | :--- |
| [StageBattleComponent_Reconstruction.md](./StageBattleComponent_Reconstruction.md) | ⚠️ 추측 | `Il2CppFormulaBase.StageBattleComponent` | `LoadMusicData()`, `InitData()`, `Load()`, `Pause()`, `Resume()`, `Exit()` (`End`/`Release`/`GameRestart`는 이름만 언급) |
| [GameMusicScene_Reconstruction.md](./GameMusicScene_Reconstruction.md) | ⚠️ 추측 | `Il2CppGameLogic.GameMusicScene` | `Init()`, `PreLoadEnemy()`, `Run()`, `OnPause()`, `OnUnPause()` |
| [DBStageInfo_Reconstruction.md](./DBStageInfo_Reconstruction.md) | ⚠️ 추측 | `Il2CppAssets.Scripts.Database.DBStageInfo` | `SetRuntimeMusicData()`, `GetMusicInfoFromConfig()`, `GetStageInfoByUidAndDiff()` |
| [NoteController_Reconstruction.md](./NoteController_Reconstruction.md) | ⚠️ 추측 | `Il2CppGameLogic.NoteController` | `OnHit()`, `OnMiss()`, `ChangeHealthValue.OnHpDeduct()` |
| [SaveDataManager_Reconstruction.md](./SaveDataManager_Reconstruction.md) | ⚠️ 추측 (모드의 정화 로직 설명은 실제 코드 기준) | `Il2CppAssets.Scripts.PeroTools.Nice.Datas.DataManager` (모드가 후킹하는 대상) | `Save()`, `Load()`, 모드의 `CleanIDataList()`/`CleanStringList()` |
| [CollabExpiration_Reconstruction.md](./CollabExpiration_Reconstruction.md) | ✅ **실측** | `DBConfigDlcUIExtension`, `TimeLimitedItemManager`, `PeroServerTime` | `Deserialize()`, `IsItemInTime()`, `GetServerTime()`/`ResetToLocal()` — 콜라보 종료일 실측 결과 포함 |

---

## 🔬 역추적 핵심 기법 (Methodology)

1. **Harmony Prefix/Postfix 델타 파악**:
   - `Prefix`에서 파라미터(`ref` 포함) 수정 및 `__result` 반환 지점 분석.
   - `Postfix`에서 객체의 `__instance` 필드 변이(State Mutation) 분석.
2. **시간-틱 연산 수식 역산**:
   - `time = tick * 240.0 / bpm` 및 누적 델타 틱 `time = time + (deltaTick * 240.0 / bpm)`, 노트별 `dt` (비행 시간), `showTick` 연산 흐름 복원.
3. **Il2Cpp System Collection 래핑 역분석**:
   - `Il2CppSystem.Collections.Generic.List<MusicData>` 처리 방식 및 오브젝트 리플렉션 필드 역추적.
