using System;
using MelonLoader;
using Il2Cpp;

namespace muse_dash_test
{
    /// <summary>
    /// zz0301 (기어 / 톱니바퀴 노트)의 Spine 슬롯 색상을 임의의 커스텀 색상(네온 라임 그린)으로 실시간 틴트 변조하는 패치입니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.PlayByKey))]
    public class GearNoteColorPatch
    {
        // 🎨 커스텀 틴트 RGB (네온 라임 그린: R=0.1, G=1.0, B=0.3)
        public static float customR = 0.1f;
        public static float customG = 1.0f;
        public static float customB = 0.3f;

        public static void Postfix(SpineActionController __instance)
        {
            try
            {
                if (__instance == null) return;

                var skeletonAnimation = __instance.skeletonAnimation;
                var skeleton = skeletonAnimation != null ? skeletonAnimation.skeleton : null;
                if (skeleton == null) return;

                string objName = __instance.gameObject != null ? __instance.gameObject.name : "";
                string skelName = skeleton.Data != null && !string.IsNullOrEmpty(skeleton.Data.name) ? skeleton.Data.name : objName;

                // zz0301 및 톱니바퀴 노트 계열 감지 (0301, 0901, 0902, gear 등)
                if (IsGearNote(objName, skelName))
                {
                    var slots = skeleton.Slots;
                    if (slots == null) return;

                    var items = slots.Items;
                    int count = slots.Count;
                    if (items == null || count == 0) return;
                    if (count > items.Length) count = items.Length;

                    for (int i = 0; i < count; i++)
                    {
                        var slot = items[i];
                        if (slot == null) continue;

                        // 그림자(shadow) 슬롯을 제외한 모든 부위에 커스텀 틴트 적용
                        string slotName = slot.data != null ? slot.data.name : "";
                        if (!string.Equals(slotName, "shadow", StringComparison.OrdinalIgnoreCase))
                        {
                            slot.r = customR;
                            slot.g = customG;
                            slot.b = customB;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[GearNoteColorPatch] 톱니바퀴 틴트 적용 중 예외 발생: {ex}");
            }
        }

        private static bool IsGearNote(string objName, string skelName)
        {
            if (objName.Contains("0301") || skelName.Contains("0301")) return true;
            if (objName.Contains("0901") || skelName.Contains("0901")) return true;
            if (objName.Contains("0902") || skelName.Contains("0902")) return true;
            if (objName.Contains("09_") || skelName.Contains("09_")) return true;
            if (objName.IndexOf("gear", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (skelName.IndexOf("gear", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }
    }
}
