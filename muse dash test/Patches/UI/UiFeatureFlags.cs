namespace muse_dash_test
{
    internal static class UiFeatureFlags
    {
        public static bool EnableUiOverrides = true;

        public static bool IsUiOverridesEnabled()
        {
            return EnableUiOverrides;
        }
    }
}