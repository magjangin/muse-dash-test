# 노트 실험 가이드

노트 실험은 `muse dash test/Patches/DBStageInfoPatch.cs`의 `ExperimentNotes` 배열만 수정하면 됩니다. 현재 방식은 원본 차트에서 첫 플레이 노트를 복사한 뒤, 원하는 값으로 바꿔서 새 차트처럼 주입하는 방식입니다.

이 문서는 “어떤 값을 넣어야 어떤 노트가 되는가”를 빠르게 보기 위한 타입 사전 역할도 합니다. 실제 게임 안에서는 일반 노트, 공중 노트, 롱노트, 샌드백, 보스 트리거가 모두 같은 `MusicData` 리스트 안에 들어갑니다.

## MusicData 구조

게임의 차트 한 줄은 대략 아래 구조입니다.

```csharp
public struct MusicData
{
    public short objId;
    public decimal tick;
    public MusicConfigData configData;
    public NoteConfigData noteData;
    public bool isLongPressing;
    public bool isLongPressEnd;
    public decimal longPressPTick;
    public decimal dt;
    public decimal showTick;
}
```

실험할 때 가장 자주 보는 필드는 아래입니다.

| 필드 | 의미 | 실험 시 주의 |
| --- | --- | --- |
| `objId` | 노트 식별자/순번 | 중복되거나 순서가 꼬이면 로그에는 있는데 화면에 안 보일 수 있습니다. 현재 코드는 자동 할당합니다. |
| `tick` | 실제 판정/등장 기준 시간 | `StartTick`, `Interval`, `Count`로 계산합니다. |
| `showTick` | 화면에 보이기 시작하는 시간 | 보통 `tick - dt`입니다. 보스 액션은 기본적으로 `showTick=tick`입니다. |
| `dt` | 화면 표시 선행 시간 | `Dt`를 지정하면 직접값, 생략하면 UID 기반 자동값을 씁니다. |
| `configData` | 차트 쪽 설정 데이터 | `time`, `length`, `note_uid`, `id` 등을 동기화합니다. |
| `noteData` | 노트 정의 데이터 | `uid`, `type`, `pathway`, `prefab_name`, `key_audio`, `speed` 등을 바꿉니다. |
| `isLongPressing` | 롱노트 중간 조각 여부 | `IsLong=true`일 때 코드가 자동 생성합니다. |
| `isLongPressEnd` | 롱노트 끝 조각 여부 | `IsLong=true`일 때 코드가 자동 생성합니다. |
| `longPressPTick` | 롱노트 시작 tick | 롱 start/middle/end가 같은 시작 tick을 공유해야 합니다. |

중요한 점은 `MusicData`가 struct라는 점입니다. 원본을 그대로 만지면 내부 참조가 꼬일 수 있어서 현재 코드는 베이스 노트를 복사하고, `configData`와 `noteData`도 새 인스턴스로 복제한 뒤 수정합니다.

## noteData.type 표

`Uid`만 바꾸면 원하는 노트로 바뀌지 않을 수 있습니다. 게임 로직은 `noteData.type`, `noteData.pathway`, `noteData.prefab_name`, `key_audio`를 함께 봅니다.

| `type` | 관찰된 의미 | 대표 실험값 | 메모 |
| --- | --- | --- | --- |
| `0` | 빈 슬롯/특수 트리거 | 보스 액션 `050101` 등 | 보스 등장/공격 시작/퇴장 트리거는 `prefab_name=empty_000`을 씁니다. |
| `1` | 일반 단타 노트 | `051001`, `051004`, `070601` | `pathway=0` 지상, `pathway=1` 공중입니다. 보스 발사체 `xx=06/07/08`, `yy=01/04`도 type 1이며 일반 노트 프리팹을 씁니다. |
| `2` | 톱니바퀴 계열 | `050301`, 보스 톱니 `xx=09` | 일반 톱니와 보스 톱니는 같은 type 2라도 내부 필드가 다를 수 있습니다. |
| `3` | 롱노트 | `050201` | start, middle, end가 여러 `MusicData` 행으로 만들어집니다. |
| `4` | 고스트 계열 | `xx=17` | 공중 계열로 관찰된 케이스가 있습니다. |
| `5` | 보스 1대 치기 노트 | 원본 보스 타격 계열 | `xx=06/07/08` 발사체와 혼동하지 않는 편이 좋습니다. |
| `6` | 하트 | `000201` | `key_audio=sfx_hp`가 필요합니다. |
| `7` | 음표 | `000301`, `000304` | `key_audio=sfx_score`가 필요합니다. |
| `8` | 샌드백/보스 멀티히트 | `020401` | 롱노트처럼 보이지만 중간 행이 없는 단일 슬롯 + `length` 구조입니다. |

## pathway와 프리팹

| 값 | 의미 | 자동 프리팹 조각 |
| --- | --- | --- |
| `pathway=0` | 지상 노트 | `road` |
| `pathway=1` | 공중 노트 | `air` |

자동 프리팹명은 보통 아래 형태입니다.

```text
{uid}_{road/air}_{nor/up/down}_1
```

예시:

```text
051001_road_nor_1
051004_air_nor_1
051707_road_up_1
051710_air_up_1
051713_road_down_1
051716_air_down_1
```

자동 프리팹명이 틀리면 `PrefabName`을 직접 지정합니다. 특히 해머, 라이더, 보스 계열은 UID만 바꾸는 것보다 `type/pathway/prefab_name/key_audio`를 같이 맞추는 쪽이 안전합니다.

## UID 해석 치트시트

현재 실험 기준 UID는 `zzxxyy`로 보고 있습니다.

| 부분 | 의미 | 예 |
| --- | --- | --- |
| `zz` | 씬 계열로 관찰 | `05`면 `scene_05` 계열 |
| `xx` | 노트 계열 | `10` 일반, `02` 롱, `04` 샌드백 등 |
| `yy` | 레인/방향 또는 액션 세부값 | `01` 지상 nor, `04` 공중 nor |

`zz`가 씬 번호일 때 `xx`는 아래처럼 관찰했습니다.

| `xx` | 의미 | 주로 쓰는 `type` |
| --- | --- | --- |
| `02` | 롱노트 | `3` |
| `03` | 일반 톱니바퀴 | `2` |
| `04` | 샌드백/보스 멀티히트 | `8` |
| `05` | 복선/동시치기 계열 | 추가 확인 필요 |
| `06` | 보스 발사체 1 | `1` |
| `07` | 보스 발사체 2 | `1` |
| `08` | 보스 발사체 3 | `1` |
| `09` | 보스 톱니바퀴 | `2` |
| `10` | 일반 1 | `1` |
| `11` | 일반 2 | `1` |
| `12` | 일반 3 | `1` |
| `13` | 빅노트 1 | 추가 확인 필요 |
| `14` | 빅노트 2 | 추가 확인 필요 |
| `15` | 해머 | UID별 보정 |
| `16` | 라이더 | UID별 보정 |
| `17` | 고스트 | `4` |

`yy`는 아래처럼 관찰했습니다.

| `yy` | 의미 |
| --- | --- |
| `01` | 지상 일반, `road_nor` |
| `04` | 공중 일반, `air_nor` |
| `07` | 지상 up, `road_up` |
| `10` | 공중 up, `air_up` |
| `13` | 지상 down, `road_down` |
| `16` | 공중 down, `air_down` |

하트와 음표는 앞 네 자리 `zzxx`를 따로 보는 편이 쉽습니다.

| `zzxx` | 의미 | 권장 `type` | 권장 `key_audio` |
| --- | --- | --- | --- |
| `0002` | 하트 | `6` | `sfx_hp` |
| `0003` | 음표 | `7` | `sfx_score` |

## 기본 구조

수정 위치:

```csharp
private static readonly ExperimentNoteSpec[] ExperimentNotes =
{
    new ExperimentNoteSpec { Label = "지상 일반 노트", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0, Speed = 5 },
};
```

여러 개를 동시에 실험하려면 배열 안에 여러 줄을 넣습니다.

```csharp
private static readonly ExperimentNoteSpec[] ExperimentNotes =
{
    new ExperimentNoteSpec { Label = "지상 일반", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0 },
    new ExperimentNoteSpec { Label = "공중 일반", Uid = "051004", NoteType = 1, Pathway = 1, StartTick = 16.0 },
};
```

## 필드 설명

| 필드 | 설명 |
| --- | --- |
| `Label` | 로그에 찍힐 이름입니다. 실험 구분용이라 게임 리소스에는 영향이 없습니다. |
| `Uid` | 만들고 싶은 노트 UID입니다. 프리팹 자동 생성, `scene_XX`, `noteUid` 계산에 사용됩니다. |
| `NoteType` | 노트 타입입니다. `1` 일반, `2` 톱니, `3` 롱, `6` 하트, `7` 음표, `8` 샌드백으로 실험했습니다. |
| `Pathway` | `0` 지상, `1` 공중입니다. 자동 프리팹명에서 `road` / `air`로 반영됩니다. |
| `PrefabName` | 직접 프리팹명을 지정할 때 사용합니다. 비우면 자동 생성됩니다. |
| `KeyAudio` | 하트/음표처럼 사운드가 중요한 노트에 지정합니다. 일부 타입은 자동 보정됩니다. |
| `BossAction` | 보스 액션 트리거 또는 보스 발사체용입니다. `in`, `boss_far_atk_1_start`, `boss_far_atk_2_start`, `boss_far_atk_2`, `out` 등을 넣습니다. |
| `Scene` | `noteData.scene`을 직접 설정합니다. 비우면 UID 앞 두 자리로 `scene_XX`가 자동 설정됩니다. 예: `Scene = "scene_00"` |
| `IbmsId` | `noteData.ibms_id`를 직접 설정합니다. 씬 전환 노트에서는 `sceneInfo` 딕셔너리의 키로 쓰이므로 중요합니다. |
| `StartTick` | 첫 노트가 배치될 tick입니다. |
| `Count` | 반복 개수입니다. 기본값은 `1`입니다. |
| `Interval` | 반복 간격입니다. `Count`가 2 이상일 때 `StartTick + Interval * index`로 배치됩니다. |
| `Length` | 롱노트/샌드백 길이입니다. |
| `IsLong` | `true`면 롱노트 체인(start/middle/end)을 만듭니다. |
| `IsMul` | `true`면 type 8 샌드백처럼 길이를 가진 단일 노트를 만듭니다. |
| `Speed` | 노트 속도 직접 지정입니다. `-1`이면 원본/기존 값을 유지합니다. 게임 필드가 정수라 반올림됩니다. |
| `Dt` | `showTick = tick - Dt`에 쓰는 dt 직접 지정입니다. `-1`이면 기존 자동 로직을 사용합니다. |

## 일반 노트

지상 일반 노트 1개:

```csharp
new ExperimentNoteSpec { Label = "지상 일반 1개", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0 },
```

공중 일반 노트 1개:

```csharp
new ExperimentNoteSpec { Label = "공중 일반 1개", Uid = "051004", NoteType = 1, Pathway = 1, StartTick = 15.0 },
```

공중 일반 노트 8개:

```csharp
new ExperimentNoteSpec { Label = "공중 연타", Uid = "051004", NoteType = 1, Pathway = 1, StartTick = 20.0, Count = 8, Interval = 0.5 },
```

## 속도와 dt

속도만 지정:

```csharp
new ExperimentNoteSpec { Label = "속도 테스트", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0, Speed = 5 },
```

dt만 지정:

```csharp
new ExperimentNoteSpec { Label = "dt 테스트", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0, Dt = 0.75 },
```

속도와 dt를 같이 지정:

```csharp
new ExperimentNoteSpec { Label = "속도/dt 테스트", Uid = "051001", NoteType = 1, Pathway = 0, StartTick = 15.0, Speed = 12.0, Dt = 0.75 },
```

`Speed=-1`이면 원본 첫 플레이 노트에서 복사된 속도를 유지합니다. `Dt=-1`이면 기존 자동 로직을 사용합니다. `NoteType=0` 보스 액션 트리거는 `Dt`를 생략하면 `dt=0`입니다. 보스 발사체(`xx=06/07/08`, `yy=01/04`)는 `BossAction`이 있어도 보이는 일반 노트라 기본 `dt=0.7`을 씁니다. `Dt`를 직접 지정하면 직접 지정값이 항상 우선합니다.

로그에서 확인할 값:

```text
실험 노트 추가: ..., tick=15, dt=0.75, showTick=14.25, speed=12, uid=051001, ...
```

## 롱노트

롱노트는 `IsLong=true`를 사용합니다.

```csharp
new ExperimentNoteSpec { Label = "롱 테스트", Uid = "050201", NoteType = 3, Pathway = 0, IsLong = true, StartTick = 25.0, Length = 2.0 },
```

여러 개 만들려면 `Count`와 `Interval`을 같이 씁니다.

```csharp
new ExperimentNoteSpec { Label = "롱 2개", Uid = "050201", NoteType = 3, Pathway = 0, IsLong = true, StartTick = 25.0, Count = 2, Interval = 4.0, Length = 2.0 },
```

롱노트는 내부적으로 start, middle, end 노트를 만듭니다. `LongMiddleStep = 0.1` 기준으로 중간 노트가 생성됩니다.

### 롱노트 내부 구조

롱노트는 한 줄짜리 노트가 아니라 여러 개의 `MusicData` 행으로 나뉩니다.

| 구분 | 조건/상태 | `tick` | `length` | `longPressPTick` | 플래그 |
| --- | --- | --- | --- | --- | --- |
| 롱 start | `type=3`, `length>0` | 시작 tick | 전체 길이 | 시작 tick | `isLongPressing=false`, `isLongPressEnd=false` |
| 롱 middle | 중간 조각 | 시작 tick + 0.1 * n | 0 | 시작 tick | `isLongPressing=true` |
| 롱 end | 끝 조각 | 시작 tick + length | 0 | 시작 tick | `isLongPressEnd=true` |

예를 들어 `StartTick=25.0`, `Length=2.0`이면 start는 25.0에 생기고, middle은 25.1, 25.2, 25.3처럼 생기며, end는 27.0에 생깁니다.

롱노트를 수정할 때는 start만 바꾸면 안 됩니다. middle과 end의 `tick`, `showTick`, `longPressPTick`, `objId`가 같이 맞아야 정상적으로 보입니다. 현재 실험 코드는 `IsLong=true`일 때 이 체인을 자동으로 만듭니다.

## 샌드백/type 8

샌드백처럼 길이를 가진 단일 노트는 `IsMul=true`를 씁니다.

```csharp
new ExperimentNoteSpec { Label = "샌드백 테스트", Uid = "020401", NoteType = 8, Pathway = 0, IsMul = true, StartTick = 32.0, Length = 1.2 },
```

### type 3 롱노트와 type 8 샌드백 차이

둘 다 `Length`를 쓰지만 구조가 다릅니다.

| 구분 | type 3 롱노트 | type 8 샌드백 |
| --- | --- | --- |
| 행 개수 | start + middle 여러 개 + end | 보통 단일 `MusicData` 행 |
| 중간 조각 | `isLongPressing=true` 행이 여러 개 있음 | 중간 행 없음 |
| 길이 표현 | start의 `length`와 middle/end 배치가 함께 필요 | 한 슬롯의 `length`로 멀티히트 구간 표현 |
| 실험 플래그 | `IsLong=true` | `IsMul=true` |
| 대표 UID | `050201` 계열 | `020401` 계열 |

샌드백을 롱노트처럼 middle/end 체인으로 만들면 게임이 기대하는 구조와 달라질 수 있습니다. 반대로 롱노트를 type 8처럼 한 줄만 만들면 롱노트 끝이나 판정이 어긋날 수 있습니다.

## 하트와 음표

하트:

```csharp
new ExperimentNoteSpec { Label = "하트", Uid = "000201", NoteType = 6, Pathway = 0, KeyAudio = "sfx_hp", StartTick = 35.0 },
```

음표:

```csharp
new ExperimentNoteSpec { Label = "음표", Uid = "000301", NoteType = 7, Pathway = 0, KeyAudio = "sfx_score", StartTick = 36.0 },
```

`NoteType=6` 또는 `Uid=0002xx`는 `sfx_hp`를 자동 적용합니다. `NoteType=7` 또는 `Uid=0003xx`는 `sfx_score`를 자동 적용합니다.

## 프리팹명 직접 지정

자동 프리팹명 생성이 맞지 않을 때는 `PrefabName`을 직접 지정합니다.

```csharp
new ExperimentNoteSpec { Label = "직접 프리팹", Uid = "051001", NoteType = 1, Pathway = 0, PrefabName = "051001_road_nor_1", StartTick = 15.0 },
```

`PrefabName`을 비우면 대략 아래 형태로 자동 생성됩니다.

```text
{uid}_{road/air}_{nor/up/down}_1
```

예:

```text
051001_road_nor_1
051004_air_nor_1
```

## 보스 액션 노트

보스 액션 트리거도 `ExperimentNotes`에서 만들 수 있습니다.

```csharp
new ExperimentNoteSpec { Label = "보스 등장", Uid = "050101", NoteType = 0, Pathway = 0, BossAction = "in", StartTick = 15.0 },
new ExperimentNoteSpec { Label = "보스 원거리1 시작", Uid = "050107", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_1_start", StartTick = 16.0 },
new ExperimentNoteSpec { Label = "보스 원거리1 후속", Uid = "050108", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_1_end", StartTick = 17.0 },
new ExperimentNoteSpec { Label = "보스 원거리2 시작", Uid = "050109", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_2_start", StartTick = 18.0 },
new ExperimentNoteSpec { Label = "보스 원거리2 후속", Uid = "050110", NoteType = 0, Pathway = 0, BossAction = "boss_far_atk_2_end", StartTick = 19.0 },
new ExperimentNoteSpec { Label = "보스 퇴장", Uid = "050102", NoteType = 0, Pathway = 0, BossAction = "out", StartTick = 22.0 },
```

`NoteType=0` 보스 액션은 보이는 노트가 아니라 빈 트리거입니다. 이 경우 `BossAction`이 있으면 `PrefabName`을 비워도 `empty_000`이 자동 적용됩니다.

보스 발사체 노트는 다릅니다. `xx=06/07/08`, `yy=01/04` 계열은 `NoteType=1`이며, `empty_000`이 아니라 일반 노트처럼 `{uid}_road_nor_1` 또는 `{uid}_air_nor_1` 프리팹을 사용합니다. `Dt`는 0.7 정도가 적당합니다. 보스 액션 없이 발사체 노트만 만들 때는 `BossAction`을 비우거나 생략하면 됩니다.

```csharp
new ExperimentNoteSpec { Label = "보스 발사체만 1개", Uid = "070601", NoteType = 1, Pathway = 0, StartTick = 20.0, BossAction = "" },
new ExperimentNoteSpec { Label = "보스 단타 노트 1개", Uid = "070601", NoteType = 1, Pathway = 0, StartTick = 20.0, PrefabName = "070601_road_nor_1", BossAction = "boss_far_atk_1_R", Dt = 0.7 },
new ExperimentNoteSpec { Label = "보스 단타 노트 1개", Uid = "070701", NoteType = 1, Pathway = 0, StartTick = 20.0, BossAction = "boss_far_atk_2", Dt = 0.7 },
```

확인된 보스 발사체 액션:

| UID 패턴 | 프리팹 예 | BossAction |
| --- | --- | --- |
| `**0601` | `070601_road_nor_1` | `boss_far_atk_1_R` |
| `**0604` | `070604_air_nor_1` | `boss_far_atk_1_L` |
| `**0701` | `070701_road_nor_1` | `boss_far_atk_2` |

## 씬 전환 노트

`0004xx` 계열은 씬 전환 노트입니다. 일반 노트처럼 UID와 프리팹만 맞추는 것으로는 부족하고, `ibms_id`가 `sceneInfo` 딕셔너리 키와 맞아야 `SceneChangeController.ChangeScene/ChangeNote`까지 호출됩니다.

성공한 예:

```csharp
new ExperimentNoteSpec { Label = "씬 변환 노트", Uid = "000401", NoteType = 9, Pathway = 0, StartTick = 20.0, PrefabName = "000401", BossAction = "0", Scene = "0", KeyAudio = "0", IbmsId = "1O" },
```

관찰된 매핑:

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

씬 전환 시 `Boss.SceneBossChange`도 같이 호출됩니다. 보스가 씬 전환 후 사라진다면 `Boss.SceneBossChange`의 인덱스를 강제로 바꾸고 있지 않은지 먼저 확인하세요. 현재는 안정성을 위해 `EnableSceneBossChangeRewrite = false`가 맞습니다.

## 로그 확인

대표 로그:

```text
실험 노트 추가: 지상 일반 노트 #1/1, objId=1, tick=15, dt=1.47, showTick=13.53, speed=5, uid=051001, type=1, pathway=0, scene=scene_05, prefab=051001_road_nor_1
실험 차트 적용 완료: 2개 노트 ([0] 원본 유지, 원본 index 1 복사 후 지정 노트로 변형)
```

로그는 찍히는데 화면에 안 보이면 아래를 봅니다.

- UID에 해당하는 리소스가 실제로 있는지
- `PrefabName`이 실제 프리팹명과 맞는지
- `Pathway`가 프리팹명과 맞는지
- `Scene`을 직접 지정했다면 `scene=scene_00`처럼 로그에 찍히는지
- 씬 전환 노트라면 `IbmsId`가 원본 매핑과 맞는지
- `showTick`이 너무 과거/미래로 밀리지 않았는지
- `Speed`가 너무 작거나 큰 값으로 들어가지 않았는지

## 주의

- `[0]` 슬롯은 안정성을 위해 그대로 둡니다.
- 실제 복사 베이스는 `SourceNoteIndex = 1`입니다.
- `SourceNoteIndex`를 `0`으로 바꾸면 더 원본 첫 노트에 가까워지지만 표시가 불안정할 수 있습니다.
- `Speed`는 게임 필드가 정수라 `12.7`을 넣으면 `13`처럼 반올림됩니다.
- `Dt`를 너무 크게 잡으면 `showTick`이 너무 앞당겨질 수 있습니다.
- UID와 프리팹 조합이 게임 리소스에 없으면 로그상 생성돼도 화면에 안 보일 수 있습니다.
- `Uid`만 바꿔서는 부족할 수 있습니다. 최소한 `NoteType`, `Pathway`, 필요하면 `PrefabName`, `KeyAudio`까지 같이 맞추는 편이 좋습니다.
