using System;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppAssets.Scripts.GameCore.Skill;

namespace muse_dash_test
{
    // === 어린 악마 마리쟈 체력 감소 속도 조절 ===
    // 어린 악마 마리쟈의 스킬(EvilSkill, Character_11)은 Apply에서 BattleProperty에
    // maxHp=200, scoreExtraRate=1.25, hpChangedPerTime=-0.1을 넣습니다.
    // 게임은 GameMusic.TimerStepTrigger(0.01초 단위)에서 isHpChangable일 때
    //   if (ts % hpChangedPerTime == 0) AddHp((int)(hpChangedPerTime / |hpChangedPerTime|));
    // 를 돌려, 0.1초마다 체력을 1씩 깎습니다(초당 10). (2026-10-25 REA/ISIL 확인)
    //
    // 그래서 깎는 양은 그대로 두고 간격만 늘립니다. 간격은 0.01초 단위로 반올림합니다.
    // 게임의 시간 눈금이 0.01초라, 그 배수가 아니면 나머지 연산이 0이 되는 순간이 어긋납니다.
    // 0%면 hpChangedPerTime을 0으로 둡니다. 게임이 '0이 아닐 때만' 깎으므로 감소가 멈춥니다.
    // 커스텀 곡에만 적용하고, 공식 곡은 원래 속도 그대로 둡니다.
    [HarmonyLib.HarmonyPatch(typeof(EvilSkill), nameof(EvilSkill.Apply))]
    public class EvilSkill_Apply_HpDrain_Patch
    {
        private const decimal VanillaInterval = 0.1m;

        public static void Postfix()
        {
            try
            {
                int percent = InputOverlay.devilMarijaHpDrainPercent;
                if (percent >= 100) return;
                if (!CustomPlaySession.Current.ShouldApplyExperimentChart) return;

                var property = Il2CppAssets.Scripts.PeroTools.Commons.Singleton<BattleProperty>.instance;
                if (property == null) return;

                if (percent <= 0)
                {
                    property.hpChangedPerTime = (Il2CppSystem.Decimal)0.0;
                    ModLogger.Msg("[EvilSkillHpDrain] 어린 악마 마리쟈 체력 감소를 껐습니다 (커스텀 곡, 설정 0%).");
                    return;
                }

                decimal interval = Math.Round(VanillaInterval * 100m / percent, 2, MidpointRounding.AwayFromZero);
                property.hpChangedPerTime = (Il2CppSystem.Decimal)(double)(-interval);

                decimal effective = Math.Round(VanillaInterval / interval * 100m, 1);
                ModLogger.Msg($"[EvilSkillHpDrain] 어린 악마 마리쟈 체력 감소 간격 0.1초 → {interval}초 (설정 {percent}%, 실제 {effective}%, 커스텀 곡)");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[EvilSkillHpDrain] 체력 감소 속도 조절 중 예외: {ex}");
            }
        }
    }
}
