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

* **ALL PERFECT! Banner Customization (올 퍼펙트 전용 배너 커스텀) [NEW]** ✅
  * 곡 완료 시 플레이어의 판정(Great 0, Miss 0, Full Combo)을 실시간으로 감지하여, 기존 FULL COMBO 배너 대신 찬란한 골드빛의 **"ALL PERFECT !"** 커스텀 텍스트 배너를 동적으로 교환 적용합니다.
  * 인게임 HUD 스코어 컴포넌트(`ChangeScoreValue`)로부터 뮤즈 대시 메인 서체인 `LuckiestGuy-Regular` 등의 **프리미엄 시그니처 폰트를 실시간 추출/캐싱**하여 결과창에 완벽히 연동시켰으며, 입체적인 3D 섀도우 및 검은색 아웃라인(`Outline`) 효과까지 그대로 재현해 인게임 정체성을 지켰습니다.

---

## ✅ Verified Results (검증된 실험 결과)

최신 실행 로그(`Latest.log`) 기준으로 아래 항목들이 정상 동작함이 확인되었습니다.

| 기능 | 상태 |
|---|---|
| 커스텀 태그(UID 1998) 동적 주입 | ✅ 완료 |
| 커스텀 태그에 곡 바인딩 | ✅ 완료 |
| `m_MaxAlbumUid` 성능 최적화 패치 | ✅ 완료 |
| 곡 제목 실시간 변조 (`PnlStage`) | ✅ 완료 |
| 아티스트명 실시간 변조 (`PnlStage`) | ✅ 완료 |
| 준비 화면 텍스트 보강 변조 (`PnlPreparation`) | ✅ 완료 |
| `MusicInfo` 속성 쓰기 가능 여부 리플렉션 스캔 | ✅ 완료 |
| UID 단독 변조 한계 확인 및 정식 등록 방향 정리 | ✅ 완료 |
| 네이티브 훅 없는 인메모리 차트 재구성 | ✅ 완료 |
| **ALL PERFECT! 배너 동적 교체 및 폰트/외곽선 적용** | ✅ 완료 |

---

## 🔮 RoadMap (향후 계획)

* **완료**: 커스텀 태그 카테고리 동적 주입 및 곡 바인딩
* **완료**: 곡 제목·아티스트·레벨 디자이너 UI 실시간 변조
* **완료**: UID 단독 변조가 UI/`MusicInfo` 불일치를 만든다는 점 확인 및 정식 등록 방향 문서화
* **완료**: `1999-0` 같은 커스텀 UID가 `GetMusicInfoFromAll` 조회에서 주입한 `MusicInfo`로 돌아오도록 게임 DB 저장소에 등록
* **진행 중**: `MusicButtonCell` 곡 셀 데이터 가로채기 성공 ➡️ 곡 셀 커버 주입 및 커스텀 앨범 정렬/순서 변경 구현 ⏳
* **진행 예정**: 커스텀 곡 선택 시 외부 JSON/채보 파일로부터 노트를 실시간으로 읽어와 실제 플레이 가능한 커스텀 차트 로더 구현
* **진행 예정**: 로컬 이미지를 커스텀 곡 커버 썸네일로 주입하는 실험
* **진행 예정**: 채보 특정 구간에서 보스 연출 트리거(`swap:[보스명]:[씬번호]`) 실시간 연동 기믹 심화

### DLC 메타데이터 정리의 목적

가상 곡과 가상 앨범은 순정 객체를 얇게 복제하므로, 복제 원본의 `needPurchase`, `pay_ids`, `dlc` 같은 상품 식별 메타데이터까지 함께 상속될 수 있습니다. `CleanPurchaseProperties`는 **모드가 생성한 가상 복제본에서만** 이 상속 정보를 제거하여 커스텀 콘텐츠가 원본 DLC 상품으로 잘못 인식되는 것을 방지합니다.

이 처리는 원본 곡/앨범 객체, 실제 DLC 소유권, 구매 상태 또는 정식 콘텐츠 잠금을 변경하기 위한 기능이 아닙니다.

단, `MemberwiseClone()`은 얕은 복사이므로 `m_MusicExInfo`, `m_AlbumExInfo` 같은 하위 객체가 원본과 같은 참조인지 반드시 확인해야 합니다. 공유 참조라면 하위 객체도 별도로 복제한 뒤 식별자를 정리해야 원본 메타데이터 변경을 확실히 방지할 수 있습니다.

---

## 📂 Directory Structure (폴더 구조)

```text
├── muse dash test/           # C# 모드 프로젝트 폴더
│   ├── Patches/              # Harmony 런타임 패치 클래스들
│   │   ├── APModPatch.cs     # 올 퍼펙트 배너 제어 및 인게임 폰트 캐싱 [NEW]
│   │   ├── Database/         # 런타임 차트 및 세이브 데이터 관련 패치
│   │   │   ├── Stage/        # 인메모리 차트 수명 주기 제어 및 로더
│   │   │   │   ├── DBStageInfoPatch.cs
│   │   │   │   ├── DBStageInfoExperimentChart.cs
│   │   │   │   ├── DBStageInfoExperimentChart.Helpers.cs
│   │   │   │   └── DBStageInfoSetStageInfoPatch.cs
│   │   │   ├── Skill/        # 캐릭터 스킬 및 오토플레이 제어
│   │   │   │   └── DBSkillPatch.cs
│   │   │   └── Save/         # 세이브 가상 데이터 클렌징 (오염 방지) [NEW]
│   │   │       └── SaveDataManagerPatch.cs
│   │   ├── Battle/           # 인게임 배틀 제어 및 연출
│   │   │   ├── Mechanics/    # 오토플레이, 피버 차단, 보스 런타임 스왑
│   │   │   │   ├── AutoPlayPatch.cs
│   │   │   │   ├── BossPatch.cs
│   │   │   │   └── ChangeFeverValuePatch.cs
│   │   │   └── UI/           # 배틀 스크린 영상 재생 및 진행바 은폐
│   │   │       ├── PnlBattleGameStartPatch.cs
│   │   │       ├── ProgressBarPatch.cs
│   │   │       └── StageBattleComponentPatch.cs
│   │   ├── UI/               # UI 정보 변조 및 커스텀 가상 앨범
│   │   │   ├── Common/       # 공용 메타데이터 추출 및 래핑
│   │   │   │   ├── PnlMusicUtils.cs
│   │   │   │   ├── PnlMusicUtils.Helpers.cs
│   │   │   │   ├── PnlStagePatchHelper.cs
│   │   │   │   ├── PnlStagePatchHelper.TextDebug.cs
│   │   │   │   ├── Wrappers/    # Il2Cpp 데이터 강타입 래퍼 [NEW]
│   │   │   │   │   ├── AlbumsInfoWrapper.cs
│   │   │   │   │   ├── Il2CppWrapperBase.cs
│   │   │   │   │   └── MusicInfoWrapper.cs
│   │   │   │   ├── Reflection/  # 고성능 리플렉션 [NEW]
│   │   │   │   │   └── ModReflection.cs
│   │   │   │   ├── Diagnostics/ # 음악 정보 덤프 및 진단 [NEW]
│   │   │   │   │   ├── PnlMusicUtils.Diagnostics.cs
│   │   │   │   │   └── PnlMusicUtils.Log.cs
│   │   │   │   └── Search/      # 곡 정보 정밀 통합 검색 [NEW]
│   │   │   │       └── PnlStagePatchHelper.Search.cs
│   │   │   ├── Stage/        # 곡 선택 화면 패치
│   │   │   │   ├── Preparation/ # 준비 단계 변조
│   │   │   │   │   └── PnlPreparationPatch.cs
│   │   │   │   ├── Record/      # 기록 변조
│   │   │   │   │   └── PnlRecordPatch.cs
│   │   │   │   └── Selection/   # 곡 및 타이틀 선택 제어
│   │   │   │       ├── PnlStagePatch.cs
│   │   │   │       ├── LongSongNameControllerPatch.cs
│   │   │   │       └── MusicButtonAreaTitlePatch.cs
│   │   │   ├── Custom/       # 커스텀 태그 및 체력바 개조
│   │   │   │   ├── Tags/        # 동적 가상 앨범/태그 이식
│   │   │   │   │   ├── CustomTagPatch.cs
│   │   │   │   │   ├── CustomTagPatch.AlbumPatches.cs
│   │   │   │   │   └── CustomTagRegistry.cs
│   │   │   │   └── HpMod/       # 배틀 체력바 스타일러
│   │   │   │       ├── HywStageManager.cs
│   │   │   │       └── HywTextStyler.cs
│   │   │   └── Music/        # 스크롤 뷰 동적 로딩 및 정렬
│   │   │       ├── FancyScrollViewPatch.cs
│   │   │       ├── MusicButtonCellPatch.cs
│   │   │       └── PnlMusicTagPatch.cs
│   │   ├── Diagnostics/      # 글로벌 시퀀스 메서드 트레이스
│   │   │   └── UidMethodTracePatches.cs
│   │   └── Scene/            # 씬 흐름 강제 제어
│   │       ├── GameMusicScenePatch.cs
│   │       └── SceneFlowPatch.cs
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
