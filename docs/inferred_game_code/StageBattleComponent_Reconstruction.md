# StageBattleComponent Inferred Method Body Reconstruction

`Il2CppFormulaBase.StageBattleComponent` 클래스는 Muse Dash 인게임 배틀 세션의 상태 제어, 차트 데이터(`MusicData`) 로드 및 라이프사이클 이벤트(시작, 일시정지, 종료, 재시작, 리소스 해제)를 관장하는 핵심 인게임 관리자입니다.

---

## 🏗 추측된 클래스 구조 (Inferred Field Layout)

```csharp
namespace Il2CppFormulaBase
{
    public class StageBattleComponent : UnityEngine.MonoBehaviour
    {
        // [추측 필드]
        private Il2CppSystem.Collections.Generic.List<MusicData> m_MusicDataList;
        private MusicData m_CurrentMusicData;
        private bool m_IsPaused;
        private bool m_IsBattleEnded;
        private float m_BattleTime;
        private string m_StageUid;

        // 라이프사이클 및 핵심 메서드 바디 추측
    }
}
```

---

## 🔍 핵심 메서드 바디 복원 (Reconstructed Method Bodies)

### 1. `LoadMusicData()`
**연관 패치**: [StageBattleComponent_LoadMusicData_Patch](../../muse%20dash%20test/Patches/Battle/UI/StageBattleComponentPatch.cs#L12-L29)
**역추적 설명**: `DBStageInfo`에서 구성된 `MusicData` 리스트를 전달받아 인게임 틱(Tick) 및 시간 단위로 정렬하고, 각 노트 오브젝트(`noteData`, `configData`)의 프리팹 프리로드 큐를 구축합니다.

```csharp
public void LoadMusicData()
{
    // [추측된 메서드 바디]
    if (this.m_MusicDataList == null)
    {
        this.m_MusicDataList = DBStageInfo.instance.GetRuntimeMusicData();
    }

    if (this.m_MusicDataList != null)
    {
        for (int i = 0; i < this.m_MusicDataList.Count; i++)
        {
            MusicData note = this.m_MusicDataList[i];
            if (note != null && note.noteData != null)
            {
                // showTick = tick - dt * (BPM / 240.0)
                note.showTick = note.tick - (float)(note.dt * (note.bpm / 240.0));
                
                // 보스 이벤트 / 씬 전환 프리팹 예약
                if (note.noteData.type == NoteType.SceneToggle && note.noteData.sceneChangeNames != null)
                {
                    PreloadSceneAssets(note.noteData.sceneChangeNames);
                }
            }
        }
    }
}
```

---

### 2. `InitData()`
**연관 패치**: [StageBattleComponent_InitData_Patch](../../muse%20dash%20test/Patches/Battle/UI/StageBattleComponentPatch.cs#L178-L190)
**역추적 설명**: 곡 UID 정보를 받아 스테이지 초기 세션 변수(점수, 판정 카운트, 피버 게이지, 체력)를 초기화합니다.

```csharp
public void InitData()
{
    // [추측된 메서드 바디]
    this.m_BattleTime = 0f;
    this.m_IsPaused = false;
    this.m_IsBattleEnded = false;
    
    // 점수 및 콤보 보더 초기화
    GameLogic.TaskStageTarget.instance.ResetScoreAndCombo();
    
    // 플레이어 캐릭터 / 파트너 스킬 능력치 적재
    Il2CppPeroPeroGames.GlobalSave.SaveDataManager.instance.ApplyEquippedSkill();
}
```

---

### 3. `Load()`
**연관 패치**: [StageBattleComponent_Load_Patch](../../muse%20dash%20test/Patches/Battle/UI/StageBattleComponentPatch.cs#L192-L212)
**역추적 설명**: 리소스 적재 완료 후 배틀 미디어, 사운드트랙, 인게임 판정 라인 UI 컴포넌트를 준비시키는 메서드입니다.

```csharp
public void Load()
{
    // [추측된 메서드 바디]
    this.LoadMusicData();
    this.PrepareAudioTrack();
    this.PrepareBackgroundSpine();
    
    // 하모니 포스트픽스를 통해 커스텀 배틀 미디어(BGA/BGM 커스텀 주입) 컨트롤러가 트리거됨
}
```

---

### 4. `Pause(bool pauseCoroutine)` & `Resume(bool isExit)`
**연관 패치**: [StageBattleComponent_Pause_Patch](../../muse%20dash%20test/Patches/Battle/UI/StageBattleComponentPatch.cs#L214-L232)
**역추적 설명**: 인게임 시계열 코루틴 및 오디오 트랙 재생 상태를 일시정지하거나 복구합니다.

```csharp
public void Pause(bool pauseCoroutine)
{
    // [추측된 메서드 바디]
    this.m_IsPaused = true;
    UnityEngine.Time.timeScale = 0f;
    AudioMaster.instance.PauseBgm();
    
    if (pauseCoroutine)
    {
        this.StopAllBattleCoroutines();
    }
}

public void Resume(bool isExit)
{
    // [추측된 메서드 바디]
    if (isExit)
    {
        this.Exit();
        return;
    }
    
    this.m_IsPaused = false;
    UnityEngine.Time.timeScale = 1f;
    AudioMaster.instance.ResumeBgm();
}
```

---

### 5. `End()`, `Exit()`, `Release()`, `GameRestart()`
**연관 패치**: [StageBattleComponent Exit/Release Patches](../../muse%20dash%20test/Patches/Battle/UI/StageBattleComponentPatch.cs#L234-L280)
**역추적 설명**: 스테이지 클리어/사망/강제 종료 시 호출되며 세션 정리, 미디어 정지, 결과 창 전환 및 승리 흐름 가드(`VictoryFlowGuard.MarkCompleted()`)를 실행시킵니다.

```csharp
public void Exit()
{
    // [추측된 메서드 바디]
    this.m_IsBattleEnded = true;
    AudioMaster.instance.StopBgm();
    
    // 메모리 정리 및 오브젝트 풀 반환
    ObjectPoolManager.instance.ClearAllPools();
    
    // 메인 메뉴/선곡 화면 씬으로 전환 준비
    SceneFlowController.instance.ChangeScene("Music");
}
```
