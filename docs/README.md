# 📚 문서 목차 (Docs Index)

`muse-dash-custom-chart` 문서는 읽는 목적에 따라 아래 6개 폴더로 나뉘어 있습니다.
처음 왔다면 **getting-started → architecture → guides** 순서로 읽는 것을 권장합니다.

---

## 🚪 getting-started/ — 처음 읽는 문서

| 문서 | 내용 |
| --- | --- |
| [MODDING_MINDSET.md](getting-started/MODDING_MINDSET.md) | 모딩에서 진짜 중요한 것 (문법이 아니라 탐색·문제 해결) |
| [ANALOGIES.md](getting-started/ANALOGIES.md) | 비유로 이해하는 모드 구조 |
| [MODDING.md](getting-started/MODDING.md) | 실험 모드 전체 가이드 (빌드·연동 기초) |

## 🏛️ architecture/ — 시스템 구조와 코드 지도

| 문서 | 내용 |
| --- | --- |
| [ARCHITECTURE.md](architecture/ARCHITECTURE.md) | 유지보수 지도, 어디부터 읽어야 하는지 |
| [MOD_SYSTEM_BLUEPRINT.md](architecture/MOD_SYSTEM_BLUEPRINT.md) | 통합 시스템 설계도 및 기술 명세서 |
| [CODE_REFERENCE.md](architecture/CODE_REFERENCE.md) | C# 파일별 역할·클래스·흐름 레퍼런스 |
| [CAST_AND_CUSTOM_TAG_GUIDE.md](architecture/CAST_AND_CUSTOM_TAG_GUIDE.md) | 유니버설 래퍼 패턴 및 커스텀 태그 동적 주입 구조 |
| [PERFORMANCE_REVIEW_2026-09-20.md](architecture/PERFORMANCE_REVIEW_2026-09-20.md) | 전체 추적 파일 성능 검색, 병목 후보·실행 근거·검증 결과 (문서 끝에 후속 처리 상태 표 있음) |
| [PERFORMANCE_REVIEW_2026-09-20_FILES.md](architecture/PERFORMANCE_REVIEW_2026-09-20_FILES.md) | 위 검토의 파일 목록 스냅샷 (기준 커밋 시점 것이라 이후 삭제·이동된 파일은 링크가 없음) |

## 🛠️ guides/ — 제작·설정·운영 가이드

| 문서 | 내용 |
| --- | --- |
| [CUSTOM_CHART_GUIDE.md](guides/CUSTOM_CHART_GUIDE.md) | 커스텀 곡/차트/음원/BGA 제작 및 폴더 구성 |
| [BMS_PARSING.md](guides/BMS_PARSING.md) | BMS 파싱·매핑 규칙 및 셀 코드 사양 |
| [OFFLINE_CUSTOM_SANDBOX_GUIDE.md](guides/OFFLINE_CUSTOM_SANDBOX_GUIDE.md) | 오프라인 샌드박스 구성 및 플래그 설정 |
| [DISCORD_RICH_PRESENCE.md](guides/DISCORD_RICH_PRESENCE.md) | 디스코드 리치 프레젠스 연동 명세 |
| [LOGGING_AND_TROUBLESHOOTING.md](guides/LOGGING_AND_TROUBLESHOOTING.md) | 로그 읽는 법과 문제 해결 순서 |
| [MOBILE_TOUCH_AND_INPUT_GUIDE.md](guides/MOBILE_TOUCH_AND_INPUT_GUIDE.md) | 모바일 터치 설정창 복원 및 인게임 마우스/터치 브릿지 명세 |
| [CHECKLIST.md](guides/CHECKLIST.md) | 변경·릴리즈 전 검토 체크리스트 (실제 사고 지점만 수록) |

## 🧪 experiments/ — 기능별 실험 기록

| 문서 | 내용 |
| --- | --- |
| [NOTE_EXPERIMENTS.md](experiments/NOTE_EXPERIMENTS.md) | 노트 타입 사전 및 `ExperimentNotes` 실험법 |
| [BOSS_EXPERIMENTS.md](experiments/BOSS_EXPERIMENTS.md) | 보스 액션 트리거와 보스 프리팹 교체 |
| [SCENE_BACKGROUND_SWAP.md](experiments/SCENE_BACKGROUND_SWAP.md) | 배경만 바꾸고 노트 정체는 유지하는 씬 스왑 원리 |
| [UID_INJECTION.md](experiments/UID_INJECTION.md) | 가상 곡 UID 등록 초기 설계·탐색 메모 |
| [DIALOG_INJECTION.md](experiments/DIALOG_INJECTION.md) | 커스텀 곡 대사(Dialog) 주입 설계 및 실측 기록 (보류) |
| [GHOST_NOTE_ALPHA_HOLD.md](experiments/GHOST_NOTE_ALPHA_HOLD.md) | 고스트 노트가 사라지지 않게 하는 Spine 알파 키 재작성 원리와 미해결 이슈 |
| [NOTE_COLOR_TINTING.md](experiments/NOTE_COLOR_TINTING.md) | 노트 종류별 색조(Tint) 변조 가능성 실측 (참고용 코드, 현재 모드에는 포함되지 않음) |

## 🔮 muse-dash-2/ — 차기작 대비 분석

| 문서 | 내용 |
| --- | --- |
| [MUSE_DASH_2_SPECULATIVE_GUIDE.md](muse-dash-2/MUSE_DASH_2_SPECULATIVE_GUIDE.md) | 뮤즈 대시 2 모딩 선행 가이드 및 ILSpy 검색 전략 |
| [MD2_TAG_RETARGET_MAP.md](muse-dash-2/MD2_TAG_RETARGET_MAP.md) | 태그/카테고리 시스템 MD2 재타깃팅 대조 지도 |

## 🔍 inferred_game_code/ — 게임 원본 코드 역추론

게임 내부 클래스를 다룬 문서 모음입니다. **`CollabExpiration_Reconstruction.md`만 실제 게임에서 실측한 기록**이고, 나머지 5개(`DBStageInfo`, `GameMusicScene`, `NoteController`, `SaveDataManager`, `StageBattleComponent`)는 패치 코드에서 짐작해 쓴 **의사코드(추측)** 입니다. 근거로 삼기 전에 [inferred_game_code/README.md](inferred_game_code/README.md)의 안내를 먼저 읽으세요.

| 문서 | 성격 |
| --- | --- |
| [CollabExpiration_Reconstruction.md](inferred_game_code/CollabExpiration_Reconstruction.md) | 실측 (콜라보 종료일 판정) |
| [DBStageInfo](inferred_game_code/DBStageInfo_Reconstruction.md) · [GameMusicScene](inferred_game_code/GameMusicScene_Reconstruction.md) · [NoteController](inferred_game_code/NoteController_Reconstruction.md) · [SaveDataManager](inferred_game_code/SaveDataManager_Reconstruction.md) · [StageBattleComponent](inferred_game_code/StageBattleComponent_Reconstruction.md) | 추측 의사코드 |
