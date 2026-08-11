using System;
using System.Collections.Generic;
using System.Text;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime;
using NoteVisibleController = Il2CppAssets.Scripts.GameCore.GameObjectLogics.GameObjectControl.NormalNoteVisibleController;

namespace muse_dash_test
{
    // === 고스트 노트(UID zz17yy, type 4) 렌더 페이드 관찰 프로브 ===
    //
    // 값은 하나도 바꾸지 않습니다. 어셈블리에서 노트 렌더러를 페이드시키는 유일한 클래스인
    // NormalNoteVisibleController가 "어떤 노트에 붙는지"를 확인하기 위한 진단입니다.
    //   - type 4(고스트)에만 붙는다  → 고스트 전용 컨트롤러이므로 여기만 만지면 됩니다.
    //   - 일반 노트에도 붙는다       → 페이드 여부는 컨트롤러가 아니라 다른 조건이 가릅니다.
    //
    // 관측 경로가 둘입니다.
    //   1) set_NoteMData Postfix : non-virtual public이라 후킹이 안전한 유일한 지점.
    //      다만 필드 대입 한 줄짜리라 IL2CPP가 호출부에 인라인하면 영영 안 돕니다.
    //   2) 씬 스캔            : 후킹 없이 살아 있는 인스턴스를 직접 훑습니다. 1)이 침묵해도 답이 나옵니다.
    // 로그의 출처 표기로 둘 중 어느 쪽이 잡았는지 구분됩니다.
    //
    // OnAppear(bool)/Init()은 virtual final이라 건드리지 않습니다. 이 프로젝트에서 virtual 훅은
    // 로그도 남기지 않는 네이티브 크래시를 낸 전력이 있습니다(커밋 7c5e685).

    internal static class GhostNoteProbeStats
    {
        private const double SummaryIntervalSeconds = 10.0;

        private static readonly HashSet<string> seenInstances = new HashSet<string>();
        private static readonly HashSet<string> seenUids = new HashSet<string>();
        private static readonly Dictionary<uint, int> countByType = new Dictionary<uint, int>();
        private static int total;
        private static bool pendingSummary;
        private static DateTime lastSummaryTime = DateTime.MinValue;

        /// <summary>
        /// 컨트롤러 하나를 기록합니다. 같은 인스턴스에 같은 노트가 물려 있으면 두 번 세지 않고,
        /// 상세 로그는 UID당 최초 1회만 남깁니다. (노트는 한 곡에 수백 개입니다.)
        /// </summary>
        internal static void Report(NoteVisibleController controller, string source)
        {
            try
            {
                if (controller == null) return;

                var md = controller._NoteMData_k__BackingField;
                if (md == null) return;

                var note = md.noteData;
                if (note == null) return;

                string uid = note.uid ?? "(null)";
                if (!seenInstances.Add($"{controller.Pointer}:{uid}")) return;

                uint type = note.type;
                total++;
                countByType.TryGetValue(type, out int n);
                countByType[type] = n + 1;
                pendingSummary = true;

                if (seenUids.Add(uid))
                {
                    MelonLogger.Msg($"[GhostNote.Probe] 렌더 컨트롤러 부착({source}): uid={uid}, type={type}, pathway={note.pathway}, " +
                                    $"prefab={note.prefab_name}, objId={md.objId}, tick={md.tick}, showTick={md.showTick}, dt={md.dt}, " +
                                    $"skeleton={(controller.m_SkeletonAnimation != null)}");
                }

                if ((DateTime.UtcNow - lastSummaryTime).TotalSeconds >= SummaryIntervalSeconds)
                {
                    LogSummary();
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[GhostNote.Probe] 기록 중 예외 발생: {ex}");
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

            MelonLogger.Msg($"[GhostNote.Probe] 렌더 컨트롤러가 붙은 노트 누적 {total}개 | {sb}");
        }
    }

    /// <summary>
    /// 후킹 없이 씬에서 컨트롤러 인스턴스를 직접 훑는 2차 관측 경로입니다.
    /// 세터가 인라인돼 훅이 안 돌더라도 이쪽은 답을 냅니다.
    /// </summary>
    internal static class GhostNoteProbeScanner
    {
        private const float ScanIntervalSeconds = 0.5f;

        private static float lastScanTime;
        private static int lastInstanceCount = -1;

        internal static void Scan()
        {
            if (Time.unscaledTime - lastScanTime < ScanIntervalSeconds) return;
            lastScanTime = Time.unscaledTime;

            var found = UnityEngine.Object.FindObjectsOfType(Il2CppType.Of<NoteVisibleController>());
            if (found == null) return;

            if (found.Length != lastInstanceCount)
            {
                lastInstanceCount = found.Length;
                MelonLogger.Msg($"[GhostNote.Probe] 씬에 살아 있는 렌더 컨트롤러 인스턴스 수: {found.Length}");
            }

            for (int i = 0; i < found.Length; i++)
            {
                var obj = found[i];
                if (obj == null) continue;

                GhostNoteProbeStats.Report(obj.TryCast<NoteVisibleController>(), "씬 스캔");
            }
        }
    }

    /// <summary>
    /// 노트 데이터가 렌더 컨트롤러에 물리는 순간을 잡아, 그 노트의 정체를 남깁니다.
    /// 세터 파라미터 이름에 의존하지 않도록 세터가 쓰고 간 백킹 필드를 읽습니다.
    /// </summary>
    [HarmonyLib.HarmonyPatch(typeof(NoteVisibleController), GameBindings.NormalNoteVisibleController.SetNoteMData)]
    public class NormalNoteVisibleController_SetNoteMData_Probe_Patch
    {
        public static void Postfix(NoteVisibleController __instance)
        {
            GhostNoteProbeStats.Report(__instance, "세터 훅");
        }
    }
}
