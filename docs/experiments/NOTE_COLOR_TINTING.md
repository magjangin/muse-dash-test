# 🎨 Muse Dash 노트 색조(Tint) 변조 가능성 및 실측 명세서

> [!NOTE]
> 런타임 진단 패치([`NoteColorDiagnosticsPatch.cs`](../../muse%20dash%20test/Patches/Diagnostics/NoteColorDiagnosticsPatch.cs))를 통해 *Muse Dash* 인게임 배틀 씬의 모든 노트 Spine 스켈레톤, 슬롯 RGBA 및 애니메이션 타임라인을 100% 실측 분석한 색조 변조 기술 명세서입니다.

---

## 1. 🔍 실측 검증 결과 (Executive Summary)

1. **ColorTimeline 부재 입증 (가장 결정적 발견)**:
   * 고스트 노트(`Type 4`, `xx=17`)를 제외한 **모든 일반 소형/대형/샌드백/연타 노트의 애니메이션(`in_up_26`, `in_down_26`, `in_nor_38`, `out_p`, `hurt_1` 등)에는 색상/알파를 강제로 덮어쓰는 `ColorTimeline`이 단 1개도 존재하지 않습니다.**
   * **의미**: C# 코드에서 Spine `Slot.R, G, B, A` (또는 `Slot.SetColor`)를 1회만 변경해 주면, 애니메이션 프레임에 의해 덮어씌워지지 않고 **100% 지속적인 실시간 색조(Tint) 변조가 완벽하게 적용**됩니다.

2. **순정 슬롯 틴트 상태**:
   * 모든 노트 슬롯의 초기 RGBA 값은 `(1.00, 1.00, 1.00, 1.00)` (Pure White) 상태로 들어옵니다.
   * Spine 곱셈 틴트(Multiply Tint) 공식이 100% 깨끗하게 정투영됩니다.

---

## 2. 📐 곱셈 틴트(Multiply Tint) 기본 원리와 한계

Spine 렌더링 파이프라인의 최종 픽셀 색상은 아래 연산을 따릅니다:

$$\text{FinalColor}_{RGB} = \text{TexturePixel}_{RGB} \times \text{SlotColor}_{RGB} \times \text{TimelineColor}_{RGB}$$

### ⚠️ 물리적 제약 사항 (Physical Constraints)
1. **채널 상한선 제약**: 곱셈 연산이므로 **원본 텍스처 픽셀의 RGB 채널 수치를 초과하거나 0인 채널을 살려낼 수 없습니다.**
2. **발광/밝기 증가 불가**: 원본 슬롯 색상이 `1.0` 순백색이므로, 틴트를 주면 색상이 어두워지거나 특정 채널로 이동할 뿐 원본보다 더 밝게 하거나(발광) 흰색으로 탈색시킬 수 없습니다.
3. **채널 억제 연산**: 원본 픽셀의 특정 채널(예: Green)이 20% 이하라면, 틴트 색상에서 Green을 아무리 `1.0`으로 주어도 실제 투영되는 Green은 20%를 넘지 못합니다.

---

## 3. 📊 노트 타입별 틴트 변조 매트릭스 (Tint Matrix)

| 노트 타입 | UID / Type | 원본 Base 색상 | 틴트 자유도 | 변조 가능한 대표 색상 | 변조 불가능한 색상 |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **톱니바퀴 (Gear)** | `xx=09` / Type 2 | 회색조 (`#A0A0A0`) | **⭐ 최상 (100%)** | **빨강, 초록, 파랑, 노랑, 보라, 민트 등 전색상** | 흰색보다 밝은 발광 |
| **샌드백 (Sandbag)** | `xx=04` / Type 8 | 주황/노랑 (`#FFE040`) | **상 (80%)** | **노랑, 주황, 빨강, 연두, 앰버** | 파랑, 남색, 보라 |
| **음표 (Score Note)** | `xx=03` / Type 7 | 밝은 노랑 (`#FFF060`) | **상 (80%)** | **노랑, 주황, 연두, 초록, 핫핑크** | 파랑, 남색 |
| **고스트 (Ghost)** | `xx=17` / Type 4 | 자홍/보라 (`#FF3EB9`) | **중 (50%)** | **빨강, 자홍, 보라, 남색** | 초록, 노랑, 흰색, 파스텔 |
| **일반 소형/대형** | `xx=01,04..` / Type 1 | 핑크 / 파랑 (몬스터별) | **중 (50%)** | 핑크 기반: 빨강/보라<br>블루 기반: 파랑/민트 | 핑크에서 초록/노랑<br>블루에서 빨강/주황 |
| **하트 (Heart)** | `xx=02` / Type 6 | 핫핑크 (`#FF4080`) | **중 (50%)** | **빨강, 자홍, 보라** | 초록, 노랑, 청록 |
| **롱 노트 (Hold)** | `xx=02..` / Type 3 | 핑크/블루 띠 | **중하 (40%)** | 핑크 기반: 빨강/보라 | 초록, 노랑 (부위별 색 이질감) |

---

## 🔍 노트 종별 정밀 기술 분석

### 1. 톱니바퀴 / 기어 노트 (`Type 2`, `xx=09`) — **틴트 변조 최강**
* **실측 구조**: 슬롯 15개 전체 순정 RGBA `(1,1,1,1)`.
* **원리**: 텍스처 픽셀이 무채색 회색조(R≒G≒B≒75%)로 구성되어 있어 3개 채널의 여유 수치가 균등함.
* **결과**: **빨강, 초록, 파랑, 노랑, 보라, 민트 등 모든 원색이 100% 원형 그대로 투영**되는 최고의 틴트 대상.

### 2. 샌드백 / 연타 노트 (`Type 8`, `xx=04`) & 음표 (`Type 7`, `xx=03`)
* **실측 구조**: R(100%)과 G(85%) 채널이 매우 높음.
* **결과**: **노랑 ↔ 주황 ↔ 빨강 ↔ 연두 ↔ 초록** 범위 내에서 완벽한 색조 변화 지원. (B 채널이 낮아 남색/보라 적용 시 어둡게 죽음).

### 3. 고스트 노트 (`Type 4`, `xx=17`)
* **실측 구조**: `in_nor_38` 애니메이션 내 **8개의 `ColorTimeline` 존재** (투명도 페이드 내장).
* **원리**: R(100%), B(73%), G(24%).
* **결과**: **빨강 ↔ 자홍 ↔ 보라 ↔ 남색** 지원. G 채널 상한이 24%라 초록/노란 유령은 틴트로 불가능 (몸통이 칙칙한 검은보라색으로 죽음).
* **특이사항**: 애니메이션 `ColorTimeline` 알파 키 덮어쓰기 로직([`GHOST_NOTE_ALPHA_HOLD.md`](./GHOST_NOTE_ALPHA_HOLD.md))과 병행 필요.

### 4. 롱 노트 (`Type 3`)
* **주의사항**: Head(머리), Body(몸통 띠), Tail(꼬리) 영역이 분리되어 있으므로, 3개 영역에 동일한 틴트 RGB를 동시에 주입해야 이질감이 발생하지 않음.

---

## 🛠️ 커스텀 방식 선택 가이드

| 구분 | Spine 틴트 (C# 코드 기반) | CustomSkinInjector (에셋 텍스처 스왑) |
| :--- | :--- | :--- |
| **구현 방식** | `Slot.R, G, B` 수치 조작 | PNG/Atlas 에셋 교체 |
| **메모리 / 로딩** | ⚡ 0ms (실시간 코드 조작) | 에셋 로딩 필요 |
| **색상 자유도** | ⚠️ 원본 RGB 상한선 내 제한 | 🎨 100% 완전한 자유 (초록 고스트, 흰색, 야광 등) |
| **추천 케이스** | 톱니바퀴(모든색), 샌드백, 보라계열 고스트 | 초록/노란 고스트 노트, 완전 흰색/파스텔 노트 |

---

## 💻 4계층 전신 틴트 덮어쓰기 C# 레퍼런스 소스 코드

이 코드는 실측 검증을 통해 완성된 **4계층 전신 틴트 덮어쓰기(Spine Slot + SlotData + Attachments + Unity Renderers) 및 `zz03yy` (단, `zz != 00`) 핀포인트 조건 필터링 핵심 참조 로직**입니다. 나중에 다시 틴트 기능을 구현하거나 수정할 때 그대로 활용할 수 있습니다.

```csharp
using System;
using System.Collections.Generic;
using MelonLoader;
using Il2Cpp;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// Spine 슬롯, SlotData, Attachments 및 Unity Component Renderers까지 
    /// 4계층 100% 전신 틴트 덮어쓰기를 수행하는 참조용 로직입니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.PlayByKey))]
    public class NoteColorTintReferencePatch
    {
        // 🎨 커스텀 틴트 RGB (네온 라임 그린: R=0.1, G=1.0, B=0.3)
        public static float customR = 0.1f;
        public static float customG = 1.0f;
        public static float customB = 0.3f;

        public static void Postfix(SpineActionController __instance, string actionKey)
        {
            try
            {
                if (__instance == null) return;

                string objName = __instance.gameObject != null ? __instance.gameObject.name : "";
                if (string.IsNullOrEmpty(objName)) return;

                // 캐릭터 / 플레이어 / 이펙트 제외
                if (objName.Contains("girl") || objName.Contains("char") || objName.Contains("player") || objName.Contains("Elfin")) return;

                var skeletonAnimation = __instance.skeletonAnimation;
                var skeleton = skeletonAnimation != null ? skeletonAnimation.skeleton : null;
                if (skeleton == null) return;

                string skelName = skeleton.Data != null && !string.IsNullOrEmpty(skeleton.Data.name) ? skeleton.Data.name : objName;

                // 🎯 zz03yy (xx=03 & zz != 00) 타겟 조건 필터링
                if (!IsZz03Yy(objName) && !IsZz03Yy(skelName)) return;

                // -------------------------------------------------------------
                // 1계층: Spine 스켈레톤, 런타임 슬롯, SetupPose SlotData & Attachments
                // -------------------------------------------------------------
                var slots = skeleton.Slots;
                if (slots != null && slots.Items != null)
                {
                    int count = Math.Min(slots.Count, slots.Items.Length);
                    for (int i = 0; i < count; i++)
                    {
                        var slot = slots.Items[i];
                        if (slot == null) continue;

                        string slotName = slot.data != null ? slot.data.name : "";
                        if (string.Equals(slotName, "shadow", StringComparison.OrdinalIgnoreCase)) continue;

                        // 1-1. Runtime Slot Color
                        slot.r = customR;
                        slot.g = customG;
                        slot.b = customB;

                        // 1-2. SetupPose SlotData Color (포즈 초기화 시 되돌아감 방지)
                        if (slot.data != null)
                        {
                            slot.data.r = customR;
                            slot.data.g = customG;
                            slot.data.b = customB;
                        }

                        // 1-3. Region / Mesh Attachment Color (부위별 틴트 100% 덮어쓰기)
                        var attachment = slot.Attachment;
                        if (attachment != null)
                        {
                            var region = attachment.TryCast<Il2CppSpine.RegionAttachment>();
                            if (region != null)
                            {
                                region.r = customR;
                                region.g = customG;
                                region.b = customB;
                            }

                            var mesh = attachment.TryCast<Il2CppSpine.MeshAttachment>();
                            if (mesh != null)
                            {
                                mesh.r = customR;
                                mesh.g = customG;
                                mesh.b = customB;
                            }
                        }
                    }
                }

                // -------------------------------------------------------------
                // 2계층: Unity 하위 SpriteRenderer / MeshRenderer (롱노트 몸통 띠 메쉬 등 커버)
                // -------------------------------------------------------------
                if (__instance.gameObject != null)
                {
                    var spriteRenderers = __instance.gameObject.GetComponentsInChildren<SpriteRenderer>(true);
                    if (spriteRenderers != null)
                    {
                        foreach (var sr in spriteRenderers)
                        {
                            if (sr != null)
                            {
                                Color old = sr.color;
                                sr.color = new Color(customR, customG, customB, old.a);
                            }
                        }
                    }

                    var meshRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>(true);
                    if (meshRenderers != null)
                    {
                        foreach (var mr in meshRenderers)
                        {
                            if (mr != null && mr.material != null)
                            {
                                try
                                {
                                    if (mr.material.HasProperty("_Color"))
                                    {
                                        Color old = mr.material.color;
                                        mr.material.color = new Color(customR, customG, customB, old.a);
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[NoteColorTint] 틴트 처리 중 예외 발생: {ex}");
            }
        }

        /// <summary>
        /// zz03yy 패턴 필터: xx="03" 이고 zz != "00" 인 노트 매칭 (예: 070301, 020301 등)
        /// </summary>
        private static bool IsZz03Yy(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;

            // 1. "0003..." 등 zz="00" 인 명칭 제외
            if (name.StartsWith("0003")) return false;

            // 2. "zz03yy" 패턴 검사 (6자리 이상 UID 명칭에서 xx=03 위치 검사)
            if (name.Length >= 4 && char.IsDigit(name[0]) && char.IsDigit(name[1]) && name[2] == '0' && name[3] == '3')
            {
                // zz가 "00"이면 제외 (zz != "00")
                if (name[0] == '0' && name[1] == '0') return false;

                return true;
            }

            // 3. 명시적 키워드 "0301", "0304" 등 포함되지만 zz="00"이 아닌 경우
            if (name.Contains("0301") || name.Contains("0304") || name.Contains("0302") || name.Contains("0305"))
            {
                if (!name.Contains("0003")) return true;
            }

            return false;
        }
    }
}
```
