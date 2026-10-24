# 홀드 꼬리 렌더 지연 (미해결)

공중 홀드 꼬리와 지상 홀드 머리가 같은 박자에 있을 때, **공중 홀드 꼬리가 판정 시각보다 살짝 늦게
판정선에 닿아 보이는** 문제입니다. 지상 홀드 머리는 박자에 맞습니다. 원인을 찾지 못해 코드는 v0.11.0 그대로 두었습니다.

- 조사일: 2026-10-24, 게임 6.7.0, 모드 v0.11.0
- 재현: Star Trace(`hwa\Star Trace\hwa2.bms`, BPM 175) 100마디 = 137.1429초
  - 공중 홀드 #10 `040206`(속도 3, `_dt1.07`) 135.77 → **137.14 꼬리**
  - 지상 홀드 #11 `040203`(속도 3, `_dt1.07`) **137.14 머리** → 138.17
- 증상(사용자 육안): 꼬리 렌더가 판정보다 아주 살짝 뒤. 지상 머리는 정박.

---

## 1. 확인해서 원인이 아닌 것

| 후보 | 확인 방법 | 결과 |
| --- | --- | --- |
| dt가 서로 달라서 | BMS 파싱 + `MoveNote` 계산 | 둘 다 dt 1.07, showTick 136.07로 같음 |
| 꼬리가 늦게 생성돼서 | `SpineActionController.OnControllerStart` 진단 | 꼬리·머리 모두 136.066에 같은 프레임 생성(지연 −0.004초). 홀드 26개 전부 제때 생성 |
| 예전 "동시치기 짝 큰 dt 통일" 로직을 뺀 탓(`0913189`) | 코드 이력 | 그 로직은 type 1 짝에만 적용돼 홀드에 쓰인 적이 없음. 두 노트 dt도 같아서 복원해도 변화 없음 |
| v0.11.0 스폰 지연 보정(`NoteAnimationSyncPatch`) | 같은 훅(`PlayByKey("in")`)에 진단을 걸어 봄 | 홀드 노트는 `PlayByKey("in")`을 아예 타지 않음(0줄). 홀드에는 이 보정이 걸리지 않음 |
| 몸통 간격·꼬리 위치 규칙이 공식과 달라서 | 공식 곡(48-4) 채보와 `GameStart` 시점 데이터 비교 | 몸통 0.1초 간격, 몸통 수 ⌊길이/0.1⌋, 꼬리 tick = 머리 + 길이, 각 노트 `showTick = tick − dt`로 **공식과 같음**. 공식도 마지막 몸통과 꼬리가 0.002초까지 붙음 |
| 길이 반올림(1.3714 → 1.37) | 계산 | 0.0014초 차이라 보이지 않을 크기 |

## 2. 남은 차이: 홀드 연결 필드

타이밍 값은 같지만, 머리·몸통·꼬리를 잇는 필드는 공식과 모드가 다릅니다(공식 48-4, `GameStart` 시점).

| 필드 | 공식 | 모드(`AddLongNote`) |
| --- | --- | --- |
| 머리 `longPressPTick` / `endIndex` / `longPressNum` | 0 / 829(모든 머리가 같은 값) / 0 | 머리 tick / 자기 꼬리 인덱스 / ⌈길이/0.1⌉ |
| 몸통·꼬리 `longPressPTick` | 머리 tick이 아님(머리 showTick + 0.638로 보임) | 머리 tick |
| 몸통·꼬리 `endIndex` | 인덱스가 아닌 값(2614, 4741, 8146 …) | 꼬리 인덱스 |
| 몸통·꼬리 `longPressNum` | 0 | ⌈길이/0.1⌉ |

공식 값의 의미는 밝히지 못했습니다. 공식 머리의 `config.length`는 박자 기준 원래 값(예: 1.2765957…)이고, 모드는 소수 둘째 자리로 반올림한 값입니다.
홀드는 생성 시점에 Spine 애니메이션이 비어 있어(`currentAnimationName` 빈 값), 위치를 Spine 비행 애니메이션이 아니라
`LongPressController`(자식 `m_LongChild`, 표시 `LongPressNoteVisibleController`) 쪽이 정하는 것으로 보입니다.

## 3. 해 보지 못한 것 (다음에 이어서 볼 곳)

1. **공식 곡에서 같은 상황 비교.** 한 레인 홀드 꼬리와 반대 레인 홀드 머리가 같은 박자인 공식 채보를 찾아,
   원작도 꼬리가 늦어 보이는지 확인합니다. 찾는 진단(곡 선택 화면에서 F9 → 공식 곡 전 난이도
   `{music}_map{diff}`를 `ResourcesManager.LoadFromName<StageInfo>`로 불러 ±5ms 일치 검색)은 만들었지만 실행하지 않았습니다.
   원작도 같다면 엔진 연출이므로 고칠 대상이 아닙니다.
2. **연결 필드를 공식처럼 맞추기.** 공식 값의 의미를 모르는 채로 흉내 내야 하므로 판정이 깨질 위험이 있습니다.
   `LongPressController.Init`의 `_Init_b__12_0(MusicData)` 조건(자식 홀드 노트를 고르는 술어)과
   `GetPMusicIndex`를 ISIL로 읽어 `longPressPTick`·`endIndex`의 쓰임을 먼저 확정하십시오.

## 4. 이번에 시도했다가 되돌린 것

- **홀드 머리·꼬리를 동시치기 짝(`isDouble`/`doubleIdx`)에 넣기**(`GetDoubleExclusionReason`에서 type 3 허용).
  홀드를 짝으로 넣은 곳마다 `BaseEnemyObjectController.OnControllerAttacked`에서 NRE가 났습니다
  (Star Trace, 홀드 시각 48.00 ~ 138.17초 13곳, 수정 전 0건). 렌더 어긋남도 그대로였습니다. 2026-06 `287711a` 이후처럼
  일반 노트(type 1)만 짝으로 둡니다. 게임의 `MusicData.IsDouble`은 `isDouble && doubleIdx > 0`만 보지만,
  공격 처리 경로가 홀드 짝을 감당하지 못합니다.

관련: [NOTE_SPEED_DT.md](NOTE_SPEED_DT.md) (dt와 등장 애니메이션), [GHOST_NOTE_ALPHA_HOLD.md](GHOST_NOTE_ALPHA_HOLD.md)
