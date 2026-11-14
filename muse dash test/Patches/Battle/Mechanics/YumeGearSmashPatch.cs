using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppFormulaBase;
using Il2CppGameLogic;

namespace muse_dash_test
{
    // === 유메 톱니 부수기 (설정 파일 '유메톱니부수기' 연동) ===
    //
    // 유메(GCSkill, Character_15)의 스킬은 BattleProperty.isGcCharacter(@0x12B) 하나로 켜집니다. GCSkill.Apply가
    // 이 값을 true로 두고(그리고 HP 250), 이 값을 읽는 곳은 GameMissPlay.MissCube 한 군데뿐입니다(ISIL 확인).
    // 그래서 SkillManager.Apply 뒤에 true로 덮으면 캐릭터와 무관하게 톱니를 부술 수 있습니다. HP는 건드리지 않습니다.
    //
    // 원본 판정 흐름(GameMusic.TimerStepTrigger → MissCube):
    //   - 톱니(NoteType.Block=2)가 판정 시점에 아직 결과가 없으면 MissCube를 부릅니다.
    //   - 유메면 MissCube가 m_MissHardTime <= 0일 때 SetPlayResult(idx, Miss=1) + 업적 카운트(3001) 후 true를 돌려주고,
    //     TimerStepTrigger는 SkillManager.YumeSkill(idx)로 부수는 연출("note_destroy" + sfx_block)만 하고
    //     점수 경로를 건너뜁니다. 그래서 부순 톱니는 피한 톱니와 달리 점수·톱니 통과 수가 오르지 않았습니다.
    //   - MissCube가 false를 돌려주면 TimerStepTrigger는 정상 통과 경로(TaskStageTarget.AddScore →
    //     SetPlayResult(idx, 4) → AddBlock)로 갑니다. 유메 분기는 m_MissHardTime > 0일 때도 피해 없이 false입니다.
    //
    // 이 패치는 "톱니 + 유메 플래그"일 때만 MissCube 원본을 건너뛰고, 원본과 같은 조건에서 YumeSkill 연출을 직접 부른 뒤
    // false를 돌려줍니다. 기록은 게임의 정상 통과 경로가 그대로 하므로 피한 톱니와 똑같이 집계됩니다.
    // 원본이 하던 업적 카운트(3001, 톱니 파괴 수)는 부르지 않습니다.

    [HarmonyLib.HarmonyPatch(typeof(SkillManager), nameof(SkillManager.Apply))]
    public class SkillManager_Apply_YumeGearSmashPatch
    {
        public static void Postfix()
        {
            if (!Patches.UI.Role.CustomRoleSlotExperiment.IsClonedRoleActive()) return;

            try
            {
                var property = Singleton<BattleProperty>.instance;
                if (property == null) return;

                bool before = property.isGcCharacter;
                property.isGcCharacter = true;
                YumeGearSmashPatch.ResetBattle();
                ModLogger.Msg($"[YumeGear] 유메 톱니 부수기를 켰습니다. (isGcCharacter {before}→true, 부순 톱니는 통과로 기록)");
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[SkillManager.Apply.Postfix] 유메 톱니 설정 예외: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(GameMissPlay), nameof(GameMissPlay.MissCube))]
    public class YumeGearSmashPatch
    {
        private const uint NoteTypeBlock = 2;
        private const int VerboseFirst = 3;

        // 톱니 하나에는 판정 시간 칸(TimeNodeOrder)이 여러 개 있어, 충돌 구간(tno.result==3) 동안 캐릭터가 같은 줄에
        // 있으면 칸마다 MissCube가 불립니다. 원본은 첫 칸에서 Miss를 기록해 나머지를 막지만, 이 패치는 기록을 마지막 칸
        // (tno.isLast)의 통과 경로에 맡기므로 그사이 호출이 반복됩니다(2026-10-25 실측: idx=2가 한 프레임에 3번 이상).
        // 그래서 부수는 연출은 톱니마다 한 번만 냅니다.
        private static readonly System.Collections.Generic.HashSet<int> smashedIdx = new System.Collections.Generic.HashSet<int>();

        internal static void ResetBattle() => smashedIdx.Clear();

        public static bool Prefix(GameMissPlay __instance, int idx, ref bool __result)
        {
            if (!Patches.UI.Role.CustomRoleSlotExperiment.IsClonedRoleActive()) return true;

            try
            {
                var property = Singleton<BattleProperty>.instance;
                if (property == null || !property.isGcCharacter) return true;

                var stage = StageBattleComponent.instance;
                if (stage == null || stage.GetMusicDataByIdx(idx).noteData.type != NoteTypeBlock) return true;

                // 원본 유메 분기와 같은 조건에서만 부수는 연출을 냅니다(피격 무적 중이면 연출 없이 통과).
                __result = false;
                if ((double)__instance.m_MissHardTime <= 0.0 && smashedIdx.Add(idx))
                {
                    SkillManager.YumeSkill(idx);
                    if (smashedIdx.Count <= VerboseFirst)
                        ModLogger.Msg($"[YumeGear] 톱니를 부쉈습니다 #{smashedIdx.Count} (idx={idx}, 통과로 기록)");
                }
                return false;
            }
            catch (System.Exception ex)
            {
                ModLogger.Error($"[GameMissPlay.MissCube.Prefix] 유메 톱니 처리 예외, 원본으로 넘깁니다: {ex}");
                return true;
            }
        }
    }
}
