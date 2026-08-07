using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Structs;
using Il2CppSystem.Collections.Generic;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// DBStageInfo 로딩 시점에서 대사(Dialog) 데이터를 MelonLogger로 덤프하는 진단 유틸리티.
    /// <para>
    /// <c>sceneDialogEvents</c> (씬별 대사),
    /// <c>sceneDialogDictionary</c> (시간별 대사),
    /// <c>Expression.talkInfosList</c> (표정/타이밍) 정보를 모두 출력합니다.
    /// </para>
    /// </summary>
    internal static class DialogDiagnostics
    {
        private const string Tag = "[DialogDump]";

        /// <summary>
        /// DBStageInfo 인스턴스에서 sceneDialogEvents + sceneDialogDictionary를 모두 덤프합니다.
        /// </summary>
        internal static void DumpAllDialogData(DBStageInfo instance)
        {
            if (instance == null)
            {
                MelonLogger.Msg($"{Tag} DBStageInfo 인스턴스가 null입니다.");
                return;
            }

            string mapName = "(unknown)";
            try { mapName = instance.mapName ?? "(null)"; } catch { /* 접근 실패 시 무시 */ }
            MelonLogger.Msg($"{Tag} ========== 대사 데이터 덤프 시작: mapName={mapName} ==========");

            DumpSceneDialogEvents(instance);
            DumpSceneDialogDictionary(instance);

            MelonLogger.Msg($"{Tag} ========== 대사 데이터 덤프 완료 ==========");
        }

        /// <summary>
        /// Dictionary&lt;string, List&lt;GameDialogArgs&gt;&gt; sceneDialogEvents 덤프.
        /// Key = 씬 이름, Value = 해당 씬의 대사 목록.
        /// </summary>
        private static void DumpSceneDialogEvents(DBStageInfo instance)
        {
            Dictionary<string, List<GameDialogArgs>> dialogEvents = null;
            try
            {
                dialogEvents = instance.sceneDialogEvents;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} sceneDialogEvents 접근 실패: {ex.Message}");
                return;
            }

            if (dialogEvents == null)
            {
                MelonLogger.Msg($"{Tag} sceneDialogEvents = null (대사 없는 곡)");
                return;
            }

            int keyCount = dialogEvents.Count;
            MelonLogger.Msg($"{Tag} --- sceneDialogEvents (씬별 대사): {keyCount}개 씬 ---");

            var enumerator = dialogEvents.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var pair = enumerator.Current;
                string sceneKey = pair.Key ?? "(null)";
                var argsList = pair.Value;

                if (argsList == null)
                {
                    MelonLogger.Msg($"{Tag}   씬='{sceneKey}': (null list)");
                    continue;
                }

                MelonLogger.Msg($"{Tag}   씬='{sceneKey}': {argsList.Count}개 대사");
                for (int i = 0; i < argsList.Count; i++)
                {
                    LogGameDialogArgs($"    [{i}]", argsList[i]);
                }
            }
            enumerator.Dispose();
        }

        /// <summary>
        /// Dictionary&lt;Decimal, List&lt;GameDialogArgs&gt;&gt; sceneDialogDictionary 덤프.
        /// Key = 시간(Decimal), Value = 해당 시간의 대사 목록.
        /// DialogDataToDic()에서 sceneDialogEvents를 시간별로 재구성한 것.
        /// </summary>
        private static void DumpSceneDialogDictionary(DBStageInfo instance)
        {
            Dictionary<Il2CppSystem.Decimal, List<GameDialogArgs>> dialogDic = null;
            try
            {
                dialogDic = instance.sceneDialogDictionary;
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} sceneDialogDictionary 접근 실패: {ex.Message}");
                return;
            }

            if (dialogDic == null)
            {
                MelonLogger.Msg($"{Tag} sceneDialogDictionary = null");
                return;
            }

            int keyCount = dialogDic.Count;
            MelonLogger.Msg($"{Tag} --- sceneDialogDictionary (시간별 대사): {keyCount}개 시간 키 ---");

            var enumerator = dialogDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var pair = enumerator.Current;
                string timeKey = pair.Key.ToString();
                var argsList = pair.Value;

                if (argsList == null)
                {
                    MelonLogger.Msg($"{Tag}   time={timeKey}: (null list)");
                    continue;
                }

                MelonLogger.Msg($"{Tag}   time={timeKey}: {argsList.Count}개 대사");
                for (int i = 0; i < argsList.Count; i++)
                {
                    LogGameDialogArgs($"    [{i}]", argsList[i]);
                }
            }
            enumerator.Dispose();
        }

        /// <summary>
        /// Expression.talkInfosList (List&lt;List&lt;TalkInfo&gt;&gt;)를 덤프합니다.
        /// Expression 인스턴스를 직접 받아 처리합니다.
        /// </summary>
        internal static void DumpExpressionTalkInfos(Expression expression, string label = "")
        {
            if (expression == null)
            {
                MelonLogger.Msg($"{Tag} Expression = null {label}");
                return;
            }

            string animName = "(null)";
            try { animName = expression.animName ?? "(null)"; } catch { /* 접근 실패 */ }
            float weight = 0f;
            try { weight = expression.weight; } catch { /* 접근 실패 */ }

            MelonLogger.Msg($"{Tag} --- Expression {label}: animName='{animName}', weight={weight} ---");

            // texts 덤프
            try
            {
                var texts = expression.texts;
                if (texts != null)
                {
                    MelonLogger.Msg($"{Tag}   texts ({texts.Count}개):");
                    for (int i = 0; i < texts.Count; i++)
                    {
                        string t = "(null)";
                        try { t = texts[i] ?? "(null)"; } catch { /* 접근 실패 */ }
                        MelonLogger.Msg($"{Tag}     [{i}] \"{t}\"");
                    }
                }
                else
                {
                    MelonLogger.Msg($"{Tag}   texts = null");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag}   texts 접근 실패: {ex.Message}");
            }

            // audioNames 덤프
            try
            {
                var audioNames = expression.audioNames;
                if (audioNames != null)
                {
                    MelonLogger.Msg($"{Tag}   audioNames ({audioNames.Length}개):");
                    for (int i = 0; i < audioNames.Length; i++)
                    {
                        MelonLogger.Msg($"{Tag}     [{i}] \"{audioNames[i] ?? "(null)"}\"");
                    }
                }
                else
                {
                    MelonLogger.Msg($"{Tag}   audioNames = null");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag}   audioNames 접근 실패: {ex.Message}");
            }

            // talkInfosList 덤프
            try
            {
                var talkInfosList = expression.talkInfosList;
                if (talkInfosList == null)
                {
                    MelonLogger.Msg($"{Tag}   talkInfosList = null");
                    return;
                }

                MelonLogger.Msg($"{Tag}   talkInfosList ({talkInfosList.Count}개 그룹):");
                for (int groupIdx = 0; groupIdx < talkInfosList.Count; groupIdx++)
                {
                    var innerList = talkInfosList[groupIdx];
                    if (innerList == null)
                    {
                        MelonLogger.Msg($"{Tag}     그룹[{groupIdx}] = null");
                        continue;
                    }

                    MelonLogger.Msg($"{Tag}     그룹[{groupIdx}] ({innerList.Count}개 TalkInfo):");
                    for (int j = 0; j < innerList.Count; j++)
                    {
                        LogTalkInfo($"      [{j}]", innerList[j]);
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag}   talkInfosList 접근 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 단일 GameDialogArgs를 로그로 출력합니다.
        /// 런타임 OnInvokeDialog 훅에서도 사용됩니다.
        /// </summary>
        internal static void LogGameDialogArgs(string prefix, GameDialogArgs args)
        {
            if (args == null)
            {
                MelonLogger.Msg($"{Tag} {prefix} (null)");
                return;
            }

            try
            {
                int index = args.index;
                string time = args.time.ToString();
                int dialogType = args.dialogType;
                int dialogIndex = args.dialogIndex;
                string text = args.text ?? "(null)";
                float speed = args.speed;
                int fontSize = args.fontSize;
                var state = args.dialogState;

                string typeName = dialogType switch
                {
                    0 => "ROLE",
                    1 => "BOSS",
                    2 => "UI",
                    _ => $"Unknown({dialogType})"
                };

                string dirName = dialogIndex switch
                {
                    0 => "UP",
                    1 => "DOWN",
                    _ => $"Unknown({dialogIndex})"
                };

                MelonLogger.Msg($"{Tag} {prefix} idx={index}, time={time}, type={typeName}, dir={dirName}, state={state}, speed={speed}, fontSize={fontSize}, text=\"{text}\"");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} {prefix} GameDialogArgs 읽기 실패: {ex.Message}");
            }
        }

        /// <summary>
        /// 단일 TalkInfo를 로그로 출력합니다.
        /// </summary>
        private static void LogTalkInfo(string prefix, TalkInfo info)
        {
            if (info == null)
            {
                MelonLogger.Msg($"{Tag} {prefix} (null)");
                return;
            }

            try
            {
                float duration = info.duration;
                float interval = info.interval;
                var talkType = info.talkType;

                MelonLogger.Msg($"{Tag} {prefix} duration={duration}, interval={interval}, talkType={talkType}");
            }
            catch (Exception ex)
            {
                MelonLogger.Warning($"{Tag} {prefix} TalkInfo 읽기 실패: {ex.Message}");
            }
        }
    }
}

// =====================================================================
// DialogMasterControl.OnInvokeDialog 런타임 훅
// 실제 게임 플레이 중 대사가 호출될 때마다 내용을 실시간 로깅합니다.
// =====================================================================
[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.DialogMasterControl), "OnInvokeDialog")]
public static class DialogMasterControl_OnInvokeDialog_Patch
{
    public static void Prefix(Il2Cpp.DialogMasterControl __instance, GameDialogArgs args)
    {
        try
        {
            string lang = "(unknown)";
            try { lang = __instance.language ?? "(null)"; } catch { /* 접근 실패 */ }

            MelonLogger.Msg($"[DialogRuntime] OnInvokeDialog 호출: lang={lang}");
            muse_dash_test.DialogDiagnostics.LogGameDialogArgs("[DialogRuntime]", args);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[DialogRuntime] OnInvokeDialog 로깅 실패: {ex.Message}");
        }
    }
}

// =====================================================================
// DBStageInfo.SetDialogArgs 훅
// 대사 데이터가 실제로 채워지는 시점에서 딕셔너리 내용을 덤프합니다.
// SetRuntimeMusicData 시점에서는 아직 null이므로, 이 훅이 핵심입니다.
// =====================================================================
[HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.Database.DBStageInfo), "SetDialogArgs")]
public static class DBStageInfo_SetDialogArgs_Patch
{
    public static void Postfix(
        Il2CppAssets.Scripts.Database.DBStageInfo __instance,
        Il2CppSystem.Collections.Generic.Dictionary<string, Il2CppSystem.Collections.Generic.List<GameDialogArgs>> dialogArgs,
        bool isRefresh)
    {
        try
        {
            MelonLogger.Msg($"[DialogDump] === SetDialogArgs 호출됨: isRefresh={isRefresh} ===");

            // dialogArgs 파라미터 직접 덤프
            if (dialogArgs == null)
            {
                MelonLogger.Msg("[DialogDump] SetDialogArgs: dialogArgs 파라미터 = null");
            }
            else
            {
                int keyCount = dialogArgs.Count;
                MelonLogger.Msg($"[DialogDump] SetDialogArgs: dialogArgs 파라미터 ({keyCount}개 씬):");

                var enumerator = dialogArgs.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var pair = enumerator.Current;
                    string sceneKey = pair.Key ?? "(null)";
                    var argsList = pair.Value;

                    if (argsList == null)
                    {
                        MelonLogger.Msg($"[DialogDump]   씬='{sceneKey}': (null list)");
                        continue;
                    }

                    MelonLogger.Msg($"[DialogDump]   씬='{sceneKey}': {argsList.Count}개 대사");
                    for (int i = 0; i < argsList.Count; i++)
                    {
                        muse_dash_test.DialogDiagnostics.LogGameDialogArgs($"    [{i}]", argsList[i]);
                    }
                }
                enumerator.Dispose();
            }

            // Postfix이므로 sceneDialogEvents/sceneDialogDictionary가 이제 채워져 있을 수 있음
            muse_dash_test.DialogDiagnostics.DumpAllDialogData(__instance);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[DialogDump] SetDialogArgs 로깅 실패: {ex.Message}");
        }
    }
}

// =====================================================================
// DBStageInfo.DialogDataToDic 훅
// sceneDialogEvents → sceneDialogDictionary 재구성 시점 캡처.
// =====================================================================
[HarmonyLib.HarmonyPatch(typeof(Il2CppAssets.Scripts.Database.DBStageInfo), "DialogDataToDic")]
public static class DBStageInfo_DialogDataToDic_Patch
{
    public static void Postfix(Il2CppAssets.Scripts.Database.DBStageInfo __instance)
    {
        try
        {
            MelonLogger.Msg("[DialogDump] === DialogDataToDic 완료 → sceneDialogDictionary 덤프 ===");
            muse_dash_test.DialogDiagnostics.DumpAllDialogData(__instance);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[DialogDump] DialogDataToDic 로깅 실패: {ex.Message}");
        }
    }
}
