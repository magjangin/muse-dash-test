using UnityEngine;

namespace made_by_hyw.GameObjects
{
    public static class HealthBarFinder
    {
        // 체력바 관련 오브젝트 이름들
        private static readonly string[] HealthBarNames = {
            "TxtHealthValue", "HealthValue", "UIValue", "SldHp", "ImgHpBg",
            "Slider_Hp", "HealthBar", "HP_Slider", "HpSlider", "Health_Slider",
            "Slider_Health", "HPBar", "HealthBar_Slider", "Health", "HP", "Hp"
        };

        public static GameObject FindHealthBar()
        {
            foreach (string name in HealthBarNames)
            {
                var obj = GameObject.Find(name);
                if (obj != null) return obj;
            }
            return null;
        }

        public static UnityEngine.UI.Text FindHealthText()
        {
            var healthBar = FindHealthBar();
            if (healthBar != null)
            {
                return healthBar.GetComponentInChildren<UnityEngine.UI.Text>();
            }
            return null;
        }
    }
}
