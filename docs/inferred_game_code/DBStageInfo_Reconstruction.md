# DBStageInfo Inferred Method Body Reconstruction

`Il2CppAssets.Scripts.Database.DBStageInfo` 클래스는 *Muse Dash*의 전체 채보(Chart), 곡 앨범 정보(`MusicTagMetaData`), 난이도별 데이터 및 실행 시점 `musicList` 파싱 데이터베이스를 관리하는 핵심 싱글톤 데이터 레이어입니다.

---

## 🏗 추측된 클래스 구조 (Inferred Field Layout)

```csharp
namespace Il2CppAssets.Scripts.Database
{
    public class DBStageInfo : Il2CppSystem.Object
    {
        public static DBStageInfo instance;

        // [추측된 필드]
        public Il2CppSystem.Collections.Generic.List<MusicData> musicList;
        public Il2CppSystem.Collections.Generic.Dictionary<string, StageInfo> m_AllStageInfo;
        public Il2CppSystem.Collections.Generic.Dictionary<string, MusicTagMetaData> m_AllMusicTagMetaData;
        private string m_CurrentSelectUid;
        private int m_CurrentDifficulty;
    }
}
```

---

## 🔍 핵심 메서드 바디 복원 (Reconstructed Method Bodies)

### 1. `SetRuntimeMusicData()`
**연관 패치**: [DBStageInfo_SetRuntimeMusicData_Patch](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoPatch.cs#L8-L120)
**역추적 설명**: 선택된 곡 UID 및 난이도 번호(Easy: 1, Hard: 2, Master: 3 등)에 따라 내장 텍스트/JSON 맵(bms 파일데이터)을 로드하여 `MusicData` 객체 리스트인 `musicList`를 구성합니다.

```csharp
public void SetRuntimeMusicData(string uid, int difficulty)
{
    // [추측된 메서드 바디]
    this.m_CurrentSelectUid = uid;
    this.m_CurrentDifficulty = difficulty;

    // 1. 해당 곡의 StageInfo 검색
    StageInfo stage = this.GetStageInfoByUidAndDiff(uid, difficulty);
    if (stage == null) return;

    // 2. BMS / Json 채보 원문 텍스트 로드
    string chartJson = AssetBundleManager.instance.LoadChartText(stage.bmsName);

    // 3. BMS 파싱 및 MusicData 리스트 구축
    this.musicList = BMSParser.ParseToMusicDataList(chartJson, stage.bpm);

    // 4. 모드 하모니 포스트픽스(SetRuntimeMusicData_Patch)에서
    //    커스텀 BMS/실험 노트 주입(ApplyExperimentChart)이 실행됨
}
```

---

### 2. `GetMusicInfoFromConfig()`
**역추적 설명**: 곡 설정(BPM, 곡 제목, 아티스트, 커넥션 씬 파라미터) 메타데이터를 캐시에서 조회하거나 새로 가져옵니다.

```csharp
public MusicTagMetaData GetMusicInfoFromConfig(string uid)
{
    // [추측된 메서드 바디]
    if (this.m_AllMusicTagMetaData != null && this.m_AllMusicTagMetaData.ContainsKey(uid))
    {
        return this.m_AllMusicTagMetaData[uid];
    }

    // 기본 대체 메타데이터 반환
    MusicTagMetaData defaultData = new MusicTagMetaData();
    defaultData.musicUid = uid;
    defaultData.name = "Unknown Music";
    defaultData.author = "Unknown Artist";
    defaultData.bpm = 120.0f;
    return defaultData;
}
```

---

### 3. `GetStageInfoByUidAndDiff()`
**역추적 설명**: 곡 UID 및 난이도 키 조합(`{uid}_{diff}`)을 통해 스테이지 구성 정보(배경 씬 이름, 보스 이름, 프리팹 맵핑)를 가져옵니다.

```csharp
public StageInfo GetStageInfoByUidAndDiff(string uid, int diff)
{
    // [추측된 메서드 바디]
    string key = uid + "_" + diff.ToString();
    if (this.m_AllStageInfo != null && this.m_AllStageInfo.ContainsKey(key))
    {
        return this.m_AllStageInfo[key];
    }
    return null;
}
```
