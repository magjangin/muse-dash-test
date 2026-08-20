# 변경·릴리즈 체크리스트

코드를 고치거나 릴리즈를 올리기 전에 훑는 목록입니다.
각 항목은 **실제로 이 저장소에서 사고가 났던 지점**만 담았습니다. 일반론은 넣지 않습니다.
괄호 안은 그 항목이 생긴 이유이며, 근거가 사라지면 항목도 지우세요.

---

## 1. 코드를 고칠 때

- [ ] **게임 원본 필드를 덮어쓰기 전에 그 필드의 용도를 확인했는가?**
      표시용 값으로 에셋 키를 덮지 말 것. `MusicInfo.music`은 제목이 아니라 음원 에셋 키(`iyaiya_music`)이고,
      제목 렌더링은 `musicName` / `music_name` 마스크가 담당합니다.
      (v0.9.2~v0.9.5: `music`을 표시 제목으로 덮어써 `PnlBattle.MusicProgressInit`이 NRE. 커밋 `6025537`)

- [ ] **원인을 계측으로 확정했는가?** 증상에서 원인을 단정하지 말 것.
      고친 뒤에는 수정 전/후 로그를 **같은 시퀀스 위치**에서 대조해 확인합니다.

- [ ] **IL2CPP 패치 함정을 건드리지 않았는가?**
      `private` + `out` 조합이나 `virtual` 메서드 패치는 로그 한 줄 없이 네이티브 크래시가 납니다.
      한 줄짜리 세터는 인라인돼서 훅이 아예 안 걸립니다. 패치 헬스체크가 초록불이어도 실행을 보장하지 않습니다.

- [ ] **원본을 막았으면 대체품을 넣었는가?**
      게임 동작을 가로채 차단할 때는 "채울 것이 있을 때만 막기". 막아 놓고 비워두면 그 자리가 죽습니다.

---

## 2. 빌드·배포

- [ ] **Mods 폴더를 눈으로 확인했는가?**
      `[AutoDeploy] Successfully deployed`는 **복사가 실패해도 찍힙니다.**
      `DeployToMods` 타깃의 `Copy`에 `ContinueOnError="WarnAndContinue"`가 걸려 있고 `Message`는 무조건 출력됩니다.

      ```bash
      ls -la "H:/muse dash hwa/Mods/"
      ```

- [ ] **Mods 폴더에 DLL이 하나뿐인가?**
      MelonLoader는 폴더 안의 `.dll`을 전부 로드합니다. 이름만 다른 같은 모드가 두 개 있으면
      Harmony 패치와 가상 곡 등록이 중복됩니다.

- [ ] **릴리즈 첨부는 ZIP으로 올렸는가?**
      **GitHub는 릴리즈 첨부 파일 이름의 공백을 점으로 자동 변환합니다.** `AssemblyName`이
      `muse dash custom chart`(공백 포함)라서, DLL을 그대로 올리면 `muse.dash.custom.chart.dll`로
      바뀌어 올라갑니다. `gh release upload`의 `#표시이름` 문법으로도 못 막습니다(v0.10.1에서 실측).

      그러면 내려받은 DLL과 직접 빌드한 DLL이 이름이 달라 **`Mods` 폴더에 나란히 남습니다.**
      (2026-08-20: v0.9.6 에셋 `muse.dash.custom.chart.dll`이 빌드본 옆에 461,824바이트로 남아 있었습니다.)
      ZIP으로 감싸면 압축 해제 시 실제 이름이 복원되어 기존 파일을 덮어씁니다.

- [ ] **릴리즈에 올릴 산출물은 `-c Release`로 빌드했는가?**
      Debug는 `DEBUG` 상수가 정의되고 최적화가 꺼집니다. 크기로 구분됩니다 — v0.10.1 기준 Release 469KB / Debug 503KB.
      확실히 하려면 `DebuggableAttribute`를 봅니다. Release는 `01-00-02-00-...`(값 2), Debug는 최적화 해제 비트가 켜집니다.

      ```bash
      dotnet build "muse dash test/muse dash test.csproj" -c Release
      ```

---

## 3. 실행 검증

- [ ] **로그의 `[ERROR]`를 확인했는가?** 이 한 줄이 이번 버그를 찾아낸 전부입니다.

      ```bash
      grep -c "\[ERROR\]" "H:/muse dash hwa/MelonLoader/Latest.log"
      ```

- [ ] **폴백 경로도 실제로 밟아봤는가?** ← 가장 자주 빠뜨리는 항목
      정상 경로만 확인하면 폴백은 아무도 안 밟습니다. 최소한 아래 두 상태를 각각 한 판씩 플레이합니다.

      | 상태 | 만드는 법 | 로그에서 확인 |
      | --- | --- | --- |
      | 정상 (앨범 있음) | hwa 폴더에 BMS 있는 곡 | `주입 시도 === sourceUid=0-4` 처럼 실제 원본 곡 |
      | 폴백 (앨범 없음) | hwa 폴더를 비우거나 BMS 없는 슬롯 선택 | `원본 곡을 찾지 못하여 기본 곡(0-0)으로 폴백` |

      **곡 선택 화면까지만 보고 끝내지 말 것.** 배틀에 실제로 진입해야 `StageBattleComponent.InitData` →
      `PnlBattle.GameStart` → `MusicProgressInit` 경로가 돕니다. NRE는 `GameStart` 약 5초 뒤에 터졌습니다.

- [ ] **관찰용 Postfix 로그가 찍혔는가?**
      원본이 예외를 던지면 Harmony Postfix는 건너뛰어집니다. 그래서 **"있어야 할 로그가 없는 것"이 예외의 신호**입니다.
      예: `[ProgressBarPatch] sldProgress 슬라이더 감지됨`이 안 보이면 `MusicProgressInit`이 던진 것입니다.

---

## 4. 릴리즈 전

- [ ] **`git fetch origin` 후 ahead/behind를 확인했는가?** (여러 클론에서 작업하므로 실제로 갈라진 적이 있습니다)

- [ ] **태그가 그 커밋을 정말 포함하는가?** 날짜순 ≠ 포함 관계입니다.
      `2ac888a`(08-06)는 날짜상 앞서지만 `v0.9.1`(08-07)에 들어가지 않았고, `v0.9.2`부터 나갔습니다.

      ```bash
      git tag --contains <커밋> --sort=creatordate
      ```

- [ ] **릴리즈 노트의 영향 범위를 `--contains` 결과로 적었는가?** 기억이나 날짜로 적지 말 것.

- [ ] **에셋 재업로드 시 기존 파일을 확인했는가?** `--clobber`는 기존 에셋을 대체하고 다운로드 수를 0으로 되돌립니다.

      ```bash
      gh api repos/magjangin/muse-dash-test/releases/tags/<태그> --jq '.assets[] | {name, size, updated_at, download_count}'
      ```

---

## 관련 문서

- [LOGGING_AND_TROUBLESHOOTING.md](LOGGING_AND_TROUBLESHOOTING.md) — 로그 읽는 법과 로그별 의미
- [CUSTOM_CHART_GUIDE.md](CUSTOM_CHART_GUIDE.md) — hwa 폴더 구성 (폴백 경로 테스트용 상태 만들 때)
- [CAST_AND_CUSTOM_TAG_GUIDE.md](../architecture/CAST_AND_CUSTOM_TAG_GUIDE.md) — 래퍼 패턴과 가상 곡 주입 구조
