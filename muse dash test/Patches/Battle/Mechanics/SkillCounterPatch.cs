using Il2CppAssets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using Il2CppPeroPeroGames.GlobalDefines;

namespace muse_dash_test
{
    // === 판정 보정 스킬 횟수 999 (설정 파일 '스킬횟수999' 연동) ===
    //
    // 불량소녀 린의 "GREAT→PERFECT"와 엘핀 라봇-233의 "MISS→GREAT"는 둘 다 BattleProperty의 남은 횟수
    // (greatToPerfect @0x28, missToGreat @0x2C)만 보고 발동합니다. 원본은 RampageSkill.Apply /
    // RabbotSkill.Apply가 각각 5를 넣을 뿐이고, 발동부(SkillManager.RampageSkill ← GameTouchPlay.TouchResult,
    // SkillManager.RobotSkill ← GameMissPlay.SkillMiss)에는 캐릭터·엘핀 장착 검사가 없습니다(ISIL 확인).
    // 그래서 SkillManager.Apply가 끝난 뒤 두 횟수를 999로 덮으면, 누구를 장착했든 공식곡·커스텀 곡 모두에서 켜집니다.
    //
    // 덮는 자리를 Apply 뒤로 둔 이유: StageBattleComponent.InitGame이 BattleProperty를 새로 만든 다음
    // SkillManager.Apply를 부르고, 그 뒤의 SetSpecialSwitch는 이 두 필드를 건드리지 않습니다.

    [HarmonyLib.HarmonyPatch(typeof(SkillManager), nameof(SkillManager.Apply))]
    public class SkillManager_Apply_SkillCounterPatch
    {
        internal const int Count = 999;

        public static void Postfix()
        {
            if (!Patches.UI.Role.CustomRoleSlotExperiment.IsClonedRoleActive()) return;

            try
            {
                var property = Singleton<BattleProperty>.instance;
                if (property == null) return;

                int greatBefore = property.greatToPerfect;
                int missBefore = property.missToGreat;
                property.greatToPerfect = Count;
                property.missToGreat = Count;
                SkillCounterStats.Reset();
                ModLogger.Msg($"[SkillCounter] 판정 보정 횟수를 {Count}로 설정했습니다. (GREAT→PERFECT {greatBefore}→{Count}, MISS→GREAT {missBefore}→{Count})");
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[SkillManager.Apply.Postfix] 스킬 횟수 설정 예외: {ex}");
            }
        }
    }

    /// <summary>
    /// 두 스킬이 실제로 판정을 바꿨는지 로그로 남깁니다(관찰 전용, 판정에는 손대지 않음).
    /// 999를 넣은 것만으로는 "발동했다"는 증거가 안 되므로, 판마다 처음 몇 번과 10초 간격 누계를 찍습니다.
    /// </summary>
    internal static class SkillCounterStats
    {
        private const int VerboseFirst = 3;
        private const double SummaryIntervalSeconds = 10.0;

        private static int greatToPerfect;
        private static int missToGreat;
        private static System.DateTime lastSummary = System.DateTime.MinValue;

        internal static void Reset()
        {
            greatToPerfect = 0;
            missToGreat = 0;
            lastSummary = System.DateTime.UtcNow;
        }

        internal static void Record(bool isGreatToPerfect)
        {
            int total = isGreatToPerfect ? ++greatToPerfect : ++missToGreat;
            if (total <= VerboseFirst)
            {
                ModLogger.Msg($"[SkillCounter] {(isGreatToPerfect ? "GREAT→PERFECT" : "MISS→PERFECT")} 발동 #{total} (남은 횟수 {Remaining()})");
                return;
            }

            if ((System.DateTime.UtcNow - lastSummary).TotalSeconds < SummaryIntervalSeconds) return;
            ModLogger.Msg($"[SkillCounter] 누계: GREAT→PERFECT {greatToPerfect}회, MISS→PERFECT {missToGreat}회 (남은 횟수 {Remaining()})");
            lastSummary = System.DateTime.UtcNow;
        }

        private static string Remaining()
        {
            var property = Singleton<BattleProperty>.instance;
            return property == null ? "?" : $"G→P {property.greatToPerfect} / M→G {property.missToGreat}";
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(SkillManager), nameof(SkillManager.RampageSkill))]
    public class SkillManager_RampageSkill_SkillCounterProbe
    {
        public static void Prefix(ref byte result, out byte __state)
        {
            __state = result;
        }

        public static void Postfix(ref byte result, byte __state)
        {
            try
            {
                if (__state == (byte)TaskResult.Great && result == (byte)TaskResult.Prefect)
                    SkillCounterStats.Record(isGreatToPerfect: true);
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[SkillManager.RampageSkill.Postfix] 관찰 예외: {ex}");
            }
        }
    }

    /// <summary>
    /// 라봇-233을 장착하지 않았을 때 쓸 라봇 이펙트입니다.
    ///
    /// <para><c>SkillManager.RobotSkill</c>은 MISS를 GREAT로 바꾸기 <b>전에</b>
    /// <c>AttackEffectManager.InvokeElfinEffect()</c>로 엘핀 이펙트를 꺼내 노트 위치로 옮깁니다. 그런데
    /// <c>m_ElfinEffect</c>는 <c>InitAttackSkillFx</c>가 장착 엘핀이 라봇(인덱스 3)일 때만
    /// <c>EffectManager.Preload("elfin_carrot_robot_fx_skill", 3, -1)</c>로 채웁니다. 다른 엘핀이면 null이라
    /// 원본이 NRE를 던지고 판정은 MISS로 남습니다(2026-10-25 실측: 미스마다
    /// "During invoking native->managed trampoline … SkillManager.RobotSkill").</para>
    ///
    /// <para>그래서 같은 이펙트를 미리 로드해 두고, RobotSkill이 도는 동안에만 <c>m_ElfinEffect</c>에 끼웠다가
    /// 끝나면 null로 되돌립니다. <c>m_ElfinEffect</c>는 일시정지·콤보·실패 처리 등 다른 곳에서도 읽으므로
    /// 상시로 채워 두지 않습니다. 불량소녀 린 쪽은 <c>SetEffectLocate</c>가 null을 검사해서 이 처리가 필요 없습니다.</para>
    /// </summary>
    internal static class RabotEffectFallback
    {
        private const string EffectUid = "elfin_carrot_robot_fx_skill";
        private static Effect cached;

        internal static void Preload(AttackEffectManager manager)
        {
            cached = null;
            if (!Patches.UI.Role.CustomRoleSlotExperiment.IsClonedRoleActive() || manager == null || manager.m_ElfinEffect != null) return;

            var effectManager = Singleton<EffectManager>.instance;
            if (effectManager == null) return;
            cached = effectManager.Preload(EffectUid, 3, -1, null);
            ModLogger.Msg($"[SkillCounter] 라봇-233 미장착: MISS→PERFECT용 이펙트 '{EffectUid}'를 미리 로드했습니다. (성공={cached != null})");
        }

        /// <summary>엘핀 이펙트가 비어 있으면 대체 이펙트를 끼우고, 끼웠는지를 돌려줍니다.</summary>
        internal static bool Attach()
        {
            if (cached == null) return false;
            var manager = AttackEffectManager.instance;
            if (manager == null || manager.m_ElfinEffect != null) return false;
            manager.m_ElfinEffect = cached;
            return true;
        }

        internal static void Detach()
        {
            var manager = AttackEffectManager.instance;
            if (manager != null) manager.m_ElfinEffect = null;
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(AttackEffectManager), nameof(AttackEffectManager.InitAttackSkillFx))]
    public class AttackEffectManager_InitAttackSkillFx_RabotFallback
    {
        public static void Postfix(AttackEffectManager __instance)
        {
            try
            {
                RabotEffectFallback.Preload(__instance);
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[AttackEffectManager.InitAttackSkillFx.Postfix] 라봇 이펙트 로드 예외: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(SkillManager), nameof(SkillManager.RobotSkill))]
    public class SkillManager_RobotSkill_SkillCounterProbe
    {
        /// <summary>RobotSkill이 안에서 부르는 TouchResult를 알아보기 위한 표시입니다(게임 로직은 메인 스레드 한 줄기).</summary>
        internal static bool InsideRobotSkill;

        public static void Prefix(out bool __state)
        {
            __state = false;
            InsideRobotSkill = Patches.UI.Role.CustomRoleSlotExperiment.IsClonedRoleActive();
            try
            {
                __state = RabotEffectFallback.Attach();
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[SkillManager.RobotSkill.Prefix] 라봇 이펙트 연결 예외: {ex}");
            }
        }

        public static void Postfix(bool __result)
        {
            try
            {
                if (__result) SkillCounterStats.Record(isGreatToPerfect: false);
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[SkillManager.RobotSkill.Postfix] 관찰 예외: {ex}");
            }
        }

        // 원본이 예외를 던져도 대체 이펙트가 m_ElfinEffect에 남지 않도록 Finalizer에서 되돌립니다.
        public static void Finalizer(bool __state)
        {
            InsideRobotSkill = false;
            if (!__state) return;
            try
            {
                RabotEffectFallback.Detach();
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[SkillManager.RobotSkill.Finalizer] 라봇 이펙트 해제 예외: {ex}");
            }
        }
    }

    /// <summary>
    /// 라봇 스킬이 살려낸 노트를 GREAT가 아니라 PERFECT로 기록·표시합니다(사용자 요청, 원본은 MISS→GREAT).
    /// RobotSkill은 내부에서 <c>TouchResult(idx, Great(3), …, isElfinSkill: true)</c>를 부르므로, 그 호출만 골라
    /// resultCode를 Prefect(4)로 바꿉니다. 판정 표시·콤보·집계가 모두 이 값을 따르므로 화면과 기록이 어긋나지 않습니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameTouchPlay), nameof(Il2CppGameLogic.GameTouchPlay.TouchResult))]
    public class GameTouchPlay_TouchResult_RobotSkillPerfect
    {
        public static void Prefix(ref byte resultCode, bool isElfinSkill)
        {
            if (!SkillManager_RobotSkill_SkillCounterProbe.InsideRobotSkill || !isElfinSkill) return;
            if (resultCode == (byte)TaskResult.Great) resultCode = (byte)TaskResult.Prefect;
        }
    }
}
