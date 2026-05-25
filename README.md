# muse-dash-test 🎵

> **뮤즈대시(Muse Dash)에서 네이티브 훅(Native Hook) 없이도 실시간 커스텀 차트 및 보스 연출이 가능하다는 것을 증명하는 PoC(Proof of Concept) 실험용 모드입니다.**

---

## 🌟 Key Features (핵심 특징)

* **No Native Hook (네이티브 훅 불필요)**
  * 기존의 어려운 메모리 패치나 네이티브 훅 방식 대신, 게임 런타임에 인메모리에 생성된 `m_MusicTickData` 계열의 `MusicData` 리스트를 **In-place(메모리 내부 제자리 수정)** 방식으로 안전하게 재구성하여 완벽하고 안정적인 커스텀 차트 로드를 증명합니다.

* **Dynamic Boss Swap & Rescue (실시간 보스 교체 및 부활)**
  * 런타임 보스 초기화(`Boss.InitBossObject`)를 제어합니다.
  * 보스 액션 중 `out` 처리로 인해 Unity Game Object가 강제 비활성화(`SetActive(false)`)되어 발생하는 퇴장 상태를 자동으로 감지 및 극복하여, 원하는 시점에 완전히 다른 보스 프리팹을 실시간으로 교체 및 강제 활성화(`swap:[보스명]:[씬번호]`)하는 고난도 기믹을 제공합니다.

* **UI Metadata Manipulation (곡 메타데이터 실시간 조작)**
  * 리플렉션과 Unity 컴포넌트 깊이 탐색을 조합하여, 곡 선택 및 플레이 준비 화면의 복잡한 UI 구조 하위에 숨겨진 텍스트 컴포넌트까지 찾아내 곡 제목, 아티스트, 레벨 디자이너 정보(라벨 및 이름)를 런타임에 원하는 텍스트로 실시간 덮어씁니다.

* **Custom Tag Injection (커스텀 태그 동적 주입)** ✅
  * `MusicTagManager.InitAlbumTagInfo` Postfix 패치를 통해 게임 시작 시 글로벌 DB에 커스텀 앨범 태그 카테고리를 동적으로 주입합니다.
  * 커스텀 태그 하위에 원하는 곡 목록(`music_list`)을 바인딩하고, `m_AlbumTagsSort` 정렬 목록에 안전하게 삽입하는 전 과정이 검증 완료되었습니다.
  * `MusicTagManager.InitDatas` Postfix 패치로 1000개 앨범 순회 병목(`m_MaxAlbumUid` 초과)으로 인한 메뉴 렉을 방지하는 성능 최적화가 적용되어 있습니다.

---

## ✅ Verified Results (검증된 실험 결과)

최신 실행 로그(`Latest.log`) 기준으로 아래 항목들이 정상 동작함이 확인되었습니다.

| 기능 | 상태 |
|---|---|
| 커스텀 태그(UID 998) 동적 주입 | ✅ 완료 |
| 커스텀 태그에 곡 바인딩 | ✅ 완료 |
| `m_MaxAlbumUid` 성능 최적화 패치 | ✅ 완료 |
| 곡 제목 실시간 변조 (`PnlStage`) | ✅ 완료 |
| 아티스트명 실시간 변조 (`PnlStage`) | ✅ 완료 |
| 준비 화면 텍스트 보강 변조 (`PnlPreparation`) | ✅ 완료 |
| `MusicInfo` 속성 쓰기 가능 여부 리플렉션 스캔 | ✅ 완료 |
| UID 단독 변조 한계 확인 및 정식 등록 방향 정리 | ✅ 완료 |
| 네이티브 훅 없는 인메모리 차트 재구성 | ✅ 완료 |

---

## 🔮 RoadMap (향후 계획)

* **완료**: 커스텀 태그 카테고리 동적 주입 및 곡 바인딩
* **완료**: 곡 제목·아티스트·레벨 디자이너 UI 실시간 변조
* **완료**: UID 단독 변조가 UI/`MusicInfo` 불일치를 만든다는 점 확인 및 정식 등록 방향 문서화
* **진행 중 (핵심 과제)**: `999-0` 같은 커스텀 UID가 `GetMusicInfoByMusicUid` 계열 조회에서 실제 `MusicInfo`로 돌아오도록 게임 DB 저장소에 정식 등록하는 루트 확인
* **진행 중**: `MusicButtonCell` 곡 셀 데이터 가로채기 성공 ➡️ 곡 셀 커버 주입 및 커스텀 앨범 정렬/순서 변경 구현 ⏳
* **진행 예정**: 커스텀 곡 선택 시 외부 JSON/채보 파일로부터 노트를 실시간으로 읽어와 실제 플레이 가능한 커스텀 차트 로더 구현
* **진행 예정**: 로컬 이미지를 커스텀 곡 커버 썸네일로 주입하는 실험
* **진행 예정**: 채보 특정 구간에서 보스 연출 트리거(`swap:[보스명]:[씬번호]`) 실시간 연동 기믹 심화

---

## 📂 Directory Structure (폴더 구조)

```text
├── muse dash test/           # C# 모드 프로젝트 폴더
│   ├── Patches/              # Harmony 런타임 패치 클래스들
│   │   ├── Database/         # 런타임 차트(MusicData) 가로채기 및 재구성
│   │   │   ├── DBStageInfoPatch.cs          # 인메모리 차트 In-place 재구성 메인 패치
│   │   │   └── DBStageInfoExperimentChart.cs # 커스텀 차트 노트 실험 데이터
│   │   ├── Battle/           # 보스 스왑 및 런타임 데이터 분석 덤프
│   │   │   ├── BossPatch.cs                 # 보스 실시간 교체 및 강제 활성화
│   │   │   └── StageBattleComponentPatch.cs # 전투 컴포넌트 런타임 분석
│   │   ├── UI/               # 곡 정보 실시간 텍스트 변조 및 커스텀 태그
│   │   │   ├── CustomTagPatch.cs            # 커스텀 태그 동적 주입 및 성능 최적화
│   │   │   ├── PnlMusicUtils.cs             # 곡 메타데이터 UI 탐색 유틸리티
│   │   │   ├── PnlStagePatch.cs             # PnlStage 직접 후킹 패치 (다이어트 완료)
│   │   │   ├── PnlStagePatchHelper.cs       # [NEW] PnlStage 및 UI 조회용 헬퍼 유틸
│   │   │   ├── LongSongNameControllerPatch.cs # [NEW] 곡 이름 스크롤 컨트롤러 패치
│   │   │   ├── PnlMusicTagScrollViewPatch.cs # [NEW] 태그 스크롤 뷰 및 캐시타이틀 패치
│   │   │   ├── MusicButtonAreaTitlePatch.cs # [NEW] 타이틀 버튼 영역 텍스트 갱신 패치
│   │   │   ├── MusicButtonCellPatch.cs      # [NEW] 곡 셀 데이터 초기화 가로채기 패치
│   │   │   └── PnlPreparationPatch.cs       # 곡 준비 화면 정보 변조 패치
│   │   └── Scene/            # 로딩 씬 강제 제어 및 흐름 제어
│   │       ├── GameMusicScenePatch.cs       # 게임 뮤직 씬 패치
│   │       └── SceneFlowPatch.cs            # 씬 전환 흐름 제어
│   ├── main.cs               # MelonLoader 진입점 (MelonMod)
│   └── muse dash test.csproj # C# .NET 6.0 프로젝트 파일
│
├── docs/                     # 고도로 정리된 기능별 실험/분석 한글 가이드
│   ├── MODDING.md                      # 전체 모딩 빌드 및 연동 기초
│   ├── NOTE_EXPERIMENTS.md             # 커스텀 노트 스펙 설계 가이드
│   ├── BOSS_EXPERIMENTS.md             # 실시간 보스 교환 기믹 가이드
│   ├── UID_INJECTION.md                # 커스텀 UID 정식 등록 및 UI 선택 흐름 정리
│   ├── CODE_REFERENCE.md               # C# 패치 코드 상세 분석 참고서
│   └── LOGGING_AND_TROUBLESHOOTING.md  # 로그 분석 및 트러블슈팅 가이드
│
├── build.bat                 # MSBuild 자동 추적 및 모드 파일(DLL) 빌드/배포 스크립트
└── README.md                 # 본 프로젝트 소개 파일
```

---

## 🚀 Quick Start (빌드 및 적용)

프로젝트 루트에 위치한 `build.bat`을 실행하면 시스템 내의 MSBuild를 자동으로 찾아 빌드를 수행한 뒤, 게임 경로의 `Mods` 폴더에 배포 및 유효성 검증까지 한 번에 완료해 줍니다.

```powershell
# 수동 빌드 시
dotnet build "muse dash test\muse dash test.csproj" --configuration Debug
```

* **빌드 결과물**: `muse dash test/bin/Debug/net6.0/muse_dash_test.dll`
* **적용 위치**: Muse Dash 설치 폴더의 `Mods/` 디렉토리
