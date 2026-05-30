using UnityEngine;
using UnityEngine.UI;

namespace muse_dash_test
{
    public static class HywTextStyler
    {
        public static void ApplyMadeByHywStyle(Text textComponent)
        {
            // 기본 텍스트 설정
            textComponent.text = "made in 화영왕";
            textComponent.fontSize = 18;
            textComponent.color = new Color(1f, 1f, 1f, 1f); // 흰색
            textComponent.fontStyle = FontStyle.Bold;
            textComponent.alignment = TextAnchor.MiddleCenter;

            // 그림자 효과 (게임 UI에 친화적)
            var shadow = textComponent.gameObject.GetComponent<Shadow>();
            if (shadow == null)
                shadow = textComponent.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.6f); // 반투명 검은색
            shadow.effectDistance = new Vector2(1, 1);

            // 외곽선 효과 (Muse Dash 스타일)
            var outline = textComponent.gameObject.GetComponent<Outline>();
            if (outline == null)
                outline = textComponent.gameObject.AddComponent<Outline>();
            outline.effectColor = new Color(0.1f, 0.1f, 0.1f, 1f); // 어두운 회색
            outline.effectDistance = new Vector2(0.5f, 0.5f);
        }
    }
}
