using System;
using System.Collections.Generic;
using MelonLoader;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace muse_dash_test
{
    // === 고스트 노트가 사라지지 않게 하기 ===
    //
    // 고스트 노트(UID zzxxyy의 xx=17, type 4)는 판정선에 가까워질수록 투명해집니다.
    // 원인은 C# 코드가 아니라 **Spine 애니메이션 데이터**입니다.
    //
    // 액션 계약서(SpineActionContract 덤프)로 확인한 고스트 노트의 액션은 셋뿐입니다.
    //   in          → in_nor_44     ← 비행 1.5초(dt=1.48) 동안 재생되는 유일한 애니메이션
    //   note_out_g  → out_g         ← 판정 시점(Great)
    //   note_out_p  → out_p         ← 판정 시점(Perfect)
    // 이 중 in_nor_44가 알파를 깎습니다. 그래서 `in`이 깔린 직후 그 애니메이션의 컬러 타임라인에서
    // 알파 키만 1로 덮어씁니다. 이동·스케일·회전 타임라인은 건드리지 않으므로 등장 모션은 원본 그대로입니다.
    //
    // 켜고 끄는 곳이 둘입니다. 공식곡은 config.txt의 '공식곡에서도 고스트 노트 보이기',
    // 커스텀 곡은 각자의 hwa info.txt에 적은 '커스텀 곡 고스트 노트 보이기'를 따릅니다.
    // SkeletonData는 프로세스 내내 공유되므로 덮기 전 알파를 기억해 뒀다가, 꺼진 곡에서는 되돌립니다.
    // 그러지 않으면 한 곡에서 켠 뒤로는 끈 곡에서도 계속 보이게 됩니다.
    //
    // 확인된 막다른 길(같은 곳을 다시 파지 않기 위해 남깁니다):
    //   - SpineActionController.SetAlpha(float), SpineActionController.OnNoteDisappear,
    //     BaseEnemyObjectController.NoteDisappearLogic → 고스트 노트에 대해 한 번도 호출되지 않습니다.
    //   - 애니메이션을 standby로 통째 교체하면 노트가 화면 중앙에 멈춥니다. 비행 이동도 in_nor_44에 들어 있습니다.
    //   - 알파/투명도 필드는 NoteConfigData·MusicData 어디에도 없어 BMS 주입·zz 복구 레이어에서는 손댈 수 없습니다.

    /// <summary>
    /// 이 곡에서 고스트 노트를 보이게 할지 정합니다.
    ///   커스텀 곡 → `hwa info.txt`의 '커스텀 곡 고스트 노트 보이기'
    ///   공식곡     → `config.txt`의 '공식곡에서도 고스트 노트 보이기'
    /// 커스텀 곡인데 그 줄이 없으면 전역 설정을 그대로 따릅니다.
    /// </summary>
    internal static class GhostNoteVisibility
    {
        internal static bool IsEnabledForCurrentSong()
        {
            try
            {
                string uid = CustomPlaySession.Current.LastKnownMusicUid;
                if (!string.IsNullOrEmpty(uid) && CustomContentIds.IsVirtualSong(uid))
                {
                    var manifest = HwaResourceManager.GetManifest(uid);
                    if (manifest != null && manifest.ShowGhostNotes.HasValue)
                    {
                        return manifest.ShowGhostNotes.Value;
                    }
                }
            }
            catch (Exception) { }

            return InputOverlay.showGhostNotes;
        }
    }

    internal static class GhostNoteIdentity
    {
        private const uint GhostType = 4;
        private const string GhostXx = "17";

        /// <summary>
        /// 고스트 노트인지 판정합니다. `m_MusicData`가 비어 있는 컨트롤러도 있어서,
        /// 노트 데이터가 없으면 오브젝트 이름(`071701_road_nor_1(Clone)`)의 앞 6자리를 UID로 읽습니다.
        /// </summary>
        internal static bool IsGhost(SpineActionController controller)
        {
            if (controller == null) return false;

            try
            {
                var note = controller.m_MusicData?.noteData;
                if (note != null && (note.type != 0 || !string.IsNullOrEmpty(note.uid)))
                {
                    return note.type == GhostType || IsGhostUid(note.uid);
                }
            }
            catch (Exception) { }

            try
            {
                string name = controller.gameObject != null ? controller.gameObject.name : null;
                return name != null && name.Length >= 6 && IsGhostUid(name.Substring(0, 6));
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

    /// <summary>
    /// 고스트 노트의 비행 애니메이션에서 알파 키를 걷어냅니다.
    /// `PlayByKey`는 `public` + `string`/`bool`이라 이 프로젝트에서 안전이 확인된 훅 모양입니다.
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

        /// <summary>
        /// 캐시 키 → 손대기 전의 알파 값들(타임라인 순서 → 키 순서로 평평하게).
        /// `SkeletonData`는 프로세스 내내 공유되므로, 설정을 끄면 이 값으로 되돌려야 원래 페이드가 살아납니다.
        ///
        /// <para><b>키에 스켈레톤을 포함해야 합니다.</b> 지상(`071701_road_nor_1`)과 공중(`071704_air_nor_1`) 고스트 노트는
        /// 서로 다른 프리팹이라 `SkeletonData`도 따로인데, 애니메이션 이름은 같을 수 있습니다.
        /// 이름만으로 키를 잡으면 먼저 처리된 쪽 때문에 나머지가 "이미 했다"고 건너뛰어집니다.</para>
        /// </summary>
        private static readonly Dictionary<string, float[]> originalAlphas = new Dictionary<string, float[]>();

        /// <summary>캐시 키 → 지금 불투명으로 덮인 상태인가. 상태가 같으면 아무것도 하지 않습니다.</summary>
        private static readonly Dictionary<string, bool> opaqueAnimations = new Dictionary<string, bool>();

        // ─────────────────────────────────────────────────────────────────────
        //  임시 진단: "고스트 보이기가 저절로 풀린다"의 원인을 계측으로 확정하기 위한 상태.
        //
        //  위 두 캐시는 스켈레톤 '이름'으로 키를 잡고 어디서도 비워지지 않습니다. 그래서 씬 재진입
        //  등으로 SkeletonData가 다시 만들어지면 알파는 원본으로 돌아갔는데 캐시는 "이미 덮었다"고
        //  기억한 채라, 아래 조기 반환이 실제 덮어쓰기를 영영 건너뛸 수 있습니다. 그게 사실인지
        //  추측이 아니라 눈으로 보려고 실제 알파와 네이티브 포인터를 같이 찍습니다.
        //
        //  원인이 확정되면 이 필드들과 ProbeCachedState / ReadMinAlpha / ScanAlpha /
        //  RecordProbePointer, 그리고 두 호출 지점을 통째로 지웁니다.
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>덮어쓸 당시의 SkeletonData 네이티브 포인터. 지금 것과 다르면 데이터가 다시 만들어진 것입니다.</summary>
        private static readonly Dictionary<string, IntPtr> probeDataPtr = new Dictionary<string, IntPtr>();

        /// <summary>정상(일치) 결과를 이미 남긴 캐시 키. 불일치는 이 목록과 무관하게 매번 남깁니다.</summary>
        private static readonly HashSet<string> probeLogged = new HashSet<string>();

        /// <summary>진단이 무한정 비용을 물지 않도록 하는 상한. 소진되면 조용히 멈춥니다.</summary>
        private const int ProbeBudget = 300;
        private static int probeCount;

        public static void Postfix(SpineActionController __instance, string actionKey)
        {
            try
            {
                // 게이트 순서가 곧 성능입니다. PlayByKey는 캐릭터·모든 노트가 공유하는 뜨거운 경로라
                // 대부분의 호출이 첫 줄에서 끝나야 합니다. 곡별 설정 조회는 진짜 고스트 노트일 때만 합니다.
                if (!string.Equals(actionKey, FlightActionKey, StringComparison.Ordinal)) return;
                if (!GhostNoteIdentity.IsGhost(__instance)) return;

                ApplyToCurrentAnimation(__instance, GhostNoteVisibility.IsEnabledForCurrentSong());
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[GhostNote.AlphaTimeline] 알파 타임라인 처리 중 예외 발생: {ex}");
            }
        }

        /// <summary>지금 재생 중인 애니메이션의 알파 키를 설정에 맞춰 1로 덮거나 원본으로 되돌립니다.</summary>
        private static void ApplyToCurrentAnimation(SpineActionController controller, bool opaque)
        {
            var skeleton = controller.skeletonAnimation != null ? controller.skeletonAnimation.skeleton : null;
            var data = skeleton != null ? skeleton.Data : null;
            if (data == null) return;

            string animationName = controller.currentAnimationName;
            if (string.IsNullOrEmpty(animationName)) return;

            string cacheKey = SkeletonNameOf(controller, data) + "|" + animationName;

            // 이미 원하는 상태면 끝. 스켈레톤+애니메이션 조합당 한 번씩만 실제로 손댑니다.
            if (opaqueAnimations.TryGetValue(cacheKey, out bool state) && state == opaque)
            {
                ProbeCachedState(controller, data, animationName, cacheKey, opaque);   // ← 임시 진단
                return;
            }

            // 아직 한 번도 안 건드렸는데 "보이지 않게"라면, 데이터가 이미 원본이라 할 일이 없습니다.
            if (!opaque && !originalAlphas.ContainsKey(cacheKey))
            {
                opaqueAnimations[cacheKey] = false;
                return;
            }

            var animation = data.FindAnimation(animationName);
            if (animation == null)
            {
                ModLogger.Msg($"[GhostNote.AlphaTimeline] 애니메이션을 찾지 못했습니다: {animationName}");
                return;
            }

            var timelines = animation.timelines;
            // ExposedList는 인덱서가 없습니다. 내부 배열(Items)이 Count보다 클 수 있어 둘 다 봅니다.
            var items = timelines != null ? timelines.Items : null;
            int count = timelines == null ? 0 : timelines.Count;
            if (items == null) return;
            if (count > items.Length) count = items.Length;

            // 첫 방문이면 원본 알파를 받아 적으면서 덮습니다. 되돌릴 때는 그 기록을 되짚습니다.
            float[] original = originalAlphas.TryGetValue(cacheKey, out var saved) ? saved : null;
            var capture = original == null ? new List<float>() : null;

            int colorTimelines = 0;
            int alphaKeys = 0;
            int cursor = 0;

            for (int i = 0; i < count; i++)
            {
                var timeline = items[i];
                if (timeline == null) continue;

                var color = timeline.TryCast<Il2CppSpine.ColorTimeline>();
                if (color != null)
                {
                    colorTimelines++;
                    alphaKeys += WriteAlpha(color.frames, ColorEntries, opaque, original, capture, ref cursor);
                    continue;
                }

                var twoColor = timeline.TryCast<Il2CppSpine.TwoColorTimeline>();
                if (twoColor != null)
                {
                    colorTimelines++;
                    alphaKeys += WriteAlpha(twoColor.frames, TwoColorEntries, opaque, original, capture, ref cursor);
                }
            }

            if (capture != null) originalAlphas[cacheKey] = capture.ToArray();
            opaqueAnimations[cacheKey] = opaque;
            RecordProbePointer(cacheKey, data);   // ← 임시 진단 (원인 확정 후 제거)

            ModLogger.Msg($"[GhostNote.AlphaTimeline] {ObjectNameOf(controller)} '{cacheKey}' {(opaque ? "고정" : "복원")} 완료: 타임라인 {count}개 중 컬러 {colorTimelines}개, " +
                            $"알파 키 {alphaKeys}개를 {(opaque ? "1로 고정" : "원본으로 되돌림")} (이동/스케일 타임라인은 그대로)");
        }

        // ═══════════════════════════════════════════════════════════════════════
        //  임시 진단 — 아래 세 메서드와 위 probe* 필드는 원인 확정 후 통째로 지웁니다.
        // ═══════════════════════════════════════════════════════════════════════

        /// <summary>임시 진단: 실제로 덮어쓴 그 순간의 SkeletonData 네이티브 포인터를 적어 둡니다.</summary>
        private static void RecordProbePointer(string cacheKey, Il2CppSpine.SkeletonData data)
        {
            try { probeDataPtr[cacheKey] = data.Pointer; }
            catch (Exception) { }
        }

        /// <summary>
        /// 임시 진단: 캐시가 "이미 불투명"이라 말할 때, 실제 알파도 정말 1인지 확인합니다.
        /// 어긋나면 캐시가 낡은 것이고, 그 경우 SkeletonData 포인터도 덮을 때와 달라져 있을 것입니다.
        /// 그 둘을 한 줄에 같이 찍어야 "캐시가 거짓말한다"와 "설정이 꺼졌다"를 구분할 수 있습니다.
        /// </summary>
        private static void ProbeCachedState(SpineActionController controller, Il2CppSpine.SkeletonData data,
                                             string animationName, string cacheKey, bool opaque)
        {
            if (!ModLogger.IsLevelEnabled(ModLogLevel.Verbose) || probeCount >= ProbeBudget) return;

            try
            {
                probeCount++;

                var animation = data.FindAnimation(animationName);
                int alphaKeys = 0;
                float? actual = animation != null ? ReadMinAlpha(animation, out alphaKeys) : null;

                // 덮어쓴 뒤라면 모든 알파 키가 1이므로 최솟값도 1입니다. 1보다 작으면 원본으로 돌아간 것입니다.
                bool mismatch = opaque && actual.HasValue && actual.Value < 0.999f;

                // 어긋남은 볼 때마다, 정상은 캐시 키당 한 번만 남깁니다.
                if (!mismatch && !probeLogged.Add(cacheKey)) return;

                string then = probeDataPtr.TryGetValue(cacheKey, out IntPtr recorded)
                    ? "0x" + recorded.ToString("X")
                    : "(기록 없음)";

                ModLogger.Msg($"[GhostNote.Probe] {(mismatch ? "★불일치★" : "일치")} '{cacheKey}': " +
                              $"캐시={opaque}, 실제 알파 최솟값={(actual.HasValue ? actual.Value.ToString("0.###") : "(못 읽음)")}(키 {alphaKeys}개), " +
                              $"덮을 때 포인터={then}, 지금 포인터=0x{data.Pointer.ToString("X")}, " +
                              $"uid={CustomPlaySession.Current.LastKnownMusicUid ?? "(null)"}, " +
                              $"설정={GhostNoteVisibility.IsEnabledForCurrentSong()}, obj={ObjectNameOf(controller)}");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[GhostNote.Probe] 진단 중 예외: {ex.Message}");
            }
        }

        /// <summary>
        /// 임시 진단: 컬러 타임라인의 알파 키 중 가장 작은 값을 돌려줍니다.
        /// 첫 키가 아니라 최솟값을 봐야 하는 이유 — 페이드는 보통 알파 1에서 시작해 내려가므로,
        /// 첫 키만 보면 "덮어쓴 상태"와 "원본"이 똑같이 1로 보입니다.
        /// </summary>
        private static float? ReadMinAlpha(Il2CppSpine.Animation animation, out int alphaKeys)
        {
            alphaKeys = 0;
            float min = float.MaxValue;

            var timelines = animation.timelines;
            var items = timelines != null ? timelines.Items : null;
            int count = timelines == null ? 0 : timelines.Count;
            if (items == null) return null;
            if (count > items.Length) count = items.Length;

            for (int i = 0; i < count; i++)
            {
                var timeline = items[i];
                if (timeline == null) continue;

                var color = timeline.TryCast<Il2CppSpine.ColorTimeline>();
                if (color != null)
                {
                    ScanAlpha(color.frames, ColorEntries, ref min, ref alphaKeys);
                    continue;
                }

                var twoColor = timeline.TryCast<Il2CppSpine.TwoColorTimeline>();
                if (twoColor != null)
                {
                    ScanAlpha(twoColor.frames, TwoColorEntries, ref min, ref alphaKeys);
                }
            }

            return alphaKeys > 0 ? min : (float?)null;
        }

        /// <summary>임시 진단: 프레임 배열의 알파 자리만 훑어 최솟값과 키 개수를 모읍니다.</summary>
        private static void ScanAlpha(Il2CppStructArray<float> frames, int entries, ref float min, ref int alphaKeys)
        {
            if (frames == null) return;

            for (int i = AlphaOffset; i < frames.Length; i += entries)
            {
                float value = frames[i];
                if (value < min) min = value;
                alphaKeys++;
            }
        }

        /// <summary>
        /// 캐시 키에 쓸 스켈레톤 이름. 지상·공중 고스트처럼 프리팹이 다르면 이 이름이 달라야
        /// 서로를 덮어쓰지 않습니다. 셋 다 비면 오브젝트 이름으로 떨어집니다.
        /// </summary>
        private static string SkeletonNameOf(SpineActionController controller, Il2CppSpine.SkeletonData data)
        {
            try
            {
                if (!string.IsNullOrEmpty(data.name)) return data.name;

                var asset = controller.skeletonAnimation != null ? controller.skeletonAnimation.skeletonDataAsset : null;
                if (asset != null && !string.IsNullOrEmpty(asset.name)) return asset.name;
            }
            catch (Exception) { }

            return ObjectNameOf(controller);
        }

        private static string ObjectNameOf(SpineActionController controller)
        {
            try
            {
                return controller.gameObject != null ? controller.gameObject.name : "(이름 없음)";
            }
            catch (Exception)
            {
                return "(이름 없음)";
            }
        }

        /// <summary>
        /// 프레임 배열의 알파 자리만 씁니다. <paramref name="opaque"/>면 1, 아니면 <paramref name="original"/>의 값으로.
        /// <paramref name="capture"/>가 있으면 덮기 전 값을 순서대로 받아 적습니다. 바꾼 키 개수를 돌려줍니다.
        /// </summary>
        private static int WriteAlpha(Il2CppStructArray<float> frames, int entries, bool opaque,
                                      float[] original, List<float> capture, ref int cursor)
        {
            if (frames == null) return 0;

            int changed = 0;
            for (int i = AlphaOffset; i < frames.Length; i += entries)
            {
                float current = frames[i];
                if (capture != null) capture.Add(current);

                // 기록해 둔 원본이 모자라면(있을 수 없지만) 지금 값을 그대로 둡니다.
                float target = opaque ? 1f
                    : (original != null && cursor < original.Length ? original[cursor] : current);
                cursor++;

                if (current == target) continue;
                frames[i] = target;
                changed++;
            }
            return changed;
        }
    }
}
