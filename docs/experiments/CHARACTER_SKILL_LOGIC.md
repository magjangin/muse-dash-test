# 캐릭터·엘핀 스킬 로직 지도 (6.7.0)

캐릭터와 엘핀 스킬이 게임 안에서 무엇을 바꾸는지 정리한 표입니다. 스킬을 고치거나 커스텀 곡에서 스킬 때문에 생기는 문제를 볼 때 출발점으로 씁니다.

- 조사일: 2026-10-25, 게임 6.7.0(IL2CPP)
- 방법: Cpp2IL ISIL로 `Assets/Scripts/GameCore/Skill/*Skill.Apply()` 본문을 읽고, 쓰는 필드 위치를 `cpp2il_out`의 `BattleProperty`(`ilspycmd -t Assets.Scripts.GameCore.Managers.BattleProperty`) 필드 위치와 맞췄습니다. 체력 감소 경로는 REA(Ghidra)로 `GameMusic.TimerStepTrigger`를 디컴파일해 확인했습니다.
- **정적 분석입니다.** uid(`Character_N`/`Elfin_N`)와 게임 속 캐릭터 이름의 대응은 코드에 없어서, 실기로 확인한 것만 이름을 적었습니다.

---

## 1. 구조

```
StageBattleComponent.InitGame()            ← 배틀 시작(곡 선택이 끝난 뒤)
  └ SkillManager.Apply()
      └ SkillManager.ApplyRoleSkill(roleIdx) → <캐릭터>Skill.Apply(), <엘핀>Skill.Apply()
            └ Singleton<BattleProperty>.instance 의 필드를 바꿈
배틀 중: GameMusic.TimerStepTrigger, GameTouchPlay.TouchResult, TimeNodeOrderFactory.RangeFixed 등이
         BattleProperty 값과 SkillManager.<캐릭터>_... 메서드를 읽어 효과를 냅니다.
```

- `Apply`는 대부분 **숫자만** 넣습니다. 발동 조건이 있는 스킬(횟수, 특정 노트 반응)은 `SkillManager`의 캐릭터별 메서드에 있습니다(예: `RampageSkill`(불량소녀 린 GREAT→PERFECT), `PirateRin_*`, `MikuSkill_*`, `RinLenSkill_*`, `WisadelSkill_*`, `BloodHeirSkill_*`, `SkillRacerRin_*`).
- 모드에서 스킬 값을 바꾸려면 **그 스킬의 `Apply` Postfix에서 `BattleProperty` 필드를 고치는 것**이 가장 좁은 자리입니다. 배틀마다 `Apply`가 다시 불리므로 값이 다음 판으로 새지 않습니다. 다만 **지금 모드는 캐릭터 스킬을 바꾸지 않기로 했습니다**(2026-10-25 결정, 3절).

## 2. 스킬별 Apply가 바꾸는 값

값은 ISIL의 즉시값이며, `float`은 16진수를 풀어 적었습니다.

### 캐릭터

| uid | 클래스 | Apply가 넣는 값 | 비고 |
| --- | --- | --- | --- |
| Character_0 | RockSkill | maxHp 300 | |
| Character_1 | RampageSkill | maxHp 250, greatToPerfect 5 | 불량소녀 린(실기 확인, [CHARACTER_SKILL_IDEAS.md](CHARACTER_SKILL_IDEAS.md)). 실제 변환은 `TouchResult` 안 `SkillManager.RampageSkill` |
| Character_2 | SleepSkill | maxHp 200, `DBSkill.SetAutoPlay` 호출 | 자동 연주 계열 |
| Character_3 | BunnySkill | maxHp 200, heartNoteRate·musicNoteRate·hideNoteRate·blockNoteRate 3.0 | |
| Character_4 | PilotSkill | maxHp 250, isFeverGod | |
| Character_5 | IdolSkill | maxHp 200, expRate(값은 레지스터라 미확인) | |
| Character_6 | ZombieSkill | maxHp 250, hasGodChance, godTimeCount 1 | |
| Character_7 | JokerSkill | maxHp 200, comboRate 0.7 | |
| Character_8 | ViolinSkill | maxHp 250, missComboMax 100 | |
| Character_9 | MaidSkill | maxHp 250, hpSkillCount 1, hpRevive 100, reviveRate 2.5, skillMissHardTime 2.0 | 부활 계열 |
| Character_10 | MagicSkill | maxHp 200, maxFever 100 | |
| **Character_11** | **EvilSkill** | **maxHp 200, hpChangedPerTime −0.1, scoreExtraRate 1.25** | **시간당 체력 감소가 있는 유일한 스킬.** 어린 악마 마리쟈로 추정(이름은 실기 미확인) |
| Character_12 | BlackSkill | maxHp 250, hitRangeAddValue +0.01, blockRG −0.03, isCloseOptimize | 판정 범위 확대. `TimeNodeOrderFactory.RangeFixed`가 칸을 만들기 전에 더함 |
| Character_13 | SantaRinSkill | maxHp 250 | |
| Character_14 | JKSkill | maxHp 250, isCatchAvailable, scoreCoefficient 0.8 | |
| Character_15 | GCSkill | maxHp 250, isGcCharacter | |
| Character_16 | NekoSkill | maxHp 250 | |
| Character_17 | WorkerSkill | maxHp 250 | |
| Character_18 | ReimuSkill | maxHp 250, maxFever 120, blockRG −0.03, isCloseOptimize, jumpAndFloat | |
| Character_19 | ClearSkill | maxHp 250 | |
| Character_20 | SisterSkill | maxHp 250 | 노트 처리는 `SkillManager.SisterSkill`(채보 로딩 중 호출) |
| Character_21 | MarisaSkill | maxHp 200, heartNoteRate·musicNoteRate 3.0, jumpAndFloat | |
| Character_22 | AmiyaSkill | maxHp 250 | `SkillManager.AmiyaSkillG2P`(TouchResult) |
| Character_23 | OlaSkill | maxHp 250, isFeverGod | |
| Character_24 | TaoistSkill | maxHp 200, feverScoreRate 1.3 | |
| Character_25 | MikuSkill | maxHp 250 | 판정 반전 계열 `SkillManager.MikuSkill_*` |
| Character_26 | RinLenSkill | maxHp 250 | `SkillManager.RinLenSkill_*` |
| Character_29 | WisadelSkill | maxHp 250 | `SkillManager.WisadelSkill_*` |
| Character_34 | HorseSkill | maxHp 250, missComboMax 100 | |
| Character_35 | SpectreSkill | maxHp 250 | |
| Character_36 | ThiefSkill | maxHp 200, 다른 캐릭터 스킬을 `ApplyRoleSkill`로 대신 적용 | 훔친 스킬의 `Apply`가 그대로 불림 |

### 엘핀

| uid | 클래스 | Apply가 넣는 값 |
| --- | --- | --- |
| Elfin_0 | MioSirSkill | feverTime 7.0 |
| Elfin_1 | AngelSkill | hurtReduce 6 |
| Elfin_2 | ThanatosSkill | isBloodMissHardTime, missHardTime(값 미확인) |
| Elfin_3 | RabbotSkill | missToGreat 5 |
| Elfin_4 | FanRobotSkill | reviveDeadline 50 |
| Elfin_5 | MagicGirlSkill | feverScoreRate 1.2 |
| Elfin_6 | DragonGirlSkill | bossAttackScoreRate 1.3 |
| Elfin_7 | LilithSkill | perfectScoreExtra 1.05, 조건부 perfectHpRevive 2·musicNoteAddHp 2 |
| Elfin_8 | RhythmSkill | isRhythmElfin 등 리듬 상태 초기화 |
| Elfin_10 | NeonEggSkill | 네온 알 상태 초기화 |
| Elfin_11 | BetaDogSkill | expRate(값 미확인) |

## 3. 어린 악마 마리쟈 체력 감소 (확인됨)

`GameMusic.TimerStepTrigger(decimal ts)`(RVA `0x4977B0`)가 0.01초 눈금마다 이렇게 합니다.

```
if (BattleProperty.isHpChangable && BattleProperty.hpChangedPerTime != 0 && !StageBattleComponent.isDead)
    if (ts % hpChangedPerTime == 0)
        BattleRoleAttributeComponent.instance.AddHp((int)(hpChangedPerTime / Math.Abs(hpChangedPerTime)));
```

- `EvilSkill`은 `hpChangedPerTime = −0.1`이라 **0.1초마다 체력 1**(초당 10)이 줄고, 최대 체력은 200입니다. 회복 없이 20초면 바닥납니다.
- `isHpChangable`은 `TimerStepTrigger`가 첫 노트를 처리할 때 켜고 배틀이 끝날 때 끕니다. 홀드 애니메이션(`PlayLongPressAnimator`)과 샌드백(`MultHitEnemyController.OnControllerAttacked`/`InvokeHitEnd`)에서도 껐다 켜는데, 어느 순간에 끄는지는 아직 보지 않았습니다.
- **넣었다 뺀 옵션**: `config.txt`의 `어린 악마 마리쟈 체력 감소 속도`(0~100)로 `EvilSkill.Apply` Postfix에서 간격을 `0.1 × 100 / 값`초로 늘리는 기능을 만들었다가(커밋 `6f22de5`), 실기 확인 전에 "캐릭터는 건드리지 않는다"는 결정으로 뺐습니다. 다시 필요하면 그 커밋을 보면 됩니다. 간격은 게임 시간 눈금(0.01초)의 배수여야 나머지 연산이 0이 되는 순간이 맞고, `hpChangedPerTime = 0`이면 게임이 감소를 건너뜁니다.
