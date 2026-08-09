using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Structs;
using Il2CppSystem.Collections.Generic;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 커스텀(실험/BMS) 차트로 플레이할 때 원곡에 딸려오는 대사(Dialog) 데이터를 비우는 로직입니다.
    /// <para>
    /// 차트 주입은 노트만 교체하므로 <c>DBStageInfo.sceneDialogEvents</c>(씬별 대사) /
    /// <c>sceneDialogDictionary</c>(시간별 대사)는 원곡 그대로 남아, 새 채보와 상관없는 대사가 화면에 뜹니다.
    /// 대사 데이터가 실제로 채워지는 시점(SetDialogArgs / DialogDataToDic) 직후에 이 딕셔너리들을
    /// 빈 딕셔너리로 교체하여, 이후 틱 트리거가 찾을 대사 자체가 없게 만듭니다.
    /// </para>
    /// <para>
    /// 기존 딕셔너리를 <c>Clear()</c>하지 않고 <b>새 빈 딕셔너리로 교체</b>합니다.
    /// 전달된 원본 딕셔너리는 다른 곳에서 캐시/공유될 수 있어, 내용을 지우면 순정 곡 플레이까지
    /// 영향이 남을 수 있기 때문입니다. 교체 방식은 다음 곡 로드 시 원본 데이터가 그대로 다시 채워집니다.
    /// </para>
    /// </summary>
    internal static class DialogClearer
    {
        private const string Tag = "[DialogClear]";

        /// <summary>false로 두면 대사 제거 기능이 꺼지고 게임 원본 동작을 그대로 사용합니다.</summary>
        internal static readonly bool Enabled = true;

        /// <summary>커스텀 차트를 주입한 플레이인지 여부입니다.</summary>
        internal static bool ShouldClear()
        {
            return Enabled && CustomPlaySession.Current.ShouldApplyExperimentChart;
        }

        /// <summary>
        /// DBStageInfo의 대사 딕셔너리 두 개를 빈 딕셔너리로 교체합니다.
        /// </summary>
        /// <param name="caller">로그에 남길 호출 지점 이름.</param>
        internal static void ClearStageDialogs(DBStageInfo instance, string caller)
        {
            if (instance == null) return;

            int sceneCount = -1;
            int timeCount = -1;

            try
            {
                var events = instance._sceneDialogEvents_k__BackingField;
                if (events != null) sceneCount = events.Count;
                instance._sceneDialogEvents_k__BackingField = new Dictionary<string, List<GameDialogArgs>>();
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} sceneDialogEvents 비우기 실패({caller}): {ex.Message}");
            }

            try
            {
                var dic = instance._sceneDialogDictionary_k__BackingField;
                if (dic != null) timeCount = dic.Count;
                instance._sceneDialogDictionary_k__BackingField = new Dictionary<Il2CppSystem.Decimal, List<GameDialogArgs>>();
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} sceneDialogDictionary 비우기 실패({caller}): {ex.Message}");
            }

            // 이미 비어 있던 곡(대사 없는 곡)에서는 로그를 남기지 않습니다.
            if (sceneCount > 0 || timeCount > 0)
            {
                MelonLogger.Msg($"{Tag} 커스텀 차트 플레이 → 원곡 대사 제거({caller}): 씬 {sceneCount}개, 시간 키 {timeCount}개");
            }
        }
    }
}

// =====================================================================
// DBStageInfo.SetDialogArgs 훅
// 대사 데이터가 채워지는 지점. 채워진 직후 곧바로 비웁니다.
// =====================================================================
[HarmonyLib.HarmonyPatch(typeof(DBStageInfo), "SetDialogArgs")]
public static class DBStageInfo_SetDialogArgs_ClearPatch
{
    public static void Postfix(DBStageInfo __instance)
    {
        try
        {
            if (!muse_dash_test.DialogClearer.ShouldClear()) return;
            muse_dash_test.DialogClearer.ClearStageDialogs(__instance, "SetDialogArgs");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[DialogClear] SetDialogArgs 후처리 실패: {ex.Message}");
        }
    }
}

// =====================================================================
// DBStageInfo.DialogDataToDic 훅
// sceneDialogEvents → sceneDialogDictionary 재구성 지점.
// SetDialogArgs 외의 경로로 재구성되는 경우까지 덮습니다.
// =====================================================================
[HarmonyLib.HarmonyPatch(typeof(DBStageInfo), "DialogDataToDic")]
public static class DBStageInfo_DialogDataToDic_ClearPatch
{
    public static void Postfix(DBStageInfo __instance)
    {
        try
        {
            if (!muse_dash_test.DialogClearer.ShouldClear()) return;
            muse_dash_test.DialogClearer.ClearStageDialogs(__instance, "DialogDataToDic");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[DialogClear] DialogDataToDic 후처리 실패: {ex.Message}");
        }
    }
}
