# 보스 실험 가이드

보스 실험은 “보스 액션 트리거”와 “실제 보스 프리팹”을 반드시 나눠서 봐야 합니다. 둘이 같은 것처럼 보이지만 역할이 다릅니다.

| 구분 | 수정 위치 | 의미 |
| --- | --- | --- |
| 보스 액션 트리거 | `DBStageInfoPatch.cs`의 `ExperimentNotes` | `in`, `boss_far_atk_1_start`, `boss_far_atk_2_start`, `out` 같은 보스 동작 실행 |
| 실제 보스 프리팹 | `BossPatch.cs`의 `BossRewriteRules` | 화면에 보이는 보스 오브젝트 변경 |

## 핵심 개념

보스 액션 트리거 노트는 화면에 보이는 노트가 아닙니다. `prefab_name=empty_000`인 빈 트리거를 차트에 넣고, 그 트리거가 보스에게 `in`, `boss_far_atk_1_start`, `boss_far_atk_2_start`, `out` 같은 명령을 전달하는 구조입니다.

실제 보스 모델/오브젝트는 `empty_000`에서 나오지 않습니다. 실제 보스는 게임의 `Boss.InitBossObject(name, scene, isLast)` 호출에서 생성됩니다. 그래서 다른 씬 보스를 불러오고 싶으면 `BossPatch.cs`에서 `name`과 `scene`을 바꿔야 합니다.

## 보스 액션 트리거 만들기

수정 위치:

```text
muse dash test/Patches/DBStageInfoPatch.cs
```

예시:

```csharp
new ExperimentNoteSpec { Label = "보스 등장", Uid = "050101", NoteType = 0, Pathway = 0, BossAction = "in", StartTick = 15.0 },
new ExperimentNoteSpec { Label = "보스 원거리1 시작", Uid = "050107", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_1_start", StartTick = 16.0 },
new ExperimentNoteSpec { Label = "보스 원거리1 후속(종료)", Uid = "050108", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_1_end", StartTick = 17.0 },
new ExperimentNoteSpec { Label = "보스 원거리2 시작", Uid = "050109", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_2_start", StartTick = 18.0 },
new ExperimentNoteSpec { Label = "보스 원거리2 후속(종료)", Uid = "050110", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_2_end", StartTick = 19.0 },
new ExperimentNoteSpec { Label = "보스 퇴장", Uid = "050102", NoteType = 0, Pathway = 0, BossAction = "out", StartTick = 22.0 },
```

`NoteType=0` 보스 액션 트리거에 `BossAction`을 넣으면 자동으로 아래 값이 적용됩니다.

- `prefab_name=empty_000`
- `key_audio=""`
- `boss_action`은 지정한 값
- `Dt`를 생략하면 `dt=0`
- `showTick=tick`

보스 발사체는 위 규칙과 다릅니다. `xx=06/07/08`, `yy=01/04` 계열은 `NoteType=1`인 보이는 일반 노트입니다. `BossAction`은 들어갈 수 있지만 프리팹은 `empty_000`이 아니라 일반 노트처럼 `{uid}_road_nor_1` 또는 `{uid}_air_nor_1`을 씁니다. `Dt`는 0.7 정도가 적당합니다. 보스 액션 없이 발사체만 만들 때는 `BossAction`을 비우거나 생략합니다.

```csharp
new ExperimentNoteSpec { Label = "보스 발사체만 1개", Uid = "070601", NoteType = 1, Pathway = 0, StartTick = 20.0, BossAction = "" },
new ExperimentNoteSpec { Label = "보스 단타 노트 1개", Uid = "070601", NoteType = 1, Pathway = 0, StartTick = 20.0, PrefabName = "070601_road_nor_1", BossAction = "boss_far_atk_1_R", Dt = 0.7 },
new ExperimentNoteSpec { Label = "보스 단타 노트 1개", Uid = "070701", NoteType = 1, Pathway = 0, StartTick = 20.0, BossAction = "boss_far_atk_2", Dt = 0.7 },
```

확인된 발사체 액션:

| UID 패턴 | Pathway | 프리팹 예 | BossAction |
| --- | --- | --- | --- |
| `**0601` | `0` | `070601_road_nor_1` | `boss_far_atk_1_R` |
| `**0604` | `1` | `070604_air_nor_1` | `boss_far_atk_1_L` |
| `**0701` | `0` | `070701_road_nor_1` | `boss_far_atk_2` |

노트의 `scene`을 직접 맞추고 싶으면 `Scene`을 넣을 수 있습니다. 이 값은 `BossAction`과 별도로 `noteData.scene`만 덮어씁니다.

```csharp
new ExperimentNoteSpec { Label = "보스 등장 씬 테스트", Uid = "050101", NoteType = 0, Pathway = 0, BossAction = "in", Scene = "scene_00", StartTick = 15.0 },
```

`Dt`를 직접 지정하면 직접 지정값이 우선합니다.

```csharp
new ExperimentNoteSpec { Label = "보스 등장 dt 테스트", Uid = "050101", NoteType = 0, Pathway = 0, BossAction = "in", StartTick = 15.0, Dt = 0.25 },
```

씬 전환 노트는 `0004xx` 계열입니다. 이 노트는 `ibms_id`가 `sceneInfo` 딕셔너리 키와 맞아야 실제 전환이 일어납니다.

```csharp
new ExperimentNoteSpec { Label = "씬 변환 노트", Uid = "000401", NoteType = 9, Pathway = 0, StartTick = 20.0, PrefabName = "000401", BossAction = "0", Scene = "0", KeyAudio = "0", IbmsId = "1O" },
```

`000401`은 `IbmsId = "1O"`이고, 로그상 `sceneInfo[1O]=1`로 매핑됩니다. 씬 전환 시 `Boss.SceneBossChange`도 같이 호출되므로, 보스를 유지하려면 `Boss.SceneBossChange` 강제 변경은 끄는 편이 안전합니다.

관찰된 `IbmsId` 매핑:

| UID | `IbmsId` | `sceneInfo` |
| --- | --- | --- |
| `000401` | `1O` | `1` |
| `000402` | `1P` | `2` |
| `000403` | `1Q` | `3` |
| `000404` | `1R` | `4` |
| `000405` | `1S` | `5` |
| `000406` | `1T` | `6` |
| `000407` | `1U` | `7` |
| `000408` | `1V` | `8` |
| `000409` | `1W` | `9` |
| `000410` | `1X` | `10` |
| `000412` | `1Y` | `12` |

## UID 메모

UID의 앞 두 자리 `zz`는 씬 계열입니다. 보스 토큰은 뒤 4자리(`xxyy`) 기준으로 아래처럼 관찰했습니다.

| 뒤 4자리 | 관찰된 동작 |
| --- | --- |
| `0101` | `in` |
| `0107` | `boss_far_atk_1_start` |
| `0108` | `boss_far_atk_1_end` |
| `0109` | `boss_far_atk_2_start` |
| `0110` | `boss_far_atk_2_end` |
| `0102` | `out` |

보스는 보통 `in -> action -> out` 순서로 배치해서 테스트하는 편이 안전합니다. 너무 떨어진 tick에 배치하면 동작 확인이 헷갈릴 수 있으니 처음에는 가까운 시간대에 몰아서 실험하는 것이 좋습니다.

### 보스 UID를 읽는 방식

보스 액션 UID도 전체 형태는 `zzxxyy`로 봅니다.

```text
050101
││└─ 01 = 세부 액션 값
│└── 01 = 보스 액션 계열
└─── 05 = 씬 계열
```

즉 `050101`, `040101`, `070101`은 모두 뒤 4자리만 보면 `0101`, 보스 등장 액션입니다. 다만 앞 두 자리 `zz`는 씬 계열이라서 현재 차트/씬과 너무 동떨어진 값을 넣으면 로딩 흐름과 맞지 않을 수 있습니다.

현재 실험에서 중요하게 보는 값은 아래입니다.

| 예시 UID | 뒤 4자리 | `BossAction` | `prefab_name` | `dt/showTick` 기본값 |
| --- | --- | --- | --- | --- |
| `050101` | `0101` | `in` | `empty_000` | `dt=0`, `showTick=tick` |
| `050107` | `0107` | `boss_far_atk_1_start` | `empty_000` | `dt=0`, `showTick=tick` |
| `050108` | `0108` | `boss_far_atk_1_end` | `empty_000` | `dt=0`, `showTick=tick` |
| `050109` | `0109` | `boss_far_atk_2_start` | `empty_000` | `dt=0`, `showTick=tick` |
| `050110` | `0110` | `boss_far_atk_2_end` | `empty_000` | `dt=0`, `showTick=tick` |
| `050102` | `0102` | `out` | `empty_000` | `dt=0`, `showTick=tick` |

`empty_000`은 보스 액션 노트의 프리팹 이름입니다. 이름 때문에 보스 프리팹처럼 보일 수 있지만, 실제 역할은 “보이지 않는 액션 트리거”입니다.

### 보스 노트와 보스 액션의 차이

문서나 로그를 볼 때 아래 계열을 분리해서 보는 것이 중요합니다.

| 계열 | UID 패턴 | 주로 보이는 `type` | 의미 |
| --- | --- | --- | --- |
| 보스 액션 트리거 | `**0101`, `**0107`, `**0108`, `**0109`, `**0110`, `**0102` | `0` | 보스 등장/원거리 공격/퇴장 명령 |
| 보스 발사체 | `**0601`, `**0604`, `**0701`, `**0704`, `**0801`, `**0804` | `1` | 보스가 쏘는 일반형 노트. `empty_000`이 아니라 일반 노트 프리팹 사용 |
| 보스 톱니 | `**09yy` | `2` | 보스 연출이 섞인 톱니 계열 |
| 보스 1대 치기 | 원본 보스 타격 계열 | `5` | 발사체와는 별도인 보스 타격 노트 |
| 샌드백/멀티히트 | `**04yy` | `8` | 길이를 가진 단일 슬롯 멀티히트 |

그래서 “보스가 안 나온다”는 문제는 보통 두 종류로 나뉩니다.

- 보스 액션 트리거가 안 들어간 경우: `DBStageInfoPatch.cs`의 `ExperimentNotes`와 `empty_000` 로그를 봅니다.
- 실제 보스 모델이 원하는 것으로 안 바뀐 경우: `BossPatch.cs`의 `BossRewriteRules`와 `Boss.InitBossObject` 로그를 봅니다.
- 보스 씬 전환이 의심되는 경우: `Boss.SceneBossChange` 로그와 `SceneBossChangeRules`의 `OrigIdx/NewIdx`를 봅니다.
- 음악 씬 자체가 다르게 로드되는지 확인하려면 `GameMusicScene.LoadScene` 로그와 `LoadSceneRewriteRules`의 `OrigSceneName/NewSceneName`을 봅니다.

## 실제 보스 프리팹 바꾸기

수정 위치:

```text
muse dash test/Patches/BossPatch.cs
```

현재 확인한 정답 프리팹은 아래입니다.

```csharp
private static readonly BossRule[] BossRewriteRules = new[]
{
    new BossRule { OrigName = "*", OrigScene = null, OrigIsLast = null, NewName = "0401_boss", NewScene = 4 },
};
```

이 규칙은 모든 보스 호출을 `0401_boss`, `scene 4`로 바꿉니다. `OrigName="*"`는 원래 보스 이름을 가리지 않고 모두 매칭한다는 뜻입니다. `OrigScene=null`은 원래 씬을 가리지 않는다는 뜻입니다. `OrigIsLast=null`은 마지막 보스 여부도 가리지 않습니다.

다른 보스를 테스트하려면 `NewName`, `NewScene`만 바꿉니다.

```csharp
new BossRule { OrigName = "*", OrigScene = null, OrigIsLast = null, NewName = "0701_boss", NewScene = 7 },
```

특정 원본 보스만 바꾸고 싶으면 조건을 좁힙니다.

```csharp
new BossRule { OrigName = "0501_boss", OrigScene = 5, OrigIsLast = true, NewName = "0401_boss", NewScene = 4 },
```

## 로그 확인

실제 보스 프리팹 변경이 적용되면 아래처럼 나옵니다.

```text
Il2Cpp.Boss.InitBossObject 호출: name=0501_boss, scene=5, isLast=True
Il2Cpp.Boss.InitBossObject: 변경 적용 -> name=0401_boss, scene=4
Il2Cpp.Boss.InitBossObject 완료: name=0401_boss, scene=4, isLast=True
```

보스 액션 트리거가 추가되면 노트 로그에도 보입니다.

```text
실험 노트 추가: 보스 등장 #1/1, objId=1, tick=15, dt=0, showTick=15, speed=..., uid=050101, type=0, pathway=0, prefab=empty_000
```

## 자주 헷갈리는 부분

`empty_000`은 보스 프리팹이 아닙니다. 보스 액션을 실행시키는 빈 트리거입니다. 실제 보스 모델은 `BossPatch.cs`의 `NewName`, `NewScene`에서 결정됩니다.

`zz`는 씬 계열입니다. 앞 두 자리가 다른 UID를 넣으면 현재 로드된 보스 흐름과 맞지 않아 안 보일 수 있습니다.

`NoteType=0` 보스 액션 트리거는 화면에 일반 노트처럼 보이지 않는 것이 정상입니다. 보스 발사체는 `BossAction`이 있어도 `NoteType=1`과 일반 프리팹을 쓰므로 화면에 보이는 노트입니다.

## 안 나올 때 체크리스트

- `ExperimentNotes`에 `BossAction="in"`이 실제로 들어갔는지
- 액션 트리거라면 `실험 노트 추가` 로그에 `prefab=empty_000`이 찍히는지
- 보스 발사체라면 `prefab=070601_road_nor_1` 같은 일반 프리팹과 `dt=0.7`이 찍히는지
- `dt=0`, `dt=0.7` 또는 직접 지정한 `Dt`가 의도대로 찍히는지
- `Boss.InitBossObject: 변경 적용` 로그가 찍히는지
- `NewName`, `NewScene` 조합이 실제 존재하는 보스인지
- `in -> action -> out` 순서가 너무 이상하지 않은지
- 게임이 해당 구간에서 실제로 보스를 초기화하는 타이밍인지

## 실험 추천 순서

처음에는 보스 액션 하나만 넣습니다.

```csharp
new ExperimentNoteSpec { Label = "보스 등장", Uid = "050101", NoteType = 0, Pathway = 0, BossAction = "in", StartTick = 15.0 },
```

그 다음 `BossPatch.cs`에서 모든 보스를 원하는 보스로 리디렉션합니다.

```csharp
new BossRule { OrigName = "*", OrigScene = null, OrigIsLast = null, NewName = "0401_boss", NewScene = 4 },
```

보스가 뜨면 `boss_far_atk_1_start`, `boss_far_atk_1_end`, `boss_far_atk_2_start`, `boss_far_atk_2_end`, `out`을 추가합니다. 한 번에 너무 많은 변수를 바꾸면 원인 파악이 어려워집니다.

## 실시간 보스 교체 (Dynamic Boss Swap)

스테이지 진행 도중 하나의 보스가 퇴장한 뒤, **완전히 다른 종류의 보스를 실시간으로 교환하여 등장**시키는 연출을 구현할 수 있습니다. 

### 작동 원리

1. **`swap:[보스이름]:[씬]` 키워드 사용**:
   차트의 보스 액션(`BossAction`) 속성에 `swap:보스이름:씬번호` (예: `swap:0401_boss:4`)를 작성하면 모드가 이를 감지합니다.
2. **`out` 퇴장으로 인한 비활성화 자동 복구**:
   이전 보스가 `out` 액션을 플레이해 화면 밖으로 퇴장하면, 게임 엔진은 보스 관리 오브젝트(`Il2Cpp.Boss`)를 내부적으로 비활성화(`gameObject.SetActive(false)`) 처리합니다. 모드에서는 `swap:` 키워드가 들어오는 즉시 유니티 컴포넌트 캐스팅을 통해 보스 및 부모 오브젝트를 강제로 깨워 활성화(`SetActive(true)`)시킨 후 새 보스를 주입합니다.
3. **리디렉션 필터 우회**:
   `BossPatch.cs`에 등록된 글로벌 리디렉션 룰(`OrigName = "*"`)에 걸려 교체하려는 보스가 강제로 첫 번째 보스로 덮어써지는 일을 임시 플래그(`isDynamicSwapping`)를 통해 우회 차단합니다.
4. **자동 등장 연출**:
   새로운 보스 프리팹이 조립되는 즉시, 내부적으로 등장 애니메이션(`Play("in")`)을 자동 트리거하여 즉각 부드럽게 화면 안으로 날아 들어오게 만듭니다.

### 실시간 교체 설정 예시 (`DBStageInfoPatch.cs`)

아래 타임라인은 첫 번째 흑호 보스가 공격 후 **완벽히 완전 퇴장(`out`)**한 뒤, 새로운 핑크 음악 보스로 교체되는 고난도 태그 매치 연출의 정석 예시입니다.

```csharp
private static readonly ExperimentNoteSpec[] ExperimentNotes =
{
    // [1] 보스1 등장 및 1페이즈 공격 후 완전 퇴장 (22.0초)
    new ExperimentNoteSpec { Label = "보스1 등장", Uid = "050101", NoteType = 0, Pathway = 0, StartTick = 15.0, BossAction = "in" },
    new ExperimentNoteSpec { Label = "보스1 공격", Uid = "050107", NoteType = 0, Pathway = 0, StartTick = 17.5, BossAction = "boss_far_atk_1_start" },
    new ExperimentNoteSpec { Label = "보스1 퇴장", Uid = "050102", NoteType = 0, Pathway = 0, StartTick = 22.0, BossAction = "out" },

    // [2] 24.0초에 보스2(0401_boss)로 교체 및 자동 등장
    new ExperimentNoteSpec { Label = "보스2 교체", Uid = "050101", NoteType = 0, Pathway = 0, StartTick = 24.0, BossAction = "swap:0401_boss:4" },

    // [3] 보스2 공격 후 퇴장 (31.0초)
    new ExperimentNoteSpec { Label = "보스2 공격", Uid = "050107", NoteType = 0, Pathway = 0, StartTick = 26.5, BossAction = "boss_far_atk_1_start" },
    new ExperimentNoteSpec { Label = "보스2 퇴장", Uid = "050108", NoteType = 0, Pathway = 0, StartTick = 31.0, BossAction = "boss_far_atk_1_end" },
};
```
