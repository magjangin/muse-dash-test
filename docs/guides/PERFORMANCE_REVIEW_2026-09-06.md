# 성능 병목 점검 — 2026-09-06

## 범위와 판단 기준

- 기준 커밋: `a04ca63`. `git fetch origin` 후 `main`과 `origin/main`의 ahead/behind가 모두 0인 상태에서 시작했다.
- 추적 파일 197개(C# 139개)를 대상으로 파일 목록과 텍스트의 반복문·I/O·객체 검색·리플렉션·로그 호출을 전수 스캔했다. 실행 경로와 후보 함수는 본문 및 호출자를 추가 검토했다. 모든 줄을 수동 정독했다는 의미는 아니다.
- 문서·스크립트·테스트·SignatureDumper는 게임 런타임 코드와 구분했다. PNG는 리소스이며 이미지 내용 자체를 성능 분석하지 않았다. 빌드 생성물과 외부 게임 DLL 전체는 검토 범위 밖이다.
- 기존 `H:/muse dash hwa/MelonLoader/Latest.log`(수정 시각 2026-09-05 19:14:29)도 확인했다. 이 로그의 DLL과 현재 소스가 동일한 빌드인지 검증하지 않았으므로 참고 관측으로만 사용한다.
- 아래 우선순위는 계측할 순서다. 현재 실행 중인 게임을 프로파일링하거나 프레임 드랍을 재현하지 않았다. 로그 시각 차이는 로그 출력·스케줄링 등이 포함된 경과 시간이며 함수별 CPU 시간이나 프레임 시간은 아니다.

## 먼저 확인할 지점

### 1. 배틀 진입: 롱노트 전개와 IL2CPP 데이터 반복 처리

근거: `DBStageInfoExperimentChart.cs:13`, `:215`, `DBStageInfoPatch.cs:17`, `DBStageInfoExperimentChart.Sorting.cs:11`, `SceneZzTransformTracker.cs:163`.

`ApplyExperimentChart`는 동기적으로 스펙 생성 → 노트 복제·삽입 → 더블 판정 → 정렬·참조 재연결 → 원본 UID 보관 → 유형 집계를 수행한다. 롱노트는 0.1초 단위 몸통으로 전개된다. 길이 L초인 한 홀드는 대략 `ceil(L / 0.1) + 1`개의 런타임 노트가 된다. 파싱 전 노트 수보다 전개 후 노트 수가 비용을 더 잘 설명한다.

기존 로그에는 19:11:12.767 적용 시작, .790 BMS 변환 완료, .814 더블 판정 완료, .827 정렬 완료, .849 원본 UID 등록, .857 전체 완료가 있다. 전체 구간은 약 90ms이며, BMS 652개가 런타임 1,112개로 늘었다. 롱노트 19쌍이 머리·몸통·꼬리 497개로 전개됐다. 실제 로딩 지연의 가장 뚜렷한 관측 후보다.

계측: 위 각 단계의 elapsed time, managed allocation, 전개 후 노트 수를 곡당 한 번 기록한다. 반복되는 `noteData`/`configData` 박싱 getter를 지역 변수로 묶는 최적화와 중복 복제 제거를 우선 검토한다. 값 타입 수정 후 write-back과 정렬 후 상호참조 복구는 유지해야 한다. 롱노트 간격 변경은 게임 동작을 바꾸므로 단순 성능 수정으로 취급하지 않는다.

### 2. 곡 선택: Discord 중복 갱신, 씬 검색, 다량 Info 로그

근거: `Core/CustomPlaySession.cs:42`, `Integration/DiscordPresenceManager.cs:150`, `Patches/Diagnostics/DiscordManagerDebugPatch.cs:25`.

`RememberMusicSelection`은 중복 억제 기능이 있는 `UpdateForSelection`을 호출한 뒤 원본 `SetUpdateActivity`를 다시 직접 호출한다. 두 번째 경로는 중복 억제와 `EnableDiscordRPC` 게이트를 우회한다. 후킹된 호출마다 패널을 `FindObjectOfType`으로 최대 3번 찾고, 선택/배틀 문맥에서는 대략 10줄 이상의 Info 로그를 만든다. 로그 레벨을 내려도 패널 검색은 계속 실행된다.

기존 로그에서 Postfix 완료는 18회이며, 19:10:44.740과 .746부터 같은 곡에 대한 진단 묶음이 연속으로 보인다. 이것이 네트워크 지연이라는 증거는 없으며, 확인된 것은 로컬 검색·문자열·로그 작업의 중복이다.

개선 방향: 상태 전송 경로를 하나로 합치고 최종 상태 기준으로 중복 억제한다. 패널 참조/씬 상태를 캐시하고 상세 진단은 계산 전에 Verbose 게이트를 둔다.

### 3. 커스텀 곡 목록 첫 표시와 장시간 탐색: 커버 디코딩 및 무제한 보관

근거: `Patches/UI/Music/MusicButtonCellPatch.cs:159`, `:192`.

셀 초기화에서 `File.ReadAllBytes` → `ImageConversion.LoadImage` → `Sprite.Create`가 동기 실행된다. 이미지 크기 제한·축소·캐시 퇴출이 없고 Texture와 Sprite 모두 `DontUnloadUnusedAsset`으로 보관된다. 반복 표시 비용은 캐시가 줄여 주지만, 방문한 곡 수와 원본 이미지 해상도에 따라 메모리가 누적된다.

기존 로그에는 1080×1080, 500×500 두 장, 2048×2048 커버 로드가 있다. 2048×2048 RGBA32 한 장은 픽셀 데이터만 16MiB라는 크기 예시를 들 수 있다. 실제 텍스처 포맷·CPU/GPU 복사·mipmap 여부를 계측하지 않았으므로 실제 메모리 사용량으로 단정하지 않는다. 로그 사이 시간도 PNG 디코딩 시간으로 해석할 수 없다.

개선 방향: 표시 크기에 맞는 썸네일, 제한된 캐시, 소유 Texture/Sprite 명시적 해제. 디코딩 시간과 native/GPU 메모리를 함께 확인한다.

### 4. 고스트 노트 등장: 캐시 적중 후에도 임시 진단 전수 순회

근거: `Patches/Battle/Mechanics/GhostNoteAlphaHold.cs:188`, `:268`.

알파 상태 캐시가 맞아도 `ProbeCachedState`가 실행된다. `ReadMinAlpha`로 타임라인과 알파 키를 모두 순회한 뒤에야 `probeLogged`를 검사하므로, 정상 로그가 한 번만 보여도 검색은 반복된다. 별도 opt-in 또는 로그 레벨 게이트가 없으며 프로세스 전체 300회 예산까지 발생한다. 300회 이후에도 계속 전수 순회한다는 뜻은 아니다.

기존 로그의 19:11:39.436에 알파 키 27개를 읽은 정상 진단이 한 번 보인다. 실제 순회 횟수와 비용은 이 한 줄로 계산할 수 없다.

개선 방향: 진단은 명시적으로 켰을 때만 수행하고 포인터 변경 시점을 중심으로 계측한다. 이 진단은 캐시 불일치 조사 목적이므로 원인 확인 없이 캐시 동작 자체를 변경하지 않는다.

### 5. 키 바인딩 로드 실패 시: 2초 제한을 우회하는 매 Repaint 재시도

근거: `Patches/UI/Custom/InputOverlay.Render.cs:90`, `Patches/UI/Custom/InputOverlay.cs:78`.

`!keysLoaded` 상태에서 `checkTimer >= CheckInterval || airKeys.Count == 0`이면 검색한다. 최초 로드가 실패하면 airKeys가 빈 상태라 다음 Repaint에도 즉시 `Resources.FindObjectsOfTypeAll`과 리플렉션을 재시도한다. `LoadPlayerKeybinds`는 예외를 내부에서 처리하므로 외부 FeatureGuard가 실패 누적을 보지 못하는 경우도 있다.

키 설정이 정상 로드되는 경우에는 해당하지 않는다. 기존 로그에는 관련 오류가 없으며, 설정 객체를 못 찾는 경우는 조용히 반환하므로 오류가 없다고 실패 경로가 완전히 배제되지는 않는다.

개선 방향: 최초 1회 시도와 다음 시도 시각을 분리해 실패에도 재시도 간격을 적용한다. 설정 객체가 없거나 필드 해석이 실패하는 환경에서 호출 횟수를 확인한다.

### 6. 곡 선택·준비창 갱신: 기록 JSON 반복 읽기

근거: `Core/CustomRecordStore.cs:272`, `Patches/UI/Stage/CustomRecordUiPatchHelper.cs:24`, `Patches/UI/Stage/PnlPreparationPatch.cs:25`.

`LoadResult`는 매번 경로 확인, 동기 파일 읽기, 문자열 분할 파싱, 채보 지문 검증을 한다. 채보 해시는 캐시하지만 기록 자체는 캐시하지 않는다. 준비창은 즉시 적용 외에 0.25초·1초 지연 적용도 예약한다. 같은 기록을 짧은 간격에 반복 읽고 UI와 Info 로그를 갱신할 수 있다.

기존 로그에서 성공 읽기는 6회다. 소스 주석의 과거 86회 기록은 이번 로그의 관측 횟수가 아니다. 작은 로컬 JSON에서는 영향이 작을 수 있으므로 우선순위는 위 항목보다 낮다.

개선 방향: 곡 폴더 키·난이도·기록 수정 상태·채보 지문에 맞는 캐시를 두고 저장/채보 변경 시 무효화한다. UID만으로 묶어 과거 기록 오염 문제가 재발하지 않게 한다.

### 7. BGA 재생 중: 길이가 다른 미디어의 반복 seek 가능성

근거: `Patches/Hwa/HwaSyncManager.cs:69`.

0.1초마다 진행률에 각 미디어의 전체 길이를 곱해 목표 시간을 계산한다. 차트/오디오/영상 길이가 다르면 동일한 경과 시간으로 재생 중이어도 영상 오차가 커질 수 있다. 오차가 0.2초를 넘으면 `VideoPlayer.time`을 설정하고 0.5초 쿨다운 후 재검사한다. 반복 seek가 발생하는 경우 영상 재탐색 비용과 끊김을 의심할 수 있다.

기존 로그에서 Sync.BGA와 Sync.BGM은 모두 0회이므로 이번 관측에서 확인된 현상은 아니다. 미디어 길이·프레임 시간·seek 횟수의 동시 계측이 필요하다. 의도적으로 길이를 비례 매핑하는 사양인지도 확인한 뒤 공통 재생 시간과 명시적 오프셋 사용 여부를 결정한다.

## 추가 확장성 후보와 이미 적용된 제한

- 시작 시 모든 곡의 manifest와 BMS를 순차 선로드한다(`HwaResourceManager.cs:27`). 곡 수가 많으면 시작 시간과 캐시 메모리가 증가한다. 곡별 시간/크기를 먼저 계측한다.
- 메뉴 OGG 경로 검색은 코루틴의 첫 0.2초 대기보다 앞선다(`HwaMenuBgmController.cs:98`). 같은 UID의 트리거가 29ms 간격으로 두 번 보인다. 세대 검사가 오래된 오디오 로드는 막지만 그 전에 실행한 디렉터리 검색은 막지 못한다. 두 번 모두 음원을 디코딩했다고 해석하면 안 된다.
- 배틀 OGG 탐색은 txt×ogg 중첩 비교를 사용한다(`HwaBattleMediaController.cs:220`). 파일이 많은 곡 폴더에서는 인덱싱 후보지만 일반적인 작은 폴더에서는 후순위다.
- BMS 시간 계산은 이미 정렬 후 단일 sweep이다. WAV 해석 캐시, showTick 정렬 키 사전 추출, 원본 템플릿 두 개만 복제, 프리팹 후보 중복 제거도 존재한다. 이를 미적용 최적화로 보고하지 않는다.
- OnGUI는 Repaint만 처리하고, 판정 이력에는 생산 측 제한, 터치에는 프레임 캐시, 설정 파일에는 배틀 중 I/O 차단, 체력바에는 캐시/0.1초 제한, HitPoint 설치에는 재시도 간격/상한이 있다.
- Spine 계약서 덤프와 상세 ExperimentNotes 진단은 기본 off다. GhostNote 임시 probe와는 별개다.

## 검증과 다음 계측

- `dotnet run --project "muse dash test.LogicTests/muse dash test.LogicTests.csproj" -c Release`: 75개 통과, 실패 0개. 순수 로직 테스트이며 Unity/IL2CPP 성능 검증은 아니다.
- `docs/guides/CHECKLIST.md`를 확인했다. 런타임 소스는 수정하지 않고 이 보고서만 추가했다. 모드 빌드/게임 Mods 배포와 정상·0-0 폴백 실제 배틀 진입은 이번 점검에서 수행하지 않았다.
- 먼저 동일 곡의 첫 진입/재진입에서 차트 처리 단계별 시간과 할당량을 비교한다. 이어서 목록 스크롤의 커버 디코딩, Discord 호출 수, 고스트 구간 probe 비용을 계측한다.
- 프레임 p95/p99와 최대 시간을 함께 기록하고 로그는 매 노트가 아닌 곡당 요약으로 남긴다. 저사양 기기/큰 채보/많은 커버/설정 로드 실패/길이가 다른 BGA를 각각 분리해 재현해야 원인을 구분할 수 있다.
