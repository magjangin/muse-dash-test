using System;
using System.Collections.Generic;
using MelonLoader;
using Il2Cpp;

namespace muse_dash_test
{
    /// <summary>
    /// zz0301 / 톱니바퀴 노트의 Spine 슬롯 및 SlotData 색상을 
    /// 선명한 네온 라임 그린(Lime Green)으로 실시간 틴트 변조하는 패치입니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.PlayByKey))]
    public class GearNoteColorPatch
    {
        // 🎨 커스텀 틴트 RGB (네온 라임 그린: R=0.1, G=1.0, B=0.3)
        public static float customR = 0.1f;
        public static float customG = 1.0f;
        public static float customB = 0.3f;

        private static readonly HashSet<string> tintedObjects = new HashSet<string>();

        public static void Postfix(SpineActionController __instance, string actionKey)
        {
            try
            {
                if (__instance == null) return;

                var skeletonAnimation = __instance.skeletonAnimation;
                var skeleton = skeletonAnimation != null ? skeletonAnimation.skeleton : null;
                if (skeleton == null) return;

                string objName = __instance.gameObject != null ? __instance.gameObject.name : "";
                string skelName = skeleton.Data != null && !string.IsNullOrEmpty(skeleton.Data.name) ? skeleton.Data.name : objName;

                // 톱니바퀴 / zz0301 / 09 계열 노트 감지
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

                        // 그림자(shadow) 슬롯을 제외한 모든 슬롯 및 SetupPose SlotData 틴트 적용
                        string slotName = slot.data != null ? slot.data.name : "";
                        if (!string.Equals(slotName, "shadow", StringComparison.OrdinalIgnoreCase))
                        {
                            slot.r = customR;
                            slot.g = customG;
                            slot.b = customB;

                            if (slot.data != null)
                            {
                                slot.data.r = customR;
                                slot.data.g = customG;
                                slot.data.b = customB;
                            }
                        }
                    }

                    if (tintedObjects.Add(objName))
                    {
                        MelonLogger.Msg($"[GearNoteColorPatch] 🟢 톱니바퀴 노트 '{objName}' 틴트 적용 완료 (R={customR}, G={customG}, B={customB})");
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
            if (objName.Contains("0903") || skelName.Contains("0903")) return true;
            if (objName.Contains("0209") || skelName.Contains("0209")) return true;
            if (objName.Contains("0709") || skelName.Contains("0709")) return true;
            if (objName.Contains("0509") || skelName.Contains("0509")) return true;
            if (objName.Contains("09_") || skelName.Contains("09_")) return true;
            if (objName.IndexOf("gear", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (skelName.IndexOf("gear", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (objName.IndexOf("saw", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            if (skelName.IndexOf("saw", StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }
    }
}
