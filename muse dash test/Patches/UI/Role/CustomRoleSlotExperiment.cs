using System;
using HarmonyLib;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels.PnlRole;
using Il2CppSystem.Collections.Generic;

namespace muse_dash_test.Patches.UI.Role
{
    /// <summary>
    /// 실험용: 기존 기타 린(Bassist Rin) 캐릭터 슬롯을 복제하여 캐릭터 선택 UI(<see cref="PnlRole"/>)에
    /// 새 슬롯으로 끼워 넣습니다.
    /// </summary>
    internal static class CustomRoleSlotExperiment
    {
        internal static bool Enabled = true;

        internal static int SourceRoleIndex { get; private set; } = -1;
        internal static int ClonedRoleIndex { get; private set; } = -1;
        internal static int ClonedRoleOrder { get; private set; } = -1;

        private static CharacterInfo _clonedInfo;
        private static LocalCharacterInfo _clonedLocalInfo;

        internal static void EnsureClonedRoleInjected(DBConfigCharacter config)
        {
            if (!Enabled || config == null) return;

            var items = config.m_Items;
            if (items == null || items.Count == 0) return;

            if (_clonedInfo != null && ClonedRoleIndex >= 0 && ClonedRoleIndex < items.Count)
            {
                var existing = items[ClonedRoleIndex];
                if (existing != null && existing.order == ClonedRoleOrder)
                    return;
            }

            CharacterInfo source = FindGuitarRin(items);
            if (source == null)
            {
                ModLogger.Warning("[RoleSlotExp] 기타 린(원본 CharacterInfo)을 찾지 못해 실험 슬롯을 주입하지 못했습니다.");
                return;
            }

            int maxOrder = 0;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item != null && item.order > maxOrder)
                    maxOrder = item.order;
            }

            int newIndex = items.Count;
            int newOrder = maxOrder + 1;

            var clone = CloneCharacterInfo(source, newIndex, newOrder);
            items.Add(clone);

            if (config.list != null && !ReferenceEquals(config.list, items) && config.list.Count == newIndex)
            {
                config.list.Add(clone);
            }

            SourceRoleIndex = source.listIndex;
            ClonedRoleIndex = newIndex;
            ClonedRoleOrder = newOrder;
            _clonedInfo = clone;
            _clonedLocalInfo = null;

            TryInjectLocalCharacter(config);

            ModLogger.Msg(
                $"[RoleSlotExp] 기타 린 복제 슬롯 주입 완료: sourceIdx={SourceRoleIndex} (order={source.order}, cos={source.cosName}) " +
                $"-> clonedIdx={ClonedRoleIndex} (order={ClonedRoleOrder})");
        }

        private static void TryInjectLocalCharacter(DBConfigCharacter config)
        {
            var localDb = config.GetLocal();
            if (localDb == null) return;

            var localItems = localDb.m_Items;
            if (localItems == null) return;

            var localClone = GetOrCreateClonedLocalInfo(localDb);
            if (localClone == null) return;

            if (localItems.Count == ClonedRoleIndex)
            {
                localItems.Add(localClone);
                if (localDb.list != null && !ReferenceEquals(localDb.list, localItems) && localDb.list.Count == ClonedRoleIndex)
                {
                    localDb.list.Add(localClone);
                }
                ModLogger.Msg($"[RoleSlotExp] DBConfigLocalCharacter.m_Items[{ClonedRoleIndex}]에 복제 현지화 항목 추가 완료");
            }
            else if (ClonedRoleIndex >= 0 && ClonedRoleIndex < localItems.Count)
            {
                localItems[ClonedRoleIndex] = localClone;
            }
        }

        internal static LocalCharacterInfo GetOrCreateClonedLocalInfo(DBConfigLocalCharacter localDb)
        {
            if (_clonedLocalInfo != null)
                return _clonedLocalInfo;

            if (localDb == null || SourceRoleIndex < 0)
                return null;

            var src = localDb.GetInfoByIndex(SourceRoleIndex);
            if (src == null)
                return null;

            var clone = new LocalCharacterInfo
            {
                listIndex = ClonedRoleIndex,
                characterName = src.characterName,
                cosName = "[실험] 기타 린 (복제)",
                cosNames = CloneStringList(src.cosNames, "[실험] 기타 린 (복제)"),
                description = src.description,
                skill = src.skill,
                skills = src.skills,
                thiefSkill = src.thiefSkill,
                chipName = src.chipName,
                chipDescription = src.chipDescription,
                cv = src.cv,
                cvs = src.cvs,
                expressions = src.expressions,
                reviewExpressions = src.reviewExpressions,
                skinNames = src.skinNames,
                catExpressions = src.catExpressions,
                foolishExpressions = src.foolishExpressions,
                autoExpressions = src.autoExpressions
            };

            _clonedLocalInfo = clone;
            return _clonedLocalInfo;
        }

        private static CharacterInfo FindGuitarRin(List<CharacterInfo> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item == null) continue;
                if (!string.IsNullOrEmpty(item.cosName) &&
                    item.cosName.IndexOf("Bassist", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return item;
                }
            }

            return items[0];
        }

        private static CharacterInfo CloneCharacterInfo(CharacterInfo src, int newIndex, int newOrder)
        {
            var clone = new CharacterInfo
            {
                listIndex = newIndex,
                characterName = src.characterName,
                cosName = "[실험] 기타 린 (복제)",
                description = src.description,
                defaultHP = src.defaultHP,
                skillName = src.skillName,
                skill = src.skill,
                chipName = src.chipName,
                chipDescription = src.chipDescription,
                cv = src.cv,
                chipAmount = src.chipAmount,
                chipImage = src.chipImage,
                mainShow = src.mainShow,
                battleShow = src.battleShow,
                feverShow = src.feverShow,
                atkFxGreat = src.atkFxGreat,
                atkFxPerfect = src.atkFxPerfect,
                atkFxCrit = src.atkFxCrit,
                jumpFxDust = src.jumpFxDust,
                victoryShow = src.victoryShow,
                failShow = src.failShow,
                bgm = src.bgm,
                unlockType = 0,
                unlockCount = 0,
                rarity = src.rarity,
                free = true,
                hide = false,
                egg = false,
                exchange = false,
                special = src.special,
                characterType = src.characterType,
                releasedVersion = src.releasedVersion,
                order = newOrder,
                skins = src.skins,
                expressions = src.expressions,
                reviewExpressions = src.reviewExpressions,
                catExpressions = src.catExpressions,
                foolishExpressions = src.foolishExpressions,
                autoExpressionTriggerDuration = src.autoExpressionTriggerDuration,
                autoExpressions = src.autoExpressions
            };
            return clone;
        }

        private static List<string> CloneStringList(List<string> src, string firstOverride)
        {
            if (src == null) return null;
            var copy = new List<string>(src.Count);
            for (int i = 0; i < src.Count; i++)
            {
                copy.Add(i == 0 && !string.IsNullOrEmpty(firstOverride) ? firstOverride : src[i]);
            }
            return copy;
        }
    }

    [HarmonyPatch(typeof(PnlRole), nameof(PnlRole.RefreshValidRoleList))]
    internal static class PnlRole_RefreshValidRoleList_ExperimentPatch
    {
        private static void Prefix(PnlRole __instance)
        {
            try
            {
                if (!CustomRoleSlotExperiment.Enabled || __instance == null) return;
                CustomRoleSlotExperiment.EnsureClonedRoleInjected(__instance.m_ConfigCharacter);
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[RoleSlotExp] RefreshValidRoleList Prefix 실패: {ex}");
            }
        }

        private static void Postfix()
        {
            try
            {
                if (!CustomRoleSlotExperiment.Enabled) return;
                int clonedOrder = CustomRoleSlotExperiment.ClonedRoleOrder;
                if (clonedOrder < 0) return;

                var validOrders = PnlRole.s_ValidRoleOrder;
                if (validOrders != null && !validOrders.Contains(clonedOrder))
                {
                    validOrders.Add(clonedOrder);
                    ModLogger.Msg($"[RoleSlotExp] s_ValidRoleOrder에 복제 슬롯 order={clonedOrder} 추가 완료 (총 {validOrders.Count}개)");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[RoleSlotExp] RefreshValidRoleList Postfix 실패: {ex}");
            }
        }
    }

    [HarmonyPatch(typeof(DBConfigLocalCharacter), nameof(DBConfigLocalCharacter.GetInfoByIndex))]
    internal static class DBConfigLocalCharacter_GetInfoByIndex_ExperimentPatch
    {
        private static bool Prefix(DBConfigLocalCharacter __instance, int index, ref LocalCharacterInfo __result)
        {
            try
            {
                if (!CustomRoleSlotExperiment.Enabled) return true;
                if (index < 0 || index != CustomRoleSlotExperiment.ClonedRoleIndex) return true;

                var cloned = CustomRoleSlotExperiment.GetOrCreateClonedLocalInfo(__instance);
                if (cloned != null)
                {
                    __result = cloned;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[RoleSlotExp] GetInfoByIndex Prefix 실패: {ex}");
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PnlRole), nameof(PnlRole.SetProperties))]
    internal static class PnlRole_SetProperties_ExperimentPatch
    {
        private static void Prefix(PnlRole __instance)
        {
            try
            {
                if (!CustomRoleSlotExperiment.Enabled || __instance == null) return;
                var cur = __instance.m_CurrentCharacterInfo;
                if (cur == null || cur.listIndex != CustomRoleSlotExperiment.ClonedRoleIndex) return;

                if (__instance.m_CurrentLocalCharacterInfo == null)
                {
                    var localDb = __instance.m_ConfigCharacter?.GetLocal();
                    var cloned = CustomRoleSlotExperiment.GetOrCreateClonedLocalInfo(localDb);
                    if (cloned != null)
                    {
                        __instance.m_CurrentLocalCharacterInfo = cloned;
                        ModLogger.Msg("[RoleSlotExp] SetProperties에서 null이던 m_CurrentLocalCharacterInfo를 복제본으로 보정했습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[RoleSlotExp] SetProperties Prefix 실패: {ex}");
            }
        }
    }
}
