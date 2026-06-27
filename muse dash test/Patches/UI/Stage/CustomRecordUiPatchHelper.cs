using MelonLoader;
using System;
using UnityEngine;
using UnityEngine.UI;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;

namespace muse_dash_test
{
    public static class CustomRecordUiPatchHelper
    {
        public static void ApplyCustomRecordToPnlStage(PnlStage stage, MusicInfo musicInfo)
        {
            try
            {
                if (stage == null || musicInfo == null) return;
                if (!CustomContentIds.IsVirtualSong(musicInfo.uid)) return;

                int difficulty = 1;
                if (GlobalDataBase.s_DbBattleStage != null)
                {
                    difficulty = GlobalDataBase.s_DbBattleStage.selectedDifficulty;
                }

                MelonLogger.Msg($"[CustomRecordUiPatchHelper.PnlStage] 적용 감지: uid={musicInfo.uid}, diff={difficulty}");

                var record = CustomRecordStore.LoadResult(musicInfo.uid, difficulty);
                if (record != null)
                {
                    if (stage.stageAchievementPercent != null)
                    {
                        var textComp = stage.stageAchievementPercent.GetComponentInChildren<Text>(true);
                        if (textComp != null)
                        {
                            textComp.text = $"{record.accuracy:0.00}%";
                            stage.stageAchievementPercent.SetActive(true);
                            MelonLogger.Msg($"[CustomRecordUiPatchHelper.PnlStage] UI 갱신 성공 -> {record.accuracy:0.00}%");
                        }
                    }
                }
                else
                {
                    if (stage.stageAchievementPercent != null)
                    {
                        stage.stageAchievementPercent.SetActive(false);
                        MelonLogger.Msg("[CustomRecordUiPatchHelper.PnlStage] UI 갱신 성공 -> 기록 없음으로 비활성화");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomRecordUiPatchHelper.PnlStage] Custom record UI apply error: {ex}");
            }
        }

        public static void ApplyCustomRecordToPnlPreparation(Il2Cpp.PnlPreparation prep)
        {
            try
            {
                if (prep == null) return;
                string uid = CustomPlaySession.Current.SelectedMusicUid;
                if (string.IsNullOrEmpty(uid)) uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
                if (!CustomContentIds.IsVirtualSong(uid)) return;

                int difficulty = 1;
                if (GlobalDataBase.s_DbBattleStage != null)
                {
                    difficulty = GlobalDataBase.s_DbBattleStage.selectedDifficulty;
                }

                MelonLogger.Msg($"[CustomRecordUiPatchHelper.PnlPrep] 적용 감지: uid={uid}, diff={difficulty}");

                var record = CustomRecordStore.LoadResult(uid, difficulty);
                if (record != null)
                {
                    if (prep.btnDownloadReport != null)
                    {
                        prep.btnDownloadReport.gameObject.SetActive(true);
                        prep.btnDownloadReport.interactable = true;
                        MelonLogger.Msg("[CustomRecordUiPatchHelper.PnlPrep] btnDownloadReport 활성화");
                    }
                    if (prep.stageAchievementValue != null)
                    {
                        prep.stageAchievementValue.text = $"{record.accuracy:0.00}%";
                        MelonLogger.Msg($"[CustomRecordUiPatchHelper.PnlPrep] UI 갱신 성공 -> {record.accuracy:0.00}%");
                    }
                }
                else
                {
                    if (prep.btnDownloadReport != null)
                    {
                        prep.btnDownloadReport.gameObject.SetActive(false);
                        prep.btnDownloadReport.interactable = false;
                        MelonLogger.Msg("[CustomRecordUiPatchHelper.PnlPrep] btnDownloadReport 비활성화 (기록 없음)");
                    }
                    if (prep.stageAchievementValue != null)
                    {
                        prep.stageAchievementValue.text = "";
                        MelonLogger.Msg("[CustomRecordUiPatchHelper.PnlPrep] UI 갱신 성공 -> 기록 없음 (빈 문자열)");
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomRecordUiPatchHelper.PnlPrep] Custom record UI apply error: {ex}");
            }
        }

        public static System.Collections.IEnumerator DelayedApplyPrep(Il2Cpp.PnlPreparation prep, float delay)
        {
            yield return new WaitForSeconds(delay);
            ApplyCustomRecordToPnlPreparation(prep);
        }

        public static void ApplyCustomRecordToPnlRecord(Il2Cpp.PnlRecord pnlRecord)
        {
            try
            {
                if (pnlRecord == null) return;
                string uid = CustomPlaySession.Current.SelectedMusicUid;
                if (string.IsNullOrEmpty(uid)) uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
                if (!CustomContentIds.IsVirtualSong(uid)) return;

                int difficulty = 1;
                if (GlobalDataBase.s_DbBattleStage != null)
                {
                    difficulty = GlobalDataBase.s_DbBattleStage.selectedDifficulty;
                }

                MelonLogger.Msg($"[CustomRecordUiPatchHelper.PnlRecord] 적용 감지: uid={uid}, diff={difficulty}");

                var record = CustomRecordStore.LoadResult(uid, difficulty);
                if (record != null)
                {
                    if (pnlRecord.txtAccuracy != null)
                    {
                        pnlRecord.txtAccuracy.text = $"{record.accuracy:0.00}%";
                    }
                    if (pnlRecord.txtClear != null)
                    {
                        pnlRecord.txtClear.text = "1";
                    }

                    if (pnlRecord.txtCombo != null)
                    {
                        if (record.isFullCombo)
                        {
                            pnlRecord.txtCombo.text = $"{record.noteCount}";
                        }
                        else
                        {
                            pnlRecord.txtCombo.text = $"{record.perfect + record.great}";
                        }
                    }

                    if (pnlRecord.imgIconFc != null)
                    {
                        pnlRecord.imgIconFc.SetActive(record.isFullCombo);
                    }

                    if (pnlRecord.txtScore != null)
                    {
                        int score = (record.perfect * 300) + (record.great * 150);
                        pnlRecord.txtScore.text = $"{score:N0}";
                    }

                    MelonLogger.Msg($"[CustomRecordUiPatchHelper.PnlRecord] UI 상세정보 갱신 성공 -> acc={record.accuracy:0.00}%, FC={record.isFullCombo}, AP={record.isAllPerfect}");
                }
                else
                {
                    if (pnlRecord.txtAccuracy != null) pnlRecord.txtAccuracy.text = "-";
                    if (pnlRecord.txtClear != null) pnlRecord.txtClear.text = "-";
                    if (pnlRecord.txtCombo != null) pnlRecord.txtCombo.text = "-";
                    if (pnlRecord.imgIconFc != null) pnlRecord.imgIconFc.SetActive(false);
                    if (pnlRecord.txtScore != null) pnlRecord.txtScore.text = "-";
                    MelonLogger.Msg("[CustomRecordUiPatchHelper.PnlRecord] UI 상세정보 갱신 성공 -> 기록 없음으로 초기화 (-)");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomRecordUiPatchHelper.PnlRecord] Custom record UI apply error: {ex}");
            }
        }
    }
}
