# 성능 검토 — 2026-10-25

검토 기준: `82ce540`(로컬 main). C# 135개 파일을 대상으로 매 프레임 코드(`MainMod.OnUpdate/OnGUI`), 자주 불리는 게임 메서드에 건 Harmony 훅, 채보 로딩 경로를 다시 봤습니다. [2026-09-20 검토](PERFORMANCE_REVIEW_2026-09-20.md)의 후속입니다.

**정적 검토만 했습니다.** 프로파일러로 프레임 시간을 재지 않았으므로 아래는 "후보"이고, 우선순위도 측정 순위가 아닙니다. 런타임 코드는 바꾸지 않았습니다.

---

## 1. 지난 검토 항목의 현재 상태

| # | 항목 | 지금 |
| --- | --- | --- |
| 1 | 고스트 노트 캐시 적중 후 진단 순회 | 해결. `ProbeCachedState`가 없어졌고 `PlayByKey` 첫 줄에서 `actionKey`로 거름 |
| 2 | BMS 캐시 조회에 숨은 채보 순회 | 해결. 설명 없는 `TryGetCachedHwaBmsChart(uid, out chart)`가 따로 있고, 설명을 만드는 쪽은 채보 주입 때 1회뿐 |
| 3 | 음악 정보 진단이 Verbose 꺼도 실행 | 해결. `PnlMusicDiagnostics`가 `IsLevelEnabled(Verbose)`로 막음 |
| 4 | 키 설정 실패 시 2초 제한 우회 | 해결. `CurrentRetryInterval()`과 실패 횟수로 재시도 |
| 5 | 같은 곡 메뉴 BGM 요청 중복 | 해결. 이미 재생 중인 uid면 건너뜀 |
| 6 | 기록 UI마다 JSON 재파싱 | 해결. `FileStampCache`로 파일 시각이 같으면 캐시 사용 |
| 7 | 커버 캐시 무한 증가 | 해결. LRU 상한(`MaxCacheEntries`) |

로그도 `ModLogMessageHandler`(`shouldAppend`)라 레벨이 꺼져 있으면 구멍 안의 식까지 평가하지 않습니다. `OnUpdate`의 `FeatureGuard.Run`은 정적 메서드 그룹이라 매 프레임 델리게이트를 만들지 않습니다.

## 2. 새로 찾은 후보

### 2.1 꺼져 있는 모바일 터치 기능의 입력 훅 8개가 항상 걸려 있다 (중간, 측정 필요)

- 위치: [MouseTouchBridgePatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/MouseTouchBridgePatch.cs)
- 대상: `StandloneController.GetButton/GetButtonDown/GetButtonUp`, `InputManager.GetButton/GetButtonDown/GetButtonUp(MDButtonType)`, `InputManager.controllerType` getter, `HideCursor.Update`. 모두 매 프레임(버튼 종류마다) 불리는 메서드입니다.
- `config.txt`의 모바일 터치 기본값은 꺼짐(`InputOverlay.enableMobileTouch = false`)이라 본문은 첫 줄에서 돌아갑니다. 그래도 IL2CPP에서는 훅이 걸려 있는 것만으로 호출마다 네이티브→관리 트램펄린을 지나고, `__instance`·`List<int> __result` 같은 참조 인자를 관리 래퍼로 감쌉니다(호출마다 작은 할당).
- 개선 방향: 기능이 꺼져 있으면 이 클래스를 걸지 않거나, `config.txt`에서 켤 때 걸고 끌 때 푸는 방식. 지금 `PatchInstaller`는 모든 클래스를 무조건 겁니다.
- 검증: 기능을 끈 상태에서 이 훅을 뺀 빌드와 비교해 배틀 중 GC 할당량과 프레임 시간을 잽니다.
- 같은 성격(훅 비용은 남고 본문은 가벼움): `AudioSource.PlayOneShot` ×2(모든 효과음), `SpineActionController.PlayByKey` Postfix ×2(모든 노트·캐릭터 애니메이션), `GameTouchPlay.TouchResult` ×2(모든 타격). 기능상 필요한 훅이라 빼기보다 측정해 볼 대상입니다.

### 2.2 씬 스왑 추적기가 결과를 아무도 안 읽는데 채보마다 돈다 (낮음) — ✅ 제거함

> 2026-10-25 `feature/tracker-cleanup-devil-hp`에서 추적기 3개 파일과 등록 호출을 지웠습니다. 씬 배경 스왑 실기 확인이 남아 있습니다.

- 위치: `Patches/Scene/SceneZzTransformTracker*.cs` 3개 파일(합계 약 730줄, 삭제됨)
- 씬 스왑이 `IsIgnoreCurrentNoteChecking` 방식으로 바뀐 뒤 `Record`, `RestoreIdentities`, `RestoreRuntimeObjects`, `TryGetBmsOriginalUid`를 부르는 곳이 없습니다(2026-10-25 저장소 검색).
- 그런데 `DBStageInfoExperimentChart.cs`가 채보를 주입할 때마다 `RegisterBmsOriginalIdentities`를 불러, 노트마다 IL2CPP 문자열 필드 약 14개를 읽어 사전에 담습니다. 이 사전은 다시 읽히지 않습니다.
- 개선 방향: 등록 호출과 추적기 파일을 제거. 씬 배경 스왑을 실기로 한 번 확인해야 합니다([SCENE_BACKGROUND_SWAP.md](../experiments/SCENE_BACKGROUND_SWAP.md)).

### 2.3 작은 것들 (낮음)

- `AudioSource.clip` setter Prefix([HwaMenuBgmController.cs](../../muse%20dash%20test/Patches/Hwa/HwaMenuBgmController.cs))가 클립을 넣을 때마다 `gameObject.name == "BGM"`으로 거릅니다. 이름 문자열 마샬링이 매번 생기고, 이름으로 고르는 방식이라 [CHECKLIST](../guides/CHECKLIST.md)의 "이름으로 고르지 말 것"에도 걸립니다. [GameBgmSource.cs](../../muse%20dash%20test/Core/GameBgmSource.cs)의 게임 참조와 비교하면 둘 다 풀립니다.
- 판정바 훅이 타격마다 `tno.md`(큰 `MusicData` 구조체)를 통째로 꺼내 `tick` 하나만 씁니다. 판정바를 `GameGlobal.gTouch.tickTime` 기준으로 바꾸면 함께 정리됩니다([JUDGMENT_BAR_TIMING.md](../experiments/JUDGMENT_BAR_TIMING.md)).

## 3. 문제없다고 본 것

- 체력바 탐색(`HealthBarFinder`)은 찾은 객체를 재사용하고, 워터마크 확인은 0.1초 주기입니다.
- 진행바 비율(`ProgressBarPatch`)은 채보 길이를 한 번 계산해 캐시합니다.
- 보스 BMS 조회(`TryGetFirstBmsBoss`, `PrewarmBmsBosses`)는 보스 초기화 때만 돕니다.
- 600줄 넘는 소스 파일은 없습니다(가장 큰 것: `ExperimentHitPointInstaller.cs` 503줄).
