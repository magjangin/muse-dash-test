# GameMusicScene Inferred Method Body Reconstruction

`Il2CppGameLogic.GameMusicScene` 클래스는 *Muse Dash*의 인게임 3D/2D 배경 씬, 보스 스테이지 씬 스위칭, Spine 애니메이션 컨트롤러 및 씬 슬롯(Scene Slots)의 동적 프리로드 및 오버레이 관리를 담당하는 핵심 인게임 호스트 컴포넌트입니다.

---

## 🏗 추측된 클래스 구조 (Inferred Field Layout)

```csharp
namespace Il2CppGameLogic
{
    public class GameMusicScene : UnityEngine.MonoBehaviour
    {
        // [추측된 멤버 필드 및 컬렉션]
        public Il2CppSystem.Collections.Generic.List<UnityEngine.GameObject> scenes;
        public Il2CppSystem.Collections.Generic.Dictionary<string, UnityEngine.Animator[]> scenesAnimas;
        public Il2CppSystem.Collections.Generic.Dictionary<int, SceneSubControl> SceneSubCtrls;

        private int m_CurrentSceneIndex;
        private bool m_IsRunning;
    }
}
```

---

## 🔍 핵심 메서드 바디 복원 (Reconstructed Method Bodies)

### 1. `Init()`
**연관 패치**: [GameMusicSceneInitPatch](file:///h:/source/repos/muse%20dash%20test/muse%20dash%20test/Patches/Scene/GameMusicSceneInitPatch.cs)
**역추적 설명**: 인게임 진입 시 현재 스테이지의 기본 배경 씬(Main Scene Prefab)과 보스 씬 슬롯 인스턴스들을 생성하고 초기화합니다.

```csharp
public void Init()
{
    // [추측된 메서드 바디]
    this.scenes = new Il2CppSystem.Collections.Generic.List<UnityEngine.GameObject>();
    this.scenesAnimas = new Il2CppSystem.Collections.Generic.Dictionary<string, UnityEngine.Animator[]>();
    this.SceneSubCtrls = new Il2CppSystem.Collections.Generic.Dictionary<int, SceneSubControl>();

    StageInfo stage = GlobalDataBase.s_StageInfo.GetCurrentStageInfo();
    string baseSceneName = stage.sceneName;

    // 기본 배경 오브젝트 로드 및 씬 슬롯 등록
    UnityEngine.GameObject baseSceneObj = AssetBundleManager.instance.LoadScenePrefab(baseSceneName);
    if (baseSceneObj != null)
    {
        UnityEngine.GameObject inst = UnityEngine.Object.Instantiate(baseSceneObj, this.transform);
        this.scenes.Add(inst);
        this.RegisterSceneAnimators(baseSceneName, inst);
    }
}
```

---

### 2. `PreLoadEnemy()`
**연관 패치**: [GameMusicScenePreLoadEnemyPatch](file:///h:/source/repos/muse%20dash%20test/muse%20dash%20test/Patches/Scene/GameMusicScenePreLoadEnemyPatch.cs)
**역추적 설명**: 차트의 `MusicData` 리스트에 포함된 잡몹 및 보스 몬스터, 배경 장애물의 Spine 애니메이션/스프라이트를 비동기 또는 프레임 분할로 사전 적재합니다.

```csharp
public void PreLoadEnemy()
{
    // [추측된 메서드 바디]
    var musicList = GlobalDataBase.s_StageInfo.musicList;
    if (musicList == null) return;

    for (int i = 0; i < musicList.Count; i++)
    {
        MusicData note = musicList[i];
        if (note != null && note.noteData != null)
        {
            string prefabName = note.noteData.prefab_name;
            if (!string.IsNullOrEmpty(prefabName))
            {
                EnemyPoolManager.instance.PreloadPrefab(prefabName);
            }
        }
    }
}
```

---

### 3. `Run()`
**연관 패치**: [GameMusicScene_Run_Patch](file:///h:/source/repos/muse%20dash%20test/muse%20dash%20test/Patches/Scene/GameMusicSceneRunPatch.cs#L5-L121)
**역추적 설명**: 로딩이 완료된 후 배경 애니메이션 트랙과 스크롤링 루틴을 활성화하는 실제 스테이지 스타트 실행 메서드입니다.

```csharp
public void Run()
{
    // [추측된 메서드 바디]
    this.m_IsRunning = true;

    // 활성화된 모든 씬 슬롯의 애니메이터 및 씬 서브 컨트롤 활성화
    for (int i = 0; i < this.scenes.Count; i++)
    {
        UnityEngine.GameObject sceneObj = this.scenes[i];
        if (sceneObj != null)
        {
            sceneObj.SetActive(true);
        }
    }

    // 씬 스크롤러 및 배경 미디어 동기화 코루틴 시작
    this.StartCoroutine(this.SceneScrollRoutine());
}
```

---

### 4. `OnPause()` & `OnUnPause()`
**역추적 설명**: 배틀 일시정지 시 씬 내부 Spine 애니메이터의 `speed`를 0으로 설정하거나 원래 속도로 복구시킵니다.

```csharp
public void OnPause()
{
    // [추측된 메서드 바디]
    var enumerator = this.scenesAnimas.GetEnumerator();
    while (enumerator.MoveNext())
    {
        UnityEngine.Animator[] anims = enumerator.Current.Value;
        if (anims != null)
        {
            for (int i = 0; i < anims.Length; i++)
            {
                if (anims[i] != null) anims[i].speed = 0f;
            }
        }
    }
}

public void OnUnPause()
{
    // [추측된 메서드 바디]
    var enumerator = this.scenesAnimas.GetEnumerator();
    while (enumerator.MoveNext())
    {
        UnityEngine.Animator[] anims = enumerator.Current.Value;
        if (anims != null)
        {
            for (int i = 0; i < anims.Length; i++)
            {
                if (anims[i] != null) anims[i].speed = 1f;
            }
        }
    }
}
```
