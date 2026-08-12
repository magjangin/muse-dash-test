using System;
using System.Collections.Generic;
using MelonLoader;
using Il2Cpp;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 오직 톱니바퀴 노트(Gear Note / xx=09, 0901, 0902, gear 등)만 
    /// 핀포인트로 감지하여 네온 라임 그린 색상으로 100% 틴트 변조하는 패치입니다.
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

                string objName = __instance.gameObject != null ? __instance.gameObject.name : "";
                if (string.IsNullOrEmpty(objName)) return;

                // 캐릭터 / 플레이어 / 이펙트 제외
                if (objName.Contains("girl") || objName.Contains("char") || objName.Contains("player") || objName.Contains("Elfin")) return;

                var skeletonAnimation = __instance.skeletonAnimation;
                var skeleton = skeletonAnimation != null ? skeletonAnimation.skeleton : null;
                if (skeleton == null) return;

                string skelName = skeleton.Data != null && !string.IsNullOrEmpty(skeleton.Data.name) ? skeleton.Data.name : objName;

                // 🎯 오직 톱니바퀴 노트 계열만 핀포인트 검사
                if (!IsGearNote(objName, skelName)) return;

                // 1. Spine 스켈레톤, 슬롯, SlotData 및 Attachment (Region / Mesh) 틴트
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

                        // 1-2. SetupPose SlotData Color
                        if (slot.data != null)
                        {
                            slot.data.r = customR;
                            slot.data.g = customG;
                            slot.data.b = customB;
                        }

                        // 1-3. Region / Mesh Attachment Color
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

                // 2. 자식 오브젝트의 Unity SpriteRenderer / MeshRenderer 틴트
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

                if (tintedObjects.Add(objName))
                {
                    MelonLogger.Msg($"[GearNoteColorPatch] 🟢 톱니바퀴 노트 '{objName}' (Skel: '{skelName}', Action: '{actionKey}') 100% 틴트 완료!");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[GearNoteColorPatch] 톱니바퀴 틴트 처리 중 예외 발생: {ex}");
            }
        }

        private static bool IsGearNote(string objName, string skelName)
        {
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
