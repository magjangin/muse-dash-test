# 🗺️ 태그/카테고리 시스템 — 뮤즈 대시 2 재타깃팅 지도

> **이 문서의 목적**: 뮤즈 대시 2가 나왔을 때, 태그/카테고리 부분을 **처음부터 다시 짜지 않고**, 이 표의 "MD2 이름" 칸만 ILSpy로 채우면 되도록 만든 대조 지도입니다.
> 태그 시스템은 모드에서 **가장 많은 게임 타입(약 7개)·멤버(약 20개)** 에 묶여 있어 제일 어렵습니다. 그래서 따로 지도를 둡니다.

---

## 🧭 먼저 큰 그림 — 태그 시스템이 하는 4가지 "임무"

```
 게임 부팅
    │
    ▼
[임무 A] 태그 탭 만들어 등록        ← MusicTagManager.InitAlbumTagInfo 후크
    │   "실험 모드" 카테고리를 게임 태그 목록에 끼워 넣는다
    ▼
[임무 B] 가상 곡/앨범을 DB에 심기    ← 순정 곡/앨범을 얇게 복제(MemberwiseClone)
    │   1999-x 곡, 1998-0 앨범을 만들어 글로벌 DB에 주입
    ▼
[임무 C] 조회 시 안 터지게 우회      ← DBConfigAlbums 조회 메서드 Prefix 후크
    │   게임이 1999-/1998- UID를 찾을 때 NullReference로 죽지 않게 우리 데이터 반환
    ▼
[임무 D] 태그 아이콘 이미지 덮어쓰기  ← AlbumTagToggle 후크
        탭에 우리 tag_icon.png를 강제로 표시
```

> **핵심 직관**: A는 "등록", B는 "데이터 만들기", C는 "게임이 안 죽게 받쳐주기", D는 "겉모습 입히기".
> MD2에서 **A·C·D는 후크(메서드 이름)가 바뀌어 다시 찾아야** 하고, **B는 데이터 타입(필드 이름)이 바뀌어 칸을 갈아끼워야** 합니다.

---

## ⚠️ 깨질 위험도 신호등 (어디부터 의심할지)

| 신호 | 의미 | 이 지도에서 해당되는 것 |
|------|------|------------------------|
| 🔴 | **가장 많이 바뀜.** UI 계층 + MD2 Live2D 재설계 직격 | `AlbumTagToggle`, `PnlMusicTag` (임무 D) |
| 🟡 | **이름은 바뀌어도 구조는 비슷.** 데이터 DB 계층 | `GlobalDataBase.dbMusicTag`, `DBConfigAlbums`, `MusicInfo` (임무 B·C) |
| 🟢 | **거의 그대로 갈 가능성.** 패턴/유틸 | `Singleton<ConfigManager>`, `MemberwiseClone` 방식 |

---

## 📋 임무 A — 태그 탭 만들어 등록

**파일**: [CustomTagPatch.cs](../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagPatch.cs), [CustomTagRegistry.cs](../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagRegistry.cs)

| # | MD1 게임 타입 | MD1 멤버 (메서드/필드) | 종류 | 우리가 하는 일 | 위험 | **MD2 이름 (← 채울 칸)** |
|---|---------------|------------------------|------|----------------|------|--------------------------|
| A1 | `MusicTagManager` | `InitAlbumTagInfo()` | 후크(Postfix) | 태그 등록 진입점 | 🟡 | |
| A2 | `MusicTagManager` | `InitDatas()` | 후크(Postfix) | `m_MaxAlbumUid` 보정(1000앨범 렉 방지) | 🟡 | |
| A3 | `GlobalDataBase` | `.dbMusicTag` | 필드 | 태그 DB 핸들 | 🟡 | |
| A4 | `dbMusicTag` | `AddAlbumTagData(int, AlbumTagInfo)` | 호출 | 태그 데이터 최종 등록 | 🟡 | |
| A5 | `dbMusicTag` | `AddCustomAlbumTagsSort(int)` | 호출 | 정렬 목록에 태그 추가 | 🟡 | |
| A6 | `dbMusicTag` | `AllAlbumTagsSortContains(int)` | 호출 | 중복 등록 방지 검사 | 🟡 | |
| A7 | `AlbumTagInfo` | `name`, `tagUid`, `iconName` | 필드 | 태그 기본 정보 세팅 | 🟡 | |
| A8 | `AlbumTagInfo` | `InitCustomTagInfo(CustomTagInfo)` | 호출 | 다국어/아이콘/곡목록 주입 | 🟡 | |
| A9 | `AlbumTagInfo` | `SetTagUids(List<string>)`, `m_MusicUids`, `m_DisplayMusicUids`, `m_AlbumsInfos` | 메서드/필드 | 태그-곡 바인딩 | 🟡 | |
| A10 | `DBConfigCustomTags.CustomTagInfo` | `tag_name`(Dict), `tag_picture`, `music_list` | 필드 | 다국어명 + 아이콘 경로 + 곡목록 | 🟡 | |
| A11 | `AlbumDisplayMusic` | 생성자(`AlbumsInfo`), `AddRangeMusicUid(List<string>)` | 타입 | 태그 화면 곡 스크롤 데이터 | 🟡 | |

> **다국어 주의** ([가이드 Phase 2 경고](MUSE_DASH_2_SPECULATIVE_GUIDE.md)): A10의 `tag_name` 딕셔너리 키(`Korean`, `English`, `Japanese`, `ChineseSimplified`, `ChineseTraditional`)가 MD2 언어 코드와 정확히 일치해야 탭 라벨이 빈 문자열로 안 뜸.

---

## 🔑 핵심 추론 — "태그 제목"은 어떻게 화면에 뜨게 됐는가

> 임무 A에서 제일 이해하기 어려운 부분. 코드를 보면 제목이 **두 군데**(`AlbumTagInfo.name` 과 `CustomTagInfo.tag_name`)에 들어가는데, **왜 둘 다 필요한지**가 핵심입니다. 그냥 외운 게 아니라 아래 추론으로 도달한 결과입니다.

### 1단계 — 처음의 잘못된 가정과 증상

처음엔 당연히 이렇게 생각합니다:
```csharp
var info = new AlbumTagInfo { name = "실험 모드" };   // ← 제목을 직접 박으면 되겠지?
```
**결과: 탭이 뜨긴 하는데 라벨이 빈 칸("")으로 나옴.**
→ 여기서 깨닫는 사실: **`name` 문자열은 화면이 직접 그리는 값이 아니다.** 게임 UI는 다른 곳에서 제목을 가져온다.

### 2단계 — "그럼 진짜 제목은 어디서 오는가?" (ILSpy 추적)

게임의 **순정 태그**가 정의된 곳(`DBConfigCustomTags`)을 ILSpy로 까보면, 순정 태그 제목이 단일 문자열이 아니라:
```
CustomTagInfo.tag_name = Dictionary<언어코드, 번역된이름>
   { "Korean":"기본 곡", "English":"Default", "Japanese":"...", ... }
```
**언어별 딕셔너리**로 들어 있습니다. 그리고 라벨을 그리는 UI 코드는 `tag_name[현재_시스템_언어]` 를 조회해서 화면에 찍습니다.

➡️ **추론 도달점**: 태그 제목은 *저장 시점*이 아니라 **\*표시 시점에 현재 언어로 번역되어 결정\*** 된다. 단일 문자열을 박으면 게임이 "이 태그엔 언어 사전이 없네 → 빈 칸" 으로 처리한 것이다.

### 3단계 — 그래서 만든 해법 (코드의 실제 흐름)

이 추론대로, 제목이 뜨게 하려면 **언어 사전을 만들어 게임이 찾아갈 자리에 꽂아야** 합니다:

```
① CreateTagLanguages()  →  { Korean:"실험 모드", English:"Experiment Mod", 日:"実験モード", 简:"实验模式", 繁:"實驗模式" }
                            (게임이 쓰는 언어코드 키와 똑같이 맞춤 ★)
                            out defaultName = "Experiment Mod" (English 값)
        │
        ▼
② new AlbumTagInfo { name = defaultName }   ←  ⚠️ 폴백용. 사전 조회가 실패할 때 대비한 비상 문자열
        │
        ▼
③ new CustomTagInfo { tag_name = ①의 사전 }  ←  ★진짜 제목의 출처. 언어별 번역 묶음
        │
        ▼
④ info.InitCustomTagInfo(customInfo)         ←  ★결정적 한 줄.
                                                 태그가 "내 제목은 이 언어 사전에서 가져와라"라고 내부 배선됨
        │
        ▼
⑤ AddAlbumTagData(TagUid, info)              ←  배선 끝난 태그를 글로벌 DB에 등록 → UI가 그릴 때 ④의 사전을 조회
```

### 4단계 — "왜 `name` 과 `tag_name` 둘 다 있나"의 최종 답

| 필드 | 역할 | 누가 읽나 |
|------|------|-----------|
| `CustomTagInfo.tag_name`(사전) | **실제로 화면에 뜨는 제목.** 현재 언어로 번역 | UI 라벨 그리기 코드 (정상 경로) |
| `AlbumTagInfo.name`(단일 문자열) | **폴백.** 현재 언어 키가 사전에 없을 때만 사용 | 조회 실패 시 비상 경로 |

➡️ 즉 **`tag_name`이 본진, `name`은 안전망.** 한국어로 게임을 켜면 `tag_name["Korean"]="실험 모드"`가 뜨고, 만약 우리가 안 넣은 언어(예: 프랑스어)로 켜면 `name="Experiment Mod"`로 떨어집니다.

### 5단계 — 증상으로 역추적하는 법 (MD2에서 그대로 쓸 진단)

- **탭은 뜨는데 라벨이 빈 칸("")** → `tag_name` 사전에 **현재 게임 언어 키가 없음**. (MD2가 언어코드를 `Korean`→`ko` 식으로 바꿨을 수 있음 → ILSpy로 언어 매니저의 키 형식부터 확인)
- **탭 자체가 안 뜸** → 제목 문제가 아니라 ④ `InitCustomTagInfo` 또는 ⑤ `AddAlbumTagData`가 실패 (임무 A의 후크가 안 걸린 것)

> **MD2 이식 시 이 추론의 의미**: 제목 로직 자체는 "언어 사전 + InitCustomTagInfo"라는 **패턴이 그대로 갈 가능성이 높습니다(🟡)**. MD2에서 새로 확인할 것은 딱 두 개 — **(a) 언어코드 키 형식**(`Korean` vs `ko` vs 정수 enum)과 **(b) `InitCustomTagInfo` 대응 메서드 이름**뿐입니다. 즉 "제목이 왜 뜨는가"의 원리는 재사용되고, 채울 칸만 두 개입니다.

---

## 📋 임무 B — 가상 곡/앨범을 DB에 심기

**파일**: [CustomTagRegistrySupport.cs](../muse%20dash%20test/Patches/UI/Custom/Tags/Support/CustomTagRegistrySupport.cs)

| # | MD1 게임 타입 | MD1 멤버 | 종류 | 우리가 하는 일 | 위험 | **MD2 이름 (← 채울 칸)** |
|---|---------------|----------|------|----------------|------|--------------------------|
| B1 | `Singleton<ConfigManager>` | `.instance.GetConfigObject<DBConfigAlbums>()` | 패턴 | 앨범 설정 객체 획득 | 🟢 | |
| B2 | `DBConfigAlbums` | `.m_Items` (List<AlbumsInfo>) | 필드 | 가상 앨범을 추가할 리스트 | 🟡 | |
| B3 | `DBConfigAlbums.AlbumsInfo` | `MemberwiseClone()` → `TryCast` | 방식 | **Live2D 꼬임 방지 얇은 복제** | 🟢 | |
| B4 | `AlbumsInfo` | `uid`, `title`, `tag`, `jsonName`, `prefabsName`, `m_AlbumExInfo` | 필드(래퍼) | 복제본 메타데이터 교체 | 🟡 | |
| B5 | `MusicInfo` | `MemberwiseClone()` → `TryCast` | 방식 | 순정 곡 얇은 복제 | 🟢 | |
| B6 | `MusicInfo` | `uid`, `name`, `author`, `levelDesigner`, `difficulty1~5` | 필드(래퍼) | 가상 곡 메타데이터 | 🟡 | |
| B7 | `MusicInfo` | `callBackDifficulty1~5` | 리플렉션 | 난이도 콜백값 | 🟡 | |
| B8 | `MusicInfo` | `albumUidName`, `albumIndex`, `albumJsonIndex`, `albumJsonName`, `m_MusicExInfo` | 리플렉션 | 곡↔앨범 연결 마스크 | 🟡 | |
| B9 | `dbMusicTag` | `.m_AllMusicInfo` (Dictionary<string,MusicInfo>) | 필드 | **가상 곡을 실제로 심는 곳** | 🟡 | |
| B10 | `dbMusicTag` | `GetMusicInfoFromAll(string)` | 호출 | 주입 후 검증 + 원본 곡 탐색 | 🟡 | |
| B11 | (상품 메타) | `needPurchase`, `free`, `pay_ids`, `dlc` | 리플렉션 | **복제본에서만** DLC 식별자 제거 | 🟡 | |

> **B11 경계 검증 필수**([가이드 Phase 2.1](MUSE_DASH_2_SPECULATIVE_GUIDE.md)): 이 정리 함수는 **새로 만든 커스텀 객체에만** 호출돼야 함. 원본/구매상태/DLC 소유권을 건드리는 게 아님. MD2 이식 시 호출 경계 반드시 재확인.

---

## 📋 임무 C — 조회 시 안 터지게 우회 (★게임이 죽지 않게 하는 안전망)

**파일**: [CustomTagPatch.AlbumPatches.cs](../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagPatch.AlbumPatches.cs), [CustomTagPatch.cs](../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagPatch.cs)

| # | MD1 게임 타입 | MD1 멤버 | 종류 | 우리가 하는 일 | 위험 | **MD2 이름 (← 채울 칸)** |
|---|---------------|----------|------|----------------|------|--------------------------|
| C1 | `DBConfigAlbums` | `GetAlbumInfoByMusicInfo(MusicInfo)` | 후크(Prefix) | 1999- 곡 → 커스텀 앨범 반환 | 🟡 | |
| C2 | `DBConfigAlbums` | `GetAlbumsInfoByUid(string)` | 후크(Prefix) | 1998-0 → 커스텀 앨범 반환 | 🟡 | |
| C3 | `DBConfigAlbums` | `GetAlbumIndexByUid(string)` | 후크(Prefix) | 1998-0 → TagUid 인덱스 반환 | 🟡 | |
| C4 | `MusicTagManager` | `RefreshStageDisplayMusics(int)` | 후크(Prefix) | 커스텀 tagIndex일 때 원본 실행 차단(NRE 방지) | 🟡 | |

> **임무 C의 패턴은 가이드의 "② 조회/반환(Getter) 계열" 그대로**입니다. MD2에서도 `Get...By...`, `Find...` 키워드로 ILSpy 검색하면 대응물이 나옵니다.

---

## 📋 임무 D — 태그 아이콘 이미지 덮어쓰기 (🔴 MD2에서 가장 많이 깨질 곳)

**파일**: [AlbumTagTogglePatch.cs](../muse%20dash%20test/Patches/UI/Custom/Tags/AlbumTagTogglePatch.cs), [PnlMusicTagPatch.cs](../muse%20dash%20test/Patches/UI/Music/PnlMusicTagPatch.cs)

| # | MD1 게임 타입 | MD1 멤버 | 종류 | 우리가 하는 일 | 위험 | **MD2 이름 (← 채울 칸)** |
|---|---------------|----------|------|----------------|------|--------------------------|
| D1 | `Il2Cpp.AlbumTagToggle` | `Init()` | 후크(Postfix) | 가상 태그 감지 + 아이콘 교체 시작 | 🔴 | |
| D2 | `AlbumTagToggle` | `SetIconAsync(ref Texture2D)` | 후크(Prefix) | 텍스처를 커스텀으로 스왑 | 🔴 | |
| D3 | `AlbumTagToggle` | `SetStateIcon(bool, bool)` | 후크(Postfix) | 상태 갱신 시 텍스처 재스왑 | 🔴 | |
| D4 | `AlbumTagToggle` | `m_IconImg` (RawImage) | 필드 | 실제 텍스처를 박는 대상 | 🔴 | |
| D5 | `PnlMusicTag` | `RefreshScrollViewItem()` | 후크(Postfix) | 태그 스크롤 항목 표시 | 🔴 | |

> 🔴 이유: D는 전부 **로비 UI 컴포넌트**입니다. MD2 트레일러의 **Live2D 앨범 커버** = 앨범/태그 UI가 거의 확실히 재설계됨. `AlbumTagToggle`이라는 타입 자체가 사라지거나 완전히 새 구조일 수 있어, **여기는 "이름 바꾸기"가 아니라 "메커니즘 다시 분석"이 필요할 가능성**이 높습니다. → MD2 작업 시 **가장 먼저, 가장 많은 시간을 잡을 곳**.

---

## ✅ MD2 덤프 뜨면 할 일 (순서대로)

1. **[임무 B·C 먼저]** 데이터 계층(🟡)은 구조가 비슷하니 ILSpy에서 `dbMusicTag` / `DBConfigAlbums` / `MusicInfo` 대응 타입을 찾아 위 표의 빈칸을 채운다. → 모드가 **컴파일되는 상태** 회복.
2. **[임무 A]** `InitAlbumTagInfo` 대응 진입점(`Init...`, `AddAlbumTag...` 키워드)을 찾아 태그 등록 후크를 복원한다.
3. **[임무 D 마지막]** 🔴 UI 부분은 Live2D 재설계를 감안해 **구조부터 다시 분석**한다. 아이콘 안 떠도 곡은 돌아가므로 우선순위 최하위.
4. 각 칸을 채울 때마다 [CustomTagRegistry.cs](../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagRegistry.cs)의 디버그 로그(`[성공] m_AllMusicInfo 맵에...`, `[대성공] GetMusicInfoFromAll...`)로 단계별 검증.

> **요약**: 이 지도에서 🟡(데이터)는 "칸 채우기"로 끝나고, 🔴(UI 아이콘)만 진짜 재분석이 필요합니다. 즉 **"태그 전체를 다시 짠다"가 아니라 "아이콘 부분만 다시 분석 + 나머지는 이름 교체"** 가 현실적인 규모입니다.

---

## 📋 정확도 보정 및 결과 화면(AP) 연출 재타깃팅 지도

**파일**: [APModPatch.cs](../muse%20dash%20test/Patches/APModPatch.cs)

| # | MD1 게임 타입 | MD1 멤버 (메서드/필드) | 종류 | 우리가 하는 일 | 위험 | **MD2 이름 (← 채울 칸)** |
|---|---|---|---|---|---|---|
| C1.1 | `TaskStageTarget` | `GetAccuracy()` | 후크(Postfix) | 3자리 반올림 전체 정확도 반환 | 🟡 | |
| C1.2 | `TaskStageTarget` | `GetTrueAccuracy()` | 후크(Postfix) | 일반 노트 정밀 정확도 반환 | 🟡 | |
| C1.3 | `TaskStageTarget` | `GetTrueAccuracyNew()` | 후크(Postfix) | 전체 오브젝트 정밀 정확도 반환 | 🟡 | |
| C1.4 | `TaskStageTarget` | `AddScore(int, int, string, ...)` | 후크(Prefix) | `TaskStageTarget` 캐싱 및 HUD 폰트 가로채기 | 🟡 | |
| C1.5 | `TaskStageTarget` | `IsFullCombo()` | 후크(Postfix) | 결과창 노출 전 `TaskStageTarget` 인스턴스 백업 | 🟡 | |
| C1.6 | `TaskStageTarget` | `m_PerfectResult`, `m_GreatResult`, `m_MissResult`, `m_JumpOverResult`, `m_EnergyCount`, `m_BluePoint` | 필드 | 판정 변수 값 조회 (분자/분모 산출) | 🟡 | |
| C1.7 | `PnlVictory2dManager` | `OnShowVictory()` | 후크(Postfix) | AP 판정 검증 및 Custom AP 골드 배너 동적 주입 | 🔴 | |
| C1.8 | `PnlBattle` | `instance.currentComps.scoreValue` | UI 경로 | 팝아트풍 폰트(`LuckiestGuy-Regular`) 리소스 추출 | 🔴 | |

---

## 📋 체력바 커스텀 및 수치 변경 감지 재타깃팅 지도

**파일**: [ChangeHealthValuePatch.cs](../muse%20dash%20test/Patches/UI/Custom/HpMod/ChangeHealthValuePatch.cs), [HywStageManager.cs](../muse%20dash%20test/Patches/UI/Custom/HpMod/HywStageManager.cs)

| # | MD1 게임 타입 | MD1 멤버 (메서드/필드) | 종류 | 우리가 하는 일 | 위험 | **MD2 이름 (← 채울 칸)** |
|---|---|---|---|---|---|---|
| H1.1 | `ChangeHealthValue` | `OnGameStart()` | 후크(Postfix) | 배틀 시작 시 체력 텍스트 스타일러 강제 시동 | 🟡 | |
| H1.2 | `ChangeHealthValue` | `OnHpRateChange(float)` | 후크(Postfix) | 체력 비율 변경 시 체력바 텍스트 및 서식 갱신 | 🟡 | |
| H1.3 | `ChangeHealthValue` | `OnHpDeduct(int)` | 후크(Postfix) | 피해 입을 시 스타일 갱신 및 유지 | 🟡 | |
| H1.4 | `ChangeHealthValue` | `OnHpAdd(int)` | 후크(Postfix) | 회복 시 스타일 갱신 및 유지 | 🟡 | |
| H1.5 | `SldHp` / `TxtHp` | (Unity 계층 구조) | UI 경로 | 체력바 프리팹 내 텍스트 컴포넌트 강제 스타일링 | 🔴 | |

---

## 📋 세이브 데이터 정화(오염 방지) 재타깃팅 지도

**파일**: [SaveDataManagerPatch.cs](../muse%20dash%20test/Patches/Database/Save/SaveDataManagerPatch.cs)

| # | MD1 게임 타입 | MD1 멤버 (메서드/필드) | 종류 | 우리가 하는 일 | 위험 | **MD2 이름 (← 채울 칸)** |
|---|---|---|---|---|---|---|
| S1.1 | `SaveDataManager` 또는 `DataManager` | `Save()` | 후크(Prefix) | 세이브 직전 가상 키(`1999-`, `1998-`) 컬렉션 정화 | 🟡 | |
