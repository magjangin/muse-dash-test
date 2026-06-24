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

        // 직전에 찾은 체력바 오브젝트를 캐싱합니다. GameObject.Find는 씬 전체를 순회하는
        // 고비용 호출이라, 0.1초 주기로 이름 16개를 매번 검색하면 게임플레이 중 렉을 유발합니다.
        private static GameObject cachedHealthBar;

        public static GameObject FindHealthBar()
        {
            // 캐시가 살아 있고 활성 상태(풀링으로 비활성화되지 않음)면 검색을 건너뜁니다.
            // 파괴되었으면 Unity의 == 오버로드로 null 처리되어 자동으로 재탐색합니다.
            if (cachedHealthBar != null && cachedHealthBar.activeInHierarchy)
            {
                return cachedHealthBar;
            }

            foreach (string name in HealthBarNames)
            {
                var obj = GameObject.Find(name);
                if (obj != null)
                {
                    cachedHealthBar = obj;
                    return obj;
                }
            }

            cachedHealthBar = null;
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
