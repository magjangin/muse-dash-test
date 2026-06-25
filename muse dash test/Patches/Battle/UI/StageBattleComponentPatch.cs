using MelonLoader;
using System;
using System.Reflection;
using Il2CppFormulaBase;
using Il2CppGameLogic;

namespace muse_dash_test
{
    // Il2CppFormulaBase.StageBattleComponent.LoadMusicData 하모니 패치
    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "LoadMusicData")]
    public class StageBattleComponent_LoadMusicData_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            try
            {
                MelonLogger.Msg($"[StageBattleComponent.LoadMusicData] 호출됨: {__instance}");
                // [비활성] MusicData 덤프 로직 중단 (요청에 따라 꺼둠)
                // StageBattleMusicDataDump.Dump(__instance);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[StageBattleComponent.LoadMusicData] MusicData 덤프 예외: {ex}");
            }
        }
    }

    internal static class StageBattleMusicDataDump
    {
        private const BindingFlags InstanceMembers =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static void Dump(StageBattleComponent component)
        {
            if (component == null)
            {
                MelonLogger.Msg("[StageBattleComponent.MusicDataDump] component=null");
                return;
            }

            bool dumpedAny = false;
            var type = component.GetType();
            foreach (var field in type.GetFields(InstanceMembers))
            {
                dumpedAny |= TryDumpMemberValue(field.Name, SafeGet(() => field.GetValue(component)));
            }

            foreach (var prop in type.GetProperties(InstanceMembers))
            {
                if (prop.GetIndexParameters().Length != 0) continue;
                dumpedAny |= TryDumpMemberValue(prop.Name, SafeGet(() => prop.GetValue(component)));
            }

            if (!dumpedAny)
            {
                MelonLogger.Msg("[StageBattleComponent.MusicDataDump] MusicData/List<MusicData> 멤버를 찾지 못했습니다.");
            }
        }

        private static bool TryDumpMemberValue(string memberName, object value)
        {
            if (value == null) return false;

            if (value is MusicData single)
            {
                MelonLogger.Msg($"[StageBattleComponent.MusicDataDump] member={memberName}, single MusicData");
                DumpNote(memberName, 0, single, force: true);
                return true;
            }

            if (value is Il2CppSystem.Collections.Generic.List<MusicData> list)
            {
                MelonLogger.Msg($"[StageBattleComponent.MusicDataDump] member={memberName}, List<MusicData>.Count={list.Count}");
                int emitted = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    var note = list[i];
                    bool force = i < 5;
                    if (DumpNote(memberName, i, note, force))
                    {
                        emitted++;
                    }

                    if (emitted >= 80)
                    {
                        MelonLogger.Msg($"[StageBattleComponent.MusicDataDump] member={memberName}, 덤프 80개에서 중단");
                        break;
                    }
                }
                return true;
            }

            return false;
        }

        private static bool DumpNote(string memberName, int index, MusicData note, bool force)
        {
            if (note?.noteData == null)
            {
                if (force)
                {
                    MelonLogger.Msg($"[StageBattleComponent.MusicDataDump] {memberName}[{index}] note/null");
                    return true;
                }
                return false;
            }

            string uid = note.noteData.uid ?? "";
            string ibmsId = note.noteData.ibms_id ?? "";
            bool interesting =
                force ||
                note.noteData.type == DBStageInfo_SetRuntimeMusicData_Patch.NoteTypes.SceneToggle ||
                uid.StartsWith("0004", StringComparison.OrdinalIgnoreCase) ||
                !string.IsNullOrWhiteSpace(ibmsId);

            if (!interesting) return false;

            MelonLogger.Msg(
                $"[StageBattleComponent.MusicDataDump] {memberName}[{index}] objId={Safe(() => note.objId)}, " +
                $"tick={Safe(() => note.tick)}, dt={Safe(() => note.dt)}, showTick={Safe(() => note.showTick)}, " +
                $"uid={uid}, mirror_uid={Safe(() => note.noteData.mirror_uid)}, ibms_id={ibmsId}, " +
                $"type={Safe(() => note.noteData.type)}, noteUid={Safe(() => note.noteData.noteUid)}, m_BmsUid={Safe(() => note.noteData.m_BmsUid)}, " +
                $"scene={Safe(() => note.noteData.scene)}, sceneChangeNames={FormatSceneChangeNames(note.noteData.sceneChangeNames)}, " +
                $"prefab={Safe(() => note.noteData.prefab_name)}, key_audio={Safe(() => note.noteData.key_audio)}, boss_action={Safe(() => note.noteData.boss_action)}, " +
                $"config.id={Safe(() => note.configData?.id)}, config.time={Safe(() => note.configData?.time)}, config.note_uid={Safe(() => note.configData?.note_uid)}");
            return true;
        }

        private static string FormatSceneChangeNames(Il2CppSystem.Collections.Generic.List<string> names)
        {
            if (names == null)
            {
                return "(null)";
            }

            try
            {
                var values = new System.Collections.Generic.List<string>();
                for (int i = 0; i < names.Count; i++)
                {
                    values.Add(names[i] ?? "(null)");
                }

                return "[" + string.Join(",", values) + "]";
            }
            catch (Exception ex)
            {
                return $"(예외:{ex.GetType().Name})";
            }
        }

        private static object SafeGet(Func<object> getter)
        {
            try { return getter(); }
            catch { return null; }
        }

        private static string Safe(Func<object> getter)
        {
            try
            {
                object value = getter();
                return value != null ? value.ToString() : "(null)";
            }
            catch (Exception ex)
            {
                return $"(예외:{ex.GetType().Name})";
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "InitData")]
    public class StageBattleComponent_InitData_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid ?? "(unknown)";
            }
            MelonLogger.Msg($"StageBattleComponent.InitData 호출됨: {__instance}, 곡 UID={uid}");
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Load")]
    public class StageBattleComponent_Load_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            try
            {
                MelonLogger.Msg($"[StageBattleComponent.Load] 호출됨: {__instance}");
                // 매 배틀 로드 시마다 미디어 주입 시작 상태 초기화 및 주입 실행
                HwaBattleMediaController.ResetState();
                HwaBattleMediaController.StartBattleMediaInjection();

                // APMod (All Perfect Mod) 폰트 탐색 상태 리셋
                Patches.VictoryDataCache.AttemptedFontCache = false;
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[StageBattleComponent.Load] 예외 발생: {ex}");
            }
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Pause")]
    public class StageBattleComponent_Pause_Patch
    {
        public static void Postfix(StageBattleComponent __instance, bool pauseCorountine)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Pause 호출됨");
            HwaBattleMediaController.PauseMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Resume")]
    public class StageBattleComponent_Resume_Patch
    {
        public static void Postfix(StageBattleComponent __instance, bool isExit)
        {
            MelonLogger.Msg($"[StageBattleComponentPatch] StageBattleComponent.Resume 호출됨 (isExit={isExit})");
            HwaBattleMediaController.ResumeMedia(isExit);
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "End")]
    public class StageBattleComponent_End_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.End 호출됨");
            HwaBattleMediaController.StopMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Exit")]
    public class StageBattleComponent_Exit_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Exit 호출됨");
            HwaBattleMediaController.StopMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "Release")]
    public class StageBattleComponent_Release_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.Release 호출됨");
            HwaBattleMediaController.StopMedia();
        }
    }

    [HarmonyLib.HarmonyPatch(typeof(StageBattleComponent), "GameRestart")]
    public class StageBattleComponent_GameRestart_Patch
    {
        public static void Postfix(StageBattleComponent __instance)
        {
            MelonLogger.Msg("[StageBattleComponentPatch] StageBattleComponent.GameRestart 호출됨");
            HwaBattleMediaController.StopMedia();
        }
    }
}
