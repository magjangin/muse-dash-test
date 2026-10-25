using MelonLoader;
using muse_dash_test;

// BMS 노트 스펙의 dt를 권장표(BmsDtTable)와 대조합니다.
//  - info.txt 'dt를 권장 표로 고치기 = 활성화': 권장표에 있는 계열은 WAV _dt를 권장값으로 덮어씁니다.
//  - 그 밖: 값은 그대로 두고, 허용 범위를 벗어난 WAV만 묶어서 경고합니다.
public partial class DBStageInfo_SetRuntimeMusicData_Patch
{
    // 같은 uid·dt 조합을 한 줄로 묶어 경고하되, 채보가 크게 어긋나도 로그가 넘치지 않게 자릅니다.
    private const int MaxDtWarningLines = 20;

    private static void ApplyDtTable(System.Collections.Generic.List<ExperimentNoteSpec> specs, string activeUid)
    {
        if (specs == null || specs.Count == 0) return;

        var manifest = HwaResourceManager.GetManifest(activeUid);
        bool fix = manifest?.FixDtToRecommended == true;

        int changed = 0;
        var offenders = new System.Collections.Generic.Dictionary<string, int>(System.StringComparer.Ordinal);

        foreach (var spec in specs)
        {
            if (spec == null) continue;

            var family = BmsDtTable.Classify(spec.Uid, spec.NoteType, spec.BossAction);
            int speed = BmsDtTable.Speed(spec.Uid);

            if (fix)
            {
                if (!BmsDtTable.TryGetRecommended(family, speed, out double recommended)) continue;
                if (System.Math.Abs(spec.Dt - recommended) > 0.0005) changed++;
                spec.Dt = recommended;
                continue;
            }

            if (BmsDtTable.IsAllowed(family, speed, spec.Dt, out string expected)) continue;

            string key = $"uid={spec.Uid}, {BmsDtTable.Describe(family)} 속도 {speed}, _dt{spec.Dt:0.###} (허용 {expected})";
            offenders.TryGetValue(key, out int count);
            offenders[key] = count + 1;
        }

        if (fix)
        {
            ModLogger.Msg($"[ExperimentChart.Dt] 'dt를 권장 표로 고치기' 활성화: 권장값으로 바꾼 노트 {changed}개 / 전체 {specs.Count}개");
            return;
        }

        if (offenders.Count == 0) return;

        int lines = 0;
        foreach (var pair in offenders)
        {
            if (lines++ >= MaxDtWarningLines)
            {
                ModLogger.Warning($"[ExperimentChart.Dt] ...외 {offenders.Count - MaxDtWarningLines}종 생략");
                break;
            }

            ModLogger.Warning($"[ExperimentChart.Dt] 권장 범위 밖 dt: {pair.Key} × {pair.Value}개");
        }

        ModLogger.Warning("[ExperimentChart.Dt] WAV _dt를 그대로 씁니다. 권장표로 맞추려면 info.txt에 'dt를 권장 표로 고치기 = 활성화'를 적으세요.");
    }
}
