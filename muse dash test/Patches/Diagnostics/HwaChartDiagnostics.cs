using MelonLoader;
using System;
using System.Collections.Generic;

namespace muse_dash_test
{
    internal static class HwaChartDiagnostics
    {
        internal static string DescribeBmsChart(BmsChart chart)
        {
            if (chart == null)
            {
                return "(null)";
            }

            int metadataCount = chart.Metadata != null ? chart.Metadata.Count : 0;
            int wavCount = CountBmsWavMetadata(chart);
            int noteCount = chart.Notes != null ? chart.Notes.Count : 0;
            int bpmCount = chart.BpmChanges != null ? chart.BpmChanges.Count : 0;
            int swapCount = 0;
            try
            {
                swapCount = BmsBossSwapPlanner.BuildSwapEvents(chart).Count;
            }
            catch
            {
                swapCount = -1;
            }

            return "path=" + (chart.SourcePath ?? "(null)")
                + ", title=" + (chart.Title ?? "(null)")
                + ", artist=" + (chart.Artist ?? "(null)")
                + ", defaultBpm=" + chart.DefaultBpm.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)
                + ", notes=" + noteCount
                + ", bpmChanges=" + bpmCount
                + ", metadata=" + metadataCount
                + ", wav=" + wavCount
                + ", bossSwapCandidates=" + swapCount;
        }

        internal static int CountBmsWavMetadata(BmsChart chart)
        {
            if (chart?.Metadata == null)
            {
                return 0;
            }

            int count = 0;
            foreach (var key in chart.Metadata.Keys)
            {
                if (!string.IsNullOrWhiteSpace(key) && key.StartsWith("WAV", StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            return count;
        }

        internal static void LogBmsWavMappingSummary(BmsChart chart)
        {
            if (chart?.Notes == null || chart.Notes.Count == 0)
            {
                MelonLogger.Msg("[HwaResourceManager.Bms] 노트가 없어 WAV 매핑 샘플을 건너뜁니다.");
                return;
            }

            int logged = 0;
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var note in chart.Notes)
            {
                if (note == null || string.IsNullOrWhiteSpace(note.RawValue) || !seen.Add(note.RawValue))
                {
                    continue;
                }

                var wavInfo = BmsBossSwapPlanner.ResolveWavInfo(chart, note);
                if (wavInfo == null)
                {
                    MelonLogger.Msg($"[HwaResourceManager.Bms] WAV 매핑 샘플: raw={note.RawValue}, wav=(null)");
                }
                else
                {
                    MelonLogger.Msg($"[HwaResourceManager.Bms] WAV 매핑 샘플: raw={note.RawValue}, wav={wavInfo.RawWavName}, uid={wavInfo.Uid ?? "(null)"}, type={wavInfo.NoteType}, prefab={wavInfo.PrefabName ?? "(null)"}, dt={wavInfo.Dt}, keyAudio={wavInfo.KeyAudio ?? "(null)"}, bossAction={wavInfo.BossAction ?? "(null)"}");
                }

                logged++;
                if (logged >= 12)
                {
                    break;
                }
            }

            var swapEvents = BmsBossSwapPlanner.BuildSwapEvents(chart);
            for (int i = 0; i < swapEvents.Count && i < 5; i++)
            {
                var evt = swapEvents[i];
                MelonLogger.Msg($"[HwaResourceManager.Bms] 보스 스왑 후보 #{i + 1}: outTick={evt.OutNote?.Tick}, inTick={evt.InNote?.Tick}, delay={evt.DelaySeconds:0.###}s, action={evt.BossAction}");
            }
        }
    }
}
