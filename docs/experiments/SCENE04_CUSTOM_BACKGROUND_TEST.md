# scene_04 배경 스프라이트 이름 매칭 교체 실험 (실측, 2026-11-14)

scene_04 배경의 스프라이트를 **이름이 같은 PNG**로 바꿔도 게임에서 정상으로 보이는지 확인한 실험입니다.
결과는 성공이었고, 실험 코드는 실측 후 되돌렸습니다(커밋 기록은 브랜치 `feature/scene04-custom-background`).

## 1. 목적

- 커스텀 배경의 가장 단순한 방식(A안, 스프라이트 이름 → PNG 교체)이 실제 게임에서 동작하는지 확인
- 원본 PPU·pivot을 따라 월드 크기와 기준점이 유지되는지 확인
- 씬 루트를 찾는 방법(`Resources.FindObjectsOfTypeAll<SceneSubControl>()`)이 실제로 잡히는지 확인

## 2. 방법

- 대상 씬: 공식 곡의 `scene_04`(커스텀 곡은 제외, `ShouldApplyExperimentChart == false`일 때만 적용)
- 시점: `GameMusicScene.InitSceneObject(name, isActive)` Postfix
- 입력 폴더: `{게임 루트}\skin test\custom_background\`
  - 파일 이름은 원본 스프라이트 이름과 같아야 합니다. 예: `s04_building_b1.png`
  - 들어 있던 파일: `s04_building_a1`, `a2`, `a3`, `b1`, `c1`, `c2`, `c3` (PNG 7개, 가로 120~167px, 세로 163~775px)
- 교체 규칙
  - 원본 스프라이트와 같은 이름의 PNG가 있으면 교체합니다. 없으면 원본을 둡니다.
  - PPU = 원본 PPU × 새 폭 ÷ 원본 폭. 그래서 가로 폭(월드 크기)은 원본과 같고, 세로는 PNG 비율을 따릅니다.
  - pivot = 원본 pivot ÷ 원본 rect 크기(정규화).
  - 같은 PNG는 한 번만 읽어 캐시합니다.
- 확인: 로그 한 줄과 화면 육안 확인

## 3. 실측 결과

로그(`MelonLoader/Latest.log`, 13:28:01):

```
[SceneBackgroundSwap] scene_04: 씬 루트 2개, PNG로 교체 332개 (SpriteRenderer 전체 660개):
  s04_building_b1x128, s04_building_a2x24, s04_building_a1x72,
  s04_building_c1x28, s04_building_c2x56, s04_building_c3x24
```

| 항목 | 값 |
| --- | --- |
| 씬 루트 개수 | 2 (이번 로드의 루트와 이전에 남은 루트로 추정, 로드 횟수 확인은 안 함) |
| SpriteRenderer 전체 | 660 (루트 2개 × 330) |
| PNG로 교체 | 332 (루트 2개 × 166) |
| 루트 1개당 교체 수 | b1 64, a1 36, c2 28, c1 14, c3 12, a2 12 |
| `[ERROR]` 줄 | 0 |

- 루트 1개당 교체 수는 씬 조사 문서(`CUSTOM_BACKGROUND_OTHER_SCENES.md`)의 스프라이트 개수와 맞습니다. `s04_building_b1` 64, `a1` 36, `c2` 28, `c1` 14, `c3` 12, `a2` 12.
- 실행 전후 시간은 측정하지 않았습니다.
- 육안 확인: 교체된 건물이 제자리에 원래 크기로 보였습니다. 사용자 확인으로 "제대로 입혀짐".

## 4. 결론

- A안(이름 매칭 PNG 교체)은 scene_04에서 동작합니다.
- 씬 루트는 `FindObjectsOfTypeAll<SceneSubControl>()` + 이름 비교로 잡힙니다. 이전 로드의 루트도 함께 잡히지만, 이미 바꾼 스프라이트는 이름이 비어 있어 두 번 바뀌지 않습니다.
- PNG가 원본보다 작아도 월드 크기는 PPU 보정으로 유지됩니다.

## 5. 실패·우회 기록 (같은 실험 중)

- **숨김(`enabled = false`)**: 로그상 성공했지만 원하는 결과가 아니었습니다.
- **틴트(`color`)·외곽선(뒤에 키운 자식)**: 동작은 했지만 시각 확인 단계에서 되돌렸습니다.
- **이름 부분 일치 `building`**: 고양이 실루엣 `s04_building_b4`가 걸렸습니다. 게임 이름이 `building`이어도 실제 그림이 건물이 아닐 수 있습니다. 이름 부분 일치는 오탐을 부릅니다. 정확한 이름 목록을 쓰는 쪽이 안전합니다.

## 6. 한계와 다음 단계

- 공식 곡에만 적용했습니다. 커스텀 곡(`info.txt` 옵션)으로 켜는 방식은 아직 없습니다.
- PNG 해상도가 원본보다 낮으면 선명도가 떨어질 수 있습니다. 그림 해상도 기준은 정하지 않았습니다.
- 씬 루트가 여러 개 남는 구조라서, 로드를 반복하면 오래된 루트도 매번 훑습니다. 루트 수가 늘면 비용이 커질 수 있습니다.
- scene_04 `_christmas` 판, 다른 씬(01~12)은 확인하지 않았습니다.

## 관련 파일

- 실험 코드: 커밋 `e8af072`(브랜치 `feature/scene04-custom-background`)에 있고, 이후 되돌림 커밋으로 `main`에는 없습니다.
- 씬 조사: [CUSTOM_BACKGROUND_OTHER_SCENES.md](CUSTOM_BACKGROUND_OTHER_SCENES.md)
- 씬 구조: [CUSTOM_BACKGROUND_FEASIBILITY.md](CUSTOM_BACKGROUND_FEASIBILITY.md)
