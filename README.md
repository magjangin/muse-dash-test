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

---

## 📂 Directory Structure (폴더 구조)

```text
├── muse dash test/           # C# 모드 프로젝트 폴더
│   ├── Patches/              # Harmony 런타임 패치 클래스들
│   │   ├── Database/         # 런타임 차트(MusicData) 가로채기 및 재구성 (DBStageInfoPatch.cs)
│   │   ├── Battle/           # 보스 스왑 및 런타임 데이터 분석 덤프 (BossPatch.cs, StageBattleComponentPatch.cs)
│   │   ├── UI/               # 곡 정보 실시간 텍스트 변조 유틸 (PnlStagePatch.cs, PnlPreparationPatch.cs)
│   │   └── Scene/            # 로딩 씬 강제 제어 및 흐름 제어 (GameMusicScenePatch.cs)
│   ├── main.cs               # MelonLoader 진입점 (MelonMod)
│   └── muse dash test.csproj # C# .NET 6.0 프로젝트 파일
│
├── docs/                     # 고도로 정리된 기능별 실험/분석 한글 가이드
│   ├── MODDING.md            # 전체 모딩 빌드 및 연동 기초
│   ├── NOTE_EXPERIMENTS.md   # 커스텀 노트 스펙 설계 가이드
│   ├── BOSS_EXPERIMENTS.md   # 실시간 보스 교환 기믹 가이드
│   └── CODE_REFERENCE.md     # C# 패치 코드 상세 분석 참고서
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
