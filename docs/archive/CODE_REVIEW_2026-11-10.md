# 코드 검토 — 2026-11-10

검토 기준: `3146285`(main). 매 프레임 코드, 자주 불리는 Harmony 훅, 최근 두 커밋(`9bf98d4`, `dc5e804`)에서 바뀐 코드, 기록 저장(`CustomRecordStore`), 문자열로 게임 멤버를 가리키는 곳을 봤습니다. [2026-10-25 성능 검토](PERFORMANCE_REVIEW_2026-10-25.md)의 후속입니다.

**정적 검토입니다.** 프레임 시간은 재지 않았습니다. 검증은 빌드(경고 0), 로직 테스트 146개 통과, 게임 시작 스모크 테스트(패치 클래스 126개 적용, 대상 141개 해석, 곡 선택 화면까지 `[ERROR]` 0건)까지 했습니다. **커스텀 곡 미리듣기와 배틀 진입은 밟지 않았습니다.** 아래 1.1·1.2·2.1·2.2는 그 경로에서 도는 코드이므로 [CHECKLIST](../guides/CHECKLIST.md) 3절의 실기 확인이 남아 있습니다.

---

## 1. 고친 것

### 1.1 곡 선택 화면 판정의 스캔 제한이 준비 패널 탐색을 굶겼다 (버그)

- 위치: [HwaMenuBgmController.cs](../../muse%20dash%20test/Patches/Hwa/HwaMenuBgmController.cs) `IsStageSelectionContextActive`
- `9bf98d4`가 씬 스캔을 30프레임 간격으로 제한하면서 `PnlStage`와 `PnlPreparation`이 `lastScanFrame` 하나를 따로따로 검사했습니다. 앞 블록이 `lastScanFrame`을 지금 프레임으로 올린 직후라 뒤 블록의 조건(`0 >= 30`)은 늘 거짓이었습니다.
- 결과: `PnlStage`를 찾지 못하는 동안(캐시가 비어 있고 패널이 꺼져 있을 때)에는 `PnlPreparation`을 한 번도 찾지 않습니다. 그 상태로 준비 화면에 있으면 "곡 선택 화면이 아님"으로 판정되어 커스텀 미리듣기가 멈추고 원곡 클립 차단이 풀립니다.
- 수정: 스캔 허용 여부를 한 번만 판정하고 같은 차례에 두 패널을 모두 찾습니다. 간격은 `ScanIntervalFrames` 상수로 뺐습니다.

### 1.2 BGM 소스 판정을 이름에서 게임 참조로 (업데이트 내성 + 할당)

- 위치: 같은 파일 `AudioSource_SetClip_Patch`
- `AudioSource.clip` 세터는 모든 오디오 소스가 공유하는데, Prefix와 Postfix가 호출마다 `gameObject.name == "BGM"`으로 걸렀습니다. 지난 검토 2.3의 남은 항목입니다.
- 수정: `GameBgmSource`(= `AudioManager.bgm`)의 포인터와 비교합니다. 포인터는 프레임당 한 번만 다시 얻습니다. 참조를 못 얻을 때만 이름 비교로 돌아갑니다. 클립을 주입하는 `FindMenuAudioSource`가 이미 같은 참조를 먼저 쓰므로 주입 대상과 감시 대상의 기준이 같아졌습니다.

### 1.3 판정바 훅이 타격마다 `MusicData`를 두 번 복사 (할당)

- 위치: [JudgmentBar.cs](../../muse%20dash%20test/Patches/UI/Custom/JudgmentBar.cs) `GameTouchPlay_TouchResult_Patch`
- `tno.md == null` 검사와 `var md = tno.md`가 각각 구조체 전체를 새 박스로 복사했습니다. 박싱 getter는 null을 돌려주지 못하므로 앞의 검사는 늘 거짓입니다([CHECKLIST](../guides/CHECKLIST.md) "값 타입 필드" 항목). 한 번만 읽도록 고쳤고 `GameGlobal.gGameMusic`도 한 번만 읽습니다.

### 1.4 BGA가 없는 곡에서 0.1초마다 카메라를 다시 찾음

- 위치: [HwaSyncManager.cs](../../muse%20dash%20test/Patches/Hwa/HwaSyncManager.cs)
- `isCacheInitialized`는 "찾아봤다"를 기억하려던 플래그였지만 조건이 `!isCacheInitialized || cachedBgaPlayer == null`이라 못 찾은 동안에는 매번 다시 찾았습니다. BGA 없는 커스텀 곡은 곡 내내 초당 10회 `Camera.main`(태그 검색)과 `Transform.Find`를 불렀습니다.
- 수정: 못 찾은 동안에는 1초에 한 번만 찾습니다(쿼드가 늦게 생길 수 있어 재탐색 자체는 남겼습니다). 플래그는 지웠습니다. 함께 5단 중첩을 조기 반환과 메서드 두 개(`GetOrFindBgaPlayer`, `CorrectVideoDrift`)로 풀었습니다.

### 1.5 쓰이지 않는 `GameBindings` 상수 20개 삭제 (업데이트 내성)

- 패치 대상을 `nameof(...)`로 옮긴 뒤 `GameBindings`의 패치용 상수는 쓰는 곳이 없었고, `PatchHealthCheck`도 이 파일을 읽지 않습니다. 그런데 파일 머리말과 [ARCHITECTURE.md](ARCHITECTURE.md)는 "업데이트로 패치가 깨지면 이 파일부터 고치라"고 안내하고 있었습니다. 고쳐도 아무 효과가 없는 곳입니다.
- 실제로 쓰이는 `Il2CppCollection.AddMaskValue`만 남기고 문서의 안내를 `[HarmonyPatch(typeof, nameof)]` 쪽으로 바꿨습니다.

### 1.6 줄바꿈이 깨진 파일 2개

- `9bf98d4`에서 `HwaMenuBgmController.cs`와 `docs/experiments/HIDDEN_SHEET_LOGIC.md`가 `CR CR LF`로 커밋됐습니다. git이 두 파일을 바이너리(`-text`)로 취급해 diff가 "파일 전체 변경"으로 나왔고(963줄), 편집 도구의 문자열 치환도 실패했습니다. CRLF로 되돌렸습니다. CHECKLIST에 항목을 추가했습니다.

### 1.7 가독성

- `HywStageManager`: 세 군데에 복사돼 있던 스테이지 종료 처리를 `LeaveStage`로 모았습니다.
- `AllPerfectSound`: `PlayOneShot` 두 오버로드의 같은 Prefix 본문을 `ShouldPlayOneShot` 하나로 모았습니다(예전에 한쪽에만 설정 검사가 빠졌던 자리입니다).
- `ModReflection`: `ResolveMember`의 문서 주석이 엉뚱한 필드 위에 떠 있던 것을 제자리로 옮겼습니다.
- 영문 주석 11줄을 한국어로 바꿨습니다(`APModPatch*`, `BmsWavParser`, `BmsNoteMatcher`).
- `Patch_SkinNameProbe`: `ToLowerInvariant().Contains` 대신 `IndexOf(OrdinalIgnoreCase)`.

---

## 2. 고치지 않고 적어 두는 것

동작이 바뀌는 것, 측정이 먼저인 것은 손대지 않았습니다.

### 2.1 영문 축약형 교정기의 오탐 (낮음, 동작 변경이라 보류)

- 위치: [TextNormalizer.cs](../../muse%20dash%20test/Core/TextNormalizer.cs)
- 단어 뒤에 오는 **따옴표로 감싼 한 글자**나 **복수 소유격 뒤의 단어**를 축약형으로 오인합니다. 실제 실행 결과입니다.

  | 입력 | 출력 |
  | --- | --- |
  | `Type 'S' Mix` | `Type'S' Mix` |
  | `Garden 'T' Party` | `Garden'T' Party` |
  | `Stars' Re:Birth` | `Stars'Re:Birth` |
  | `Lovers' D-Day` | `Lovers'D-Day` |

- `Rock 'n' Roll`, `Girls' Summer`, `Run 'til dawn`은 그대로입니다. 곡 제목·아티스트에만 적용되므로 영향은 표시 문자열에 그칩니다.
- 고친다면: 따옴표 **앞**에 공백이 있고 접미사 뒤에 닫는 따옴표가 오는 경우를 제외하거나, 따옴표 앞뒤 공백이 둘 다 있는 경우에만 붙이도록 좁히는 방법이 있습니다. 어느 쪽이든 지금 통과하는 테스트 입력(`you ' re`)과의 경계를 정해야 합니다.

### 2.2 남아 있는 성능 후보 (측정 필요)

- **꺼진 모바일 터치의 입력 훅 8개**: 10-25 검토 2.1 그대로입니다. `PatchInstaller`가 모든 클래스를 무조건 겁니다.
- **입력 오버레이의 캐시 유효성 검사**: `DrawInputOverlay`가 Repaint마다 `style.normal.background == null`을 세 번 확인합니다. IL2CPP 래퍼가 프레임당 6개 만들어집니다. 바로 앞의 텍스처 null 검사와 겹치지만, 씬 언로드로 스타일 쪽 참조만 끊기는 경우를 잡으려던 것이라 실기 확인 없이 빼지 않았습니다.
- **고스트 노트 캐시 키**: `ApplyToCurrentAnimation`이 고스트 노트가 날아올 때마다 `스켈레톤 이름|포인터|애니메이션 이름` 문자열을 새로 만듭니다. 고스트 노트가 많은 채보에서만 의미가 있습니다.
- **정확도 계산의 리플렉션 6회**: `AccuracyCalculator`가 `GetAccuracy` 호출마다 `ModReflection.GetInt`를 6번 부릅니다(멤버는 캐시되고 값만 박싱). 이름이 바뀌면 0으로 떨어지게 하려는 의도된 설계라 두었습니다. 다만 그때 정확도가 조용히 틀린 값이 되고, 신호는 `ModReflection`의 1회 경고뿐입니다.

### 2.3 그대로 둔 것

- 빈 `catch` 28곳: 진단 로그용 값 읽기(`Time.frameCount`, 목록 개수)와 폴백 직전의 시도입니다. 삼켜도 동작이 달라지지 않는 자리였습니다.
- 로그 메시지의 이모지: 로그를 눈으로 훑을 때 쓰는 표시라 두었습니다.
- 600줄 넘는 소스 파일은 없습니다(가장 큰 것: `HwaMenuBgmController.cs` 약 540줄, `ExperimentHitPointInstaller.cs` 503줄).

---

## 3. 업데이트 내성 점검 결과

| 항목 | 상태 |
| --- | --- |
| Harmony 패치 대상 | 전부 `typeof` + `nameof`. 문자열 리터럴 대상 0개 |
| 컴파일러 생성 이름 | `PnlBattle._MusicProgressInit_b__48_1` 1곳. 이름이 없으면 `_MusicProgressInit_b__*` + `void(float)`로 찾는 폴백이 있고, 6.7.0 덤프에서 그 조건에 맞는 메서드는 하나뿐임 |
| 필드 오프셋·RVA 하드코딩 | 없음 |
| 문자열 리플렉션(`ModReflection`) | 33곳. 대부분 가상 곡 복제(`CustomTagRegistrySupport`)와 결과 화면 값 읽기. 실패하면 1회 경고 후 폴백 |
| 이름으로 고르는 게임 오브젝트 | BGM 세터 패치는 이번에 참조 비교로 바꿈. 남은 것은 `HealthBarFinder`(체력바 이름 목록)와 참조를 못 얻었을 때의 폴백 경로 |
| 시작 시 점검 | `PatchInstaller` 126개 클래스 적용, `PatchHealth` 141개 대상 해석 (2026-11-10 실행 로그) |
