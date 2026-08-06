# 콜라보(IP) DLC 종료일 판정 로직 Reconstruction

*Muse Dash*의 콜라보(IP) DLC/음악팩이 언제 종료되는지를 클라이언트 코드/데이터에서 실제로 찾아내고, 그 판정에 쓰이는 "지금" 시각이 어디서 오는지까지 역추적한 문서입니다. 아래 내용은 추측이 아니라 실제 게임을 실행해서 Harmony 로깅으로 **실측**한 결과입니다.

---

## 1. 콜라보 종료일이 저장되는 곳: `DlcUIExtensionInfo.dlcEndTime`

- `Il2CppAssets.Scripts.Database.DBConfigDlcUIExtension`이 게임 시작 시 내장 JSON을 `Deserialize(string)`해서 `List<DlcUIExtensionInfo>`(`list`/`m_Items`)를 채운다.
- 각 `DlcUIExtensionInfo` 항목은 `jsonIndex`(int, 콜라보 식별자)와 `dlcEndTime`(DateTime, 종료 시각)을 갖는다. 나머지 필드(`dlcCountdownPosition`, `dlcCountDownTxtPadding`, `dlcCountDownBaseImg` 등)는 카운트다운 UI 표시 위치/스타일용 설정값이다.
- 조회는 `DBConfigDlcUIExtension.GetDlcUIExtensionByJsonIndex(int jsonIndex)`로 한다.

**중요한 한계**: `DlcUIExtensionInfo`는 "모든 콜라보의 종료일 저장소"가 아니라 **"현재 카운트다운 UI를 띄워야 하는 콜라보"에만 등록되는 표**다. `AlbumJsonIndexDefine`에 정의된 47개 이름 중 실제로 이 표에 항목이 있는 건 11개뿐이었다(`djmax`, `wacca`, `arcaea`, `cytus` 등 다수는 항목 자체가 없음 — 기간 제한이 없거나, 이미 오래 전에 정리됐거나, 다른 경로로 관리되는 것으로 추정).

## 2. jsonIndex → 이름 매핑: `AlbumJsonIndexDefine`

`Il2CppPeroPeroGames.GlobalDefines.AlbumJsonIndexDefine`(struct)는 `cytus`, `djmax`, `ark_nights`처럼 이름이 붙은 `public static int` 필드들로 콜라보/앨범 jsonIndex 값을 정의해둔다. `DlcUIExtensionInfo.jsonIndex`엔 숫자만 있어서, 이 struct를 리플렉션(`GetProperties(BindingFlags.Public | BindingFlags.Static)`)으로 훑어 `jsonIndex -> 이름` 역매핑을 만들어야 사람이 읽을 수 있는 이름이 나온다.

## 3. "지금" 시각의 출처: `PeroServerTime`

`Il2CppAssets.Scripts.Common.PeroServerTime`(싱글톤)이 만료 판정에 쓰이는 기준 시각을 관리한다.

- `GetServerTime(Action<DateTime> callback, Action<long,string> failCallback, bool force)` / `RequireTime(...)` — 실제로 서버에 네트워크 요청을 보내 서버 시각(UTC)을 받아온다. PC 로컬 시계 조작으로 카운트다운을 속이지 못하게 하려는 설계로 보인다.
- `serverUtcTime` / `nowLocalServerTime` — 한 번 받아온 서버 시각 + 그 이후 경과 시간(`Stopwatch`)으로 "지금"을 근사 계산.
- **`ResetToLocal()`** — 서버 시각 확보 실패 시 로컬 PC 시계로 폴백하는 지점. **오프라인(Goldberg 에뮬레이터 등)에서는 서버 요청이 애초에 안 되므로 이 경로를 탈 가능성이 높고, 그 경우 만료 판정 기준이 PC 시스템 시계로 바뀔 수 있다.** (실제로 오프라인 상태에서 `ResetToLocal`이 호출되는지는 아직 실측 전 — 확인 필요 시 여기에 후킹을 다시 추가할 것.)

> ⚠️ 주의: `PeroServerTime.serverUtcTime`/`nowLocalServerTime` getter의 실제 반환 타입은 `Il2CppSystem.DateTime`이며(`System.DateTime`이 아님), Harmony로 `ref System.DateTime __result`로 후킹하면 **네이티브 크래시**가 발생함을 확인했다(예외 로그 없이 뚝 끊김). 이 두 getter를 후킹할 땐 반드시 `ref Il2CppSystem.DateTime __result`로 타입을 맞출 것.

## 4. 실제 만료 판정 함수: `TimeLimitedItemManager`

`Il2CppAssets.Scripts.GameCore.Managers.TimeLimitedItemManager` (static)가 "이 아이템이 아직 유효 기간 안이냐"를 최종 판정한다.

- **`IsItemInTime(string itemType, int itemIndex, bool needServerTime = false)`** → `bool` — 핵심 판정 함수. 내부적으로 `PeroServerTime`(private static `serverTime` 프로퍼티)을 사용.
- `CheckItemArgsInTime(TimeLimitedItemArgs, bool needServerTime)` — 실제 날짜 비교 로직(private).
- `TimeoutItemFiliter(string itemType, List<int> itemIndexs)` — 리스트에서 만료된 항목을 걸러내는 필터.
- `GetTimeOutItemArgs(List<TimeLimitedItemArgs> container)` — 만료된 항목만 뽑아냄.
- 판정 근거 데이터는 `Il2CppAssets.Scripts.Structs.TimeLimitedItemArgs`(`itemType`, `itemIndex`, `startTime`, `endTime`, `useOnSaleMaxVersion`).

**실측된 `itemType` 값들** (게임을 로딩~메인 화면까지 진행하며 관찰): `loading`, `welcome`, `character`, `elfin`. `collab`/`dlc` 계열 `itemType`은 **DLC/상점 패널을 직접 열어야만 호출**되는 것으로 보이며, 이번 조사에서는 그 화면까지 들어가지 않아 실측하지 못했다. 실제로 만료된 사례도 하나 잡았다: `itemType=welcome, itemIndex=38 -> False`.

---

## 5. 실측 결과: 콜라보 종료일 전체 목록 (측정일 2026-08-03)

`DlcUIExtensionInfo`에 실제로 항목이 있던 11개 (jsonIndex 오름차순):

| jsonIndex | 이름 (AlbumJsonIndexDefine) | dlcEndTime | 상태 (2026-08-03 기준) |
| :--- | :--- | :--- | :--- |
| 60 | ark_nights | 2026-01-26 15:59:00 | 이미 종료 |
| 61 | maimai_dx_limited_time_suite | 2026-01-31 15:00:00 | 이미 종료 |
| 66 | neon_abyss | 2026-08-17 15:59:00 | **약 2주 뒤 종료** |
| 67 | miku_in_museland | 2024-09-27 15:59:00 | 이미 종료 |
| 71 | rin_len_s_mirrorland | 2024-09-27 15:59:00 | 이미 종료 |
| 75 | chunithm_class_muse | 2027-05-22 15:00:00 | 아직 많이 남음 |
| 82 | md_level_tactical_training_blu_ray | 2025-12-27 15:59:00 | 이미 종료 |
| 83 | ark_nights_2 | 2026-01-26 15:59:00 | 이미 종료 |
| 85 | fool_day_2025 | 0001-01-01 (미설정) | 영구(종료일 없음) |
| 94 | (AlbumJsonIndexDefine에 이름 매칭 안 됨) | 0001-01-01 (미설정) | 영구(종료일 없음) |
| 96 | love_horse | 0001-01-01 (미설정) | 영구(종료일 없음) |

`AlbumJsonIndexDefine`에 이름은 있지만 `DlcUIExtensionInfo` 항목이 아예 없는 것들(총 47개 중 나머지, 일부만 예시): `djmax`(49), `wacca`(46), `arcaea`(48), `cytus`(34), `TouhouBag`(43), `ark_nights`/`ark_nights_2`를 제외한 대부분의 이벤트/패키지성 항목. 음수 jsonIndex(`collab`=-40, `vip`=-30, `muse_plus`=-10, `week_free`=-2, `unlock_all`=-1, `no_found`=-100)는 카테고리 상수로 보이며 개별 콜라보가 아니다.

---

## 6. 판정 흐름 요약 (추정)

```
DBConfigDlcUIExtension.Deserialize(json)
    -> List<DlcUIExtensionInfo> 로드 (jsonIndex, dlcEndTime, UI 표시 설정)

(플레이어가 DLC/상점 화면 진입 또는 콘텐츠 접근 시도)
    -> TimeLimitedItemManager.IsItemInTime("collab" 등, jsonIndex, needServerTime=true)
        -> CheckItemArgsInTime(TimeLimitedItemArgs{ itemType, itemIndex, startTime, endTime }, needServerTime)
            -> PeroServerTime.nowLocalServerTime (또는 serverUtcTime) 와 endTime 비교
                -> PeroServerTime.RequireTime()으로 서버 시각 요청
                    -> 실패 시 ResetToLocal() 폴백 -> PC 로컬 시계 사용 가능성
        -> false 반환 시 해당 콜라보 콘텐츠/구매 버튼 비활성화 또는 목록에서 제외(TimeoutItemFiliter)
```

---

## 7. 조사 범위에서 제외된 것

같은 조사 세션에서 "고스트(Hide=4) 노트가 페이드 아웃되는 로직"도 함께 찾아봤으나, 다음 이유로 **구현을 포기**했다(문서화하지 않음, 관련 코드도 전부 롤백/삭제됨):
- `BaseEnemyObjectController.OnUpdate()`, `SpineActionController.SetAlpha(float)` 모두 실제로는 호출되지 않는 죽은 경로였음.
- 알파 페이드가 Spine 애니메이션 클립 자체에 커브로 베이크되어 있는 것으로 추정되어, C# 메서드 후킹만으로는 개입 지점을 찾지 못함.
- `skeleton.A` 강제 고정(Update/LateUpdate 양쪽 시도)도 효과 없었음.

---

## 8. 참고: 관련 진단 패치

- [CollabEndTimeDumpPatch.cs](file:///h:/source/repos/muse%20dash%20test/muse%20dash%20test/Patches/Diagnostics/CollabEndTimeDumpPatch.cs) — `DBConfigDlcUIExtension.Deserialize` Postfix에서 위 5번 표를 그대로 로그로 덤프함(이름 매칭 포함, `AlbumJsonIndexDefine` 전체 역조회 포함). 현재 프로젝트에 남아있는 유일한 진단 패치.
