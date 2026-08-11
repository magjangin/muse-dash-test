using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace muse_dash_test
{
    /// <summary>
    /// Spine 컬러 타임라인의 <c>frames</c> 배열을 어떻게 읽는지 정한 곳입니다.
    /// 알파를 덮어쓰는 패치와, 원본을 그대로 찍어 두는 진단이 같은 규칙을 씁니다.
    ///
    /// 근거는 런타임 원본의 시그니처입니다(Decompiled_SpineUnity에서 확인):
    ///   ColorTimeline.SetFrame(int frameIndex, float time, float r, float g, float b, float a)
    ///   TwoColorTimeline.SetFrame(int frameIndex, float time, float r, float g, float b, float a, float r2, float g2, float b2)
    /// 즉 frames는 키 하나가 [time, r, g, b, a(, r2, g2, b2)] 순서로 평평하게 이어 붙은 배열입니다.
    /// 이 가정이 실제로 맞는지는 <see cref="GhostNoteAlphaDiagnostics"/>가 런타임 값으로 다시 검증합니다.
    /// </summary>
    internal static class SpineColorFrames
    {
        /// <summary>ColorTimeline 키 한 칸의 float 개수: time, r, g, b, a.</summary>
        internal const int ColorEntries = 5;

        /// <summary>TwoColorTimeline 키 한 칸의 float 개수: time, r, g, b, a, r2, g2, b2.</summary>
        internal const int TwoColorEntries = 8;

        /// <summary>키 한 칸 안에서 각 값이 놓인 자리. 알파 자리는 두 타임라인이 같습니다.</summary>
        internal const int TimeOffset = 0;
        internal const int ROffset = 1;
        internal const int GOffset = 2;
        internal const int BOffset = 3;
        internal const int AlphaOffset = 4;

        /// <summary>불투명으로 볼 기준값. float 반올림 오차를 감안했습니다.</summary>
        internal const float OpaqueThreshold = 0.999f;

        /// <summary>
        /// 컬러 계열 타임라인이면 <paramref name="view"/>를 채우고 true를 돌려줍니다.
        /// 이동·스케일·회전 등 나머지 타임라인은 false입니다(건드리지 않습니다).
        /// </summary>
        internal static bool TryDescribe(Il2CppSpine.Timeline timeline, out ColorTimelineView view)
        {
            view = default;
            if (timeline == null) return false;

            var color = timeline.TryCast<Il2CppSpine.ColorTimeline>();
            if (color != null)
            {
                view = new ColorTimelineView(color, color.frames, ColorEntries, color.slotIndex, isTwoColor: false);
                return true;
            }

            var twoColor = timeline.TryCast<Il2CppSpine.TwoColorTimeline>();
            if (twoColor != null)
            {
                view = new ColorTimelineView(twoColor, twoColor.frames, TwoColorEntries, twoColor.slotIndex, isTwoColor: true);
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 컬러 타임라인 하나를 "키 번호로" 읽게 해 주는 얇은 껍데기입니다.
    /// 평평한 float 배열에 매번 <c>key * Entries + 오프셋</c>을 손으로 쓰지 않으려고 둡니다.
    /// </summary>
    internal readonly struct ColorTimelineView
    {
        internal ColorTimelineView(Il2CppSpine.CurveTimeline curve, Il2CppStructArray<float> frames,
                                   int entries, int slotIndex, bool isTwoColor)
        {
            Curve = curve;
            Frames = frames;
            Entries = entries;
            SlotIndex = slotIndex;
            IsTwoColor = isTwoColor;
        }

        /// <summary>ColorTimeline·TwoColorTimeline 모두 CurveTimeline을 상속하므로 커브 배열을 여기서 읽습니다.</summary>
        internal Il2CppSpine.CurveTimeline Curve { get; }

        internal Il2CppStructArray<float> Frames { get; }
        internal int Entries { get; }
        internal int SlotIndex { get; }
        internal bool IsTwoColor { get; }

        internal string TypeName => IsTwoColor ? "TwoColorTimeline" : "ColorTimeline";
        internal int FloatCount => Frames != null ? Frames.Length : 0;

        /// <summary>[time,r,g,b,a] 가정으로 계산한 키 개수. 런타임 FrameCount와 비교해 가정을 검증합니다.</summary>
        internal int KeyCount => Entries > 0 ? FloatCount / Entries : 0;

        internal float Time(int key) => Frames[key * Entries + SpineColorFrames.TimeOffset];
        internal float R(int key) => Frames[key * Entries + SpineColorFrames.ROffset];
        internal float G(int key) => Frames[key * Entries + SpineColorFrames.GOffset];
        internal float B(int key) => Frames[key * Entries + SpineColorFrames.BOffset];
        internal float Alpha(int key) => Frames[key * Entries + SpineColorFrames.AlphaOffset];
    }
}
