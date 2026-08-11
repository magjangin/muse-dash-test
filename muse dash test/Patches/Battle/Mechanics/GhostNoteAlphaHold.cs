using System;
using MelonLoader;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace muse_dash_test
{
    // === 고스트 노트 페이드 차단 ===
    //
    // 알파를 만드는 후보가 둘입니다. 둘 다 `Il2Cpp.SpineActionController`에 있습니다.
    //   1. `SetAlpha(float alpha)`                              - public. 람다가 float·Color·Color 세 쌍이라
    //                                                             스켈레톤 알파 + 머티리얼/렌더러 컬러를 함께 트윈합니다.
    //   2. `OnNoteDisappear(object, object, object[])`           - private. 노트 사라짐 이벤트 핸들러.
    // 실측에서 1번은 고스트 노트에 대해 한 번도 불리지 않았으므로 2번을 함께 막습니다.
    //
    // 고스트 판정은 세 단계로 폭을 넓혔습니다. `m_MusicData`가 비어 있는 컨트롤러도 있어서
    // 그것만 보면 조용히 통과해 버립니다(1번이 침묵한 원인일 수 있음).
    //   type == 4  →  UID xx == 17  →  GameObject 이름의 6자리 UID(`071701_road_nor_1(Clone)`)
    //
    // 캐릭터/롱노트도 같은 메서드를 쓰므로 고스트로 확인되지 않은 호출은 반드시 통과시킵니다.

    internal static class GhostNoteIdentity
    {
        private const uint GhostType = 4;
        private const string GhostXx = "17";

        /// <summary>고스트 노트인지 판정하고, 무엇을 근거로 판정했는지 함께 돌려줍니다.</summary>
        internal static bool IsGhost(SpineActionController controller, out string detail)
        {
            detail = "(판정 불가)";
            if (controller == null) return false;

            // 1·2단계: 노트 데이터가 붙어 있으면 그걸 신뢰합니다.
            try
            {
                var note = controller.m_MusicData?.noteData;
                if (note != null)
                {
                    string uid = note.uid;
                    uint type = note.type;
                    detail = $"uid={uid ?? "(null)"}, type={type}";

                    if (type == GhostType) return true;
                    if (IsGhostUid(uid)) return true;
                    return false;
                }
            }
            catch (Exception) { }

            // 3단계: 노트 데이터가 없으면 오브젝트 이름에서 UID를 읽습니다.
            // 노트 프리팹 클론은 `071701_road_nor_1(Clone)` 꼴이라 앞 6자리가 UID입니다.
            try
            {
                string name = controller.gameObject != null ? controller.gameObject.name : null;
                detail = $"musicData 없음, name={name ?? "(null)"}";
                if (name != null && name.Length >= 6 && IsGhostUid(name.Substring(0, 6)))
                {
                    return true;
                }
            }
            catch (Exception) { }

            return false;
        }

        /// <summary>노트 오브젝트 컨트롤러 쪽 판정. 같은 3단계를 씁니다.</summary>
        internal static bool IsGhostNoteObject(BaseEnemyObjectController controller, out string detail)
        {
            detail = "(판정 불가)";
            if (controller == null) return false;

            try
            {
                var note = controller.m_MusicData?.noteData;
                if (note != null)
                {
                    string uid = note.uid;
                    uint type = note.type;
                    detail = $"uid={uid ?? "(null)"}, type={type}";

                    if (type == GhostType) return true;
                    if (IsGhostUid(uid)) return true;
                    return false;
                }
            }
            catch (Exception) { }

            try
            {
                string name = controller.gameObject != null ? controller.gameObject.name : null;
                detail = $"musicData 없음, name={name ?? "(null)"}";
                if (name != null && name.Length >= 6 && IsGhostUid(name.Substring(0, 6))) return true;
            }
            catch (Exception) { }

            return false;
        }

        private static bool IsGhostUid(string uid)
        {
            if (uid == null || uid.Length < 6) return false;
            for (int i = 0; i < 6; i++)
            {
                if (!char.IsDigit(uid[i])) return false;
            }
            return uid.Substring(2, 2) == GhostXx;
        }
    }

    /// <summary>차단 횟수와 관측 표본을 모아 로그로 남깁니다. 침묵이 생기지 않게 항상 뭔가는 남깁니다.</summary>
    internal static class GhostFadeBlockStats
    {
        private static int observeBudget = 24;
        private static readonly System.Collections.Generic.Dictionary<string, int> blockedBySource =
            new System.Collections.Generic.Dictionary<string, int>();
        private static DateTime lastSummaryTime = DateTime.MinValue;
        private static bool pendingSummary;

        /// <summary>고스트가 아니어도 호출 자체를 몇 번은 남깁니다. "안 불렸다"와 "필터에 걸렸다"를 구분하기 위함입니다.</summary>
        internal static void Observe(string source, string detail, bool isGhost)
        {
            if (observeBudget <= 0) return;
            observeBudget--;
            MelonLogger.Msg($"[GhostNote.FadeBlock.Observe] {source} 호출 감지: 고스트={isGhost}, {detail}");
        }

        internal static void Blocked(string source)
        {
            blockedBySource.TryGetValue(source, out int n);
            blockedBySource[source] = n + 1;
            pendingSummary = true;

            if ((DateTime.UtcNow - lastSummaryTime).TotalSeconds < 10.0) return;

            lastSummaryTime = DateTime.UtcNow;
            if (!pendingSummary) return;
            pendingSummary = false;

            var sb = new System.Text.StringBuilder();
            foreach (var entry in blockedBySource)
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append($"{entry.Key}={entry.Value}");
            }
            MelonLogger.Msg($"[GhostNote.FadeBlock] 누적 차단: {sb}");
        }
    }

    /// <summary>
    /// 노트 오브젝트 쪽 사라짐 처리. `SpineActionController` 두 후보가 모두 침묵해서 추가한 세 번째 후보입니다.
    /// 이름이 같은 오버로드가 둘이라(실제 메서드 + params 편의 오버로드) 인자 타입을 반드시 못박아야 합니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(BaseEnemyObjectController), "NoteDisappearLogic",
        new Type[]
        {
            typeof(Il2CppSystem.Object),
            typeof(Il2CppSystem.Object),
            typeof(Il2CppReferenceArray<Il2CppSystem.Object>)
        })]
    public class BaseEnemyObjectController_NoteDisappearLogic_GhostNote_Patch
    {
        public static bool Prefix(BaseEnemyObjectController __instance)
        {
            try
            {
                if (!InputOverlay.showGhostNotes) return true;

                bool isGhost = GhostNoteIdentity.IsGhostNoteObject(__instance, out string detail);
                GhostFadeBlockStats.Observe("NoteDisappearLogic", detail, isGhost);
                if (!isGhost) return true;

                GhostFadeBlockStats.Blocked("NoteDisappearLogic");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[BaseEnemyObjectController.NoteDisappearLogic.Prefix] 고스트 사라짐 차단 중 예외 발생: {ex}");
                return true;
            }
        }
    }

    /// <summary>
    /// 페이드의 실체는 C# 알파 호출이 아니라 Spine 애니메이션이었습니다.
    /// 고스트 노트의 `in` 액션은 `in_nor_44` 하나로 풀리고, 날아오는 1.5초 동안 재생되는 것이 그것뿐입니다.
    ///
    /// 애니메이션을 통째로 바꾸면 안 됩니다 — `standby`로 교체해 보니 노트가 화면 중앙에 멈췄습니다.
    /// 즉 비행 이동도 같은 애니메이션이 갖고 있습니다. 그래서 애니메이션은 그대로 재생시키고
    /// 그 안의 **컬러 타임라인 알파 키만 1로 덮어씁니다**. 이동·스케일·회전 타임라인은 손대지 않으므로
    /// 등장 모션은 원본 그대로고 투명해지는 것만 사라집니다.
    ///
    /// SkeletonData는 같은 에셋을 쓰는 모든 노트가 공유하므로 애니메이션당 한 번만 손봅니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.PlayByKey))]
    public class SpineActionController_PlayByKey_GhostNote_Patch
    {
        /// <summary>노트가 날아오는 동안 재생되는 액션 키.</summary>
        private const string FlightActionKey = "in";

        /// <summary>Spine ColorTimeline 프레임 한 칸의 float 개수: time, r, g, b, a.</summary>
        private const int ColorEntries = 5;

        /// <summary>Spine TwoColorTimeline 프레임 한 칸: time, r, g, b, a, r2, g2, b2.</summary>
        private const int TwoColorEntries = 8;

        /// <summary>프레임 한 칸에서 알파가 놓인 위치.</summary>
        private const int AlphaOffset = 4;

        private static readonly System.Collections.Generic.HashSet<string> patchedAnimations =
            new System.Collections.Generic.HashSet<string>();

        public static void Postfix(SpineActionController __instance, string actionKey)
        {
            try
            {
                if (!InputOverlay.showGhostNotes) return;
                if (!GhostNoteIdentity.IsGhost(__instance, out string detail)) return;

                GhostFadeBlockStats.Observe("PlayByKey", $"actionKey={actionKey ?? "(null)"}, {detail}", true);
                SpineActionContract.DumpSupplyForce(__instance);

                if (!string.Equals(actionKey, FlightActionKey, StringComparison.Ordinal)) return;

                StripAlphaFromCurrentAnimation(__instance);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[SpineActionController.PlayByKey.Postfix] 고스트 알파 타임라인 처리 중 예외 발생: {ex}");
            }
        }

        /// <summary>지금 재생 중인 애니메이션에서 컬러 타임라인의 알파 키를 전부 1로 만듭니다.</summary>
        private static void StripAlphaFromCurrentAnimation(SpineActionController controller)
        {
            var skeletonAnimation = controller.skeletonAnimation;
            if (skeletonAnimation == null) return;

            var skeleton = skeletonAnimation.skeleton;
            var data = skeleton != null ? skeleton.Data : null;
            if (data == null) return;

            string animationName = controller.currentAnimationName;
            if (string.IsNullOrEmpty(animationName)) return;
            if (!patchedAnimations.Add(animationName)) return;

            var animation = data.FindAnimation(animationName);
            if (animation == null)
            {
                MelonLogger.Msg($"[GhostNote.AlphaTimeline] 애니메이션을 찾지 못했습니다: {animationName}");
                return;
            }

            var timelines = animation.timelines;
            // ExposedList는 인덱서가 없습니다. 내부 배열(Items)이 Count보다 클 수 있어 둘 다 봅니다.
            var items = timelines != null ? timelines.Items : null;
            int count = timelines == null ? 0 : timelines.Count;
            if (items != null && count > items.Length) count = items.Length;

            int colorTimelines = 0;
            int alphaKeys = 0;

            for (int i = 0; i < count; i++)
            {
                var timeline = items[i];
                if (timeline == null) continue;

                var color = timeline.TryCast<Il2CppSpine.ColorTimeline>();
                if (color != null)
                {
                    colorTimelines++;
                    alphaKeys += ForceOpaque(color.frames, ColorEntries);
                    continue;
                }

                var twoColor = timeline.TryCast<Il2CppSpine.TwoColorTimeline>();
                if (twoColor != null)
                {
                    colorTimelines++;
                    alphaKeys += ForceOpaque(twoColor.frames, TwoColorEntries);
                }
            }

            MelonLogger.Msg($"[GhostNote.AlphaTimeline] '{animationName}' 처리 완료: 타임라인 {count}개 중 컬러 {colorTimelines}개, " +
                            $"알파 키 {alphaKeys}개를 1로 고정 (이동/스케일 타임라인은 그대로)");
        }

        /// <summary>프레임 배열에서 알파 자리만 1로 덮어쓰고, 바꾼 키 개수를 돌려줍니다.</summary>
        private static int ForceOpaque(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<float> frames, int entries)
        {
            if (frames == null) return 0;

            int changed = 0;
            for (int i = AlphaOffset; i < frames.Length; i += entries)
            {
                if (frames[i] >= 0.999f) continue;
                frames[i] = 1f;
                changed++;
            }
            return changed;
        }
    }

    /// <summary>알파 트윈 진입점. 고스트 노트면 호출을 건너뜁니다.</summary>
    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.SetAlpha))]
    public class SpineActionController_SetAlpha_GhostNote_Patch
    {
        public static bool Prefix(SpineActionController __instance, float alpha)
        {
            try
            {
                if (!InputOverlay.showGhostNotes) return true;

                bool isGhost = GhostNoteIdentity.IsGhost(__instance, out string detail);
                GhostFadeBlockStats.Observe("SetAlpha", $"alpha={alpha:0.###}, {detail}", isGhost);
                if (!isGhost) return true;

                GhostFadeBlockStats.Blocked("SetAlpha");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[SpineActionController.SetAlpha.Prefix] 고스트 알파 차단 중 예외 발생: {ex}");
                return true;
            }
        }
    }

    /// <summary>
    /// 노트 사라짐 이벤트 핸들러. 고스트 노트면 건너뜁니다.
    /// private 메서드지만 파라미터가 전부 참조 타입이고 byref/out이 없어, 과거 크래시 조합
    /// (private + out 구조체)과는 다릅니다. Prefix에서 파라미터를 아예 받지 않아 바인딩 위험도 없앴습니다.
    ///
    /// 인자 타입을 반드시 명시해야 합니다. Il2CppInterop이 같은 이름으로 두 개를 만들어 두기 때문에
    /// (`Il2CppReferenceArray&lt;Object&gt;` 실제 메서드 + `params Object[]` 편의 오버로드)
    /// 이름만 주면 `AmbiguousMatchException`으로 패치 등록이 실패합니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), "OnNoteDisappear",
        new Type[]
        {
            typeof(Il2CppSystem.Object),
            typeof(Il2CppSystem.Object),
            typeof(Il2CppReferenceArray<Il2CppSystem.Object>)
        })]
    public class SpineActionController_OnNoteDisappear_GhostNote_Patch
    {
        public static bool Prefix(SpineActionController __instance)
        {
            try
            {
                if (!InputOverlay.showGhostNotes) return true;

                bool isGhost = GhostNoteIdentity.IsGhost(__instance, out string detail);
                GhostFadeBlockStats.Observe("OnNoteDisappear", detail, isGhost);
                if (!isGhost) return true;

                GhostFadeBlockStats.Blocked("OnNoteDisappear");
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[SpineActionController.OnNoteDisappear.Prefix] 고스트 사라짐 차단 중 예외 발생: {ex}");
                return true;
            }
        }
    }
}
