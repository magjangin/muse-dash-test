using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Structs;
using Il2CppSystem.Collections.Generic;
using System;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 커스텀 곡 폴더의 <c>dialog.txt</c>(<see cref="HwaDialogFile"/>)를 게임의 대사 데이터로 주입합니다.
    /// <para>
    /// 주입 지점은 <c>GameMusicScene.InitTimer</c> <b>직전</b>입니다. 이 시점이 중요한데,
    /// InitTimer가 틱 타임라인을 만들 때 존재하는 대사 시간에 대해서만 트리거를 등록하기 때문입니다.
    /// 그 뒤에 채워 넣으면 <c>DialogEventTrigger</c>가 영영 호출되지 않습니다. (실측 확인됨)
    /// </para>
    /// <para>
    /// 원곡 대사는 그 앞 단계(<see cref="DialogClearer"/>)에서 이미 비워지므로,
    /// dialog.txt가 없으면 대사 없는 상태가 그대로 유지되고, 있으면 우리 대사로 채워집니다.
    /// </para>
    /// <para>
    /// 말풍선의 모양과 색은 <c>dialogType</c>/<c>dialogIndex</c>가 고르는 프리팹 스프라이트가 결정합니다.
    /// (<c>TalkBaseImg</c>의 roleLT/roleLB, bossRT/bossRB, asideUp/asideDown)
    /// 아래 스타일 상수는 공식 곡(how_to_make_otoge_kyoku)에서 실측한 값입니다.
    /// </para>
    /// </summary>
    internal static class DialogInjector
    {
        private const string Tag = "[DialogInject]";

        /// <summary>false로 두면 dialog.txt를 무시합니다.</summary>
        internal static readonly bool Enabled = true;

        /// <summary>공식 곡 실측값: 글자색은 흰색입니다.</summary>
        private static readonly Color TextColor = Color.white;

        /// <summary>
        /// 공식 곡 실측값. 다만 실험 결과 이 필드는 화면에 반영되지 않습니다
        /// (말풍선은 프리팹 스프라이트로 그려짐). 원본과 값을 맞춰 두기만 합니다.
        /// </summary>
        private static readonly Color BgColor = new Color(0f, 0f, 0f, 0.498f);

        /// <summary>공식 곡 실측값: 텍스트는 가운데 정렬입니다.</summary>
        private const Il2Cpp.SuperTextMesh.Alignment TextAlignment = Il2Cpp.SuperTextMesh.Alignment.Center;

        /// <summary>공식 곡 실측값: 텍스트 표시 속도입니다.</summary>
        private const float TextSpeed = 1f;

        /// <summary>원본 sceneDialogEvents가 쓰는 언어 키입니다. 어느 언어로 플레이하든 같은 대사를 보여줍니다.</summary>
        private static readonly string[] Languages = { "English", "ChineseS", "ChineseT", "Japanese", "Korean" };

        /// <summary>
        /// 현재 곡의 dialog.txt를 읽어 대사 데이터를 채웁니다. 파일이 없으면 아무것도 하지 않습니다.
        /// </summary>
        internal static void InjectForCurrentSong()
        {
            if (!Enabled) return;
            if (!CustomPlaySession.Current.ShouldApplyExperimentChart) return;

            string uid = ResolveActiveUid();
            if (string.IsNullOrEmpty(uid)) return;

            if (!HwaResourceManager.TryGetSongDirectory(uid, out string songDir))
            {
                return;
            }

            if (!HwaDialogFile.TryLoad(songDir, out var lines, out string description))
            {
                // 파일이 없는 것은 정상(대사 없는 커스텀 곡)이므로 조용히 넘어갑니다.
                return;
            }

            var db = GlobalDataBase.s_StageInfo;
            if (db == null)
            {
                MelonLogger.Warning($"{Tag} 주입 실패: s_StageInfo가 null입니다. ({description})");
                return;
            }

            try
            {
                // 재시도(리트라이)로 InitTimer가 다시 돌 수 있으므로, 항상 새로 만들어 통째로 갈아끼웁니다.
                var timeDic = new Dictionary<Il2CppSystem.Decimal, List<GameDialogArgs>>();
                var flat = new List<GameDialogArgs>();

                foreach (var line in lines)
                {
                    AppendLine(timeDic, flat, line);
                }

                db._sceneDialogDictionary_k__BackingField = timeDic;

                // 언어별 원본도 함께 채웁니다. 초기화 로직이 이쪽을 보고 대사 유무를 판단할 수 있습니다.
                var events = new Dictionary<string, List<GameDialogArgs>>();
                foreach (string language in Languages)
                {
                    events[language] = flat;
                }
                db._sceneDialogEvents_k__BackingField = events;

                MelonLogger.Msg($"{Tag} 커스텀 대사 주입 완료: uid={uid}, {description} → 시간 키 {timeDic.Count}개, 항목 {flat.Count}개");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"{Tag} 커스텀 대사 주입 실패: uid={uid}, {description}, {ex}");
            }
        }

        private static string ResolveActiveUid()
        {
            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid))
            {
                uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid() ?? CustomPlaySession.Current.LastClickedMusicUid;
            }
            return uid;
        }

        /// <summary>
        /// 대사 한 줄을 게임 구조로 변환해 넣습니다.
        /// 말풍선을 여는 줄은 원본과 같이 <c>Show</c> + <c>None</c> 두 항목으로 펼칩니다.
        /// </summary>
        private static void AppendLine(
            Dictionary<Il2CppSystem.Decimal, List<GameDialogArgs>> timeDic,
            List<GameDialogArgs> flat,
            HwaDialogLine line)
        {
            switch (line.Action)
            {
                case HwaDialogAction.Show:
                    Add(timeDic, flat, line, DialogState.Show);
                    Add(timeDic, flat, line, DialogState.None);
                    break;

                case HwaDialogAction.Update:
                    Add(timeDic, flat, line, DialogState.None);
                    break;

                case HwaDialogAction.Hide:
                    Add(timeDic, flat, line, DialogState.Hide);
                    break;
            }
        }

        private static void Add(
            Dictionary<Il2CppSystem.Decimal, List<GameDialogArgs>> timeDic,
            List<GameDialogArgs> flat,
            HwaDialogLine line,
            DialogState state)
        {
            var key = (Il2CppSystem.Decimal)line.Time;

            List<GameDialogArgs> list;
            if (timeDic.ContainsKey(key))
            {
                list = timeDic[key];
            }
            else
            {
                list = new List<GameDialogArgs>();
                timeDic[key] = list;
            }

            var args = MakeArgs(line, state);
            list.Add(args);
            flat.Add(args);
        }

        private static GameDialogArgs MakeArgs(HwaDialogLine line, DialogState state)
        {
            var args = new GameDialogArgs();

            args.index = 0;
            args.time = (Il2CppSystem.Decimal)line.Time;
            args.dialogType = line.DialogType;
            args.dialogIndex = line.DialogIndex;
            args.dialogState = state;
            args.text = line.Text ?? string.Empty;
            args.speed = TextSpeed;
            args.fontSize = line.FontSize;
            args.textColor = TextColor;
            args.bgColor = BgColor;
            args.dialogSize = ResolveDialogSize(line);
            args.alignment = TextAlignment;

            return args;
        }

        /// <summary>
        /// 말풍선 크기입니다. dialog.txt에 적혀 있으면 그 값을, 비어 있으면 타입별 기본값(공식 곡 실측값)을 씁니다.
        /// UI 아래쪽 말풍선만 원본이 가로 400을 쓰며, <c>DialogSubControl</c>이 150x100 ~ 1200x200 범위로 클램프합니다.
        /// </summary>
        private static Vector2Int ResolveDialogSize(HwaDialogLine line)
        {
            if (line.BoxWidth > 0 && line.BoxHeight > 0)
            {
                return new Vector2Int(line.BoxWidth, line.BoxHeight);
            }

            int width = line.DialogType == DialogTypeCodes.Ui && line.DialogIndex == DialogDirCodes.Down
                ? DialogBoxSizes.UiDownWidth
                : DialogBoxSizes.DefaultWidth;

            return new Vector2Int(width, DialogBoxSizes.DefaultHeight);
        }
    }
}

// =====================================================================
// GameMusicScene.InitTimer 훅
// 틱 타임라인이 만들어지기 전에 대사를 채워 넣어야 트리거가 등록됩니다.
// =====================================================================
[HarmonyLib.HarmonyPatch(typeof(Il2CppGameLogic.GameMusicScene), "InitTimer",
    new Type[] { typeof(Il2CppSystem.Decimal) })]
public static class GameMusicScene_InitTimer_DialogInjectPatch
{
    public static void Prefix()
    {
        try
        {
            muse_dash_test.DialogInjector.InjectForCurrentSong();
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[DialogInject] InitTimer Prefix 예외: {ex}");
        }
    }
}
