// BmsNoteMatcher가 MelonLoader.MelonLogger를 직접 호출하기 때문에,
// 게임/MelonLoader 없이 로직만 돌리기 위한 최소 스텁입니다.
namespace MelonLoader
{
    public static class MelonLogger
    {
        public static void Msg(string message) { }
        public static void Msg(object message) { }
        public static void Warning(string message) { }
        public static void Warning(object message) { }
        public static void Error(string message) { }
        public static void Error(object message) { }
    }
}
