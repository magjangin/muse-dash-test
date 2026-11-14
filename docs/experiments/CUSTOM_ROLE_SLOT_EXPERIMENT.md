# 커스텀 캐릭터 슬롯 & 복합 스킬 실험 기록 (2026-11-14)

`PnlRole`(캐릭터 선택 화면)에 기존 캐릭터(기타 린 `Bassist`)를 복제한 커스텀 슬롯을 추가하고, 체력 `999` + `GREAT/MISS → PERFECT` 999회 카운터 배지 + 유메 톱니바퀴 부수기 스킬을 결합해 인게임 동작을 검증한 실험입니다.  
실기 확인 후 모드 코드는 실험 전 상태로 되돌렸고, 실험 중 확인한 구조·주의사항을 이 문서에 남깁니다.

## 1. 목적

- `DBConfigCharacter` / `DBConfigLocalCharacter` / `PnlRole` 복제 방식으로 새 캐릭터 슬롯을 안전하게 끼워 넣고 해금·선택·배틀 진입까지 동작하는지 확인
- 복제 슬롯 전용으로 3가지 효과를 동시에 바인딩했을 때 충돌 없이 동작하는지 확인
  1. 최대 체력 `999` (`BattleProperty.maxHp = 999`)
  2. `GREAT → PERFECT` 999회, `MISS → PERFECT` 999회 (`SkillCounterPatch`) 및 좌상단 `SkillIconGrid` 배지 표시 (`SkillCounterBadge`)
  3. 유메(Yume) 톱니바퀴 부수기 (`YumeGearSmashPatch`)

## 2. 실험 구성 (브랜치 `feature/custom-role-slot-experiment`)

- **캐릭터 슬롯 복제 (`CustomRoleSlotExperiment.cs`)**
  - `DBConfigCharacter.m_Items`에서 기타 린(`Bassist`)을 복제해 `ClonedRoleIndex = 37`, `ClonedRoleOrder = 38`로 추가
  - `DBConfigLocalCharacter.m_Items[37]`에 `[실험] 기타 린 (복제)` 현지화 행 주입
  - `PnlRoleSkinContainer.m_SelectedSkinIndexs` 길이 확장 및 `RefreshButtonState` 해금(`BtnApply.ButtonState.Apply` / `InUse`, `hasGotCharacter = true`)
  - `BattleProperty` 초기화 시 `IsClonedRoleActive()`이면 `maxHp = 999` 적용
- **999 카운터 & 좌상단 배지 (`SkillCounterPatch.cs`, `SkillCounterBadge.cs`)**
  - `BattleProperty.greatToPerfect = 999`, `missToGreat = 999` 설정 및 `Il2CppGameLogic.GameTouchPlay.TouchResult`에서 차감·`PERFECT` 전환
  - `Object.FindObjectOfType<SkillIconGridShowControl>()`로 `SkillIconGrid` 탐색 후 `ImgRampageSkill`, `ImgCarrotRobotSkill` 활성화 유지
- **유메 톱니바퀴 부수기 (`YumeGearSmashPatch.cs`)**
  - `IsClonedRoleActive()` 조건으로 `isGcCharacter = true`, `MissCube` 차단 우회, `YumeSkill` 이펙트·일반 통과 점수 적용

## 3. 실측 결과 및 핵심 발견

- **슬롯 복제 및 전투 진입**: `PnlRole`에 `[실험] 기타 린 (복제)` 슬롯이 정상 노출·해금·선택되었고, 인게임에서 `rock_girl_battle(Clone)` 스폰 및 하단 체력바 `999/999` 표시를 확인했습니다.
- **IL2CPP / 바인딩 주의점**:
  - `ButtonState`는 `BtnApply` 중첩 타입(`BtnApply.ButtonState`)입니다.
  - `GameTouchPlay` Harmony 타깃은 `GameBindings.GameTouchPlay`가 아니라 `Il2CppGameLogic.GameTouchPlay.TouchResult`입니다.
- **좌상단 스킬 배지(`SkillIconGrid`) 실측 발견**:
  - `SkillIconGridShowControl.Awake()`(ISIL 확인)는 `selectedElfinIndex == 9`일 때만 `SkillIconGrid`를 숨기지만, 개별 아이콘(`ImgRampageSkill`, `ImgCarrotRobotSkill`)은 `PnlBattle` 초기화 과정에서 다시 비활성화(`active=False`)됩니다. 첫 프레임에 `shownGreat`/`shownMiss`를 캐시하고 조기 반환하면 아이콘이 꺼진 채로 남으므로, `icon.gameObject.SetActive(true)` 유지는 횟수 변화와 분리해야 합니다.
  - 아이콘 하단의 기본 `1` 표기는 본체 스프라이트가 아니라 **`Img1`~`Img5` 자식 오브젝트**(`ChangeGreat1`, `ChangeMiss1`)입니다. 따라서 배경 박스로 덮을 필요 없이 `Img1`~`Img5`를 `SetActive(false)`로 숨기고, `Img1`의 `RectTransform` 자리에 `999` 텍스트만 올리면 게임 UI와 자연스럽게 맞습니다.
  - HUD 시그니처 폰트(`LuckiestGuy-Regular_150_115`)는 첫 노트 판정 시점(`VictoryDataCache.PremiumFont`)에 캐시되므로, 배지 갱신 경로에서도 폰트를 재확인해 적용해야 `Arial`로 남지 않습니다.
- **초기화 타이밍**: 게임 실행 직후 `PnlRole`을 열지 않고 바로 곡에 진입한 경우 `EnsureClonedRoleInjected`가 돌지 않아 `ClonedRoleIndex = -1` 상태가 되고 원본 기타 린으로 진입했습니다. 정식 기능으로 넣을 때는 게임 시작/DB 로드 시점에도 자동 주입되도록 연결해야 합니다.

## 4. 정리 상태

- 실험 코드는 되돌려 `main`을 실험 전 상태로 복구했습니다.
- 임시 실험 브랜치(`feature/custom-role-slot-experiment`, `feature/skill-counter-999`, `feature/yume-gear-smash`)는 삭제 정리했습니다.
