using UnityEngine;

namespace muse_dash_test
{
    public static partial class InputOverlay
    {
        private static string airColorName = "파랑";
        private static float airAlpha = 85f;

        private static string groundColorName = "빨강";
        private static float groundAlpha = 85f;

        private static string inactiveColorName = "검정";
        private static float inactiveAlpha = 55f;

        // GUI 단색 배경용 텍스처 캐시
        private static Texture2D airActiveTex;
        private static Texture2D groundActiveTex;
        private static Texture2D inactiveTex;

        private static string cachedAirColorHex = "";
        private static string cachedGroundColorHex = "";
        private static string cachedInactiveColorHex = "";

        private static Color ParseColorName(string name, float alphaPercent)
        {
            Color baseColor = Color.gray;
            switch (name.Trim())
            {
                case "파랑":
                case "파란색":
                    baseColor = new Color(0.2f, 0.55f, 0.9f);
                    break;
                case "빨강":
                case "빨간색":
                    baseColor = new Color(0.95f, 0.25f, 0.25f);
                    break;
                case "노랑":
                case "노란색":
                    baseColor = new Color(0.95f, 0.85f, 0.1f);
                    break;
                case "초록":
                case "초록색":
                    baseColor = new Color(0.25f, 0.8f, 0.3f);
                    break;
                case "보라":
                case "보라색":
                    baseColor = new Color(0.6f, 0.3f, 0.9f);
                    break;
                case "핑크":
                case "분홍":
                case "분홍색":
                    baseColor = new Color(0.95f, 0.4f, 0.7f);
                    break;
                case "하양":
                case "하얀색":
                case "흰색":
                    baseColor = Color.white;
                    break;
                case "검정":
                case "검은색":
                case "검정색":
                    baseColor = Color.black;
                    break;
                case "회색":
                    baseColor = new Color(0.5f, 0.5f, 0.5f);
                    break;
                case "어두운회색":
                    baseColor = new Color(0.12f, 0.12f, 0.12f);
                    break;
                default:
                    if (ColorUtility.TryParseHtmlString(name, out Color parsed))
                    {
                        baseColor = parsed;
                    }
                    break;
            }
            baseColor.a = Mathf.Clamp01(alphaPercent / 100f);
            return baseColor;
        }

        private static void UpdateTextures()
        {
            // 한글 색상명 파싱 및 갱신
            string currentAirKey = $"{airColorName}_{airAlpha}";
            if (cachedAirColorHex != currentAirKey || airActiveTex == null)
            {
                cachedAirColorHex = currentAirKey;
                Color c = ParseColorName(airColorName, airAlpha);
                if (airActiveTex != null) UnityEngine.Object.Destroy(airActiveTex);
                airActiveTex = CreateColorTexture(c);
            }

            string currentGroundKey = $"{groundColorName}_{groundAlpha}";
            if (cachedGroundColorHex != currentGroundKey || groundActiveTex == null)
            {
                cachedGroundColorHex = currentGroundKey;
                Color c = ParseColorName(groundColorName, groundAlpha);
                if (groundActiveTex != null) UnityEngine.Object.Destroy(groundActiveTex);
                groundActiveTex = CreateColorTexture(c);
            }

            string currentInactiveKey = $"{inactiveColorName}_{inactiveAlpha}";
            if (cachedInactiveColorHex != currentInactiveKey || inactiveTex == null)
            {
                cachedInactiveColorHex = currentInactiveKey;
                Color c = ParseColorName(inactiveColorName, inactiveAlpha);
                if (inactiveTex != null) UnityEngine.Object.Destroy(inactiveTex);
                inactiveTex = CreateColorTexture(c);
            }

            // 스타일 캐시 갱신은 OnGUI 컨텍스트에서만 가능(GUI.skin 접근)하므로 즉시 호출하지 않고
            // 더티 플래그만 세워 다음 OnGUI 프레임(DrawInputOverlay)에서 반영합니다.
            // (OnUpdate/초기화 등 OnGUI 바깥에서 ParseConfigFile이 호출될 때의 "You can only call GUI functions from inside OnGUI" 예외 방지)
            stylesDirty = true;
        }

        private static Texture2D CreateColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.hideFlags = HideFlags.HideAndDontSave;
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}
