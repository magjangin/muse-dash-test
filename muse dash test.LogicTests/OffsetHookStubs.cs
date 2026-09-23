// 실제 Postfix 소스를 게임 밖에서 호출하기 위한 경계 스텁.
// 게임 원본 메서드 및 Harmony 실행 순서를 시뮬레이션하지는 않습니다.
namespace HarmonyLib
{
    public enum MethodType { Getter }
    public class HarmonyPatch : System.Attribute
    {
        public HarmonyPatch() { }
        public HarmonyPatch(System.Type type, string name) { }
        public HarmonyPatch(System.Type type, string name, MethodType method) { }
    }
    public class HarmonyPostfix : System.Attribute { }
}
namespace Il2CppFormulaBase
{
    public class StageBattleComponent
    {
        public float offset;
        public void FixedOffset() { }
        public void FixedMusicOffset() { }
    }
}
namespace UnityEngine { public class AudioSource { public string name; } }
namespace Il2CppAssets.Scripts.Database
{
    public class DBStageInfo { public double delay; }
    public class DataHelper { public static int offset; }
}
namespace Il2CppAssets.Scripts.UI.Panels { }
namespace Il2CppSystem
{
    public class Decimal
    {
        private double value;
        public static explicit operator Decimal(double value) => new Decimal { value = value };
        public override string ToString() => value.ToString();
    }
}
namespace muse_dash_test
{
    public class CustomPlaySession
    {
        public static CustomPlaySession Current { get; } = new();
        public string SelectedMusicUid;
        public string LastClickedMusicUid;
        // 게임 밖에는 곡 선택 화면이 없으므로 실제 구현의 2단계(PnlStage 탐색)는 늘 비어 있는 셈입니다.
        public string LastKnownMusicUid => !string.IsNullOrEmpty(SelectedMusicUid) ? SelectedMusicUid : LastClickedMusicUid;
    }
    public static class CustomContentIds
    {
        public static bool IsVirtualSong(string uid) => uid.StartsWith("1999-");
    }
    public static class HwaResourceManager
    {
        public static HwaManifest Manifest;
        public static HwaManifest GetManifest(string uid) => Manifest;
    }
}
