# Muse Dash IL2CPP Inferred Game Code Method Body Reconstruction

이 디렉토리는 *Muse Dash* (IL2CPP / Mono 하이브리드 어셈블리)의 주요 원본 게임 클래스 및 메서드 바디(Method Body)를 모드 패치([muse dash test/Patches](file:///h:/source/repos/muse%20dash%20test/muse%20dash%20test/Patches)), 리플렉션 훅, BMS 타이밍 연산식, 하모니 패치(Prefix/Postfix/Transpiler) 및 IL2CPP Interop 스텁 분석을 통해 **역추적 및 재구성(Reconstruction)한 C# pseudo-code 및 실행 흐름 문서**입니다.

## 📌 왜 메서드 바디 재구성이 필요한가?
`Decompiled/` 폴더 내 IL2CPP Interop C# 파일은 Cpp2IL / MelonLoader Interop 생성기로 만들어진 더미 스텁 어셈블리입니다. 따라서 주요 실행 메서드는 내부 바디가 비어 있거나 `throw new NullReferenceException()`으로 처리되어 있습니다.

본 디렉토리의 문서들은 모드가 게임 내부의 어디를 조작하고, 원본 게임 로직이 어떻게 반환값을 계산하고 상태를 갱신하는지 완벽히 파악할 수 있도록 **원본 Il2Cpp 메서드 바디 및 내부 헬퍼 로직을 복원**하여 기술합니다.

---

## 🗂 문서 목록 및 맵핑 표

| 문서 파일 | 대상 원본 클래스 | 핵심 역추적 메서드 바디 |
| :--- | :--- | :--- |
| [StageBattleComponent_Reconstruction.md](./StageBattleComponent_Reconstruction.md) | `Il2CppFormulaBase.StageBattleComponent` | `LoadMusicData()`, `InitData()`, `Load()`, `Pause()`, `Resume()`, `End()`, `Exit()`, `Release()`, `GameRestart()` |
| [GameMusicScene_Reconstruction.md](./GameMusicScene_Reconstruction.md) | `Il2CppGameCore.Host.GameMusicScene` | `Init()`, `PreLoadEnemy()`, `Run()`, `OnPause()`, `OnUnPause()`, `Exit()`, `Update()` |
| [DBStageInfo_Reconstruction.md](./DBStageInfo_Reconstruction.md) | `Il2CppAssets.Scripts.Database.DBStageInfo` | `SetRuntimeMusicData()`, `GetMusicData()`, `GetMusicInfoFromConfig()`, `MusicTagMetaData` 파싱/바인딩 |
| [NoteController_Reconstruction.md](./NoteController_Reconstruction.md) | `Il2CppGameLogic.NoteController` / `TaskStageTarget` | `OnHit()`, `OnMiss()`, `ChangeHealthValue()`, `CalculateJudge()`, `TickToTime()` |
| [SaveDataManager_Reconstruction.md](./SaveDataManager_Reconstruction.md) | `Il2CppPeroPeroGames.GlobalSave.SaveDataManager` | `Init()`, `Save()`, `Load()`, `UnlockStage()`, `VerifySkillAndItem()` |
| [CollabExpiration_Reconstruction.md](./CollabExpiration_Reconstruction.md) | `DBConfigDlcUIExtension`, `TimeLimitedItemManager`, `PeroServerTime` | `Deserialize()`, `IsItemInTime()`, `GetServerTime()`/`ResetToLocal()` — 콜라보 종료일 실측 결과 포함 |

---

## 🔬 역추적 핵심 기법 (Methodology)

1. **Harmony Prefix/Postfix 델타 파악**:
   - `Prefix`에서 파라미터(`ref` 포함) 수정 및 `__result` 반환 지점 분석.
   - `Postfix`에서 객체의 `__instance` 필드 변이(State Mutation) 분석.
2. **시간-틱 연산 수식 역산**:
   - `time = tick * 240.0 / bpm` 및 노트별 `dt` (비행 시간), `showTick` 연산 흐름 복원.
3. **Il2Cpp System Collection 래핑 역분석**:
   - `Il2CppSystem.Collections.Generic.List<MusicData>` 처리 방식 및 오브젝트 리플렉션 필드 역추적.
