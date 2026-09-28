using System.Runtime.CompilerServices;

namespace muse_dash_test
{
    /// <summary>
    /// <see cref="ModLogMessageHandler{TLevel}"/>가 어느 레벨의 로그인지 알려 주는 표식입니다.
    /// 구조체 제네릭 인자로 넘기므로 JIT가 레벨별로 따로 컴파일해 판정 비용이 상수가 됩니다.
    /// </summary>
    public interface IModLogLevelTag
    {
        ModLogLevel Level { get; }
    }

    public struct InfoLogLevel : IModLogLevelTag { public ModLogLevel Level => ModLogLevel.Info; }
    public struct WarningLogLevel : IModLogLevelTag { public ModLogLevel Level => ModLogLevel.Warning; }
    public struct VerboseLogLevel : IModLogLevelTag { public ModLogLevel Level => ModLogLevel.Verbose; }

    /// <summary>
    /// 로그 레벨이 꺼져 있으면 <c>$"..."</c> 문자열을 <b>아예 만들지 않는</b> 보간 문자열 핸들러입니다.
    ///
    /// <para><b>왜 필요한가</b>: <c>ModLogger.Verbose($"... {DescribeMusicInfo(info)}")</c>처럼 쓰면
    /// 예전에는 레벨을 확인하기 <b>전에</b> 문자열이 먼저 완성됐습니다. 레벨 판정은 그 뒤에 버릴지만
    /// 정했을 뿐이라, 기본(Info) 레벨에서도 Verbose 문자열 125곳이 매번 만들어져 버려졌고,
    /// UMPC(Error 레벨)에서는 Info 로그 350여 곳까지 같은 낭비를 했습니다. 구멍 안의 IL2CPP 필드 읽기와
    /// 메서드 호출도 전부 실행됐습니다.</para>
    ///
    /// <para>이 핸들러를 받는 오버로드가 있으면 컴파일러가 <c>$"..."</c>를 이쪽으로 보냅니다.
    /// 생성자에서 레벨이 꺼져 있다고 답하면(<c>shouldAppend = false</c>) 구멍(<c>{...}</c>) 안의 식은
    /// <b>평가조차 되지 않습니다.</b> 호출부는 고칠 필요가 없습니다.
    /// 그러니 로그 구멍 안에 "로그를 안 찍어도 꼭 실행돼야 하는 일"을 넣지 마십시오.</para>
    /// </summary>
    [InterpolatedStringHandler]
    public ref struct ModLogMessageHandler<TLevel> where TLevel : struct, IModLogLevelTag
    {
        private DefaultInterpolatedStringHandler builder;

        /// <summary>이 로그를 실제로 출력할지 여부입니다. false면 아무것도 조립되지 않았습니다.</summary>
        public readonly bool IsEnabled;

        public ModLogMessageHandler(int literalLength, int formattedCount, out bool shouldAppend)
        {
            IsEnabled = ModLogger.IsLevelEnabled(default(TLevel).Level);
            shouldAppend = IsEnabled;
            builder = IsEnabled ? new DefaultInterpolatedStringHandler(literalLength, formattedCount) : default;
        }

        public void AppendLiteral(string value) => builder.AppendLiteral(value);

        public void AppendFormatted<T>(T value) => builder.AppendFormatted(value);

        public void AppendFormatted<T>(T value, string format) => builder.AppendFormatted(value, format);

        public void AppendFormatted<T>(T value, int alignment) => builder.AppendFormatted(value, alignment);

        public void AppendFormatted<T>(T value, int alignment, string format) => builder.AppendFormatted(value, alignment, format);

        public void AppendFormatted(string value) => builder.AppendFormatted(value);

        public void AppendFormatted(object value, int alignment = 0, string format = null) => builder.AppendFormatted(value, alignment, format);

        /// <summary>조립한 문자열을 돌려주고 내부 버퍼를 반납합니다. <see cref="IsEnabled"/>가 true일 때만 부르십시오.</summary>
        public string ToStringAndClear() => builder.ToStringAndClear();
    }
}
