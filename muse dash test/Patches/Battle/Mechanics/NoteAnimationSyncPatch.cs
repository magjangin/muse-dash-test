using System;
using Il2Cpp;
using Il2CppFormulaBase;

namespace muse_dash_test
{
    /// <summary>
    /// 프레임 드랍/GC 히치 등으로 인해 노트가 예정된 <c>showTick</c>보다 늦게 스폰되었을 때,
    /// Spine 노트의 비행 애니메이션(<c>"in"</c>) 재생 시각(<c>currentAnimationTime</c>)을 지연 시간(<c>lag</c>)만큼
    /// 즉시 앞당겨 노트 시각 위치가 판정 타이밍보다 뒤(오른쪽)로 밀리는 엔진 버그를 보정합니다.
    ///
    /// <para><b>배경 (Cpp2IL/ISIL 역공학 확인):</b></para>
    /// <list type="bullet">
    /// <item><description>게임의 <c>SpineActionController.OnControllerStart()</c>는 스폰 직후 <c>FixSpineAnim()</c>을 호출해
    /// 스폰 지연(<c>timeFromMusicStart - showTick</c>)을 보정하려 하지만, <c>FixSpineAnim()</c>의 네이티브 구현은
    /// <c>UnityEngine.Animator</c>만 앞당기고 <c>SkeletonAnimation</c>(Spine 노트) 분기는 누락되어 있습니다.</description></item>
    /// <item><description>그 결과 프레임 히치 직후 스폰된 Spine 노트는 <c>t = 0</c> 위치(화면 오른쪽 끝)에서 비행을 시작해,
    /// 판정선에 도달할 때까지 실제 판정 시각보다 뒤로 밀려 보이게 됩니다.</description></item>
    /// </list>
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(SpineActionController), nameof(SpineActionController.PlayByKey))]
    public class SpineActionController_PlayByKey_NoteLagSync_Patch
    {
        private const string FlightActionKey = "in";
        private const float MinLagThresholdSeconds = 0.020f;
        private const float MaxLagFractionOfDt = 0.85f;

        public static void Postfix(SpineActionController __instance, string actionKey)
        {
            try
            {
                // PlayByKey는 캐릭터 및 모든 노트가 공유하는 핫 패스이므로 첫 줄에서 빠르게 걸러냅니다.
                if (!string.Equals(actionKey, FlightActionKey, StringComparison.Ordinal)) return;
                if (__instance == null || __instance.skeletonAnimation == null) return;

                var stage = StageBattleComponent.instance;
                if (stage == null || !stage.isInGame) return;

                var md = __instance.m_MusicData;
                if (md == null) return;

                float dt = (float)md.dt;
                if (dt <= 0.05f) return;

                float showTick = (float)md.showTick;
                if (showTick <= 0f) return;

                float lag = stage.timeFromMusicStart - showTick;
                if (lag <= MinLagThresholdSeconds || lag >= dt * MaxLagFractionOfDt) return;

                __instance.currentAnimationTime = lag;
                __instance.ForceUpdateSkAnim(false);

                ModLogger.Verbose($"[NoteAnimSync] 지연 스폰 노트 위치 보정: objId={md.objId}, showTick={showTick:F3}s, now={stage.timeFromMusicStart:F3}s, lag={lag:F3}s, dt={dt:F3}s");
            }
            catch (Exception ex)
            {
                ModLogger.Verbose($"[NoteAnimSync] 무시된 예외: {ex.Message}");
            }
        }
    }
}
