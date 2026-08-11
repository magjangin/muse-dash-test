using System;
using MelonLoader;
using Il2Cpp;

namespace muse_dash_test
{
    // === 고스트 노트 알파 조작 차단 ===
    //
    // 알파를 담당하는 메서드는 `Il2Cpp.SpineActionController.SetAlpha(float alpha)` 하나입니다.
    // 컴파일러가 만든 람다가 `→ float`/`(float)`, `→ Color`/`(Color)` 세 쌍이라
    // 스켈레톤 알파와 머티리얼·렌더러 컬러를 DOTween으로 함께 트윈하는 지점입니다.
    // 고스트 노트가 판정선 근처에서 사라지는 것은 이 호출의 결과이므로, 그 호출만 건너뜁니다.
    //
    // `public` + `float` 하나짜리 시그니처라 이 프로젝트에서 안전하다고 확인된 훅 모양입니다
    // (virtual/private/byref 훅은 로그 없는 네이티브 크래시 전력이 있습니다).
    //
    // 범위는 고스트 노트로만 좁힙니다. `SetAlpha`는 롱노트(`IsLongPressAlpha`)와 캐릭터 쪽에서도
    // 쓰이므로 무조건 막으면 그쪽 연출이 깨집니다. `m_MusicData`가 없는 호출(캐릭터 등)은 그냥 통과시킵니다.

    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.SetAlpha))]
    public class SpineActionController_SetAlpha_GhostNote_Patch
    {
        /// <summary>고스트 노트임을 가리키는 NoteConfigData.type 값.</summary>
        private const uint GhostType = 4;

        /// <summary>고스트 노트를 가리키는 UID 중간 두 자리(zzxxyy의 xx).</summary>
        private const string GhostXx = "17";

        private static bool announced;
        private static int blockedCount;
        private static DateTime lastSummaryTime = DateTime.MinValue;

        public static bool Prefix(SpineActionController __instance, float alpha)
        {
            try
            {
                if (!InputOverlay.showGhostNotes) return true;
                if (!IsGhostNote(__instance, out string uid, out uint type)) return true;

                blockedCount++;
                if (!announced)
                {
                    announced = true;
                    MelonLogger.Msg($"[GhostNote.AlphaHold] SetAlpha 차단 첫 적용: uid={uid}, type={type}, 요청 alpha={alpha:0.###} " +
                                    $"→ 호출 건너뜀 (m_HasAlpha={SafeHasAlpha(__instance)})");
                }

                LogSummaryIfDue();
                return false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[SpineActionController.SetAlpha.Prefix] 고스트 알파 차단 중 예외 발생: {ex}");
                return true;
            }
        }

        /// <summary>
        /// 이 컨트롤러가 물고 있는 노트가 고스트인지 판정합니다.
        /// type(4)과 UID(xx=17) 어느 쪽으로든 걸리게 해서, 주입 경로에 따라 한쪽만 맞는 경우도 덮습니다.
        /// </summary>
        private static bool IsGhostNote(SpineActionController controller, out string uid, out uint type)
        {
            uid = null;
            type = 0;

            var md = controller.m_MusicData;
            if (md == null) return false;

            var note = md.noteData;
            if (note == null) return false;

            uid = note.uid;
            type = note.type;

            if (type == GhostType) return true;
            return uid != null && uid.Length == 6 && uid.Substring(2, 2) == GhostXx;
        }

        private static string SafeHasAlpha(SpineActionController controller)
        {
            try { return controller.m_HasAlpha.ToString(); }
            catch (Exception) { return "(읽기 실패)"; }
        }

        private static void LogSummaryIfDue()
        {
            if ((DateTime.UtcNow - lastSummaryTime).TotalSeconds < 10.0) return;

            lastSummaryTime = DateTime.UtcNow;
            MelonLogger.Msg($"[GhostNote.AlphaHold] 누적 SetAlpha 차단 {blockedCount}회");
        }
    }
}
