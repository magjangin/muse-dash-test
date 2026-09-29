# DataManager & Save System Inferred Method Body Reconstruction

> [!WARNING]
> **추측 문서입니다.** `Save()`/`Load()`의 본문(직렬화 방식, 암호화, 저장 경로)은 실측이 아닌 의사코드입니다([안내](README.md)). 모드의 정화 로직(`CleanIDataList`/`CleanStringList`)만 실제 코드([SaveDataManagerPatch.cs](../../muse%20dash%20test/Patches/Database/Save/SaveDataManagerPatch.cs))를 옮긴 것입니다.
> **정정**: 실제 세이브 파일은 `game.sav`가 아니라 `%LOCALAPPDATA%\Steam\MuseDash\<steamID>MuseDashSaves.sav`이고 Steam Cloud로 동기화됩니다([LOGGING_AND_TROUBLESHOOTING.md](../guides/LOGGING_AND_TROUBLESHOOTING.md#세이브-데이터-위치와-진행도-초기화)). `Application.persistentDataPath`(`…\LocalLow\PeroPeroGames\MuseDash`)에는 로그/캐시만 있습니다. 모드가 후킹하는 대상은 `SaveDataManager`가 아니라 **`DataManager.Save`** 입니다. 정화 대상은 `1999-` 접두사 하나입니다(`CustomContentIds.IsVirtualContent`).

`Il2CppAssets.Scripts.PeroTools.Nice.Datas.DataManager` 및 `Il2CppPeroPeroGames.GlobalSave.SaveDataManager` 클래스는 *Muse Dash*의 플레이어 진행 상황(업적, 곡 최고점수, 최근 플레이 기록 `highest`, `recentPassLevelData`, 난이도별 클리어 `easy_pass`, `hard_pass`, `master_pass`, 언락 아이템)을 디스크 JSON/바이너리로 암호화하여 저장하고 복원하는 영구 저장소 매니저입니다.

---

## 🏗 추측된 클래스 구조 (Inferred Field Layout)

```csharp
namespace Il2CppAssets.Scripts.PeroTools.Nice.Datas
{
    public class DataManager : Il2CppSystem.Object
    {
        public static DataManager instance;

        // [추측된 멤버 필드 및 계층 구조]
        public Il2CppSystem.Collections.Generic.Dictionary<string, SubData> datas;
        private string m_SaveFilePath;
    }

    public class SubData : Il2CppSystem.Object
    {
        public Il2CppSystem.Collections.Generic.Dictionary<string, Variable> fields;
    }
}
```

---

## 🔍 핵심 메서드 바디 복원 (Reconstructed Method Bodies)

### 1. `Save()`
**연관 패치**: [SaveDataManagerPatch](../../muse%20dash%20test/Patches/Database/Save/SaveDataManagerPatch.cs#L15-L218)
**역추적 설명**: 인메모리의 `datas` 컬렉션(Account, Task, IAP, Achievement)을 순회하며 JSON/AES 디스크 바이트 배열로 직렬화(Serialize)하여 로컬 `.sav` 파일에 기록합니다.

```csharp
public void Save()
{
    // [추측된 메서드 바디]
    // 모드 하모니 프리픽스(SaveDataManagerPatch.Prefix)에서 
    // 커스텀/가상 차트("1999-" 접두사) 기록을 사전에 정밀 정화(Cleanse)함.

    if (this.datas == null) return;

    // 1. datas 컬렉션을 내장 JSON / BSON 객체 형태로 변환
    string jsonString = JsonMapper.ToJson(this.datas);

    // 2. 세이브 데이터 암호화 (AES 또는 커스텀 XOR 키 처리)
    byte[] encryptedData = SecurityUtils.EncryptSaveData(jsonString);

    // 3. 파일에 동기/비동기 쓰기 (정정: 실제 경로는 위 안내 참고. persistentDataPath\game.sav가 아님)
    string savePath = System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, "game.sav");
    System.IO.File.WriteAllBytes(savePath, encryptedData);
}
```

---

### 2. `Load()`
**역추적 설명**: 세이브 파일에서 데이터를 읽어와 복호화한 후 메모리 내 `datas` 컬렉션으로 복원합니다.

```csharp
public void Load()
{
    // [추측된 메서드 바디]
    string savePath = System.IO.Path.Combine(UnityEngine.Application.persistentDataPath, "game.sav");
    if (!System.IO.File.Exists(savePath))
    {
        this.InitDefaultSaveData();
        return;
    }

    byte[] encryptedData = System.IO.File.ReadAllBytes(savePath);
    string jsonString = SecurityUtils.DecryptSaveData(encryptedData);

    this.datas = JsonMapper.ToObject<Il2CppSystem.Collections.Generic.Dictionary<string, SubData>>(jsonString);
}
```

---

### 3. `CleanIDataList()` & `CleanStringList()` (세이브 오염 방지 가드)
**역추적 설명**: `Achievement.highest` 및 `recentPassLevelData` 내에 커스텀 차트 Uid가 저장되어 순정 데이터베이스를 오염시키는 것을 막기 위해 하모니 프리픽스에서 가상 Uid 검사를 수행하는 로직입니다.

```csharp
private static int CleanIDataList(Il2CppSystem.Collections.Generic.List<IData> list)
{
    if (list == null) return 0;
    int removedCount = 0;

    for (int i = list.Count - 1; i >= 0; i--)
    {
        var item = list[i];
        if (item == null) continue;

        var songResult = item.TryCast<SavedSongResult>();
        if (songResult != null && CustomContentIds.IsVirtualContent(songResult.uid))
        {
            list.RemoveAt(i);
            removedCount++;
        }
    }
    return removedCount;
}
```
