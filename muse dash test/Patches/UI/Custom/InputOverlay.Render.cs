using System;
using UnityEngine;

namespace muse_dash_test
{
    // 화면 하단 키 입력 오버레이의 좌표 계산 및 GUI 렌더링.
    public static partial class InputOverlay
    {
        private static GUIStyle airActiveStyle;
        private static GUIStyle groundActiveStyle;
        private static GUIStyle inactiveStyle;

        // UpdateTextures가 OnGUI 바깥(OnUpdate/초기화)에서 호출될 때 스타일 재생성을 미루기 위한 더티 플래그.
        // 실제 UpdateStyles()는 GUI.skin 접근이 가능한 OnGUI(DrawInputOverlay)에서만 수행합니다.
        private static bool stylesDirty = true;

        /// <summary>
        /// 설정값이나 텍스처 변경 시 GUI 스타일을 미리 캐싱하여 온가이드 가비지 생성을 차단합니다.
        /// </summary>
        public static void UpdateStyles()
        {
            if (airActiveStyle == null) airActiveStyle = new GUIStyle(GUI.skin.box);
            airActiveStyle.normal.background = airActiveTex;
            airActiveStyle.normal.textColor = Color.white;
            airActiveStyle.alignment = TextAnchor.MiddleCenter;
            airActiveStyle.fontSize = (int)fontSize;
            airActiveStyle.fontStyle = FontStyle.Bold;

            if (groundActiveStyle == null) groundActiveStyle = new GUIStyle(GUI.skin.box);
            groundActiveStyle.normal.background = groundActiveTex;
            groundActiveStyle.normal.textColor = Color.white;
            groundActiveStyle.alignment = TextAnchor.MiddleCenter;
            groundActiveStyle.fontSize = (int)fontSize;
            groundActiveStyle.fontStyle = FontStyle.Bold;

            if (inactiveStyle == null) inactiveStyle = new GUIStyle(GUI.skin.box);
            inactiveStyle.normal.background = inactiveTex;
            inactiveStyle.normal.textColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);
            inactiveStyle.alignment = TextAnchor.MiddleCenter;
            inactiveStyle.fontSize = Math.Max(12, (int)fontSize - 2);

            stylesDirty = false;
        }

        /// <summary>
        /// 키뷰어가 활성화되어 있을 때, 키뷰어 오버레이의 최상단 Y좌표를 계산하여 반환합니다.
        /// 꺼져 있거나 표시되지 않는 경우 -1f를 반환합니다.
        /// </summary>
        public static float GetOverlayTopY()
        {
            if (!showOverlay || !keysLoaded) return -1f;

            int airCount = 0;
            for (int i = 0; i < airKeys.Count; i++)
            {
                if (airKeys[i] != KeyCode.None) airCount++;
            }

            int groundCount = 0;
            for (int i = 0; i < groundKeys.Count; i++)
            {
                if (groundKeys[i] != KeyCode.None) groundCount++;
            }

            int maxKeys = Math.Max(airCount, groundCount);
            if (maxKeys == 0) return -1f;

            float screenHeight = Screen.height;
            float startY = screenHeight - (keyHeight * 2f) - spacing - offsetFromBottom;
            return startY;
        }

        /// <summary>
        /// 인게임 화면상에 실시간 키 입력 오버레이 박스를 렌더링합니다.
        /// </summary>
        public static void DrawInputOverlay()
        {
            // 실시간으로 설정 변경 감지 및 반영
            LoadConfigIfNeeded();

            if (!showOverlay) return;

            // 키 정보가 로드되지 않았다면 로드를 시도합니다.
            if (!keysLoaded)
            {
                checkTimer += Time.deltaTime;
                if (checkTimer >= CheckInterval || airKeys.Count == 0)
                {
                    checkTimer = 0f;
                    LoadPlayerKeybinds();
                }
                return;
            }

            // None이 아닌 실제 유효한 키들만 수집하여 오버레이를 그립니다. (캐시 리스트 클리어 후 사용)
            activeAirKeys.Clear();
            for (int i = 0; i < airKeys.Count; i++)
            {
                if (airKeys[i] != KeyCode.None) activeAirKeys.Add(airKeys[i]);
            }

            activeGroundKeys.Clear();
            for (int i = 0; i < groundKeys.Count; i++)
            {
                if (groundKeys[i] != KeyCode.None) activeGroundKeys.Add(groundKeys[i]);
            }

            int maxKeys = Math.Max(activeAirKeys.Count, activeGroundKeys.Count);
            if (maxKeys == 0) return;

            // 화면 가로/세로 구하기
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            float totalWidth = (maxKeys * keyWidth) + ((maxKeys - 1) * spacing);
            float startX = (screenWidth - totalWidth) / 2f;

            // 피버 바 위쪽이자 판정 텍스트 방해하지 않는 하단 중앙 구역
            float startY = screenHeight - (keyHeight * 2f) - spacing - offsetFromBottom;

            // 텍스처 및 스타일 캐시 초기화 (null일 때만 재생성)
            if (airActiveStyle == null || groundActiveStyle == null || inactiveStyle == null || airActiveTex == null || groundActiveTex == null || inactiveTex == null)
            {
                UpdateTextures();
                UpdateStyles();
            }
            // OnGUI 바깥에서 설정이 갱신되어 스타일이 더티 상태라면, GUI 컨텍스트인 지금 반영합니다.
            else if (stylesDirty)
            {
                UpdateStyles();
            }

            // 1. 공중(위쪽 행) 그리기 - 파란색
            for (int i = 0; i < activeAirKeys.Count; i++)
            {
                var key = activeAirKeys[i];
                bool isPressed = Input.GetKey(key);
                float x = startX + (i * (keyWidth + spacing));
                float y = startY;

                string keyName = CleanKeyName(key);
                GUI.Box(new Rect(x, y, keyWidth, keyHeight), keyName, isPressed ? airActiveStyle : inactiveStyle);
            }

            // 2. 지상(아래쪽 행) 그리기 - 빨간색
            for (int i = 0; i < activeGroundKeys.Count; i++)
            {
                var key = activeGroundKeys[i];
                bool isPressed = Input.GetKey(key);
                float x = startX + (i * (keyWidth + spacing));
                float y = startY + keyHeight + spacing;

                string keyName = CleanKeyName(key);
                GUI.Box(new Rect(x, y, keyWidth, keyHeight), keyName, isPressed ? groundActiveStyle : inactiveStyle);
            }
        }

        private static string CleanKeyName(KeyCode key)
        {
            string name = key.ToString();
            // 알파벳 숫자는 그대로 표시
            if (name.StartsWith("Alpha")) return name.Substring(5);
            // 방향키 간략화
            if (name == "UpArrow") return "↑";
            if (name == "DownArrow") return "↓";
            if (name == "LeftArrow") return "←";
            if (name == "RightArrow") return "→";
            if (name == "Backspace") return "BS";
            if (name == "Delete") return "DEL";
            if (name == "Escape") return "ESC";
            return name;
        }
    }
}
