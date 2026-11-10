# PnlAchCard (종합 업적·프로필 카드) 시스템 및 오프라인 보존 로직 분석

최초 작성: 2026-11-10  
대상 게임: Muse Dash (PC 6.7.0 / Android 6.8.0, Unity 2019.4.41f1, Il2Cpp)  
분석 근거: `Assembly-CSharp.dll` Cpp2IL RVA 분석 및 `IsilDump` 역어셈블리 추적

---

## 1. 개요 (Overview)

인게임 메뉴(업적 탭 등)에서 열 수 있는 **종합 프로필 및 성적표 카드**(Achievement Card)는 `Assets.Scripts.UI.Panels.PnlAchCard` 클래스로 구동됩니다.  
이 카드는 플레이어의 레벨, 닉네임, 아바타 헤드, 난이도별 클리어 곡 수(`Clear Stats`), 등급별 판정 개수(`Rating Stats`), 최근 플레이한 4곡의 기록(`Latest Records`), 최다 사용 캐릭터(`Favorite Characters`), 그리고 커스텀 배경 및 스티커 데코레이션을 한 화면에 모아 렌더링하고, 버튼을 숨긴 깨끗한 고해상도 이미지로 스크린샷 저장을 지원합니다.

---

## 2. PnlAchCard vs PnlReportCard 차이점

| 구분 | `PnlAchCard` (종합 프로필 카드) | `PnlReportCard` (단일 곡 성적표 카드) |
| :--- | :--- | :--- |
| **타깃 화면** | 메뉴의 업적(Achievement) 섹션 | 승리(Victory) 화면의 다운로드 리포트 버튼 |
| **주요 내용** | 계정 전체 통계 (총 클리어 수, S/SS/SSS 총합, 최근 4곡, 최애 캐릭터) | 방금 플레이한 단일 곡의 스코어, 정확도, 콤보, 랭킹 |
| **서버 통신 여부** | **서버 통신 0건 (100% 순수 로컬 데이터)** | `GetRankFromPeroServer` (서버 순위 비동기 조회 필요) |
| **오프라인 동작** | **인터넷/서버 없이 완전 자립 동작** | 오프라인 시 순위 조회 실패 텍스트(`txtGetRankMsgFail`) 노출 |

---

## 3. 핵심 구조 및 필드 (PnlAchCard)

```csharp
namespace Assets.Scripts.UI.Panels
{
    public class PnlAchCard : BaseMessageBox
    {
        // 1. 캐릭터 / 엘핀 / 배경
        public Image imgCharacter;              // 좌측 캐릭터 일러스트
        public Image imgElfin;                  // 우측 하단 엘핀 일러스트
        public Image imgBg;                     // 카드 배경 일러스트
        public Image imgBgUI;                   // UI 배경 프레임
        public Image imgBgShine;                // 배경 반사광

        // 2. 플레이어 기본 정보
        public Text txtPlayerName;              // 플레이어 닉네임
        public Text txtPlayerLevel;             // 플레이어 레벨 (예: LV.628)
        public Image imgPlayerHead;             // 플레이어 헤드 아이콘

        // 3. Clear Stats (난이도별 클리어 곡 수)
        public Text txtNumEasy;                 // EASY 클리어 곡 수
        public Text txtNumHard;                 // HARD 클리어 곡 수
        public Text txtNumMaster;               // MASTER 클리어 곡 수

        // 4. Rating Stats (판정 등급 통계)
        public Text txtNumFC;                   // Full Combo 총 달성 수
        public Text txtRedS;                    // 분홍 S 총 달성 수
        public Text txtNumSilverS;              // 은색 SS 총 달성 수
        public Text txtNumGoldS;                // 금색 SSS 총 달성 수

        // 5. Latest Records (최근 클리어 4곡)
        public GameObject NewPassMusicCellRootUp;    // 상단 2곡 슬롯 루트
        public GameObject NewPassMusicCellRootDown;  // 하단 2곡 슬롯 루트
        public List<NewPassMusicCell> newPassMusicCells; // 개별 곡 셀 (자켓, 곡명, 난이도별, 등급)

        // 6. Favorite Characters (최다 플레이 캐릭터)
        public GameObject NewFavoriteCharacterCellRoot;
        public List<NewFavoriteCharacterCell> newFavoriteCharacterCells;

        // 7. 스티커 및 스크린샷
        public Image imgStickerTop;             // 좌상단 스티커 (예: 의사 부로)
        public Image imgStikerDown;             // 우하단 스티커 (예: 실리 고스트)
        public ScreenShotSaver shotSaver;       // 화면 캡처 저장기
        public Button btnChangeBg;              // 배경 변경 버튼
        public Button btnChangeSticker;         // 스티커 변경 버튼
        public Button btnSaveImg;               // 스크린샷 저장 버튼
    }
}
```

---

## 4. 데이터 소스 및 ISIL 역어셈블리 분석

`PnlAchCard`의 모든 데이터는 외부 API 호출 없이 로컬 세이브 및 매니저 객체에서 추출됩니다:

### (1) 플레이어 프로필 (`RefreshBgAndPlayerData`, RVA: `0x1ECF0C0`)
* **닉네임 (`txtPlayerName`)**:
  `ToolsMgr.GetPlayerName()` (RVA: `0x1DC52A0`) 호출.
  - 로그인 계정 정보(`Account`)가 있으면 해당 닉네임, 없으면 플랫폼(Steam 등) 닉네임 또는 로컬 기본값을 반환.
* **레벨 (`txtPlayerLevel`)**:
  `DataHelper.level` (로컬 세이브 데이터)을 문자열로 변환하여 `"LV.{0}"` 포맷팅.
* **아바타 (`imgPlayerHead`)**:
  `ToolsMgr.GetCurSelectedHeadImg()` (RVA: `0x1DC4BA0`)에서 현재 선택된 헤드 스프라이트 획득.

### (2) Clear Stats (`RefreshLevelPassStatistic`, RVA: `0x1ECFBE0`)
* `DataHelper.globalAchiEasyPass` → `txtNumEasy.text`
* `DataHelper.globalAchiHardPass` → `txtNumHard.text`
* `DataHelper.globalAchiMasterPass` → `txtNumMaster.text`
* 로컬 세이브 파일에 누적된 난이도별 클리어 곡 수가 그대로 표시됩니다.

### (3) Rating Stats (`RefreshLevelEvaluteUI`, RVA: `0x1ECFA30`)
* `DataHelper.fullComboMusic` → `txtNumFC.text` (풀 콤보 수)
* `DataHelper.GetTargetEvaluateTotalNum(4)` → `txtRedS.text` (S 랭크)
* `DataHelper.GetTargetEvaluateTotalNum(5)` → `txtNumSilverS.text` (SS 랭크)
* `DataHelper.GetTargetEvaluateTotalNum(6)` → `txtNumGoldS.text` (SSS 랭크)

### (4) Latest Records (`RefreshNewPassRecord`, RVA: `0x1ED00A0`)
* 로컬의 최근 클리어 큐(`DataHelper.newPassMusic`)에서 상위 4개 항목을 가져와 `NewPassMusicCell.Refresh(musicUid, evaluate, resultIndex)`(RVA: `0x206A1B0`)를 호출합니다.
* 셀 내부에서 자켓 이미지(`imgCover`), 곡명(`longSongNameController`), 난이도 별 개수(`starTxtValues`), 평가 등급 텍스트(`txtGrade`)를 세팅합니다.

### (5) Favorite Characters (`RefreshNewFavoriteCharacterDatas`, RVA: `0x1ECFE30`)
* 캐릭터별 누적 플레이 횟수를 정렬하여 최상위 캐릭터를 추출, `NewFavoriteCharacterCell.Refresh(characterUid, skinIndex)`(RVA: `0x2069F20`)로 아바타와 캐릭터 이름을 표시합니다.

### (6) 스크린샷 캡처 흐름 (`OnClickSaveCardImg`, RVA: `0x1ECEDB0`)
```
OnClickSaveCardImg()
  ├── StartScreenShot()
  │     └── UI 버튼들(btnChangeBg, btnChangeSticker, btnSaveImg 등) SetActive(false)
  ├── shotSaver.Save()
  │     └── UI 렌더링 텍스처를 고해상도 PNG 파일로 인코딩 후 디스크 저장
  └── EndScreenShot()
        └── 숨겼던 UI 버튼들 SetActive(true) 복구
```
이 캡처 로직 덕분에 유저는 제어 버튼이 없는 깔끔한 완성형 카드를 파일로 얻을 수 있습니다.

---

## 5. 오프라인 영구 보존(Archive) 관점에서의 시사점

1. **완전한 오프라인 독립성**:
   - `PnlAchCard`는 애초에 네트워크 요청 코드가 0줄이며, 순수 로컬 데이터베이스(`DataHelper`)만으로 완전한 동작이 보장됩니다.
   - 따라서 Goldberg Emulator 기반 오프라인 환경이나 DRM 해제 환경에서도 아무런 결함 없이 실행됩니다.

2. **모드를 통한 향후 확장 가능성 (로드맵)**:
   - **오프라인 핫키 호출 (Quick Profile Card)**:
     인게임 복잡한 메뉴 트리를 타지 않고도, 곡 선택 화면이나 배틀 일시정지 창에서 단축키(예: `F8`)를 누르면 `UIManager.instance.Show("PnlAchCard")`를 직접 트리거하여 언제든지 프로필 카드를 열 수 있습니다.
   - **오프라인 커스텀 닉네임 연동**:
     인터넷이나 스팀이 없는 환경에서 `ToolsMgr.GetPlayerName`을 Harmony Prefix로 가로채 `config.txt`에 지정된 모더 고유 닉네임(예: "화영왕")을 고정 출력할 수 있습니다.
   - **커스텀 곡 통계 가상 합산**:
     현재 모드는 공식 세이브 오염을 막기 위해 커스텀 곡 클리어 기록을 공식 DB에 저장하지 않고 격리(`RecordStorage`)하고 있습니다. 이 카드를 띄울 때만 격리된 커스텀 곡 통계를 가상으로 합산해 보여주는 옵션을 구현할 수 있습니다.
