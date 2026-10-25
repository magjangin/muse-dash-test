# 판정바 오차 계산 점검 (REA)

판정바([JudgmentBar.cs](../../muse%20dash%20test/Patches/UI/Custom/JudgmentBar.cs))가 보여 주는 오차가 게임이 실제로 판정에 쓴 오차와 같은지, 게임 본문을 REA(Ghidra)로 디컴파일해 대조한 기록입니다.

- 점검일: 2026-10-25, 게임 6.7.0(IL2CPP), 모드 v0.11.0
- 방법: `GameAssembly.dll`에서 대상 함수만 `rea-hwa/tools/pe_slice.py`로 잘라 REA `open_binary`(provider `ghidra`) → `procedure_pseudo_code`. 필드 위치는 `cpp2il_out`을 `ilspycmd -t`로 확인했습니다.
- **정적 분석만 했습니다.** 아래 "확인 필요" 항목은 실기 로그로 확정해야 합니다. 모드 코드는 바꾸지 않았습니다.

---

## 1. 게임이 판정하는 방식 (확인됨)

| 단계 | 게임 함수 | 하는 일 |
| --- | --- | --- |
| 기준 시계 | `GameGlobal.stopwatch` (`System.Diagnostics.Stopwatch`) | 곡 시작부터 흐른 실제 시간 |
| 곡 시각 | `StageBattleComponent.timeFromMusicStart` (RVA `0x1D77440`) | `stopwatch.ElapsedMilliseconds / 1000` (초, float) |
| 입력 시각 | `TouchScript.KeyHandler` (RVA `0x531CE0`) | 키를 누른 그 자리에서 `TouchScript.tickTime = timeFromMusicStart` |
| 판정 칸 조회 | `GameTouchPlay.TouchActionResult` (RVA `0x1E684A0`) → `StageBattleComponent.GetTimeNodeByTick(float)` (RVA `0x1D71C30`) | `GameGlobal.gTouch.tickTime`을 `(int)(tick * 1000)`ms 키로 바꿔 `m_TimeNodeOrders`에서 그 ms의 판정 칸 목록을 찾음 |
| 판정 칸 | `TimeNodeOrderFactory.CreateNoteTicks` → `TimeNodeOrder.CopyTimeNodeOrder` | 노트 시각 앞뒤로 **1ms 간격** 칸을 만들고, 칸의 ms 오차가 그 노트의 퍼펙트 범위를 넘으면 `result = Great(3)`, 아니면 `Prefect(4)` |
| 판정 범위 | `NoteConfigData.left/right_perfect_range`, `left/right_great_range` + `BattleProperty.hitRangeAddValue` (`TimeNodeOrderFactory.RangeFixed`) | **노트마다** 범위가 따로 있고, 캐릭터 스킬이 넓힐 수 있음(예: `BlackSkill` +0.01초) |
| 결과 전달 | `GameTouchPlay.TouchResult(idx, resultCode, actionType, tno, ...)` | 찾은 칸의 `result`를 `resultCode`로 넘김 |

즉 게임이 실제로 쓴 오차는 **`tickTime − 노트 시각`(1ms 단위)** 이고, 판정 등급은 그 오차를 **노트별 범위**와 비교해 정해집니다.

## 2. 판정바가 하는 방식

```csharp
float gap = (GameGlobal.gGameMusic.m_LastMusicTick / 1000f) - (float)tno.md.tick;   // TouchResult Postfix
// 색: |gap| ≤ 50ms 황금, ≤ 130ms 초록, 그 밖은 빠름(파랑)/느림(빨강)
```

- `GameMusic.m_LastMusicTick`은 `GameMusicFixTimerUpdate`(RVA `0x4956F0`)와 `GameMusicTimerUpdate`(RVA `0x4957D0`)가 `(float)StageBattleComponent.realTimeTick`으로 채웁니다. `realTimeTick`(RVA `0x1D772A0`)은 `stopwatch.ElapsedMilliseconds`입니다.

## 3. 대조 결과

| 항목 | 판정 | 내용 |
| --- | --- | --- |
| 시계 | ✅ 같음 | 둘 다 `GameGlobal.stopwatch`. 단위(ms → 초 변환)도 맞습니다. |
| 읽는 시점 | ⚠️ 다름 (확인 필요) | 게임은 **키를 누른 순간** `tickTime`에 담고, 판정바는 **마지막 GameMusic 타이머 갱신 때** 담긴 `m_LastMusicTick`을 씁니다. 같은 프레임 안이면 차이가 거의 없지만, 갱신 순서·주기에 따라 최대 한 갱신 간격만큼 어긋날 수 있습니다. 크기는 실측이 필요합니다. 게임이 판정에 쓴 값 그대로를 원하면 `GameGlobal.gTouch.tickTime − md.tick`이 정확합니다. |
| 판정 칸 단위 | ✅ 문제 없음 | 게임 칸이 1ms 간격이라, 판정바의 ms 표시와 자릿수가 맞습니다. |
| 색 기준(±50 / ±130ms) | ⚠️ 게임과 다를 수 있음 | 게임은 노트마다 퍼펙트·그레이트 범위가 따로 있고 스킬(`hitRangeAddValue`)로 넓어집니다. 판정바는 고정값이라 실제 판정과 색이 엇갈릴 수 있습니다(예: 블랙 마리쟈로 퍼펙트가 났는데 초록). 노트별 실제 값은 정적으로 확인하지 못했습니다. 실제 등급을 쓰려면 `resultCode`(또는 `tno.result`)로 색을 고르는 편이 정확합니다. |
| 강제 퍼펙트와의 관계 | ✅ 문제 없음 | 색이 오차만 보고 정해지므로, 강제 퍼펙트가 `resultCode`를 바꿔도 판정바는 실제 타이밍을 보여 줍니다(의도와 맞음). |
| 홀드·연타 제외 | ✅ 맞음 | 홀드 중간·꼬리와 연타는 `OnControllerHit`가 누르는 동안 칸마다 `TouchResult`를 부르므로(`LongPressController` ISIL), `isLongPressing`/`isLongPressEnd`/`isMuling` 칸을 거르는 것이 맞습니다. 홀드 머리는 일반 타격과 같이 기록됩니다. |
| 주석 오류 | 📝 코드 주석 | 훅의 `// Miss(0)` 주석은 틀렸습니다. `TaskResult`는 `None=0, Miss=1, Cool=2, Great=3, Prefect=4`입니다. 코드는 `resultCode == 0`(None)만 거르고, 칸이 없는 입력은 `TouchResult`까지 오지 않습니다. |

## 4. 결론

오차의 **시계와 단위는 맞습니다.** 다만 두 가지는 게임이 판정에 쓴 값과 다를 수 있습니다.

1. 읽는 시점: `m_LastMusicTick` 대신 `GameGlobal.gTouch.tickTime`을 쓰면 게임과 같은 값이 됩니다.
2. 색 기준: 고정 ±50/±130ms 대신 `resultCode`로 등급을 고르면 노트별 범위와 스킬 보정까지 반영됩니다.

둘 다 코드 수정이 필요해 이번 점검에서는 기록만 했습니다. 고치기 전에 실기 로그로 1번의 실제 어긋남 크기(`m_LastMusicTick` vs `tickTime`)를 재 보면 우선순위를 정할 수 있습니다.
