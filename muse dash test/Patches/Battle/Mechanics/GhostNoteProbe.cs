using System;
using System.Collections.Generic;
using System.Text;
using MelonLoader;

namespace muse_dash_test
{
    // === 고스트 노트(UID zz17yy, type 4) 렌더 페이드 관찰 프로브 ===
    //
    // 값은 하나도 바꾸지 않습니다. 어셈블리에서 노트 렌더러를 페이드시키는 유일한 클래스인
    // NormalNoteVisibleController가 "어떤 노트에 붙는지"를 확인하기 위한 진단입니다.
    //   - type 4(고스트)에만 붙는다  → 고스트 전용 컨트롤러이므로 여기만 만지면 됩니다.
    //   - 일반 노트에도 붙는다       → 페이드 여부는 컨트롤러가 아니라 다른 조건이 가릅니다.
    //
    // OnAppear(bool)/Init()은 virtual final이라 후킹하지 않습니다. 이 프로젝트에서 virtual 훅은
    // 로그도 남기지 않는 네이티브 크래시를 낸 전력이 있습니다(커밋 7c5e685). 대신 non-virtual public인
    // NoteMData 세터를 잡고, 자동 프로퍼티 백킹 필드(_NoteMData_k__BackingField, public)에서 값을 읽습니다.
    //
    // 노트는 한 곡에 수백 개라 UID 단위로 최초 1회만 상세를 남기고, 나머지는 10초 요약으로 접습니다.

    internal static class GhostNoteProbeStats
    {
        private const double SummaryIntervalSeconds = 10.0;

        private static readonly HashSet<string> seenUids = new HashSet<string>();
        private static readonly Dictionary<uint, int> countByType = new Dictionary<uint, int>();
        private static int total;
        private static bool pendingSummary;
        private static DateTime lastSummaryTime = DateTime.MinValue;

        /// <summary>이 UID를 처음 보는지 여부. 처음이면 상세 로그를 남길 차례입니다.</summary>
        internal static bool IsFirstSight(string uid)
        {
            return seenUids.Add(uid ?? "(null)");
        }

        internal static void Count(uint type)
        {
            total++;
            countByType.TryGetValue(type, out int n);
            countByType[type] = n + 1;
            pendingSummary = true;

            if ((DateTime.UtcNow - lastSummaryTime).TotalSeconds >= SummaryIntervalSeconds)
            {
                LogSummary();
            }
        }

        private static void LogSummary()
        {
            lastSummaryTime = DateTime.UtcNow;
            if (!pendingSummary) return;
            pendingSummary = false;

            var sb = new StringBuilder();
            foreach (var entry in countByType)
            {
                if (sb.Length > 0) sb.Append(", ");
                sb.Append($"type={entry.Key}: {entry.Value}개");
            }

            MelonLogger.Msg($"[GhostNote.Probe] NormalNoteVisibleController가 붙은 노트 누적 {total}개 | {sb}");
        }
    }

    /// <summary>
    /// 노트 데이터가 렌더 컨트롤러에 물리는 순간을 잡아, 그 노트의 정체를 남깁니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(
        typeof(Il2CppAssets.Scripts.GameCore.GameObjectLogics.GameObjectControl.NormalNoteVisibleController),
        GameBindings.NormalNoteVisibleController.SetNoteMData)]
    public class NormalNoteVisibleController_SetNoteMData_Probe_Patch
    {
        public static void Postfix(Il2CppAssets.Scripts.GameCore.GameObjectLogics.GameObjectControl.NormalNoteVisibleController __instance)
        {
            try
            {
                if (__instance == null) return;

                // 세터 파라미터 이름에 의존하지 않도록, 세터가 쓰고 간 백킹 필드를 직접 읽습니다.
                var md = __instance._NoteMData_k__BackingField;
                if (md == null) return;

                var note = md.noteData;
                if (note == null) return;

                uint type = note.type;
                GhostNoteProbeStats.Count(type);

                string uid = note.uid;
                if (!GhostNoteProbeStats.IsFirstSight(uid)) return;

                MelonLogger.Msg($"[GhostNote.Probe] 렌더 컨트롤러 부착: uid={uid}, type={type}, pathway={note.pathway}, " +
                                $"prefab={note.prefab_name}, objId={md.objId}, tick={md.tick}, showTick={md.showTick}, dt={md.dt}, " +
                                $"skeleton={(__instance.m_SkeletonAnimation != null)}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[NormalNoteVisibleController.set_NoteMData.Postfix] 프로브 예외 발생: {ex}");
            }
        }
    }
}
