# Muse Dash Discord Rich Presence (리치 프레젠스) 모딩 & 통합 가이드

이 문서는 *Muse Dash* 커스텀 차트 모드에서 디스코드 리치 프레젠스(Discord Rich Presence)를 성공적으로 연동하고, 게임 내부 C++ Native 동작 한계를 후킹으로 완벽하게 해결한 기술 명세 및 노하우를 기록합니다.

---

## 1. 개요 및 모딩 전략의 전환

### ❌ 기존 P/Invoke (외부 discord-rpc.dll) 방식의 한계
- 처음에는 외부 `discord-rpc.dll` P/Invoke 연동 방식을 시도했으나, 유저의 게임 실행 환경에 외부 DLL이 존재하지 않을 경우 `DllNotFoundException`이 발생하며 비활성화되는 한계가 있었습니다.

### ✅ 게임 내장 `Il2Cpp.DiscordManager` 후킹 방식 (최종 채택)
- 뮤즈대시는 자체적으로 디스코드 SDK를 내장하고 있으며, `Il2Cpp.DiscordManager` 싱글톤 클래스를 통해 디스코드 클라이언트와 통신합니다.
- 외부 DLL 의존성 없이 게임 원본 내장 `DiscordManager`의 핵심 메서드를 Harmony Prefix로 가로챔으로써, 디스코드 프로필 상태를 100% 실시간 제어할 수 있게 되었습니다.

---

## 2. 핵심 메서드 분석 및 Native C++ 동작 특성

### 🔑 결정적 핵심 메서드
```csharp
Il2Cpp.DiscordManager.SetUpdateActivity(bool isPlaying, string levelInfo)
```

### ⚠️ Native C++ 덮어쓰기 메커니즘 및 우회 기법

1. **`isPlaying = false`일 때의 특성**:
   - 게임 원본의 Native C++ 엔진 내부 로직은 `levelInfo` 매개변수로 무엇이 전달되든 이를 무시하고, 디스코드 SDK `Activity.State`를 강제로 **`"In Menu"`** 텍스트로 덮어써서 전송합니다.
   - 이로 인해 곡 목록/곡 선택 패널에서 곡을 선택하고 멈춰있을 때 디스코드 프로필이 계속 **"In Menu"**로 노출되는 현상이 발생했습니다.

2. **`isPlaying = true`일 때의 특성**:
   - 게임 엔진이 `levelInfo` 매개변수 문자열을 가공 없이 디스코드 SDK로 전달합니다.

3. **해결책 (Prefix Intercept & Force `isPlaying = true`)**:
   - Harmony Prefix에서 `ref bool isPlaying` 및 `ref string levelInfo` 매개변수를 참조로 받습니다.
   - 커스텀 곡 선택 시 `levelInfo = $"{title} - {artist} (곡 선택 중)"`으로 덮어쓰고, **`isPlaying = true`로 강제 스위칭**하여 C++ 레벨의 `"In Menu"` 덮어쓰기를 원천 차단합니다.

---

## 3. 화면 패널별 상태 제어 로직 (`DiscordManagerDebugPatch.cs`)

### 📱 패널 컨텍스트 판단 함수
- **`IsStageSelectionContextActive()`**: `PnlStage`(곡 선택 패널) 또는 `PnlPreparation`(곡 준비 패널)의 `activeInHierarchy` 여부를 확인.
- **`IsInBattleStageContext()`**: `PnlBattle`(배틀 패널)의 `activeInHierarchy` 여부를 확인.

### 🔄 상태 전환 마트릭스

| 화면 컨텍스트 | `isSelectionActive` | `isInBattle` | `levelInfo` 변조 | `isPlaying` 강제 | 디스코드 최종 표기 |
| :--- | :---: | :---: | :--- | :---: | :--- |
| **패널 홈 (메인메뉴/설정/캐릭터)** | `False` | `False` | `"In Menu"` 명시적 대입 | `False` | `In Menu` (게임 원본 복원) |
| **곡 목록 / 곡 선택 패널** | `True` | `False` | `{title} - {artist} (곡 선택 중)` | `True` | `곡명 - 아티스트 (곡 선택 중)` |
| **인게임 배틀 중** | `False/True` | `True` | `{title} - {artist} (플레이 중)` | `True` | `곡명 - 아티스트 (플레이 중)` |

---

## 4. 소스 코드 참조 연동 구조

- **[DiscordManagerDebugPatch.cs](file:///H:/source/repos/muse%20dash%20test/muse%20dash%20test/Patches/Diagnostics/DiscordManagerDebugPatch.cs)**:
  - `DiscordManager.SetUpdateActivity` Prefix/Postfix 훅 및 패널 상태 가드 구현
- **[DiscordPresenceManager.cs](file:///H:/source/repos/muse%20dash%20test/muse%20dash%20test/Integration/DiscordPresenceManager.cs)**:
  - UID 기반 곡 제목/아티스트 정보 해독 헬퍼 (`ResolveSongDetails`)
- **[CustomPlaySession.cs](file:///H:/source/repos/muse%20dash%20test/muse%20dash%20test/Core/CustomPlaySession.cs)**:
  - 곡 탐색 및 선택 이벤트 시 `DiscordManager.instance.SetUpdateActivity` 즉시 유도 호출
- **[HwaMenuBgmController.cs](file:///H:/source/repos/muse%20dash%20test/muse%20dash%20test/Patches/Hwa/HwaMenuBgmController.cs)**:
  - 곡 선택 패널 이탈(`StopCustomMenuBgm`) 시 원본 `DiscordManager.SetUpdateActivity(false, "In Menu")` 직접 호출로 빠른 갱신 보장

---

*최종 작성일: 2026-07-25*  
*작성자: 화영왕 (Hwa-young-wang) & Antigravity AI*
