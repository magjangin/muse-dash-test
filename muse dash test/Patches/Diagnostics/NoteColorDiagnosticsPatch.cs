using System;
using System.Collections.Generic;
using System.Reflection;
using MelonLoader;
using Il2Cpp;

namespace muse_dash_test
{
    /// <summary>
    /// 배틀 내 모든 노트 오브젝트의 Spine 스켈레톤, 슬롯 색상, 어태치먼트, 
    /// ColorTimeline RGB 키프레임 정보를 실시간으로 덤프하는 진단용 패치입니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.PlayByKey))]
    public class NoteColorDiagnosticsPatch
    {
        /// <summary>곡 플레이 중 이미 덤프한 스켈레톤+애니메이션 키 (로그 중복 방지)</summary>
        private static readonly HashSet<string> dumpedKeys = new HashSet<string>();

        /// <summary>씬 전환 시 덤프 기록 초기화 (GameMusicSceneInitPatch 등에서 호출 가능)</summary>
        public static void ClearDumpCache()
        {
            dumpedKeys.Clear();
        }

        public static void Postfix(SpineActionController __instance, string actionKey)
        {
            try
            {
                if (__instance == null) return;

                var skeletonAnimation = __instance.skeletonAnimation;
                var skeleton = skeletonAnimation != null ? skeletonAnimation.skeleton : null;
                var data = skeleton != null ? skeleton.Data : null;
                if (data == null) return;

                string objName = __instance.gameObject != null ? __instance.gameObject.name : "(Unknown)";
                string basePrefabName = objName;
                int cloneIdx = basePrefabName.IndexOf("(Clone)");
                if (cloneIdx >= 0) basePrefabName = basePrefabName.Substring(0, cloneIdx);

                string skeletonName = !string.IsNullOrEmpty(data.name) ? data.name : basePrefabName;
                int skelCloneIdx = skeletonName.IndexOf("(Clone)");
                if (skelCloneIdx >= 0) skeletonName = skeletonName.Substring(0, skelCloneIdx);

                string animName = __instance.currentAnimationName;
                if (string.IsNullOrEmpty(animName)) animName = actionKey;

                string cacheKey = $"{skeletonName}|{animName}";

                // 프리팹+애니메이션별 1회만 진단 출력
                if (!dumpedKeys.Add(cacheKey)) return;

                MelonLogger.Msg($"==================================================================================");
                MelonLogger.Msg($"[NoteColorDiag] 🎯 노트 진단: 오브젝트='{objName}', 스켈레톤='{skeletonName}', 액션='{actionKey}', 애니='{animName}'");

                // 1. 슬롯 및 어태치먼트 틴트 덤프
                var slots = skeleton.Slots;
                if (slots != null)
                {
                    var slotItems = slots.Items;
                    int slotCount = slots.Count;
                    if (slotItems != null && slotCount > 0)
                    {
                        if (slotCount > slotItems.Length) slotCount = slotItems.Length;
                        MelonLogger.Msg($"  📌 [Slots & Tint] 총 {slotCount}개 슬롯:");
                        for (int i = 0; i < slotCount; i++)
                        {
                            var slot = slotItems[i];
                            if (slot == null) continue;

                            string slotName = GetSlotName(slot, i);
                            string attachName = GetAttachmentName(slot);
                            var (r, g, b, a) = GetSlotRgba(slot);

                            MelonLogger.Msg($"    - [{i:D2}] 슬롯='{slotName}', 어태치먼트='{attachName}', RGBA=({r:F2}, {g:F2}, {b:F2}, {a:F2})");
                        }
                    }
                }

                // 2. 애니메이션 ColorTimeline / TwoColorTimeline RGB 키프레임 덤프
                var animation = data.FindAnimation(animName);
                if (animation != null && animation.timelines != null)
                {
                    var timelines = animation.timelines;
                    var tItems = timelines.Items;
                    int tCount = timelines.Count;
                    if (tItems != null && tCount > 0)
                    {
                        if (tCount > tItems.Length) tCount = tItems.Length;

                        int colorTimelineCount = 0;
                        for (int i = 0; i < tCount; i++)
                        {
                            var timeline = tItems[i];
                            if (timeline == null) continue;

                            var color = timeline.TryCast<Il2CppSpine.ColorTimeline>();
                            if (color != null)
                            {
                                colorTimelineCount++;
                                string targetSlot = "(Unknown)";
                                int slotIdx = color.slotIndex;
                                if (slots != null && slotIdx >= 0 && slotIdx < slots.Count && slots.Items != null)
                                {
                                    var target = slots.Items[slotIdx];
                                    if (target != null) targetSlot = GetSlotName(target, slotIdx);
                                }

                                float[] frames = color.frames;
                                int frameLen = frames != null ? frames.Length : 0;
                                int keyCount = frameLen / 5; // time, r, g, b, a

                                string keyInfo = "";
                                if (keyCount > 0 && frames != null)
                                {
                                    float t0 = frames[0], r0 = frames[1], g0 = frames[2], b0 = frames[3], a0 = frames[4];
                                    keyInfo = $"첫키(t={t0:F2}s, RGB=({r0:F2}, {g0:F2}, {b0:F2}), A={a0:F2})";
                                    if (keyCount > 1 && frameLen >= 10)
                                    {
                                        float t1 = frames[5], r1 = frames[6], g1 = frames[7], b1 = frames[8], a1 = frames[9];
                                        keyInfo += $" -> 키1(t={t1:F2}s, RGB=({r1:F2}, {g1:F2}, {b1:F2}), A={a1:F2})";
                                    }
                                }

                                MelonLogger.Msg($"    - ColorTimeline #{colorTimelineCount}: 슬롯='{targetSlot}'(Idx={slotIdx}), 키프레임 {keyCount}개 :: {keyInfo}");
                            }

                            var twoColor = timeline.TryCast<Il2CppSpine.TwoColorTimeline>();
                            if (twoColor != null)
                            {
                                colorTimelineCount++;
                                string targetSlot = "(Unknown)";
                                int slotIdx = twoColor.slotIndex;
                                if (slots != null && slotIdx >= 0 && slotIdx < slots.Count && slots.Items != null)
                                {
                                    var target = slots.Items[slotIdx];
                                    if (target != null) targetSlot = GetSlotName(target, slotIdx);
                                }

                                float[] frames = twoColor.frames;
                                int frameLen = frames != null ? frames.Length : 0;
                                int keyCount = frameLen / 8; // time, r, g, b, a, r2, g2, b2

                                string keyInfo = "";
                                if (keyCount > 0 && frames != null)
                                {
                                    float t0 = frames[0], r0 = frames[1], g0 = frames[2], b0 = frames[3], a0 = frames[4];
                                    keyInfo = $"첫키(t={t0:F2}s, LightRGB=({r0:F2}, {g0:F2}, {b0:F2}), A={a0:F2})";
                                }

                                MelonLogger.Msg($"    - TwoColorTimeline #{colorTimelineCount}: 슬롯='{targetSlot}'(Idx={slotIdx}), 키프레임 {keyCount}개 :: {keyInfo}");
                            }
                        }

                        if (colorTimelineCount == 0)
                        {
                            MelonLogger.Msg($"  📌 [Timelines] 애니메이션 '{animName}'에 ColorTimeline 없음 (슬롯 틴트 100% 사용 가능)");
                        }
                    }
                }
                MelonLogger.Msg($"==================================================================================");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[NoteColorDiag] 진단 실행 중 예외 발생: {ex}");
            }
        }

        private static string GetSlotName(Il2CppSpine.Slot slot, int defaultIndex)
        {
            try
            {
                if (slot == null) return $"Slot_{defaultIndex}";
                var type = slot.GetType();
                var dataProp = type.GetProperty("data", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) 
                            ?? type.GetProperty("Data", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                var dataObj = dataProp != null ? dataProp.GetValue(slot) : null;
                if (dataObj != null)
                {
                    var dataType = dataObj.GetType();
                    var nameProp = dataType.GetProperty("name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) 
                                ?? dataType.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    var nameVal = nameProp != null ? nameProp.GetValue(dataObj) as string : null;
                    if (!string.IsNullOrEmpty(nameVal)) return nameVal;
                }
            }
            catch { }
            return $"Slot_{defaultIndex}";
        }

        private static string GetAttachmentName(Il2CppSpine.Slot slot)
        {
            try
            {
                if (slot == null) return "(None)";
                var type = slot.GetType();
                var attachProp = type.GetProperty("attachment", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) 
                              ?? type.GetProperty("Attachment", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                var attachObj = attachProp != null ? attachProp.GetValue(slot) : null;
                if (attachObj != null)
                {
                    var attachType = attachObj.GetType();
                    var nameProp = attachType.GetProperty("name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) 
                                ?? attachType.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    var nameVal = nameProp != null ? nameProp.GetValue(attachObj) as string : null;
                    if (!string.IsNullOrEmpty(nameVal)) return nameVal;
                }
            }
            catch { }
            return "(None)";
        }

        private static (float r, float g, float b, float a) GetSlotRgba(Il2CppSpine.Slot slot)
        {
            try
            {
                if (slot == null) return (1f, 1f, 1f, 1f);
                var t = slot.GetType();
                var rProp = t.GetProperty("r", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                var gProp = t.GetProperty("g", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                var bProp = t.GetProperty("b", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                var aProp = t.GetProperty("a", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                float r = rProp != null ? Convert.ToSingle(rProp.GetValue(slot)) : 1f;
                float g = gProp != null ? Convert.ToSingle(gProp.GetValue(slot)) : 1f;
                float b = bProp != null ? Convert.ToSingle(bProp.GetValue(slot)) : 1f;
                float a = aProp != null ? Convert.ToSingle(aProp.GetValue(slot)) : 1f;

                return (r, g, b, a);
            }
            catch { }
            return (1f, 1f, 1f, 1f);
        }
    }
}
