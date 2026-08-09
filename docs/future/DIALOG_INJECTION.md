# 커스텀 대사(Dialog) 주입 — 보류된 설계

> **상태: 보류(Shelved).** 동작하는 프로토타입까지 만들어 검증을 마쳤지만, 지금 단계에서 넣을 기능이 아니라고 판단해 본체에서 걷어냈습니다. 이 문서는 나중에 재도입할 때 **처음부터 다시 파헤치지 않기 위한** 기록입니다.

## 코드 복구 방법

프로토타입 전체가 커밋 `0220555`에 보존되어 있습니다. 되살릴 때는:

```bash
git show 0220555 --stat
```

```bash
git checkout 0220555 -- "muse dash test/Patches/Hwa/HwaDialogFile.cs" "muse dash test/Patches/Database/Stage/DialogInjectPatch.cs" "muse dash test/Patches/Database/Stage/DBStageInfoDialogClearPatch.cs"
```

복구 전에 아래 **[재도입 전 반드시 고칠 것](#재도입-전-반드시-고칠-것)** 항목을 먼저 읽으세요. 알려진 파싱 버그가 하나 들어 있습니다.

---

## 1. 문제 정의

커스텀 차트 주입은 **노트만** 교체합니다. 그래서 원곡에 딸린 대사 데이터가 그대로 남아, 새 채보와 아무 상관 없는 대사가 플레이 중에 화면에 뜹니다.

필요한 건 두 가지입니다.

1. 커스텀 차트로 플레이할 때 **원곡 대사를 지운다**
2. 곡 폴더에 `dialog.txt`가 있으면 **그 내용을 대사로 채워 넣는다**

---

## 2. 게임 측 데이터 모델 (검증됨)

> 이 절의 `../../Decompiled/...` 링크는 **로컬 전용**입니다. `Decompiled/`는 `.gitignore` 대상이라 저장소에 포함되지 않습니다 — Il2CppInterop이 생성한 프록시 어셈블리를 디컴파일해 둔 참조 자료이며, 링크가 안 열리면 직접 덤프해서 보세요. 타입·멤버 이름만으로도 내용은 따라갈 수 있게 적었습니다.

대사는 `DBStageInfo`의 딕셔너리 두 개에 들어 있습니다.

| 멤버 | 타입 | 역할 |
|---|---|---|
| `sceneDialogEvents` | `Dictionary<string, List<GameDialogArgs>>` | **언어별** 대사 원본. 키는 `"English"`, `"Korean"` 등 |
| `sceneDialogDictionary` | `Dictionary<Decimal, List<GameDialogArgs>>` | **시간별** 대사. 틱 트리거가 실제로 조회하는 쪽 |

둘 다 auto-property입니다 — [DBStageInfo.cs:436](../../Decompiled/Assembly-CSharp/Il2CppAssets.Scripts.Database/DBStageInfo.cs:436)에 `<sceneDialogEvents>k__BackingField` / `<sceneDialogDictionary>k__BackingField`가 존재합니다. **즉 백킹 필드에 직접 쓰는 것은 프로퍼티 setter를 호출하는 것과 완전히 동일하며, 생략되는 로직이 없습니다.** (setter는 `protected`라서 백킹 필드 경로를 쓰는 편이 편합니다.)

### `GameDialogArgs` 필드

[GameDialogArgs.cs](../../Decompiled/Assembly-CSharp/Il2CppAssets.Scripts.Structs/GameDialogArgs.cs) 기준. IL2CPP struct이므로 인터롭에서 `sealed class GameDialogArgs : ValueType`로 생성되고, **`List`에 넣는 순간 값이 복사**됩니다. 따라서 필드를 모두 채운 뒤에 `Add`해야 합니다.

| 필드 | 비고 |
|---|---|
| `index` | 프로토타입은 전부 `0`으로 넣었고 문제없이 동작했습니다. 게임이 정렬/식별에 쓰는지는 미확인 |
| `time` | `Decimal`. 시간별 딕셔너리의 키와 같은 값이어야 합니다 |
| `dialogType` | 말풍선 스프라이트 선택 |
| `dialogIndex` | 말풍선 위치(위/아래) |
| `dialogState` | `None` / `Show` / `Hide` — [DialogState.cs](../../Decompiled/Assembly-CSharp/Il2CppAssets.Scripts.Structs/DialogState.cs) |
| `text`, `speed`, `fontSize`, `textColor`, `bgColor`, `dialogSize`, `alignment` | 표시 속성 |

### 타입/방향 상수는 하드코딩하지 말 것

`GameDialogArgs`가 **static 상수를 직접 노출합니다** — [GameDialogArgs.cs:76-132](../../Decompiled/Assembly-CSharp/Il2CppAssets.Scripts.Structs/GameDialogArgs.cs:76):

- `DIALOG_TYPE_ROLE`, `DIALOG_TYPE_BOSS`, `DIALOG_TYPE_UI`
- `DIALOG_UP`, `DIALOG_DOWN`

프로토타입은 실측한 `0`/`1`/`2`를 `DialogTypeCodes` / `DialogDirCodes`로 하드코딩했습니다. **재도입 시에는 게임 상수를 런타임에 읽으세요.** 게임 버전이 올라가도 살아남습니다.

### 실측한 스타일 값 (공식 곡 `how_to_make_otoge_kyoku`)

| 항목 | 값 |
|---|---|
| 글자색 | 흰색 |
| 정렬 | `Center` |
| 표시 속도 | `1.0` |
| 상자 기본 크기 | `300x150` |
| UI **아래쪽** 말풍선만 | 가로 `400` |
| 게임의 클램프 범위 | `150x100` ~ `1200x200` (`DialogSubControl`) |
| `bgColor` | `(0, 0, 0, 0.498)` — **화면에 반영되지 않음.** 말풍선은 프리팹 스프라이트(`TalkBaseImg`의 roleLT/roleLB, bossRT/bossRB, asideUp/asideDown)로 그려집니다 |

말풍선을 여는 줄은 원본과 동일하게 **같은 시각에 `Show` + `None` 두 항목**으로 펼쳐야 합니다.

---

## 3. 훅 지점과 순서 (여기가 핵심)

| 대상 | 시점 | 하는 일 |
|---|---|---|
| `DBStageInfo.SetDialogArgs` | **Postfix** | 원곡 대사 제거 |
| `DBStageInfo.DialogDataToDic` | **Postfix** | 위와 동일 (다른 경로로 재구성되는 경우 커버) |
| `GameMusicScene.InitTimer(Decimal)` | **Prefix** | `dialog.txt` 대사 주입 |

### 반드시 `InitTimer` **직전**에 주입해야 합니다

`InitTimer`가 틱 타임라인을 만들 때, **그 시점에 존재하는 대사 시간에 대해서만** 트리거를 등록합니다. 그 뒤에 딕셔너리를 채워도 `DialogEventTrigger`는 영영 호출되지 않습니다. (실측 확인됨)

이 사실의 파생 결론이 하나 있는데, 아래 [열린 문제](#5-열린-문제-재도입-시-확인) (B)에 적어뒀습니다.

### 왜 `Clear()`가 아니라 새 딕셔너리로 교체하는가

`SetDialogArgs`에 넘어온 원본 딕셔너리는 다른 곳에서 캐시/공유될 수 있습니다. 내용을 `Clear()`하면 **순정 곡 플레이까지** 영향이 남을 위험이 있습니다. 빈 딕셔너리로 **교체**하면 원본 객체는 그대로 살아 있어, 다음 곡 로드 시 원본 데이터가 정상적으로 다시 채워집니다.

### 패치 시그니처 주의

`InitTimer`는 반드시 인자 타입을 명시해야 합니다:

```csharp
[HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "InitTimer", new Type[] { typeof(Il2CppSystem.Decimal) })]
```

같은 이름의 메서드가 `GameMusic.InitTimer(Decimal)`, `MainManager.InitTimer()`에도 있습니다. `SetDialogArgs` / `DialogDataToDic`은 오버로드가 각 1개뿐이라 문자열 이름만으로 모호성이 없습니다.

---

## 4. `dialog.txt` 형식

슬래시 구분, 뒤쪽 칸은 생략 가능:

```text
// 시간 / 타입 / 방향 / 상태 / 폰트 / 상자크기 / 텍스트
5초    / ui   / 아래 / 활성화   / 30 / 400x150 / 첫 줄입니다
7.25초 /      /      /          /    /         / 두 번째 줄
9초    /      /      / 비활성화 /
```

- **타입/방향/폰트/상자크기를 비우면 직전 줄의 값을 물려받습니다.** 그래서 `5초 / 안녕하세요`처럼 두 칸만 적어도 됩니다.
- **상태를 비우면**, 그 말풍선이 닫혀 있으면 여는 것으로, 열려 있으면 텍스트 교체로 처리합니다.
- **마지막 칸은 항상 텍스트입니다.** 그래서 상태만 지정하고 텍스트를 안 쓸 때는 **뒤에 슬래시를 하나 더 찍어** 빈 텍스트 칸을 만들어야 합니다 (위 예시의 `9초` 줄). 이걸 빼먹으면 상태 낱말이 텍스트로 출력됩니다 — [아래 버그](#재도입-전-반드시-고칠-것) 참고.
- 텍스트에 슬래시를 쓰려면 앞의 6칸을 모두 채우세요(비워도 되니 슬래시만 찍으면 됩니다). 7번째 칸부터는 슬래시를 포함해 통째로 텍스트로 봅니다.
- 텍스트 안의 `\n`은 줄바꿈이 됩니다.
- `//` 또는 `#`로 시작하는 줄과 빈 줄은 무시합니다.
- **파일은 UTF-8로 저장하세요.** 파서가 `File.ReadAllLines`를 쓰므로 CP949(ANSI)로 저장된 한글은 예외 없이 조용히 깨집니다.

### 쓸 수 있는 낱말

| 칸 | 값 |
|---|---|
| 타입 | `ui`·`방백` / `boss`·`보스` / `role`·`캐릭터`·`주인공` |
| 방향 | `위`·`up`·`위쪽` / `아래`·`down`·`아래쪽` |
| 상태 | `활성화`·`show`·`열기`·`표시` / `비활성화`·`hide`·`숨김`·`닫기` / `유지`·`갱신`·`update`·`none`(또는 빈칸) |
| 상자크기 | `400x150` 또는 가로만 `400` |

### 시간 정밀도

게임의 시간별 딕셔너리 키가 소수 2자리라, 파서도 소수 2자리로 반올림해 맞춰야 틱이 정확히 일치합니다.

단, 프로토타입은 `Math.Round(double, 2, AwayFromZero)`를 썼는데 double 표현 오차 때문에 `0.145 → 0.14`가 됩니다. 2자리 의미를 정확히 지키려면 **`decimal`로 직접 파싱**하세요.

---

## 재도입 전 반드시 고칠 것

### 상태 낱말이 텍스트로 출력되는 파싱 버그

프로토타입의 파서는 **마지막 칸을 항상 텍스트로 취급**합니다. 그래서 `9초 / / / 비활성화`처럼 4칸만 적으면 상태 칸(index 3)이 범위 밖으로 밀려 빈칸이 되고, 결과적으로:

- 상태 → 빈칸 상속 → 슬롯이 열려 있으니 `Update`
- 텍스트 → `"비활성화"`

즉 **말풍선이 닫히는 대신 "비활성화"라고 적힌 말풍선이 뜹니다.** `5초 / hide`도 같은 부류입니다.

실제 동작을 표로 옮기면:

| 입력 | 칸 수 | 상태 칸 | 텍스트 | 결과 |
|---|---|---|---|---|
| `9초 / / / 비활성화` | 4 | (빈칸 상속) | `비활성화` | ❌ 텍스트로 출력 |
| `9초 / / / 비활성화 /` | 5 | `비활성화` | (빈칸) | ✅ 정상 종료 |

원인은 `HwaDialogFile.Column()`의 `lastMiddle = columns.Length - 2`입니다. 위 §4 형식 문서는 이미 올바른 쪽(`/` 하나 더)으로 적어뒀지만, 재도입 시에는 **텍스트가 상태 낱말과 정확히 일치하는데 상태 칸이 비어 있으면 경고를 띄우는 가드**를 넣어 같은 함정을 다시 밟지 않게 하세요.

---

## 5. 열린 문제 (재도입 시 확인)

### (A) `delay`와 대사 시간 정렬

원본에 `GameDialogArgs.FixDialogDelay(argsDic, Decimal delay)`가 존재합니다 — [GameDialogArgs.cs:367](../../Decompiled/Assembly-CSharp/Il2CppAssets.Scripts.Structs/GameDialogArgs.cs:367). 즉 **스키마 자체가 대사 시간의 delay 보정을 전제**합니다.

현재 빌드에서는 `CallerCount(0)`이라 호출되지 않는 것으로 보이고, 그래서 `dialog.txt`의 raw 초를 그대로 쓴 프로토타입이 정상 동작했습니다.

**다만 이 모드는 가상 곡의 `DBStageInfo.delay` getter를 직접 오버라이드합니다** — [OffsetHookPatches.cs](../../muse%20dash%20test/Patches/Diagnostics/OffsetHookPatches.cs). 노트 타임라인이 `delay`를 먹는다면 노트만 밀리고 대사는 안 밀려 **정확히 delay만큼 어긋납니다.**

> 확인 방법: 매니페스트 `delay`를 2초쯤 넣고 노트와 대사가 같이 밀리는지 A/B.

### (B) 주입 후 `SetDialogArgs`가 다시 불리면 대사가 영구 소실됩니다

원본 시그니처가 `SetDialogArgs(dialogArgs, bool isRefresh)`라 **리프레시 경로가 존재**합니다.

순서가 `제거(Postfix)` → `주입(InitTimer Prefix)`인데, `InitTimer` 이후에 `SetDialogArgs`가 다시 불리면 제거 패치가 주입분을 지웁니다. 그리고 §3에서 확인한 사실(`InitTimer` 이후에 채워도 트리거 미등록) 때문에 **재주입해도 못 살립니다.**

> 대응: 주입 여부 플래그를 두고, 제거 패치가 그 뒤에 실행되면 경고 로그를 남기세요. 조용히 사라지는 대신 로그로 잡힙니다.

### (C) 언어 키가 같은 List 인스턴스를 공유

프로토타입은 5개 언어(`English`, `ChineseS`, `ChineseT`, `Japanese`, `Korean`) 키에 **동일한 `List` 객체**를 넣었습니다. 어느 언어로 플레이해도 같은 대사를 보여주려는 의도였지만, 게임이 `GameDialogArgs.Format`을 다시 돌리면 같은 리스트를 5번 건드립니다. 언어별로 리스트를 따로 만드는 게 공짜입니다.

---

## 6. 고수준/저수준에 대한 정리

이 기능에는 **별도의 "저수준 해답"이 존재하지 않습니다.** 고수준으로 쓴 Harmony 패치가 **이미 네이티브 디투어**이기 때문입니다.

근거 (설치된 바이너리 기준, MelonLoader 0.7.3):

| 확인 대상 | 발견된 내용 |
|---|---|
| `Il2CppInterop.HarmonySupport.dll` | `Il2CppDetourMethodPatcher`, `nativeDetour` |
| `MelonLoader.dll` | `il2CppDetourMethodPatcher`, `NativeDetour`, `NativeHookAttach`, `NativeHookDetach`, `NativeUtils` |

그리고 디투어가 꽂히는 **C++ 원본 메서드 포인터가 인터롭 어셈블리에 그대로 노출**되어 있습니다 — [DBStageInfo.cs:71](../../Decompiled/Assembly-CSharp/Il2CppAssets.Scripts.Database/DBStageInfo.cs:71):

```text
NativeMethodInfoPtr_SetDialogArgs_Public_Void_Dictionary_2_String_List_1_GameDialogArgs_Boolean_0
```

`[HarmonyPatch(typeof(DBStageInfo), "SetDialogArgs")]` → HarmonySupport가 저 `IntPtr`을 꺼내 → 네이티브 디투어 설치. `MelonUtils.NativeHookAttach`로 손수 짜도 **같은 주소**를 때립니다. 차이는 이득이 아니라 손해입니다: 숨은 `MethodInfo*` 말미 인자를 직접 마샬링해야 하고, GC 핸들을 수동 관리해야 하며, Harmony의 패치별 예외 격리를 잃습니다.

### 수동 저수준 훅이 실제로 필요한 조건

1. 인터롭 프록시가 없는 대상 — AOT 인라인되어 독립 네이티브 함수가 없는 경우(`CallerCount(0)`), 난독화, 미생성 제네릭 인스턴스
2. `il2cpp_*` 런타임 익스포트처럼 **매니지드가 아닌** 함수
3. Harmony가 마샬링하지 못하는 시그니처(byref struct, 비표준 호출 규약)

대사 훅 지점은 셋 다 해당되지 않습니다. `GameMusicScene.InitTimer`는 `CallerCount(2)`이고, `SetDialogArgs` / `DialogDataToDic`도 프록시와 메서드 포인터가 정상입니다.

### 저수준으로 착각하기 쉬운 것들

- **`_sceneDialogEvents_k__BackingField` 직접 쓰기** — 필드 쓰기이지 **인터셉트가 아닙니다.** 프로퍼티를 우회하는 것이고 런타임을 우회하는 게 아닙니다.
- **`Il2CppSystem.*` 타입 사용** — 그냥 인터롭 타입입니다.

---

## 관련 문서

- [CUSTOM_CHART_GUIDE.md](../custom_charts/CUSTOM_CHART_GUIDE.md) — 커스텀 차트 폴더 구조와 리소스 규약
- [MOD_SYSTEM_BLUEPRINT.md](../architecture/MOD_SYSTEM_BLUEPRINT.md) — 전체 훅 지점 목록
- [BMS_PARSING.md](../custom_charts/BMS_PARSING.md) — 노트 시간 기준(대사 시간과 같은 기준)
