# 저장소 개선점 검토 — 2026-09-08

검토 기준 커밋: `bf5d68a`. 요청 범위는 전체 파일의 개선점 검토이며, 이번 변경은 보고서 추가뿐이다. 아래 결함은 아직 수정하지 않았다.

## 범위와 검증 수준

- Git 추적 파일 197개를 대상으로 목록·구조·설정·문서와 코드 경로를 점검했다. C#은 139개, 약 24,717줄이며 가장 큰 소스도 454줄로 600줄 제한을 넘지 않는다.
- BMS 파싱, 파일 감시/캐시, 가상 곡 식별, 기록 저장, 배틀 미디어와 UI 경로는 상세 검토했다. 나머지 진단·실험 코드와 문서는 정적 스캔 중심이다. 모든 파일의 모든 실행 경로를 검증했다는 의미는 아니다.
- 시작 시 `git fetch origin` 후 `main`이 `origin/main`보다 4커밋 앞서고 뒤처지지 않았음을 확인했다.
- 기존 LogicTests: **75개 통과, 실패 0개**.
- 전체 솔루션 Release 빌드: **경고 0개, 오류 0개**.
- 솔루션에 포함되지 않은 SignatureDumper도 별도 Release 빌드: **net472/net8.0 모두 성공, 경고·오류 0개**.
- 실제 소스를 연결한 임시 검증 프로그램으로 아래 5개 현상을 재현했다. 게임 의존성은 스텁 처리하고, 파일 감시 이벤트는 실제 핸들러를 직접 호출해 검증했다. OS 이벤트 전달이나 IL2CPP 실행 검증은 아니다.
- 게임을 실행하거나 세이브 초기화 스크립트를 실행하지 않았다. 정상 배틀 및 `0-0` 폴백 배틀 진입은 미검증이다.

## 우선 수정할 항목

### 1. [P1] 백업 실패 후에도 원본 세이브와 레지스트리를 삭제한다

근거: `scripts/reset_progress.bat:53–68`.

`copy`와 `reg export`의 성공 여부를 검사하지 않고 바로 `del`과 `reg delete`를 실행한다. 백업 폴더 생성 실패, 저장 공간 부족, 쓰기 권한 문제로 백업이 실패하면 복구본 없이 진행도를 지울 수 있다. 성공 안내도 무조건 출력한다.

**개선:** 백업 디렉터리 생성과 각 백업 명령의 종료 코드를 확인하고, 실패하면 삭제 전에 중단한다. 백업 파일의 존재·내용도 확인한다. 레지스트리 키가 없는 경우와 존재하지만 내보내기에 실패한 경우를 구분한다. 위험한 실제 초기화는 실행하지 않았으며 정적 코드로 확인한 결함이다.

### 2. [P1] 플레이한 채보가 아닌 저장 시점의 파일 해시로 기록을 저장한다

근거: `muse dash test/Core/CustomRecordStore.cs:164`, `Core/ChartFingerprint.cs`의 `ForUid`, `Core/CustomPlaySession.cs`.

결과 저장 시 `ChartFingerprint.ForUid(uid)`가 현재 디스크의 BMS를 읽는다. 그러나 실제 플레이 데이터는 앞서 파싱한 캐시다. 플레이 도중 BMS를 수정하거나 파일 감시가 최종 저장을 놓치면 이전 채보의 점수에 새 파일의 해시가 붙는다. 이후 기록을 읽을 때도 새 파일과 일치하므로 잘못된 기록이 정상으로 인정된다.

**재현:** 캐시에는 2노트, 디스크에는 3노트가 있는 상태에서 2노트 결과 저장 → 새 디스크 해시로 저장 → `LoadResult`가 해당 기록을 허용했다.

**개선:** 파싱에 사용한 정확한 데이터의 해시를 채보 객체에 보관하고, 배틀 시작 시 채보와 해시를 세션에 함께 고정한다. 결과 저장과 최고 기록 병합은 그 세션 해시를 사용한다. 나중에 파일을 다시 읽어 플레이 버전을 추정하지 않는다.

### 3. [P1] AP 옵션을 끄면 커스텀 기록 저장까지 건너뛴다

근거: `muse dash test/Patches/Battle/UI/APModPatch.VictoryBanner.cs:41–52`.

승리 훅이 `if (!ModConfig.EnableAPMod) return;`으로 먼저 종료한다. 그 아래의 `TrySaveCustomRecord()`가 커스텀 결과 저장을 호출하는 경로이므로 AP 기능만 끈 사용자도 로컬 기록을 남기지 못한다. 같은 게이트 아래에 배틀 미디어 정리도 묶여 있다.

**개선:** 커스텀 결과 저장과 미디어 정리를 배틀 종료 생명주기로 분리하고, AP 옵션은 AP 배너·판정 변경에만 적용한다. AP on/off × 커스텀 곡의 결과 저장을 검증한다. 정적 경로로 확인했으며 게임에서 옵션을 바꾸어 실행하지는 않았다.

### 4. [P2] 빠른 연속 저장의 마지막 파일 변경을 잃는다

근거: `muse dash test/Patches/Hwa/HwaResourceManager.Watcher.cs:86–94`.

300ms 안에 들어온 후속 이벤트를 버리는 방식이며 마지막 상태를 다시 읽는 예약이 없다. 에디터의 연속 저장 또는 파일을 여러 번에 나눠 쓰는 경우 처음 읽은 중간 상태가 캐시에 남는다. 파서는 파일을 `FileShare.ReadWrite`로 읽으므로 쓰기 중인 상태도 읽을 수 있다.

**재현:** 첫 변경에서 2노트 로드 → 300ms 이내 최종 3노트 저장 이벤트 → 디스크는 3노트, 캐시는 2노트로 유지됐다.

**개선:** 마지막 이벤트 후 일정 시간이 지나면 다시 읽는 디바운스를 사용한다. 파일 읽기/파싱 실패 시 제한된 재시도를 하고, 검증된 완성 데이터만 캐시에 반영한다.

### 5. [P2] BMS를 삭제해도 이전 채보 캐시가 남는다

근거: `muse dash test/Patches/Hwa/HwaResourceManager.Watcher.cs`의 `ReloadBmsChartForUid`.

새 채보가 `null`이면 기존 캐시를 제거하지 않는다. 삭제 이벤트를 구독하지만 마지막 BMS를 삭제한 뒤에도 삭제 전 채보 조회가 성공한다. 이름 변경 처리도 새 경로뿐 아니라 이전 경로에 대한 무효화가 필요하다.

**재현:** 로드된 BMS 삭제 → 감시 핸들러 실행 → `TryGetCachedHwaBmsChart`가 삭제 전 2노트 채보를 반환했다.

**개선:** 실제 파일 제거와 일시적인 읽기 실패를 구분한다. 제거 시 캐시 및 곡의 사용 가능 상태를 갱신하고, 재생 중인 세션 스냅샷은 별도로 유지한다.

### 6. [P2] 0틱 BPM 변경이 기본 BPM에 덮인다

근거: `muse dash test/Bms/BmsParser.cs`의 BPM 이벤트 정렬 및 시간 계산.

같은 틱의 이벤트를 `Source` 문자열로 정렬하면서 `BPM01`이 `default`보다 먼저 적용되고, 기본 BPM이 그 값을 다시 덮는다.

**재현 입력:**

```text
#BPM 120
#BPM01:240
#00008:01
#00113:01
```

다음 마디 노트가 240 BPM 기준 1초가 되어야 하지만 2초로 계산됐다.

**개선:** 기본 BPM은 초기 상태로 설정하거나 동일 틱에서 가장 먼저 적용한다. 같은 틱에 여러 변경이 있는 경우의 순서도 명시한다. 이 입력을 정식 회귀 테스트에 추가한다.

### 7. [P2] 0 BPM 입력으로 무한대 노트 시간이 만들어진다

근거: `muse dash test/Bms/BmsParser.Lexer.cs`, `BmsParser.cs`의 시간 계산.

`#BPM 0`을 허용한 뒤 BPM으로 나눈다. `#BPM 0`과 `#00113:01` 입력에서 노트 시간이 유한하지 않음을 재현했다. 이후 게임 데이터 변환에 잘못된 시간이 전달될 수 있다. 실제 게임 크래시는 확인하지 않았다.

**개선:** 기본·확장 BPM 모두 유한한 양수인지 검사하고 잘못된 입력 위치를 보고한다. 원본 게임 노트 목록을 비우기 전에 변환 결과 전체를 검증한다.

### 8. [P2] 주입한 BGM과 일시정지·싱크 보정 대상이 다를 수 있다

근거: `muse dash test/Patches/Battle/UI/HwaBattleMediaController.cs`, `HwaBattleMediaController.Lifecycle.cs:36,77`, `Patches/Hwa/HwaSyncManager.cs:46–61`.

주입은 기존 AudioSource를 찾아 재사용할 수 있지만 Pause/Resume과 싱크 보정은 이름이 `HwaBattleBgmSource`인 오브젝트만 찾는다. StopMedia는 이미 `injectedAudioSource`를 사용하므로 같은 미디어 기능 안에서 대상 결정이 다르다. 싱크 보정은 최초 조회가 실패해도 캐시 초기화를 완료해 늦게 생성된 대상도 놓칠 수 있다.

**개선:** 실제 주입한 AudioSource 참조를 일시정지·재개·정지·싱크 보정에서 공유하고 파괴/교체 시 갱신한다. 기존 소스 재사용과 새 소스 생성 두 경로를 게임에서 확인한다. 게임 자체의 오디오 처리가 일부 증상을 상쇄할 수 있으므로 실제 청각적 영향은 실행 검증이 필요하다.

### 9. [P2] 프리팹 검색 실패 캐시가 후속 재시도를 무력화한다

근거: `muse dash test/Patches/Battle/UI/ExperimentHitPointInstaller.cs`의 `FindInactivePrefab`.

찾지 못한 프리팹도 `null`로 캐시한다. 상위 코드가 반복 재시도해도 캐시에 키가 있으면 다시 검색하지 않으므로 초기 검색 뒤 로드된 프리팹을 발견하지 못할 수 있다.

**개선:** 성공한 결과만 캐시하거나 실패 결과에 만료 시간을 둔다. 씬 전환과 프리팹 파괴 때도 캐시를 무효화한다. 정적 경로로 확인했으며 지연 로드 상황의 게임 실행 검증이 필요하다.

### 10. [P2] 배포 실패에도 성공 메시지가 출력된다 — 기존 알려진 문제

근거: `muse dash test/muse dash test.csproj:50–52`, `docs/guides/CHECKLIST.md`.

`Copy`는 `ContinueOnError="WarnAndContinue"`이고 성공 메시지는 무조건 출력된다. 체크리스트에 이미 기록된 문제로, 이번에 새로 발견한 사고는 아니다.

**개선:** 배포를 요청한 빌드에서는 복사 실패를 실패로 전파한다. 일반 빌드와 게임 배포를 명시적으로 구분하고 배포 성공은 대상 파일 확인 후에만 표시한다.

## 추가 개선 후보

- **기록 파일의 원자적 저장:** `CustomRecordStore.cs:172`의 직접 `WriteAllText`를 임시 파일 작성 후 교체 방식으로 바꾸면 중단/쓰기 실패로 기존 기록이 잘리는 위험을 줄인다. 백업과 표준 JSON 직렬화도 검토한다.
- **결과 화면 플레이 횟수:** `PnlReportCardPatch.cs:240`의 `txtTotalPassCountValue.text = "1"`을 저장된 `playCount`와 일치시킨다. 기록 패널과 결과 화면의 표시가 달라질 수 있다.
- **Discord 옵션 일관성:** 세션·메뉴의 직접 Activity 갱신과 진단 패치까지 동일한 `EnableDiscordRPC` 게이트를 통과하게 정리한다. 외부 호출 실패 시 성공 캐시를 먼저 갱신하지 않는지도 함께 점검한다.
- **문서 정확성:** README의 가상 태그 `1998` 설명을 실제 `CustomContentIds`의 `1999`와 맞춘다. `CODE_REFERENCE.md`의 존재하지 않는 `NoteColorDiagnosticsPatch.cs` 참조, 문서 목차에 빠진 실험 문서, 모바일 가이드의 특정 PC `file:///H:` 링크도 정리한다.
- **독립 도구 검증:** SignatureDumper가 솔루션 밖에 있으므로 CI에서 별도 빌드하거나 솔루션에 포함한다. 덤프 실패의 종료 코드와 기존 출력 디렉터리 교체 방식도 개선 후보다.

## 검증 명령과 결과

```powershell
dotnet run --project 'muse dash test.LogicTests/muse dash test.LogicTests.csproj' -c Release
dotnet build 'muse dash test.slnx' -c Release -p:ModsDir='H:\source\repos\muse dash test\artifacts\review-mods'
dotnet build 'SignatureDumper/SignatureDumper.csproj' -c Release
```

임시 재현 프로그램은 무시 경로 `scratch/review/`에 작성했으며 이번 커밋에는 포함하지 않았다. BMS 실제 소스와 감시/기록 실제 소스를 연결하고 게임 의존성만 스텁 처리했다. 아래 출력은 모두 관찰된 결과다.

```text
BPM zero creates non-finite time: REPRODUCED
BPM at tick zero overwritten by default (actual=2, expected=1): REPRODUCED
Rapid final save discarded (cache=2 disk=3): REPRODUCED
Old cached chart result accepted under new disk fingerprint: REPRODUCED
Deleted BMS remains playable in cache: REPRODUCED
```

검토용 빌드는 저장소의 `artifacts/review-mods`로 배포 대상을 바꿨다. 빌드 출력과 해당 복사본의 SHA-256은 모두 `6240F2151DC57733C561DD704367A99121A982B1F2948B95206EDB9C4334B3C7`이었다. **게임 Mods에는 이번 빌드를 배포하지 않았다.** 실제 `H:\muse dash hwa\Mods`에는 기존 `muse-dash-custom-chart.dll` 하나가 있었으며 이는 이번 빌드 검증과 별개다.

`docs/guides/CHECKLIST.md`를 확인했다. 이번 검토는 릴리즈 검증이 아니며, 게임 로그 대조·정상 배틀·BMS/앨범 없음 → `0-0` 폴백 → 실제 배틀 진입은 수행하지 않았다. 수정 후에는 이 실행 검증이 필요하다.

## 수정 순서 제안

1. 세이브 백업 실패 시 중단 처리.
2. AP 옵션과 결과 저장 분리, 플레이 세션의 채보 해시 고정.
3. 파일 감시의 마지막 저장 보장·삭제 무효화, BPM 입력 검증과 동일 틱 순서 회귀 테스트.
4. BGM 대상 참조 통합과 프리팹 재시도 개선 후 게임 실행 검증.
5. 배포 실패 전파, 기록 파일 저장 안정성, 문서 정리.
