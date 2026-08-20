# muse-dash-custom-chart 🎵

> **뮤즈대시(Muse Dash)를 네이티브 훅(Native Detour) 없이 관리형 Harmony 계층만으로 다뤄, 실시간 커스텀 차트와 보스 연출을 구현한 최초의 모드입니다. (첫 배포 버전)**
>
> "최초"는 **이 고수준(managed) 접근 방식으로 구현한 것이 처음**이라는 뜻입니다. 커스텀 차트 모드 자체의 선후를 주장하는 것이 아닙니다.

---

## 🌟 Key Features (핵심 특징)

* **No Native Hook (네이티브 훅 불필요)**
  * 기존의 어려운 메모리 패치나 네이티브 훅 방식 대신, 게임 런타임에 인메모리에 생성된 `m_MusicTickData` 계열의 `MusicData` 리스트를 **In-place(메모리 내부 제자리 수정)** 방식으로 안전하게 재구성하여 완벽하고 안정적인 커스텀 차트 로드를 구현합니다.

* **Dynamic Boss Swap & Rescue (실시간 보스 교체 및 부활)**
  * 런타임 보스 초기화(`Boss.InitBossObject`)를 제어합니다.
  * 보스 액션 중 `out` 처리로 인해 Unity Game Object가 강제 비활성화(`SetActive(false)`)되어 발생하는 퇴장 상태를 자동으로 감지 및 극복하여, 원하는 시점에 완전히 다른 보스 프리팹을 실시간으로 교체 및 강제 활성화(`swap:[보스명]:[씬번호]`)하는 고난도 기믹을 제공합니다.

* **Custom Tag & LocalALBUMInfo Resolution (커스텀 태그 및 로컬라이제이션 원본 DB 조회 결합) [v0.9.3]** ✅
  * `MusicInfo.GetLocal(int language)` 및 `DBConfigLocalALBUM.GetLocalAlbumInfoByIndex(int index)` 훅을 연동하여 게임 엔진 본연의 로컬라이즈 DB 조회 시 커스텀 곡 제목과 아티스트명을 반환하도록 확장했습니다.
  * 이로써 언어팩 조회 경로가 원본 곡명을 되돌려 놓는 것을 차단하며, UI 상단 곡 제목 및 아티스트명이 정상 렌더링되도록 처리되었습니다. (→ [CAST_AND_CUSTOM_TAG_GUIDE.md](docs/architecture/CAST_AND_CUSTOM_TAG_GUIDE.md))
  * `MusicInfoWrapper`에 `music` 필드 래퍼 프로퍼티를 추가하고 에셋 키 참조를 차단했습니다.

* **UI Metadata Manipulation (곡 메타데이터 실시간 조작)**
  * 리플렉션과 Unity 컴포넌트 깊이 탐색을 조합하여, 곡 선택 및 플레이 준비 화면의 복잡한 UI 구조 하위에 숨겨진 텍스트 컴포넌트까지 찾아내 곡 제목, 아티스트, 레벨 디자이너 정보(라벨 및 이름)를 런타임에 원하는 텍스트로 실시간 덮어씁니다.

* **Custom Tag Injection (커스텀 태그 동적 주입)** ✅
  * `MusicTagManager.InitAlbumTagInfo` Postfix 패치를 통해 게임 시작 시 글로벌 DB에 커스텀 앨범 태그 카테고리를 동적으로 주입합니다.
  * 커스텀 태그 하위에 원하는 곡 목록(`music_list`)을 바인딩하고, `m_AlbumTagsSort` 정렬 목록에 안전하게 삽입하는 전 과정이 검증 완료되었습니다.
  * `MusicTagManager.InitDatas` Postfix 패치로 1000개 앨범 순회 병목(`m_MaxAlbumUid` 초과)으로 인한 메뉴 렉 및 실험 앨범 곡 미표시 버그를 방지하는 성능 최적화가 적용되어 있습니다.

* **ALL PERFECT! Banner Customization (올 퍼펙트 전용 배너 커스텀)** ✅
  * 곡 완료 시 플레이어의 판정(Great 0, Miss 0, Full Combo)을 실시간으로 감지하여, 기존 FULL COMBO 배너 대신 찬란한 골드빛의 **"ALL PERFECT !"** 커스텀 텍스트 배너를 동적으로 교환 적용합니다.
  * 인게임 HUD 스코어 컴포넌트(`ChangeScoreValue`)로부터 뮤즈 대시 메인 서체인 `LuckiestGuy-Regular` 등의 **프리미엄 시그니처 폰트를 실시간 추출/캐싱**하여 결과창에 완벽히 연동시켰으며, 입체적인 3D 섀도우 및 검은색 아웃라인(`Outline`) 효과까지 그대로 재현해 인게임 정체성을 지켰습니다.

* **ModConfig Feature Toggle System (개별 기능 온/오프 제어 시스템) [v0.9.3]** ✅
  * `UserData/MelonPreferences.cfg` 파일의 `[muse-dash-custom-chart-features]` 카테고리를 통해 12가지 개별 기능(커스텀 차트, 스킨 스왑, 입력 오버레이, 판정바, 디스코드 RPC, 체력바, AP 패치, 오토플레이, 강제 올퍼펙트 등)을 자유롭게 활성화/비활성화할 수 있습니다.

* **Real-Time FavGirl Swapper (인게임/준비화면 실시간 캐릭터 & 스킨 스왑)** ✅
  * 인게임 및 곡 선택/준비 화면에서 `P` / `O` 핫키를 눌러 현재 플레이어 캐릭터와 스킨을 실시간으로 핫스왑 조작할 수 있는 편의 기능을 제공합니다.

* **In-Game Input Overlay & Judgment Bar (키 입력 오버레이 & 판정 타임라인 시각화)** ✅
  * 인게임 플레이 중 실시간 키보드 입력 상황을 직관적인 HUD 오버레이로 표시하며, 화면 하단에 판정 타임라인(`JudgmentBar`)을 그래픽으로 시각화합니다.

* **Spine Custom Skin Injection (Spine 커스텀 스킨 주입)** ✅
  * Spine 애니메이션 캐릭터의 텍스처 및 아틀라스 에셋을 런타임 커스텀 에셋으로 주입 및 바인딩합니다.

* **AutoPlay & Force Perfect (오토 플레이 및 올퍼펙트 파라미터 모드)** ✅
  * 인게임 차트 자동 연주 기능 및 판정 파라미터 조작을 통한 All-Perfect 유도 기능을 선택적으로 활성화할 수 있습니다.

* **Offline Sandbox Toggle (오프라인 샌드박스 동적 토글) [NEW]** ✅
  * `save custom key/OFFLINE_SANDBOX.txt` 플래그 파일의 설정값(`오프라인_샌드박스=활성화/비활성화`)에 따라 게임을 재시작하지 않고도 실시간으로 오프라인 샌드박스 패치(전체 DLC 잠금 해제 및 검증 우회)를 활성화/비활성화할 수 있습니다.

* **Menu & Prep BGM Hot-Swapping (곡 선택/준비 화면 BGM 실시간 핫스왑) [NEW]** ✅
  * 가상/커스텀 곡(`1999-*`)을 선택하거나 플레이 준비 화면(`PnlPreparation`)에 진입할 때, 현재 씬에서 재생 중인 BGM/데모용 `AudioSource`를 실시간으로 탐색하여 로컬 디렉터리의 OGG 파일(`music.ogg`)로 오디오 클립을 비동기 핫스왑(Hot-swap) 적용합니다.
* **Mobile Touch Mode & Mouse Bridge (모바일 터치 설정 복원 및 마우스/터치 브릿지) [NEW]** ✅
  * PC 스팀 빌드 내에 잠들어 있던 모바일 전용 입력 설정창(`PnlInputMobile`)을 완벽 복원하여 좌우/상하 분할 모드, 되돌리기(반전), 오토 피버 옵션을 실시간 제어합니다.
  * 인게임 배틀에서 마우스 좌/우 클릭 및 터치스크린 입력을 공중/지상 타격 및 점프 체공(롱노트 홀드)으로 실시간 변환 주입하며, PC 기본 키 매핑 간섭을 원천 차단하는 상호 배타적 필터링이 적용되었습니다.
  * **터치스크린 10접점 멀티터치 지원**: ROG Ally, 스팀덱, 서피스 등에서 두 손가락으로 공중/지상 동시 입력이 가능합니다. 레거시 `UnityEngine.Input`이 Windows 스탠드얼론에서 터치를 읽지 못하는 문제를 새 Input System 전환으로 해결했으며, Windows의 마우스 승격으로 인한 이중 판정도 차단합니다. (→ [MOBILE_TOUCH_AND_INPUT_GUIDE.md](docs/guides/MOBILE_TOUCH_AND_INPUT_GUIDE.md))
* **UMPC Hardware Auto-Detection & Lag Optimization (UMPC 자동 감지 및 로그 레벨 렉 최적화) [v0.10.0]** ✅
  * ASUS ROG Ally, Valve Steam Deck, Lenovo Legion Go, AYANEO, GPD 등 핸드헬드 기기 및 배터리/내장 APU 환경을 시작 시 자동 감지합니다 (`DeviceDetector`).
  * UMPC 환경에서는 콘솔 출력 및 파일 I/O 부하로 인한 순간적인 프레임 드랍(스터터링)을 방지하기 위해 기본 로그 레벨을 `Error`로 대폭 낮추며, 전역 `MelonLoggerInterceptor`를 통해 모드 전반의 불필요한 로그 출력을 원천 차단합니다.
  * `MelonPreferences.cfg`의 `LogLevel` 설정(`Auto`, `Silent`, `Error`, `Warning`, `Info`, `Verbose`)을 통해 사용자 맞춤 제어를 지원합니다. (→ [LOGGING_AND_TROUBLESHOOTING.md](docs/guides/LOGGING_AND_TROUBLESHOOTING.md))

---

## ✅ Verified Results (검증된 실험 결과)

최신 실행 로그(`Latest.log`) 기준으로 아래 항목들이 정상 동작함이 확인되었습니다.

| 기능 | 상태 |
|---|---|
| 커스텀 태그(UID 1998) 동적 주입 | ✅ 완료 |
| 커스텀 태그에 곡 바인딩 | ✅ 완료 |
| `m_MaxAlbumUid` 성능 최적화 패치 | ✅ 완료 |
| **`MusicInfo.GetLocal` & `DBConfigLocalALBUM` 로컬라이제이션 훅 (`LocalALBUMInfo` 반환)** | ✅ 완료 (v0.9.3) |
| **실험 앨범(커스텀 태그) 선택 시 곡 목록 미표시 버그 수정 및 동적 태그 주입** | ✅ 완료 (v0.9.3) |
| 곡 제목 실시간 변조 (`PnlStage`) | ✅ 완료 |
| 아티스트명 실시간 변조 (`PnlStage`) | ✅ 완료 |
| 준비 화면 텍스트 보강 변조 (`PnlPreparation`) | ✅ 완료 |
| `MusicInfo` 속성 쓰기 가능 여부 리플렉션 스캔 | ✅ 완료 |
| UID 단독 변조 한계 확인 및 정식 등록 방향 정리 | ✅ 완료 |
| 네이티브 훅 없는 인메모리 차트 재구성 | ✅ 완료 |
| **ALL PERFECT! 배너 동적 교체 및 폰트/외곽선 적용** | ✅ 완료 |
| **ModConfig 개별 기능 토글 제어 (`MelonPreferences.cfg`)** | ✅ 완료 (v0.9.3) |
| **인게임 키 입력 오버레이 (`InputOverlay`) & 판정바 (`JudgmentBar`) 표시** | ✅ 완료 |
| **`P`/`O` 핫키 기반 실시간 캐릭터/스킨 핫스왑 (`RealTimeSwapper`)** | ✅ 완료 |
| **Spine 커스텀 스킨 런타임 텍스처/아틀라스 주입 (`Spine/`)** | ✅ 완료 |
| **오토 플레이(`AutoPlay`) 및 강제 올퍼펙트(`ForcePerfect`) 조작 패치** | ✅ 완료 |
| **오프라인 샌드박스 플래그 제어 및 실시간 토글** | ✅ 완료 |
| **곡 선택 및 준비 화면 BGM 실시간 핫스왑 (`music.ogg`)** | ✅ 완료 |
| **로컬 `cover.png` 기반 커스텀 곡 셀/디스크 앨범 아트 주입** | ✅ 완료 |
| **가상 곡 플레이 기록(정확도·스코어·최대 콤보·풀콤보) 로컬 JSON 저장 및 결과/기록 카드 표시** | ✅ 완료 |
| **모바일 전용 터치 조작 패널(`PnlInputMobile`) 복원 및 마우스/터치 배틀 입력 브릿지** | ✅ 완료 ([상세 문서](docs/guides/MOBILE_TOUCH_AND_INPUT_GUIDE.md)) |
| **Discord Rich Presence (디스코드 프로필 곡명/상태 실시간 연동)** | ✅ 완료 ([상세 문서](docs/guides/DISCORD_RICH_PRESENCE.md)) |
| **UMPC 하드웨어 자동 감지 및 전역 로그 레벨 렉 최적화** | ✅ 완료 (v0.10.0, [상세 문서](docs/guides/LOGGING_AND_TROUBLESHOOTING.md)) |


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
* **진행 중**: **특수 기믹 곡 신규 UID/NoteType 스캔 및 커스텀 BMS 매핑 확장**
  * `[OfficialSceneContext]` 미등록 노트 탐지기(`LogUnregisteredOriginalChartNotes`)를 통해 공식 기믹 곡들의 신규 노트 UID 및 NoteType(>17) 분석 진행 중.
  * **대상 대표 기믹 곡 목록:**
    - 🎵 `ペロペロ in the Universe` - 立秋 feat.ちょこ
    - 🎵 `Saishuu kichiku imouto Flandre-S` (최종귀축동생 플랑도르 S) - ビートまりお
    - 🎵 `Spider's Thread` (蜘蛛の糸) - kikuo×cosMo@Bousou-P feat.kagenui hana
    - 🎵 `喵斯摇 (feat. 春哥，渊神)` (Pero Shake) - DJ怪哥/DJ鹏哥
    - 🎵 `Ruler Of My Heart` (VIVINOS - 'Alien Stage Pt5') - STUDIO LICO
    - 🎵 `Nyan Cat` - daniwell
    - 🎵 `Cubibibibism（きゅびびびびずむ）` - 초절정 귀요미 천사 (Needy Streamer Overload)
  * **반영 계획:** 발견된 신규 특수 노트(Note Freeze, RGB Split glitch, Old TV CRT, Pixelate, Grayscale, Wave Shader, Quiz Question 등)를 BMS 파서 및 `#WAV` 매핑 테이블에 이식하여 커스텀 차트에서 기믹 노트로 완벽 사용할 수 있도록 정식 반영할 예정입니다.
* **진행 예정**: BMS 특정 채널 이벤트를 감지해 배경 블러, Fever 트리거 강제 작동 등 시네마틱 카메라/HUD 연출 확장
* **보류**: 커스텀 곡 대사(Dialog) 주입 — 동작하는 프로토타입까지 검증했으나 우선순위 조정으로 본체에서 제거했습니다. 훅 지점, 실측 스타일 값, 알려진 파싱 함정까지 [DIALOG_INJECTION.md](docs/experiments/DIALOG_INJECTION.md)에 보존해 두었습니다.


### DLC 메타데이터 정리의 목적

가상 곡과 가상 앨범은 순정 객체를 얇게 복제하므로, 복제 원본의 `needPurchase`, `pay_ids`, `dlc` 같은 상품 식별 메타데이터까지 함께 상속될 수 있습니다. `CleanPurchaseProperties`는 **모드가 생성한 가상 복제본에서만** 이 상속 정보를 제거하여 커스텀 콘텐츠가 원본 DLC 상품으로 잘못 인식되는 것을 방지합니다.

이 처리는 원본 곡/앨범 객체, 실제 DLC 소유권, 구매 상태 또는 정식 콘텐츠 잠금을 변경하기 위한 기능이 아닙니다.

단, `MemberwiseClone()`은 얕은 복사이므로 `m_MusicExInfo`, `m_AlbumExInfo` 같은 하위 객체가 원본과 같은 참조인지 반드시 확인해야 합니다. 공유 참조라면 하위 객체도 별도로 복제한 뒤 식별자를 정리해야 원본 메타데이터 변경을 확실히 방지할 수 있습니다.

---

## 📂 Directory Structure (폴더 구조)

```text
├── muse dash test/           # C# 모드 프로젝트 폴더
│   ├── Bms/                  # BMS 파서/렉서, WAV 코드 해석, 노트 매칭, 보스 스왑 플래너
│   ├── Core/                 # 하드웨어 감지, 로그 레벨 제어, 예외 격리, 터치 판독, 세션/기록 저장소, ModConfig 통합 설정
│   │   ├── DeviceDetector.cs # UMPC/핸드헬드 하드웨어 자동 판별
│   │   ├── ModLogger.cs      # 로그 레벨 동적 제어
│   │   ├── MelonLoggerInterceptor.cs # MelonLogger 전역 가로채기 & 음소거
│   │   ├── ModConfig.cs      # MelonPreferences 기반 13개 개별 기능 및 LogLevel 제어
│   │   └── ...
│   ├── Integration/          # Discord RPC 연동 및 실시간 리소스/스킨 스와퍼 (P/O 단축키)
│   │   ├── DiscordPresenceManager.cs
│   │   └── RealTimeSwapper.cs
│   ├── Patches/              # Harmony 런타임 패치 클래스들
│   │   ├── Battle/           # 인게임 배틀 제어 및 연출
│   │   │   ├── Mechanics/    # 오토플레이, 피버 차단, 보스 런타임 스왑
│   │   │   │   ├── AutoPlayPatch.cs
│   │   │   │   ├── BossPatch.cs
│   │   │   │   └── ChangeFeverValuePatch.cs
│   │   │   └── UI/           # 올 퍼펙트 배너, 폰트 캐싱, BGA/미디어, 진행바
│   │   │       ├── APModPatch*.cs # 정확도 재계산 + ALL PERFECT 배너 (3파일 분할)
│   │   │       ├── AllPerfectSound.cs
│   │   │       ├── ExperimentHitPointInstaller.cs
│   │   │       ├── HwaBattleMediaController*.cs
│   │   │       ├── PnlBattleGameStartPatch.cs
│   │   │       ├── PnlVictoryLoggingPatch.cs
│   │   │       ├── ProgressBarPatch.cs
│   │   │       └── StageBattleComponentPatch.cs
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
│   │   ├── Diagnostics/      # 판정 오프셋/딜레이 추적, UID 트레이스, 헬스 체크
│   │   │   ├── CollabEndTimeDumpPatch.cs
│   │   │   ├── DiscordManagerDebugPatch.cs
│   │   │   ├── HwaChartDiagnostics.cs
│   │   │   ├── OffsetHookPatches.cs
│   │   │   ├── PatchHealthCheck.cs
│   │   │   └── UidMethodTracePatches.cs
│   │   ├── Fav/              # 커스텀 곡 즐겨찾기 관리
│   │   │   └── FavManager.cs
│   │   ├── Hwa/              # Hwa 리소스, 매니페스트, BGM 스왑, 동기화
│   │   │   ├── HwaManifest.cs
│   │   │   ├── HwaManifestLoader.cs
│   │   │   ├── HwaMenuBgmController.cs
│   │   │   ├── HwaResourceManager*.cs
│   │   │   └── HwaSyncManager.cs
│   │   ├── Sandbox/          # 오프라인 샌드박스 (DLC 잠금 해제 및 검증 우회 토글)
│   │   │   └── OfflineCustomSandbox.cs
│   │   ├── Scene/            # 씬 전환 흐름 제어, 배틀 씬 초기화, 오브젝트 위치 트래킹
│   │   │   ├── GameMusicScene*.cs
│   │   │   ├── SceneFlowPatch.cs
│   │   │   └── SceneZzTransformTracker*.cs
│   │   └── UI/               # UI 정보 변조 및 커스텀 가상 앨범
│   │       ├── Custom/       # 커스텀 태그, 체력바, 입력 오버레이, 판정바
│   │       │   ├── HpMod/    # 배틀 체력바 스타일러
│   │       │   ├── Tags/     # 동적 가상 앨범/태그 이식 (+ Support/)
│   │       │   ├── InputOverlay*.cs # 키보드 입력 시각화 오버레이
│   │       │   └── JudgmentBar.cs
│   │       ├── Menu/         # 홈 화면 진단, 메뉴 BGM 정지
│   │       ├── Music/        # 곡 셀 앨범아트, 태그, 핫스왑, 덤프
│   │       │   ├── MusicButtonCellPatch.cs
│   │       │   ├── MusicStageCellPatch.cs
│   │       │   ├── PnlMusicOverride.cs
│   │       │   ├── PnlMusicTagPatch.cs
│   │       │   └── PnlMusicDiagnostics*.cs
│   │       ├── Pnl/          # PnlStage 텍스트 탐색 헬퍼, 곡명 텍스트 대치
│   │       │   ├── PnlStagePatchHelper*.cs
│   │       │   └── SetSelectedMusicNameTxtPatch.cs
│   │       └── Stage/        # 곡 선택/준비 화면, 기록 카드, 포스트카드, 랭크
│   │           ├── CustomRecordUiPatchHelper.cs
│   │           ├── PnlPreparationPatch.cs
│   │           ├── PnlRankHookPatch.cs
│   │           ├── PnlRecordPatch.cs
│   │           ├── PnlReportCardPatch.cs
│   │           ├── PnlStagePatch.cs
│   │           └── RankCellHookPatch.cs
│   ├── Spine/                # Spine 커스텀 스킨 주입 및 아키텍처 제어
│   │   ├── CustomSkinInjector.cs
│   │   ├── Patch_Inject_BlackGirlBattle.cs
│   │   ├── Patch_SkinNameProbe.cs
│   │   └── Patch_SpineActionContract.cs
│   ├── Properties/           # AssemblyInfo (MelonInfo/MelonGame 특성)
│   ├── Resources/            # DLL 내장 리소스 (tag_icon.png)
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

## ⚙️ Configuration (모드 기능 설정)

게임을 1회 실행하면 Muse Dash 설치 디렉터리의 `UserData/MelonPreferences.cfg` 파일에 `[muse-dash-custom-chart-features]` 카테고리가 자동 생성됩니다. 각 항목을 `true`/`false`로 수정하여 기능을 개별 제어할 수 있습니다.

| 설정 키 (Entry) | 기본값 | 기능 설명 |
|---|---|---|
| `EnableCustomChart` | `true` | 커스텀 차트 로더, 인메모리 BMS 주입, 실험 차트 HitPoints 프리팹 설치 활성화 |
| `EnableRealTimeSwap` | `true` | FavGirl 실시간 소녀/스킨 핫스왑 조작 활성화 (`P` / `O` 단축키) |
| `EnableInputOverlay` | `true` | 인게임 실시간 키보드 입력 오버레이 HUD 표시 |
| `EnableJudgmentBar` | `true` | 화면 하단 판정 타임라인 시각화 그래프 UI 표시 |
| `EnableDiscordRPC` | `true` | Discord Rich Presence 실시간 상태 연동 |
| `EnableHpTextMod` | `true` | 배틀 체력바 텍스트 워터마크 표시 |
| `EnableAPMod` | `true` | 올 퍼펙트 배너 및 정확도/판정 계산 오버라이드 |
| `EnableAllPerfectSound` | `true` | 올 퍼펙트 달성 시 시그니처 효과음 재생 |
| `EnableAutoPlay` | `true` | 오토 플레이 패치 활성화 |
| `EnableForcePerfect` | `true` | All-Perfect 파라미터 모드 (강제 퍼펙트 판정) 활성화 |
| `EnableBattleMedia` | `true` | 배틀 커스텀 BGA 비디오/미디어 재생기 활성화 |
| `EnableSpineSkin` | `true` | Spine 커스텀 스킨 텍스처/아틀라스 런타임 주입 활성화 |

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
