using MelonLoader;

[assembly: MelonInfo(typeof(muse_dash_test.MainMod), "muse-dash-test", "0.1.0", "화영왕")]
[assembly: MelonGame("PeroPeroGames", "MuseDash")]

namespace muse_dash_test
{
    public class MainMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("모드가 로드되었습니다.");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"씬이 로드되었습니다: {sceneName} (빌드 인덱스: {buildIndex})");
        }

        public override void OnApplicationQuit()
        {
            MelonLogger.Msg("모드가 종료되었습니다.");
        }
    }
}
