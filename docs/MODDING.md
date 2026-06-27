# 🧪 Muse Dash 실험 모드 전체 가이드 (Modding Guide)

이 저장소는 Muse Dash의 런타임 데이터를 후킹해서 노트, 보스, 곡 메타데이터를 빠르게 실험하고 확장하기 위한 모드입니다. 
기존 구조를 “대량으로 뜯어고치는” 것이 아니라, 게임이 런타임에 메모리에 올린 객체를 복제하거나 UI 컴포넌트를 조작해 원하는 값으로 실시간 덮어쓰는 방식입니다.

---

## 📁 프로젝트 구조

```text
Muse Dash/
├── Mods/
│   └── muse.dash.custom.chart.dll  # 빌드 완료된 모드 DLL
└── hwa/                            # 커스텀 리소스 루트 디렉터리 (오프라인 모드)
```

가장 자주 보는 문서는 아래 문서들입니다.

- [비유로 이해하는 모드 구조](ANALOGIES.md): 핵심 개념을 비유로 빠르게 잡기 (입문용)
- [노트 실험](NOTE_EXPERIMENTS.md): 일반 노트, 공중 노트, 롱노트, 샌드백, 하트, 음표, 속도, dt 실험
- [보스 실험](BOSS_EXPERIMENTS.md): 보스 액션 트리거와 실제 보스 프리팹 변경
- [코드 파일별 레퍼런스](CODE_REFERENCE.md): 전체 C# 파일의 역할, 주요 클래스/메서드, 읽는 순서
- [로그와 문제 해결](LOGGING_AND_TROUBLESHOOTING.md): 로그 해석, 노트/보스/UI가 안 될 때 확인 순서
- 이 문서: 전체 파일 구조, 빌드, 로그 확인, 곡 제목/아티스트/레벨 디자이너 실험

---

## 🏗️ 1. 먼저 이해해야 하는 인게임 메커니즘 (Core Concepts)

> 이 장의 개념들은 비유로 더 쉽게 잡을 수 있습니다 → [비유로 이해하는 모드 구조](ANALOGIES.md)

### 1.1 차트 데이터: 단일 타임라인 구조
`MusicData`는 지상/공중 노트, 장애물, 보스 신호가 종류별로 나뉘어 담기지 않고, 모두 tick 순서로 한 리스트에 섞여 들어가는 단일 타임라인입니다. 게임은 음악이 흐르는 동안 이 리스트에서 순서대로 노트를 꺼내 화면에 띄웁니다.

실험할 때 중요한 판단 기준은 아래 순서입니다.

| 기준 | 의미 |
| --- | --- |
| `noteData.type` | 게임 로직상 어떤 노트인지 결정하는 가장 중요한 값입니다. |
| `noteData.pathway` | 지상/공중 레인입니다. `0`은 지상, `1`은 공중입니다. |
| `noteData.uid` | 노트 리소스 키입니다. 씬, 계열, 방향 정보가 섞여 있습니다. |
| `noteData.prefab_name` | 실제로 어떤 프리팹을 붙일지 결정합니다. UID만 바꾸고 이 값이 원본이면 외형이 그대로일 수 있습니다. |
| `noteData.key_audio` | 하트, 음표 같은 특수 노트는 사운드까지 맞추는 편이 안전합니다. |
| `configData.length` | 롱노트나 샌드백처럼 길이가 있는 노트에서 중요합니다. |
| `tick`, `dt`, `showTick` | 배치 시간과 화면 표시 시작 시간을 결정합니다. |

---

### 1.2 리스트 갱신: In-place 방식
노트 리스트를 새 List 객체로 교체하면, 그 리스트를 참조하던 다른 컴포넌트들이 옛 객체를 계속 바라봐 갱신이 반영되지 않습니다. 따라서 List의 메모리 주소는 그대로 둔 채 `Clear()`로 비우고 새 노트를 채우는 **In-place 갱신**을 사용합니다.

### 1.3 `[0]`번 슬롯 보존
`[0]`번 노트는 게임이 기준점으로 사용하므로, 임의로 수정·삭제하면 노트가 스폰되지 않거나 판정이 멈출 수 있습니다. `[0]`번은 원본 그대로 두고, 실험 노트는 `[1]`번 슬롯부터 채우는 것이 표준 원칙입니다.

---

## 🛠️ 2. 핵심 파일 및 빠른 수정 코드 정보

### 2.1 주요 수정 위치

* **노트 실험**: `Database/Stage/DBStageInfoPatch.cs` 내의 `ExperimentNotes` 배열 수정
  ```csharp
  private static readonly ExperimentNoteSpec[] ExperimentNotes =
  {
      new ExperimentNoteSpec { Label = "지상 일반 노트", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0, Speed = 5 },
  };
  ```
* **곡 메타데이터 실험**: `UI/Stage/Selection/PnlStagePatch.cs` 상단 상수 수정
  ```csharp
  private const bool EnableSongTitleExperiment = true;
  private const string ExperimentTitle = "화영왕";
  private const string ExperimentArtist = "화영왕";
  private const string ExperimentLevelDesignerLabel = "레벨 디자이너";
  private const string ExperimentLevelDesignerName = "화영왕";
  ```
* **보스 프리팹 규칙**: `Battle/Mechanics/BossPatch.cs` 내의 `BossRewriteRules` 배열 수정
  ```csharp
  private static readonly BossRule[] BossRewriteRules = new[]
  {
      new BossRule { OrigName = "*", OrigScene = null, OrigIsLast = null, NewName = "0401_boss", NewScene = 4 },
  };
  ```

---

## 🦖 3. 고급 비주얼 및 연출 제어 기믹

### 3.1 🌟 실시간 보스 교체 (Dynamic Boss Swap)
스테이지 연출 도중 한 보스가 지쳐 물러난 뒤, **완전히 다른 보스를 실시간으로 교환**하여 다시 무대에 세울 수 있습니다.
1. **`swap:` 액션 트리거**: 보스 액션 속성에 `swap:보스명:씬번호` (예: `swap:0401_boss:4`)를 설정하면 런타임에 보스 모델을 즉각 재구축합니다.
2. **부활 구조**: 이전 보스가 `out` 연출로 물러나 완전히 비활성화(`SetActive(false)`)되더라도, 모드가 트리거 시점에 부모 트랜스폼을 추적하여 안전하게 깨워 활성화시킵니다.

---

### 3.2 🌟 올 퍼펙트 배너 오버레이
곡이 끝나면 표시되는 기본 `FULL COMBO!` 낱개 글자 이미지 11개(`ImgF`, `ImgU` 등)를 비활성화하고, 그 자리에 직접 만든 `ALL PERFECT!` 배너를 표시하는 연출입니다. (비유 설명 → [ANALOGIES.md](ANALOGIES.md#41-올-퍼펙트-배너--기존-간판을-끄고-새-간판으로-교체-))

1. **판정 감시**: 판정 레코드(`TaskStageTarget.AddScore`)를 가로채 정확도가 100%인지 실시간 계산합니다.
2. **시그니처 폰트 추출**: 게임 내 만화풍 폰트(`LuckiestGuy-Regular`)를 메모리에서 가로채 복사합니다.
3. **스타일링 이식**: 골드 옐로우 색상(`Color(1f, 0.85f, 0f)`)에 4px 검은색 테두리(`Outline`)와 6px 그림자(`Shadow`)를 적용해 배너를 표시합니다. 올 퍼펙트 조건에 미달하면 기존 순정 풀콤보 배너를 그대로 보여줍니다.

---

### 3.3 🌟 커스텀 BGA
VideoPlayer가 투사되는 Quad(`VideoBackgroundQuad`)를 카메라 앞에 고정 배치하고, 노트보다 뒤로 가도록 소팅 오더를 낮게 잡아 배경 영상으로 깔아줍니다. 카메라가 움직여도 영상은 화면을 꽉 채우고, 노트는 그 앞을 지나갑니다. (비유 설명 → [ANALOGIES.md](ANALOGIES.md#42-커스텀-bga--카메라-렌즈-앞에-고정한-스크린-))

---

## 🚀 4. 빌드 및 반영

1. **컴파일**: 저장소 루트에서 `build.bat`를 실행합니다.
   ```powershell
   # 개발용 디버그 빌드
   .\build.bat
   
   # 배포용 릴리즈 빌드
   .\build.bat release
   ```
2. **배포**: 결과물 DLL을 Muse Dash의 `Mods/` 경로로 복사합니다.
3. **실행**: 게임의 **"실험 모드"** 태그 탭 ➡️ **"실험 앨범"**에서 주입된 곡들을 선택해 채보 실험과 연출 테스트를 즉각 수행할 수 있습니다.
