using MelonLoader;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.GameCore.GameObjectLogics.ExtraControl;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using Il2CppAssets.Scripts.PeroTools.Nice.Actions;
using Il2CppAssets.Scripts.PeroTools.Nice.Components;
using Il2CppAssets.Scripts.PeroTools.Nice.Events;
using Il2CppAssets.Scripts.PeroTools.Nice.Interface;
using Il2CppAssets.Scripts.UI;
using Il2CppAssets.Scripts.UI.Controls;
using Il2CppAssets.Scripts.UI.GameMain;
using Il2CppAssets.Scripts.UI.Panels;
using Il2CppAssets.Scripts.UI.Panels.PnlRole;
using Il2CppPeroPeroGames.GlobalDefines;
using Il2CppPeroTools2.Resources;
using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Action = System.Action;
using Object = UnityEngine.Object;

namespace muse_dash_test
{
    [Harmony]
    public static class FavManager
    {
        public static PnlStage stagePnl;
        public static GameObject girlTxt;
        public static List<int> _oldGirl = new();

        internal static Component CopyComponent(Component original, GameObject destination)
        {
            var type = original.GetIl2CppType();
            var copy = destination.AddComponent(type);
            Il2CppSystem.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (var field in fields) field.SetValue(copy, field.GetValue(original));
            return copy;
        }

        public static bool ValidGirl(int girl)
        {
            return ValidGirl((GirlID)girl);
        }

        public static bool ValidGirl(GirlID girl)
        {
            if (girl == GirlID.NONE || !Enum.IsDefined(typeof(GirlID), girl)) return false;
            if (CharacterDefine.IsTouhouRole(DataHelper.selectedRoleIndex) &&
                !CharacterDefine.IsTouhouRole((int)girl)) return false;

            foreach (var item in DataHelper.items)
                if (item["type"].Cast<IVariable>().GetResult<string>() != "character")
                    continue;
                else if (item["index"].Cast<IVariable>().GetResult<int>() == (int)girl)
                    return item["isUnlock"].Cast<IVariable>().GetResult<bool>();
            return false;
        }

        public static void PrefixStoreGirlDoThing(bool targetGlobal, Action act = null)
        {
            try
            {
                var dataID = targetGlobal
                    ? GlobalDataBase.s_DbBattleStage.m_SelectedRole
                    : DataHelper.selectedRoleIndex;
                if (ValidGirl(FavSave.FavGirl) && ValidGirl(dataID))
                {
                    _oldGirl.Add(dataID);
                    if (targetGlobal)
                        GlobalDataBase.s_DbBattleStage.m_SelectedRole = (int)FavSave.FavGirl;
                    else
                    {
                        DataHelper.selectedRoleIndex = (int)FavSave.FavGirl;
                    }
                }

                act?.Invoke();
            }
            catch (Exception e)
            {
                MelonLogger.Error($"[FavManager] PrefixStoreGirlDoThing 예외: {e}");
            }
        }

        public static void PostfixRestoreGirlDoThing(bool targetGlobal, Action act = null)
        {
            try
            {
                var dataID = targetGlobal
                    ? GlobalDataBase.s_DbBattleStage.m_SelectedRole
                    : DataHelper.selectedRoleIndex;
                if (ValidGirl(FavSave.FavGirl) && ValidGirl(dataID))
                {
                    var girlIdx = _oldGirl[_oldGirl.Count - 1];
                    _oldGirl.RemoveAt(_oldGirl.Count - 1);
                    if (targetGlobal)
                        GlobalDataBase.s_DbBattleStage.m_SelectedRole = girlIdx;
                    else
                        DataHelper.selectedRoleIndex = girlIdx;
                }

                act?.Invoke();
            }
            catch (Exception e)
            {
                MelonLogger.Error($"[FavManager] PostfixRestoreGirlDoThing 예외: {e}");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PnlStage), nameof(PnlStage.PreWarm))]
        private static void PnlStagePreWarmPostfix(PnlStage __instance)
        {
            stagePnl = __instance;
            
            // 실시간 교체 시스템 초기화
            RealTimeSwapManager.Initialize();
        }
    }

    [HarmonyPatch(typeof(AbstractGirlManager), nameof(AbstractGirlManager.InstanceGirl))]
    internal class GirlInstancePatch
    {
        private static void Prefix()
        {
            FavManager.PrefixStoreGirlDoThing(true);
        }

        private static void Postfix()
        {
            FavManager.PostfixRestoreGirlDoThing(true, () =>
            {
                var currentRole = GlobalDataBase.s_DbBattleStage.m_SelectedRole;
                var favGirl = FavSave.FavGirl;
                MelonLogger.Msg($"[FavGirl] 현재 스킬 캐릭터: {currentRole}, 즐겨찾기 캐릭터: {favGirl}");
                
                if (GlobalDataBase.s_DbBattleStage.m_SelectedRole == (int)GirlID.RIN_SLEEP &&
                    FavSave.FavGirl != GirlID.RIN_SLEEP && FavSave.FavGirl != GirlID.NONE)
                {
                    // Sleepwalker 스킬을 사용하는 비-Sleepwalker에게 수면 파티클 적용
                    try
                    {
                        Object.Instantiate(
                            SingletonScriptableObject<ResourcesManager>.instance.LoadFromName<GameObject>(
                                "fx_sleep_skill"), GlobalManagers.girlManager.girl.transform);
                    }
                    catch (Exception e)
                    {
                        MelonLogger.Error($"[FavGirl] Sleepwalker 수면 파티클 생성 실패: {e}");
                    }
                }

                if (FavSave.FavGirl == GirlID.RIN_SLEEP &&
                    GlobalDataBase.s_DbBattleStage.m_SelectedRole != (int)GirlID.RIN_SLEEP)
                {
                    // 깨어있는 Sleepwalker에서 수면 파티클 제거
                    GlobalManagers.girlManager.girl.transform.GetChild(0).gameObject.SetActive(false);
                }
                
                // 블랙 마리쟈 스킬 효과 (판정 범위 확장 - 시각적 효과 없음)
                if (GlobalDataBase.s_DbBattleStage.m_SelectedRole == (int)GirlID.MARIJA_BLACK &&
                    FavSave.FavGirl != GirlID.MARIJA_BLACK && FavSave.FavGirl != GirlID.NONE)
                {
                    MelonLogger.Msg("[FavGirl] 블랙소녀 스킬 활성화됨 (판정 범위 확장)");
                }
                
                // 블랙 마리쟈 스킬 효과 제거
                if (FavSave.FavGirl == GirlID.MARIJA_BLACK &&
                    GlobalDataBase.s_DbBattleStage.m_SelectedRole != (int)GirlID.MARIJA_BLACK)
                {
                    MelonLogger.Msg("[FavGirl] 블랙 마리쟈 스킬 효과 제거됨");
                }
            });
        }
    }

    [HarmonyPatch(typeof(AbstractGirlManager), nameof(AbstractGirlManager.AwakeInit))]
    internal class GirlInitPatch
    {
        private static void Prefix()
        {
            FavManager.PrefixStoreGirlDoThing(true);
        }

        private static void Postfix()
        {
            FavManager.PostfixRestoreGirlDoThing(true);
        }
    }

    [HarmonyPatch(typeof(MuseShow), nameof(MuseShow.OnEnable))]
    internal class MuseShowPatch
    {
        private static void Prefix()
        {
            FavManager.PrefixStoreGirlDoThing(false);
        }

        private static void Postfix()
        {
            FavManager.PostfixRestoreGirlDoThing(false);
        }
    }

    [HarmonyPatch(typeof(CharCreate), nameof(CharCreate.OnEnable))]
    internal class CharCreatePatch
    {
        private static void Prefix(CharCreate __instance)
        {
            FavManager.PrefixStoreGirlDoThing(false);
        }

        private static void Postfix(CharCreate __instance)
        {
            FavManager.PostfixRestoreGirlDoThing(false);
        }
    }

    [HarmonyPatch]
    internal class OnVictoryPatch
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            return typeof(PnlVictory).GetMethods().Where(m => m.Name == nameof(PnlVictory.OnVictory));
        }

        private static void Prefix()
        {
            FavManager.PrefixStoreGirlDoThing(true);
        }

        private static void Postfix(PnlVictory __instance)
        {
            FavManager.PostfixRestoreGirlDoThing(true, () =>
            {
                var shouldHideDetails = FavSave.conditionalHideScoreDetails.Value && (FavSave.FavGirl == GirlID.NONE ||
                    (int)FavSave.FavGirl == DataHelper.selectedRoleIndex ||
                    !FavManager.ValidGirl(DataHelper.selectedRoleIndex));
                if (FavManager.girlTxt == null && !shouldHideDetails)
                {
                    var scoreText = __instance.m_CurControls.scoreTxt.transform.parent.gameObject;
                    var tittleText = __instance.m_CurControls.accuracyTxt.transform.parent.parent.gameObject;
                    var accText = __instance.m_CurControls.accuracyTxt.transform.parent;

                    var girlText = Object.Instantiate(accText, tittleText.transform);
                    var buildText = Object.Instantiate(accText, tittleText.transform);

                    girlText.name = "TxtGirl";
                    FavManager.girlTxt = girlText.gameObject;

                    buildText.name = "TxtBuild";
                    buildText.gameObject.GetComponent<Text>().text = "/";
                    Object.Instantiate(girlText.GetChild(0), buildText.transform);
                    girlText.gameObject.SetActive(false);
                    buildText.gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    
                    buildText.GetChild(0).gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
                    buildText.GetChild(0).localPosition = new Vector3(-120, buildText.GetChild(0).localPosition.y,
                        buildText.GetChild(0).localPosition.z);
                    buildText.GetChild(0).gameObject.GetComponent<Text>().text =
                        $"{Singleton<ConfigManager>.instance.GetConfigStringValue("character", DataHelper.selectedRoleIndex, "cosName")}";
                    
                    buildText.GetChild(1).gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                    buildText.GetChild(1).gameObject.GetComponent<Text>().text = "None";
                    buildText.GetChild(1).localPosition = new Vector3(115, buildText.GetChild(1).localPosition.y,
                        buildText.GetChild(1).localPosition.z);
                    buildText.localPosition =
                        new Vector3(-20 + buildText.GetChild(0).GetComponent<Text>().preferredWidth, 80f, -2f);
                }
            });
        }
    }

    [HarmonyPatch(typeof(StatisticsManager), nameof(StatisticsManager.OnBattleEnd))]
    internal class PPGWasGayHere
    {
        private static void Prefix()
        {
            if (DataHelper.selectedElfinIndex != GlobalDataBase.s_DbBattleStage.m_SelectedElfin)
            {
                MelonLogger.Warning("[FavGirl] 배틀 엘핀과 실제 엘핀이 다릅니다. 조정 중!");
                GlobalDataBase.s_DbBattleStage.m_SelectedElfin = DataHelper.selectedElfinIndex;
            }
        }
    }

    [HarmonyPatch(typeof(RoleBattleSubControl), nameof(RoleBattleSubControl.Init))]
    internal class RoleBattleSubControlInitPatch
    {
        private static void Postfix(RoleBattleSubControl __instance)
        {
            // 비-Touhou 스킬로 즐겨찾기된 Touhou 캐릭터를 사용하는 경우 일반 컨트롤러 강제 적용
            if (CharacterDefine.IsTouhouRole((int)FavSave.FavGirl) &&
                !CharacterDefine.IsTouhouRole(FavManager._oldGirl[FavManager._oldGirl.Count - 1]))
                __instance.m_Animator.runtimeAnimatorController = __instance.m_NormalController;
        }
    }

    [HarmonyPatch(typeof(StageLikeToggle), nameof(StageLikeToggle.OnHideMusic))]
    internal class OnHideMusicPatch
    {
        private static bool Prefix(StageLikeToggle __instance)
        {
            if (__instance == null || __instance.name != "TglLike") return false;
            return true;
        }
    }
}
