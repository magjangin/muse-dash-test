# muse-dash-custom-chart 🎵

> **뮤즈대시(Muse Dash)에서 네이티브 훅(Native Hook) 없이도 실시간 커스텀 차트 및 보스 연출을 지원하는 최초의 정식 커스텀 차트 모드(첫 배포 버전)입니다.**

---

## 🌟 Key Features (핵심 특징)

* **No Native Hook (네이티브 훅 불필요)**
  * 기존의 어려운 메모리 패치나 네이티브 훅 방식 대신, 게임 런타임에 인메모리에 생성된 `m_MusicTickData` 계열의 `MusicData` 리스트를 **In-place(메모리 내부 제자리 수정)** 방식으로 안전하게 재구성하여 완벽하고 안정적인 커스텀 차트 로드를 구현합니다.

* **Dynamic Boss Swap & Rescue (실시간 보스 교체 및 부활)**
  * 런타임 보스 초기화(`Boss.InitBossObject`)를 제어합니다.
  * 보스 액션 중 `out` 처리로 인해 Unity Game Object가 강제 비활성화(`SetActive(false)`)되어 발생하는 퇴장 상태를 자동으로 감지 및 극복하여, 원하는 시점에 완전히 다른 보스 프리팹을 실시간으로 교체 및 강제 활성화(`swap:[보스명]:[씬번호]`)하는 고난도 기믹을 제공합니다.

* **Custom Tag & LocalALBUMInfo Native Resolution (커스텀 태그 및 로컬라이제이션 네이티브 동적 결합) [v0.9.1]** ✅
  * `MusicInfo.GetLocal(int language)` 및 `DBConfigLocalALBUM.GetLocalAlbumInfoByIndex(int index)` 훅을 연동하여 게임 엔진 본연의 로컬라이즈 DB 조회 시 커스텀 곡 제목과 아티스트명을 반환하도록 확장했습니다.
  * 원본 `PnlStage.RefreshDiffUI` 실행 시점(Prefix)부터 수동 덮어쓰기 없이도 UI 상단 곡 제목(`musicNameTitle`)과 아티스트(`artistNameTitle`)가 스스로 커스텀 곡 이름으로 렌더링됩니다.
  * `MusicInfoWrapper`에 `music` 필드 래퍼 프로퍼티를 추가하고 에셋 키 참조를 차단했습니다.

* **UI Metadata Manipulation (곡 메타데이터 실시간 조작)**
  * 리플렉션과 Unity 컴포넌트 깊이 탐색을 조합하여, 곡 선택 및 플레이 준비 화면의 복잡한 UI 구조 하위에 숨겨진 텍스트 컴포넌트까지 찾아내 곡 제목, 아티스트, 레벨 디자이너 정보(라벨 및 이름)를 런타임에 원하는 텍스트로 실시간 덮어씁니다.

* **Custom Tag Injection (커스텀 태그 동적 주입)** ✅
  * `MusicTagManager.InitAlbumTagInfo` Postfix 패치를 통해 게임 시작 시 글로벌 DB에 커스텀 앨범 태그 카테고리를 동적으로 주입합니다.
  * 커스텀 태그 하위에 원하는 곡 목록(`music_list`)을 바인딩하고, `m_AlbumTagsSort` 정렬 목록에 안전하게 삽입하는 전 과정이 검증 완료되었습니다.
  * `MusicTagManager.InitDatas` Postfix 패치로 1000개 앨범 순회 병목(`m_MaxAlbumUid` 초과)으로 인한 메뉴 렉을 방지하는 성능 최적화가 적용되어 있습니다.

* **ALL PERFECT! Banner Customization (올 퍼펙트 전용 배너 커스텀)** ✅
  * 곡 완료 시 플레이어의 판정(Great 0, Miss 0, Full Combo)을 실시간으로 감지하여, 기존 FULL COMBO 배너 대신 찬란한 골드빛의 **"ALL PERFECT !"** 커스텀 텍스트 배너를 동적으로 교환 적용합니다.
  * 인게임 HUD 스코어 컴포넌트(`ChangeScoreValue`)로부터 뮤즈 대시 메인 서체인 `LuckiestGuy-Regular` 등의 **프리미엄 시그니처 폰트를 실시간 추출/캐싱**하여 결과창에 완벽히 연동시켰으며, 입체적인 3D 섀도우 및 검은색 아웃라인(`Outline`) 효과까지 그대로 재현해 인게임 정체성을 지켰습니다.

* **Offline Sandbox Toggle (오프라인 샌드박스 동적 토글) [NEW]** ✅
  * `save custom key/OFFLINE_SANDBOX.txt` 플래그 파일의 설정값(`오프라인_샌드박스=활성화/비활성화`)에 따라 게임을 재시작하지 않고도 실시간으로 오프라인 샌드박스 패치(전체 DLC 잠금 해제 및 검증 우회)를 활성화/비활성화할 수 있습니다.

* **Menu & Prep BGM Hot-Swapping (곡 선택/준비 화면 BGM 실시간 핫스왑) [NEW]** ✅
  * 가상/커스텀 곡(`1999-*`)을 선택하거나 플레이 준비 화면(`PnlPreparation`)에 진입할 때, 현재 씬에서 재생 중인 BGM/데모용 `AudioSource`를 실시간으로 탐색하여 로컬 디렉터리의 OGG 파일(`music.ogg`)로 오디오 클립을 비동기 핫스왑(Hot-swap) 적용합니다.
  * 빠른 스크롤 스킵 및 오디오 재생 겹침 방지 유효성 검증 장치가 내장되어 있어 조작감이 부드럽고 안전하게 동작합니다.


---

## ✅ Verified Results (검증된 실험 결과)

최신 실행 로그(`Latest.log`) 기준으로 아래 항목들이 정상 동작함이 확인되었습니다.

| 기능 | 상태 |
|---|---|
| 커스텀 태그(UID 1998) 동적 주입 | ✅ 완료 |
| 커스텀 태그에 곡 바인딩 | ✅ 완료 |
| `m_MaxAlbumUid` 성능 최적화 패치 | ✅ 완료 |
| **`MusicInfo.GetLocal` & `DBConfigLocalALBUM` 로컬라이제이션 훅 (`LocalALBUMInfo` 반환)** | ✅ 완료 (v0.9.1) |
| **`PnlStage.RefreshDiffUI` 원본 시점 네이티브 곡 제목/아티스트 스스로 렌더링** | ✅ 완료 (v0.9.1) |
| 곡 제목 실시간 변조 (`PnlStage`) | ✅ 완료 |
| 아티스트명 실시간 변조 (`PnlStage`) | ✅ 완료 |
| 준비 화면 텍스트 보강 변조 (`PnlPreparation`) | ✅ 완료 |
| `MusicInfo` 속성 쓰기 가능 여부 리플렉션 스캔 | ✅ 완료 |
| UID 단독 변조 한계 확인 및 정식 등록 방향 정리 | ✅ 완료 |
| 네이티브 훅 없는 인메모리 차트 재구성 | ✅ 완료 |
| **ALL PERFECT! 배너 동적 교체 및 폰트/외곽선 적용** | ✅ 완료 |
| **오프라인 샌드박스 플래그 제어 및 실시간 토글** | ✅ 완료 |
| **곡 선택 및 준비 화면 BGM 실시간 핫스왑 (`music.ogg`)** | ✅ 완료 |
| **로컬 `cover.png` 기반 커스텀 곡 셀/디스크 앨범 아트 주입** | ✅ 완료 |
| **가상 곡 플레이 기록(정확도·스코어·최대 콤보·풀콤보) 로컬 JSON 저장 및 결과/기록 카드 표시** | ✅ 완료 |
| **Discord Rich Presence (디스코드 프로필 곡명/상태 실시간 연동)** | ✅ 완료 ([상세 문서](docs/guides/DISCORD_RICH_PRESENCE.md)) |


---

## 🔮 RoadMap (향후 계획)

* **완료**: 커스텀 태그 카테고리 동적 주입 및 곡 바인딩
* **완료**: 곡 제목·아티스트·레벨 디자이너 UI 실시간 변조
* **완료**: UID 단독 변조가 UI/`MusicInfo` 불일치를 만든다는 점 확인 및 정식 등록 방향 문서화
* **완료**: `1999-0` 같은 커스텀 UID가 `GetMusicInfoFromAll` 조회에서 주입한 `MusicInfo`로 돌아오도록 게임 DB 저장소에 등록
* **완료**: 오프라인 샌드박스 한글 플래그 제어 및 동적 온/오프
* **완료**: `MusicButtonCell` 곡 셀 데이터 가로채기 성공 ➡️ 곡 셀 커버 주입 및 커스텀 앨범 정렬/순서 변경 구현
* **완료**: 커스텀 곡 선택 시 외부 BMS 파일로부터 노트를 실시간으로 읽어와 실제 플레이 가능한 커스텀 차트 로더 구현 (실시간 감시 및 핫 리로드 지원)
* **완료**: 곡 선택/준비 화면의 가상 곡 커스텀 BGM(.ogg) 실시간 핫스왑 로딩 구현
* **완료**: 로컬 `cover.png` 파일 디코딩 및 UID별 캐싱을 통한 커스텀 곡 셀/디스크 앨범 아트 동적 주입
* **완료**: 가상 곡 플레이 데이터(정확도·스코어·최대 콤보·풀콤보)를 순정 세이브 손상 없이 로컬 전용 JSON(`record/{uid}_{난이도}.json`)으로 기록하고 결과창·기록 카드·곡 선택 화면에 표시 (등급/랭크 이미지 표시는 보류)
* **진행 예정**: BMS 특정 채널 이벤트를 감지해 배경 블러, Fever 트리거 강제 작동 등 시네마틱 카메라/HUD 연출 확장


### DLC 메타데이터 정리의 목적

가상 곡과 가상 앨범은 순정 객체를 얇게 복제하므로, 복제 원본의 `needPurchase`, `pay_ids`, `dlc` 같은 상품 식별 메타데이터까지 함께 상속될 수 있습니다. `CleanPurchaseProperties`는 **모드가 생성한 가상 복제본에서만** 이 상속 정보를 제거하여 커스텀 콘텐츠가 원본 DLC 상품으로 잘못 인식되는 것을 방지합니다.

이 처리는 원본 곡/앨범 객체, 실제 DLC 소유권, 구매 상태 또는 정식 콘텐츠 잠금을 변경하기 위한 기능이 아닙니다.

단, `MemberwiseClone()`은 얕은 복사이므로 `m_MusicExInfo`, `m_AlbumExInfo` 같은 하위 객체가 원본과 같은 참조인지 반드시 확인해야 합니다. 공유 참조라면 하위 객체도 별도로 복제한 뒤 식별자를 정리해야 원본 메타데이터 변경을 확실히 방지할 수 있습니다.

---

## 📂 Directory Structure (폴더 구조)

```text
├── muse dash test/           # C# 모드 프로젝트 폴더
│   ├── Bms/                  # BMS 파서, 어휘 분석기, WAV/노트 매처
│   ├── Core/                 # 바인딩, 예외 격리, 플레이 세션, 레코드 저장소
│   ├── Integration/          # Discord RPC 연동 모듈
│   ├── Patches/              # Harmony 런타임 패치 클래스들
│   │   ├── Battle/           # 인게임 배틀 제어 및 연출
│   │   │   ├── Mechanics/    # 오토플레이, 피버 차단, 보스 런타임 스왑
│   │   │   │   ├── AutoPlayPatch.cs
│   │   │   │   ├── BossPatch.cs
│   │   │   │   └── ChangeFeverValuePatch.cs
│   │   │   └── UI/           # 올 퍼펙트 배너, 폰트 캐싱, 오프셋, 렌더링
│   │   │       ├── APModPatch.cs
   │   │       ├── PnlBattleGameStartPatch.cs
   │   │       ├── ProgressBarPatch.cs
   │   │       └── StageBattleComponentPatch.cs
│   │   ├── Common/           # Il2Cpp 래퍼, 리플렉션 헬퍼
│   │   │   ├── AlbumsInfoWrapper.cs
│   │   │   ├── Il2CppWrapperBase.cs
│   │   │   ├── ModReflection.cs
│   │   │   └── MusicInfoWrapper.cs
│   │   ├── Database/         # 런타임 차트 및 세이브 데이터 관련 패치
│   │   │   ├── Save/         # 세이브 가상 데이터 클렌징 (오염 방지)
│   │   │   │   └── SaveDataManagerPatch.cs
│   │   │   └── Stage/        # 인메모리 차트 수명 주기 제어 및 BMS 주입
│   │   │       ├── DBStageInfoPatch.cs
│   │   │       └── DBStageInfoExperimentChart*.cs
│   │   ├── Diagnostics/      # 오프셋 훅, 트레이스, 헬스 체크, 진단
│   │   │   ├── OffsetHookPatches.cs
│   │   │   ├── PatchHealthCheck.cs
│   │   │   └── UidMethodTracePatches.cs
│   │   ├── Hwa/              # Hwa 리소스, 매니페스트, BGM 스왑 제어
│   │   │   ├── HwaResourceManager.cs
│   │   │   ├── HwaManifestLoader.cs
│   │   │   └── HwaMenuBgmController.cs
│   │   ├── UI/               # UI 정보 변조 및 커스텀 가상 앨범
│   │   │   ├── Custom/       # 커스텀 태그 및 체력바/오버레이 개조
│   │   │   │   ├── HpMod/    # 배틀 체력바 스타일러
│   │   │   │   ├── InputOverlay*.cs # 키보드 입력 시각화 오버레이
│   │   │   │   └── Tags/     # 동적 가상 앨범/태그 이식
│   │   │   └── Music/        # 곡 셀 앨범아트, 핫스왑, 덤프
│   │   │       ├── MusicButtonCellPatch.cs
│   │   │       └── PnlMusicDiagnostics*.cs
│   │   └── Scene/            # 씬 오브젝트 위치 덤프 및 트래킹
│   │       └── SceneZzTransformTracker*.cs
│   ├── MainMod.cs            # MelonLoader 진입점 (MelonMod)
│   └── muse dash test.csproj # C# .NET 6.0 / Il2CppInterop 프로젝트 파일
│
├── docs/                     # 목적별로 분류된 실험/분석 한글 문서 (→ docs/README.md 목차)
│   ├── README.md             # 전체 문서 목차
│   ├── getting-started/      # 처음 읽는 문서
│   │   ├── MODDING_MINDSET.md          # 모딩에서 진짜 중요한 것
│   │   ├── ANALOGIES.md                # 비유로 이해하는 모드 구조
│   │   └── MODDING.md                  # 전체 모딩 빌드 및 연동 기초
│   ├── architecture/         # 시스템 구조와 코드 지도
│   │   ├── ARCHITECTURE.md             # 유지보수 지도 및 핵심 흐름 요약
│   │   ├── MOD_SYSTEM_BLUEPRINT.md     # 통합 시스템 설계도 및 기술 명세서
│   │   ├── CODE_REFERENCE.md           # C# 패치 코드 상세 분석 참고서
│   │   └── CAST_AND_CUSTOM_TAG_GUIDE.md # 커스텀 태그 및 캐스트 제어 가이드
│   ├── guides/               # 제작·설정·운영 가이드
│   │   ├── CUSTOM_CHART_GUIDE.md       # 커스텀 차트 환경 가이드
│   │   ├── BMS_PARSING.md              # BMS 채보 데이터 분석 및 파싱 명세
│   │   ├── OFFLINE_CUSTOM_SANDBOX_GUIDE.md # 오프라인 샌드박스 플래그 설정법
│   │   ├── DISCORD_RICH_PRESENCE.md    # 디스코드 리치 프레젠스 연동 명세
│   │   └── LOGGING_AND_TROUBLESHOOTING.md # 로그 분석 및 트러블슈팅 가이드
│   ├── experiments/          # 기능별 실험 기록
│   │   ├── NOTE_EXPERIMENTS.md         # 커스텀 노트 스펙 설계 가이드
│   │   ├── BOSS_EXPERIMENTS.md         # 실시간 보스 교환 기믹 가이드
│   │   ├── SCENE_BACKGROUND_SWAP.md    # 씬 배경 스왑 원리
│   │   └── UID_INJECTION.md            # 커스텀 UID 정식 등록 및 UI 선택 흐름 정리
│   ├── muse-dash-2/          # 차기작 대비 분석
│   │   ├── MUSE_DASH_2_SPECULATIVE_GUIDE.md # 뮤즈대시 2 대비 분석 가이드
│   │   └── MD2_TAG_RETARGET_MAP.md     # 태그 리타게팅 맵 분석
│   └── inferred_game_code/   # 디컴파일·실측 기반 게임 원본 코드 역추론
│
├── muse dash test.LogicTests/ # 게임 없이 BMS 파싱 로직만 검증하는 테스트 프로젝트
│
├── build.bat                 # MSBuild 자동 추적 및 모드 파일(DLL) 빌드/배포 스크립트
├── run-logic-tests.bat       # BMS 로직 테스트 실행 스크립트
└── README.md                 # 본 프로젝트 소개 파일
```

---

## 🚀 Quick Start (빌드 및 적용)

프로젝트 루트에 위치한 `build.bat`을 실행하면 시스템 내의 MSBuild를 자동으로 찾아 빌드를 수행한 뒤, 게임 경로의 `Mods` 폴더에 배포 및 유효성 검증까지 한 번에 완료해 줍니다.

```powershell
# 수동 빌드 시
dotnet build "muse dash test\muse dash test.csproj" --configuration Debug
```

* **빌드 결과물**: `muse dash test/bin/Debug/net6.0/muse dash custom chart.dll` (또는 Release 빌드 시 `bin/Release/net6.0/muse dash custom chart.dll`)
* **적용 위치**: Muse Dash 설치 폴더의 `Mods/` 디렉토리

---

## 🧪 로직 테스트

모드 본체는 IL2CPP 어셈블리(`Il2CppAssemblies`)를 참조하므로 게임 밖에서 로드할 수 없습니다.
대신 `Bms/` 폴더의 순수 파싱 로직만 별도 테스트 프로젝트에 **소스 링크**해서 게임 없이 검증합니다.

```powershell
.\run-logic-tests.bat
```

```powershell
# 특정 테스트만 실행 (이름 부분 일치)
.\run-logic-tests.bat BmsWavParser
```

* **검증 대상**: `BmsParser`(헤더/채널→레인/틱·시간 계산), `BmsWavParser`(UID `zzxxyy` → NoteType·보스 액션 매핑), `BmsBossSwapPlanner`(out→in 보스 교체), `BmsNoteMatcher`(홀드/샌드백 짝 매칭)
* **외부 패키지 없음**: NuGet 의존성 없이 `dotnet run`만으로 동작하며, `MelonLogger`는 스텁으로 대체됩니다.

---

## 🤝 Contributors (기여자)

* **[화영왕 (Hwa-young-wang)](https://github.com/magjangin)** - Lead Modder / Admin
* **[지미니(안티그래피티) (Gemini/Antigravity)](https://gemini.google.com)** - AI Coding Assistant
* **[클로드 (Claude)](https://anthropic.com)** - AI Coding Assistant
* **[코덱스 (Codex)](https://openai.com)** - AI Coding Assistant
