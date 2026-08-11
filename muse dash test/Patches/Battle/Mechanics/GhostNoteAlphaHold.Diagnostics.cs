using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Il2Cpp;
using MelonLoader;

namespace muse_dash_test
{
    /// <summary>
    /// 알파를 덮어쓰기 <b>직전</b>에 원본 컬러 타임라인을 통째로 읽어 남기는 진단 코드입니다.
    /// "정말 [time,r,g,b,a] 순서가 맞는지", "알파가 몇 초에 떨어지는지", "색이 뭔지"를 값으로 답하려고 만들었습니다.
    ///
    /// 애니메이션 이름당 1회만 돕니다(<see cref="SpineActionController_PlayByKey_GhostNote_Patch"/>의 HashSet 게이트 안쪽).
    /// 콘솔에는 표까지, raw float 나열은 파일에만 남습니다. 파일 위치는 <see cref="SpineActionContract.ContractDirectory"/>.
    /// </summary>
    internal static class GhostNoteAlphaDiagnostics
    {
        private const string Tag = "[GhostNote.AlphaTimeline]";

        /// <summary>Spine의 CurveTimeline.curves가 키 하나에 쓰는 float 개수(타입 1 + 베지어 제어점 9쌍).</summary>
        private const int BezierSize = 19;
        private const float CurveLinear = 0f;
        private const float CurveStepped = 1f;

        /// <summary>콘솔에 찍는 키 표의 최대 줄 수. 넘치면 파일에서 보라고 안내합니다.</summary>
        private const int ConsoleKeyLimit = 12;

        /// <summary>알파가 "내려갔다"고 볼 최소 낙차. float 잡음을 감소로 오해하지 않으려고 둡니다.</summary>
        private const float FallEpsilon = 0.0005f;

        /// <summary>덮어쓰기 전 원본을 콘솔과 파일에 남깁니다.</summary>
        internal static void DumpOriginal(SpineActionController controller, Il2CppSpine.SkeletonData data,
                                          Il2CppSpine.Animation animation, string animationName)
        {
            try
            {
                var file = new StringBuilder();
                Emit(file, $"{Tag} ════ '{animationName}' 컬러 타임라인 원본 덤프 (덮어쓰기 전) ════");
                AppendContext(file, controller, data, animation);
                AppendCensus(file, animation);
                AppendColorTimelines(file, data, animation);

                string fileName = MakeFileName(animationName);
                SpineActionContract.WriteFile(fileName, file.ToString());
                MelonLogger.Msg($"{Tag} raw float까지 담은 전체 표: {Path.Combine(SpineActionContract.ContractDirectory, fileName)}");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} 원본 덤프 실패(패치 자체는 계속 진행): {ex}");
            }
        }

        /// <summary>덮어쓴 뒤 같은 배열을 다시 읽어, 네이티브 메모리에 값이 실제로 남았는지 확인합니다.</summary>
        internal static void VerifyRewrite(Il2CppSpine.Animation animation, string animationName, int rewrittenKeys)
        {
            try
            {
                int total = 0;
                int remaining = 0;
                float lowest = 1f;
                float lowestTime = -1f;

                foreach (var view in EnumerateColorTimelines(animation))
                {
                    int keyCount = view.KeyCount;
                    for (int key = 0; key < keyCount; key++)
                    {
                        total++;
                        float alpha = view.Alpha(key);
                        if (alpha >= SpineColorFrames.OpaqueThreshold) continue;

                        remaining++;
                        if (alpha < lowest)
                        {
                            lowest = alpha;
                            lowestTime = view.Time(key);
                        }
                    }
                }

                if (remaining == 0)
                {
                    MelonLogger.Msg($"{Tag} 덮어쓰기 검증: '{animationName}'의 알파 키 {total}개가 다시 읽어도 전부 1.000 " +
                                    $"(이번에 실제로 바꾼 키 {rewrittenKeys}개, 원래 1이던 키 {total - rewrittenKeys}개)");
                }
                else
                {
                    MelonLogger.Warning($"{Tag} 덮어쓰기 검증 실패: '{animationName}'에 아직 1 미만인 알파 키가 {total}개 중 {remaining}개 남았습니다 " +
                                        $"(최저 {lowest:F3} @ t={lowestTime:F4}초). frames 해석이 틀렸거나 페이드 원인이 하나 더 있습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} 덮어쓰기 검증 실패: {ex.Message}");
            }
        }

        // ── 덤프 구성 ────────────────────────────────────────────────────────────

        /// <summary>노트·스켈레톤·재생 상태처럼 "이 덤프가 어떤 상황에서 찍혔는지"를 남깁니다.</summary>
        private static void AppendContext(StringBuilder file, SpineActionController controller,
                                          Il2CppSpine.SkeletonData data, Il2CppSpine.Animation animation)
        {
            EmitFile(file, $"# 수집 시각 : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            EmitFile(file, "# frames 읽는 법 : 키 하나 = [time, r, g, b, a] (TwoColor는 뒤에 r2,g2,b2가 더 붙어 8칸)");
            EmitFile(file, "#                  r·g·b·a는 0~1, time은 애니메이션 시작 기준 초입니다.");
            EmitFile(file, "");

            Emit(file, $"{Tag} 오브젝트   : {Safe(() => controller.gameObject.name)} " +
                       $"(액션키={Safe(() => controller.curActionKey)} 액션명={Safe(() => controller.currentActionName)})");
            Emit(file, $"{Tag} 스켈레톤   : Spine {Safe(() => data.version)} / 슬롯 {Safe(() => data.Slots.Count.ToString())}개 " +
                       $"/ 애니메이션 {Safe(() => data.Animations.Count.ToString())}개");
            Emit(file, $"{Tag} 애니메이션 : '{Safe(() => animation.Name)}' 길이 {Safe(() => animation.Duration.ToString("F4"))}초 " +
                       $"/ 타임라인 {Safe(() => animation.timelines.Count.ToString())}개");
            Emit(file, $"{Tag} 컨트롤러   : m_HasAlpha={Safe(() => controller.m_HasAlpha.ToString())} " +
                       $"duration={Safe(() => controller.duration.ToString("F4"))} " +
                       $"currentAnimationTime={Safe(() => controller.currentAnimationTime.ToString("F4"))}");
            Emit(file, $"{Tag} 스켈레톤 색: rgba=({DescribeSkeletonColor(controller)})");
            Emit(file, $"{Tag} 트랙0      : {DescribeTrack(controller)}");
            Emit(file, $"{Tag}              (트랙 값은 PlayByKey 직후 시점입니다. 비행 도중 값이 아니라 재생을 막 시작한 순간입니다.)");
        }

        /// <summary>스켈레톤 전체에 걸린 색. 여기 알파가 1 미만이면 타임라인과 무관하게 노트 전체가 반투명합니다.</summary>
        private static string DescribeSkeletonColor(SpineActionController controller)
        {
            return Safe(() =>
            {
                var skeleton = controller.skeletonAnimation.skeleton;
                return Rgba(skeleton.r, skeleton.g, skeleton.b, skeleton.a);
            });
        }

        /// <summary>재생 트랙의 상태. 여기 alpha가 1 미만이면 페이드 원인이 타임라인이 아니라 믹싱일 수 있습니다.</summary>
        private static string DescribeTrack(SpineActionController controller)
        {
            return Safe(() =>
            {
                var track = controller.skeletonAnimation.AnimationState.GetCurrent(0);
                if (track == null) return "(재생 중인 트랙 없음)";

                return $"anim='{Safe(() => track.animation.Name)}' trackTime={track.trackTime:F4} " +
                       $"animationStart={track.animationStart:F4} animationEnd={track.animationEnd:F4} " +
                       $"timeScale={track.timeScale:F2} alpha={track.alpha:F3} mixDuration={track.mixDuration:F3} " +
                       $"mixBlend={track.mixBlend} loop={track.loop}";
            });
        }

        /// <summary>타임라인 89개가 각각 무슨 종류인지 세어, 색을 건드리는 게 정말 몇 개뿐인지 보여 줍니다.</summary>
        private static void AppendCensus(StringBuilder file, Il2CppSpine.Animation animation)
        {
            var counts = new Dictionary<string, int>();
            int total = 0;

            foreach (var timeline in EnumerateTimelines(animation))
            {
                string kind = Classify(timeline);
                counts.TryGetValue(kind, out int n);
                counts[kind] = n + 1;
                total++;
            }

            var parts = new List<string>();
            foreach (var pair in counts) parts.Add($"{pair.Key} {pair.Value}");
            parts.Sort(StringComparer.Ordinal);

            Emit(file, $"{Tag} 타임라인 구성 (총 {total}개): {string.Join(", ", parts)}");
        }

        /// <summary>
        /// 컬러 타임라인마다 검증·키 표·요약을 남깁니다.
        /// 콘솔은 한 프레임 안에 찍히니, 알파가 실제로 내려가는 타임라인(과 첫 타임라인)만 자세히 흘리고
        /// 나머지는 요약 한 줄로 줄입니다. 파일에는 언제나 전부 들어갑니다.
        /// </summary>
        private static void AppendColorTimelines(StringBuilder file, Il2CppSpine.SkeletonData data, Il2CppSpine.Animation animation)
        {
            int index = -1;
            int colorCount = 0;
            int fadingCount = 0;

            foreach (var timeline in EnumerateTimelines(animation))
            {
                index++;
                if (!SpineColorFrames.TryDescribe(timeline, out var view)) continue;
                colorCount++;

                int keyCount = view.KeyCount;
                float duration = Safe(() => animation.Duration, -1f);
                bool fades = FadesOut(view, keyCount);
                if (fades) fadingCount++;

                // 첫 타임라인은 알파가 멀쩡해도 콘솔에 다 찍습니다. frames 해석 검증을 한 번은 눈으로 보라고.
                bool detailed = fades || colorCount == 1;

                EmitFile(file, "");
                Emit(file, $"{Tag} ── 타임라인[{index}] {view.TypeName} → 슬롯 #{view.SlotIndex} \"{SlotName(data, view.SlotIndex)}\" " +
                           $"키 {keyCount}개, 셋업 색 {SlotSetup(data, view.SlotIndex)}{(fades ? "  ← 여기서 알파가 내려갑니다" : string.Empty)}");
                AppendLayoutChecks(file, view, keyCount, detailed);
                AppendKeyTable(file, view, keyCount, detailed);
                Emit(file, $"{Tag}    알파 : {DescribeAlphaFall(view, keyCount, duration)}");
                Emit(file, $"{Tag}    색   : {DescribeColor(view, keyCount)}", detailed);
                AppendRawFloats(file, view);
            }

            EmitFile(file, "");
            Emit(file, $"{Tag} 컬러 계열 타임라인 총 {colorCount}개 중 알파가 내려가는 건 {fadingCount}개였습니다. " +
                       $"나머지 타임라인(이동·스케일·회전 등)은 읽지도 쓰지도 않았습니다.");
        }

        /// <summary>이 타임라인이 실제로 알파를 1 미만으로 떨어뜨리는지.</summary>
        private static bool FadesOut(ColorTimelineView view, int keyCount)
        {
            for (int key = 0; key < keyCount; key++)
            {
                if (view.Alpha(key) < SpineColorFrames.OpaqueThreshold) return true;
            }
            return false;
        }

        /// <summary>
        /// [time,r,g,b,a] 가정이 맞는지 서로 독립적인 세 가지로 확인합니다.
        /// 특히 ②의 FrameCount는 frames가 아니라 curves 배열 길이에서 나오므로, 여기서 일치하면 칸 수 가정이 사실상 확정됩니다.
        /// </summary>
        private static void AppendLayoutChecks(StringBuilder file, ColorTimelineView view, int keyCount, bool console)
        {
            int floats = view.FloatCount;
            int remainder = view.Entries > 0 ? floats % view.Entries : -1;
            int runtimeFrameCount = Safe(() => view.Curve.FrameCount, -1);

            bool timesAscend = true;
            float firstTime = keyCount > 0 ? view.Time(0) : 0f;
            float lastTime = keyCount > 0 ? view.Time(keyCount - 1) : 0f;
            for (int key = 1; key < keyCount; key++)
            {
                if (view.Time(key) < view.Time(key - 1)) { timesAscend = false; break; }
            }

            bool inRange = true;
            for (int key = 0; key < keyCount && inRange; key++)
            {
                inRange = InUnitRange(view.R(key)) && InUnitRange(view.G(key))
                          && InUnitRange(view.B(key)) && InUnitRange(view.Alpha(key));
            }

            Emit(file, $"{Tag}    검증① 길이 {floats} ÷ {view.Entries}칸 = {keyCount}키 (나머지 {remainder} → {Mark(remainder == 0)})", console);
            Emit(file, $"{Tag}    검증② 런타임 FrameCount={runtimeFrameCount} vs 계산한 키 수 {keyCount} → {Mark(runtimeFrameCount == keyCount)} " +
                       $"(FrameCount는 frames가 아니라 curves 배열 길이에서 나오는 독립 값입니다)", console);
            Emit(file, $"{Tag}    검증③ 시간열 {firstTime:F4}→{lastTime:F4} 단조증가 {Mark(timesAscend)} / r·g·b·a 전부 0~1 {Mark(inRange)}", console);
        }

        private static void AppendKeyTable(StringBuilder file, ColorTimelineView view, int keyCount, bool console)
        {
            Emit(file, $"{Tag}     키   시간(초)      r       g       b       a    색상       커브", console);

            for (int key = 0; key < keyCount; key++)
            {
                string row = $"{Tag}    [{key,2}]  {view.Time(key),8:F4}  {view.R(key),6:F3}  {view.G(key),6:F3}  " +
                             $"{view.B(key),6:F3}  {view.Alpha(key),6:F3}  {Hex(view.R(key), view.G(key), view.B(key))}  {CurveType(view, key, keyCount)}";

                Emit(file, row, console && key < ConsoleKeyLimit);
            }

            if (console && keyCount > ConsoleKeyLimit)
            {
                Emit(file, $"{Tag}    … 콘솔에는 앞 {ConsoleKeyLimit}줄만 찍었습니다(총 {keyCount}키). 나머지는 파일에 있습니다.");
            }
        }

        private static void AppendRawFloats(StringBuilder file, ColorTimelineView view)
        {
            EmitFile(file, $"    raw float ({view.Entries}개씩 끊으면 키 하나):");
            var frames = view.Frames;
            if (frames == null)
            {
                EmitFile(file, "      (frames가 null입니다)");
                return;
            }

            for (int i = 0; i < frames.Length; i += view.Entries)
            {
                var sb = new StringBuilder();
                sb.Append($"      [{i,4}..{Math.Min(i + view.Entries, frames.Length) - 1,4}] ");
                for (int j = i; j < i + view.Entries && j < frames.Length; j++)
                {
                    if (j > i) sb.Append(", ");
                    sb.Append(frames[j].ToString("F6"));
                }
                EmitFile(file, sb.ToString());
            }
        }

        // ── 값 해석 ──────────────────────────────────────────────────────────────

        /// <summary>알파가 어느 구간에서 내려가는지 한 문장으로 정리합니다.</summary>
        private static string DescribeAlphaFall(ColorTimelineView view, int keyCount, float duration)
        {
            if (keyCount == 0) return "키가 없습니다";

            int fall = -1;
            int lowestKey = 0;
            int belowOne = 0;

            for (int key = 0; key < keyCount; key++)
            {
                if (view.Alpha(key) < SpineColorFrames.OpaqueThreshold) belowOne++;
                if (view.Alpha(key) < view.Alpha(lowestKey)) lowestKey = key;
                if (fall < 0 && key > 0 && view.Alpha(key) < view.Alpha(key - 1) - FallEpsilon) fall = key - 1;
            }

            string tail = $"최저 a={view.Alpha(lowestKey):F3} @ t={view.Time(lowestKey):F4}초, 1 미만 키 {belowOne}/{keyCount}개";

            if (fall < 0)
            {
                return belowOne == 0
                    ? $"전 구간 불투명(감소 없음). {tail}"
                    : $"내려가는 구간은 없지만 처음부터 1 미만입니다. {tail}";
            }

            string curve = CurveType(view, fall, keyCount);
            bool stepped = curve == "stepped";
            float fallStart = stepped ? view.Time(fall + 1) : view.Time(fall);
            string ratio = duration > 0f ? $" = 애니메이션 길이 {duration:F4}초의 {fallStart / duration * 100f:F1}%" : string.Empty;

            return $"키[{fall}] t={view.Time(fall):F4}초 a={view.Alpha(fall):F3} → 키[{fall + 1}] t={view.Time(fall + 1):F4}초 a={view.Alpha(fall + 1):F3} " +
                   $"(커브 {curve}) ⇒ 옅어지기 시작하는 시각 t={fallStart:F4}초{ratio}. {tail}";
        }

        /// <summary>rgb가 고정인지, 도중에 바뀌는지 정리합니다(알파는 위에서 따로 봅니다).</summary>
        private static string DescribeColor(ColorTimelineView view, int keyCount)
        {
            if (keyCount == 0) return "키가 없습니다";

            var changes = new List<string>();
            string previous = null;

            for (int key = 0; key < keyCount; key++)
            {
                string hex = Hex(view.R(key), view.G(key), view.B(key));
                if (hex == previous) continue;

                changes.Add(key == 0 ? $"{hex}(시작)" : $"{hex}(키[{key}] t={view.Time(key):F4}초부터)");
                previous = hex;
            }

            string first = Hex(view.R(0), view.G(0), view.B(0));
            if (changes.Count == 1)
            {
                string note = first == "#FFFFFF" ? " — 흰색 고정이라 색조는 건드리지 않는 애니메이션입니다" : string.Empty;
                return $"전 구간 {first} rgb=({Rgb(view, 0)}) 고정{note}";
            }

            return $"{changes.Count - 1}번 바뀝니다: {string.Join(" → ", changes)}";
        }

        /// <summary>키와 키 사이를 어떻게 잇는지(linear/stepped/bezier). curves 배열 앞칸이 타입입니다.</summary>
        private static string CurveType(ColorTimelineView view, int key, int keyCount)
        {
            if (key >= keyCount - 1) return "(마지막 키)";

            return Safe(() =>
            {
                var curves = view.Curve != null ? view.Curve.curves : null;
                if (curves == null) return "(커브 없음)";

                int expected = (keyCount - 1) * BezierSize;
                if (curves.Length != expected) return $"(커브 배열 길이 {curves.Length}, 예상 {expected} → 해석 보류)";

                float type = curves[key * BezierSize];
                if (type == CurveLinear) return "linear";
                if (type == CurveStepped) return "stepped";
                return "bezier";
            });
        }

        private static string Classify(Il2CppSpine.Timeline timeline)
        {
            if (timeline == null) return "(null)";

            // Scale·Shear는 Translate를 상속하므로 반드시 먼저 봐야 합니다.
            if (timeline.TryCast<Il2CppSpine.ColorTimeline>() != null) return "Color";
            if (timeline.TryCast<Il2CppSpine.TwoColorTimeline>() != null) return "TwoColor";
            if (timeline.TryCast<Il2CppSpine.ScaleTimeline>() != null) return "Scale";
            if (timeline.TryCast<Il2CppSpine.ShearTimeline>() != null) return "Shear";
            if (timeline.TryCast<Il2CppSpine.TranslateTimeline>() != null) return "Translate";
            if (timeline.TryCast<Il2CppSpine.RotateTimeline>() != null) return "Rotate";
            if (timeline.TryCast<Il2CppSpine.AttachmentTimeline>() != null) return "Attachment";
            if (timeline.TryCast<Il2CppSpine.DeformTimeline>() != null) return "Deform";
            if (timeline.TryCast<Il2CppSpine.DrawOrderTimeline>() != null) return "DrawOrder";
            if (timeline.TryCast<Il2CppSpine.EventTimeline>() != null) return "Event";
            return "기타";
        }

        // ── 잡동사니 ─────────────────────────────────────────────────────────────

        private static IEnumerable<Il2CppSpine.Timeline> EnumerateTimelines(Il2CppSpine.Animation animation)
        {
            var timelines = animation != null ? animation.timelines : null;
            // ExposedList는 인덱서가 없습니다. 내부 배열(Items)이 Count보다 클 수 있어 둘 다 봅니다.
            var items = timelines != null ? timelines.Items : null;
            if (items == null) yield break;

            int count = Math.Min(timelines.Count, items.Length);
            for (int i = 0; i < count; i++) yield return items[i];
        }

        private static IEnumerable<ColorTimelineView> EnumerateColorTimelines(Il2CppSpine.Animation animation)
        {
            foreach (var timeline in EnumerateTimelines(animation))
            {
                if (SpineColorFrames.TryDescribe(timeline, out var view)) yield return view;
            }
        }

        private static string SlotName(Il2CppSpine.SkeletonData data, int slotIndex)
        {
            return Safe(() =>
            {
                var slots = data.Slots;
                if (slots == null || slotIndex < 0 || slotIndex >= slots.Count) return "(범위 밖)";
                var slot = slots.Items[slotIndex];
                return slot != null ? slot.name : "(null)";
            });
        }

        private static string SlotSetup(Il2CppSpine.SkeletonData data, int slotIndex)
        {
            return Safe(() =>
            {
                var slots = data.Slots;
                if (slots == null || slotIndex < 0 || slotIndex >= slots.Count) return "(범위 밖)";
                var slot = slots.Items[slotIndex];
                if (slot == null) return "(null)";
                return $"rgba=({Rgba(slot.r, slot.g, slot.b, slot.a)}) blend={slot.blendMode}";
            });
        }

        private static bool InUnitRange(float value) => value >= -0.0001f && value <= 1.0001f;

        private static string Mark(bool ok) => ok ? "일치 O" : "불일치 X";

        private static string Rgb(ColorTimelineView view, int key) => $"{view.R(key):F3},{view.G(key):F3},{view.B(key):F3}";

        private static string Rgba(float r, float g, float b, float a) => $"{r:F3},{g:F3},{b:F3},{a:F3}";

        private static string Hex(float r, float g, float b) => $"#{Channel(r):X2}{Channel(g):X2}{Channel(b):X2}";

        private static int Channel(float value) => (int)Math.Round(Math.Max(0f, Math.Min(1f, value)) * 255f);

        /// <summary>파일에는 항상, 콘솔에는 <paramref name="console"/>일 때만 남깁니다.</summary>
        private static void Emit(StringBuilder file, string line, bool console = true)
        {
            if (console) MelonLogger.Msg(line);
            file.AppendLine(line);
        }

        /// <summary>파일에만 남깁니다(raw float처럼 콘솔에 흘리기엔 긴 것).</summary>
        private static void EmitFile(StringBuilder file, string line) => file.AppendLine(line);

        private static string Safe(Func<string> read)
        {
            try { return read() ?? "(null)"; }
            catch (Exception ex) { return $"(예외:{ex.GetType().Name})"; }
        }

        private static T Safe<T>(Func<T> read, T fallback)
        {
            try { return read(); }
            catch (Exception) { return fallback; }
        }

        private static string MakeFileName(string animationName)
        {
            var sb = new StringBuilder("ghost_alpha_");
            var invalid = Path.GetInvalidFileNameChars();
            foreach (var c in animationName ?? "unknown")
            {
                sb.Append(Array.IndexOf(invalid, c) >= 0 ? '_' : c);
            }
            return sb.Append(".txt").ToString();
        }
    }
}
