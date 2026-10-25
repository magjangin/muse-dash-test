# 캐릭터·엘핀 스킬 개조 아이디어 (보류)

불량소녀 린·라봇-233·유메의 판정 스킬을 캐릭터·엘핀 장착과 무관하게 켜고 바꿔 보려던 기록입니다.
구현해서 실기까지 돌려 봤지만 **main에는 넣지 않고 되돌렸습니다**(2026-10-25). 다시 시작할 때 이 문서와
아래 브랜치를 출발점으로 삼으십시오.

- 측정일: 2026-10-25, 게임 6.7.0
- 근거: Cpp2IL ISIL 덤프(메서드 본문), `cpp2il_out` 필드 오프셋, `Latest.log` 실측
- 코드: 두 브랜치 모두 원격에 push하지 않은 **로컬 브랜치**입니다.
  - `feature/skill-counter-999` — 린·라봇 횟수 999, 라봇 MISS→PERFECT, 아이콘 숫자 (`SkillCounterPatch.cs`, `SkillCounterBadge.cs`)
  - `feature/yume-gear-smash` — 유메 톱니 부수기 + 통과 기록 (`YumeGearSmashPatch.cs`)

---

## 1. 아이디어

| 아이디어 | 상태 | 브랜치 |
| --- | --- | --- |
| 불량소녀 린 GREAT→PERFECT 횟수를 5 → 999, 장착 없이 | 실기 동작 확인 | `feature/skill-counter-999` |
| 라봇-233 MISS→GREAT 횟수를 5 → 999, 장착 없이 | 실기 동작 확인(이펙트 대체 필요) | `feature/skill-counter-999` |
| 라봇이 살린 노트를 GREAT 대신 PERFECT로 | 실기 로그로 발동 확인 | `feature/skill-counter-999` |
| 좌상단 스킬 아이콘에 남은 횟수 숫자 표시 | 실기에서 숫자 보임 | `feature/skill-counter-999` |
| 유메 톱니 부수기를 장착 없이, 부순 톱니를 MISS 대신 통과로 | 부수기·1회 처리까지 확인, **결과 화면 집계는 미확인** | `feature/yume-gear-smash` |

---

## 2. 불량소녀 린 / 라봇-233

### 2.1 횟수와 발동부

- 두 스킬의 남은 횟수는 `BattleProperty`(싱글톤) 필드입니다. `greatToPerfect` @0x28, `missToGreat` @0x2C. (`maxHp` @0x10)
- 원본은 `RampageSkill.Apply`(`Character_1`)가 `maxHp=250`, `greatToPerfect=5`를, `RabbotSkill.Apply`가 `missToGreat=5`를 넣을 뿐입니다.
- 발동부는 **남은 횟수만** 봅니다. 캐릭터·엘핀 장착 검사가 없습니다.
  - `SkillManager.RampageSkill(ref byte result, ref bool isElfinSkill, MusicData md)` ← `GameTouchPlay.TouchResult`.
    조건: `!isElfinSkill && result == 3(Great) && greatToPerfect > 0` → 횟수 -1, `result = 4`, `Battle/OnGreat2Perfect`.
    `TouchResult` 안에서는 노트 타입 1·4·5(Monster/Hide/Boss)일 때만 이 블록에 들어갑니다. 롱노트(3)는 원래도 대상이 아닙니다.
  - `SkillManager.RobotSkill(int index)` ← `GameMissPlay.SkillMiss`. 조건: `missToGreat > 0`, 롱노트·연타·Wisadel 피버 제외 등.
    통과하면 횟수 -1, `GameTouchPlay.TouchResult(idx, 3, …, isElfinSkill: true)`, `Battle/OnMiss2Great`.
- 값을 쓰는 곳은 위 Apply 둘과 발동부 둘뿐이고, `StageBattleComponent.InitGame`이 `BattleProperty`를 새로 만든 **다음에**
  `SkillManager.Apply`를 부릅니다. 그래서 `SkillManager.Apply` Postfix에서 999로 덮으면 공식곡·커스텀 곡 모두 적용됩니다.
- `RampageSkill.Apply`/`RabbotSkill.Apply`는 virtual이라 패치하지 않았습니다(체크리스트의 IL2CPP virtual 패치 함정).

### 2.2 라봇 미장착 시 NRE (실측)

- 처음 구현에서 미스마다 `During invoking native->managed trampoline … SkillManager.RobotSkill` NRE가 났고, MISS가 그대로 남았습니다.
- 원인: `RobotSkill`은 판정을 바꾸기 **전에** `AttackEffectManager.InvokeElfinEffect()` 결과의 transform을 옮깁니다.
  `m_ElfinEffect`(@0xE0)는 `InitAttackSkillFx`가 **장착 엘핀이 라봇(인덱스 3)일 때만**
  `EffectManager.Preload("elfin_carrot_robot_fx_skill", 3, -1)`로 채웁니다.
- 해결(브랜치): `InitAttackSkillFx` Postfix에서 같은 이펙트를 미리 로드해 두고, `RobotSkill` Prefix에서만 `m_ElfinEffect`에 끼웠다가
  Finalizer에서 null로 되돌립니다. `m_ElfinEffect`는 일시정지·콤보·실패 처리에서도 읽으므로 상시로 채우지 않았습니다.
- 린 쪽은 `InvokeRoleEffect()`가 null이어도 `SkillManager.SetEffectLocate`가 null을 검사해서 문제가 없습니다.

### 2.3 라봇 MISS→PERFECT

- `RobotSkill` 안에서 부르는 `TouchResult(…, isElfinSkill: true)`만 골라 `resultCode` 3 → 4로 바꿨습니다.
  `RobotSkill` 실행 중 플래그를 세우고 Finalizer에서 내립니다. 판정 표시·콤보·집계가 모두 이 값을 따릅니다.

### 2.4 좌상단 아이콘 숫자

- 아이콘은 `UI_2D/Standard/PnlBattle/PnlBattleUI/PnlBattleOthers/Score/SkillIconGrid` 아래에 있고,
  `Battle/OnGreat2Perfect`·`OnMiss2Great` 이벤트로 켜집니다.
- **게임 UI에는 남은 횟수를 그리는 숫자 칸이 없습니다.** 그리드 안에 숫자 Text가 하나도 없었고(1회성 UI 진단),
  블러드 상속자의 `m_ImgCarrotRobotSkill_Img5`처럼 "5"는 그림에 들어 있는 것으로 보입니다.
- 브랜치에서는 아이콘마다 `HwaSkillCount` Text 자식을 만들어 남은 횟수를 썼고, 실기에서 숫자가 보였습니다.

---

## 3. 유메 (톱니)

### 3.1 원본 판정 — GREAT가 아니라 MISS

- 유메는 `GCSkill`(`Character_15`). `Apply`가 `BattleProperty.isGcCharacter`(@0x12B) = true, `maxHp` = 250.
  `isGcCharacter`를 읽는 곳은 `GameMissPlay.MissCube` **한 군데뿐**입니다.
- `GameMusic.TimerStepTrigger(decimal ts)`가 판정 시간 칸(`TimeNodeOrder`, `[rbx+16]`=idx, `[rbx+18]`=result,
  `[rbx+28]`=isLast)을 돌며:
  - 칸 결과가 3(충돌 구간)이고 노트 결과가 아직 0이며 캐릭터가 같은 줄이면 `MissCube(idx, tick)`.
  - `MissCube`가 true면 `SkillManager.YumeSkill(idx)`(노트에 `note_destroy`, 캐릭터 `pc` 본 위치로 이동, `sfx_block`) 후 점수 경로를 건너뜀.
  - 칸이 `isLast`이고 결과가 1이 아니면 정상 통과 경로: `TaskStageTarget.AddScore` → `SetPlayResult(idx, 4)` → `AddBlock`.
- `MissCube`의 유메 분기: 노트 타입 2(Block) && `isGcCharacter` && `m_MissHardTime <= 0` →
  `SetPlayResult(idx, 1 = Miss)` + 업적 카운트 3001 → true. (`m_MissHardTime > 0`이면 피해 없이 false)
- 즉 **유메가 부순 톱니는 MISS(1)로 기록되고 점수·톱니 통과 수가 오르지 않습니다.** 체력만 안 깎입니다.

### 3.2 브랜치 구현

- `SkillManager.Apply` Postfix: `isGcCharacter = true` (HP는 안 바꿈).
- `MissCube` Prefix: 톱니 + 플래그면 원본을 건너뛰고, 같은 조건(`m_MissHardTime <= 0`)에서 `YumeSkill`을 직접 부른 뒤 false 반환 →
  게임의 정상 통과 경로가 기록. 업적 카운트(3001)는 부르지 않음.
- 실측 문제: 톱니 하나에 칸이 여러 개라 충돌 구간 동안 칸마다 `MissCube`가 불려, 같은 톱니가 한 프레임에 3번 이상 부서졌습니다.
  원본은 첫 칸에서 Miss를 기록해 막습니다. 판마다 부순 idx를 기억해 `YumeSkill`을 한 번만 부르도록 고쳤고, 실기에서 idx마다 1회로 확인했습니다.
- **미확인**: 결과 화면에서 부순 톱니가 MISS로 잡히지 않는지, 정확도·톱니 통과 수가 피한 톱니와 같게 오르는지.
  확인한 판은 전투 시작 약 18초 뒤에 종료되어 결과 화면 로그가 없었습니다.

---

## 4. 다시 시작할 때

1. 해당 브랜치를 main에 rebase하고 빌드합니다.
2. 유메는 톱니가 있는 곡으로 **결과 화면까지** 쳐서 MISS·정확도를 확인합니다. (Star Trace 커스텀 채보는 톱니 0개)
3. 공식곡에서는 조작된 판정이 그대로 기록에 남습니다. 강제퍼펙트와 같은 이유로 오프라인 플레이를 권합니다.
