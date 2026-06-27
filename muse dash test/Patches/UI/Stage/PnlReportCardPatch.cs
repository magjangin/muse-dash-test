using MelonLoader;
using System;
using muse_dash_test;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;

[HarmonyLib.HarmonyPatch(typeof(PnlReportCard), "RefreshBestRecord")]
public class PnlReportCard_RefreshBestRecord_Patch
{
    public static bool Prefix(PnlReportCard __instance)
    {
        try
        {
            string uid = CustomPlaySession.Current.SelectedMusicUid;
            if (string.IsNullOrEmpty(uid)) uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid();
            if (!CustomContentIds.IsVirtualSong(uid))
            {
                // 순정 곡은 오리지널 로직 실행
                return true;
            }

            int difficulty = 1;
            if (GlobalDataBase.s_DbBattleStage != null)
            {
                difficulty = GlobalDataBase.s_DbBattleStage.selectedDifficulty;
            }

            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] 가상 곡 기록 적용: uid={uid}, diff={difficulty}");

            // 1. 곡명 및 아티스트 이름 주입
            string title = "Custom Chart";
            string artist = "Custom Artist";
            if (MainMod.TryGetHwaPrimarySong(
                    uid,
                    out string manifestTitle,
                    out string manifestArtist,
                    out _,
                    out _, out _, out _, out _, out _, out _))
            {
                if (!string.IsNullOrWhiteSpace(manifestTitle)) title = manifestTitle;
                if (!string.IsNullOrWhiteSpace(manifestArtist)) artist = manifestArtist;
            }
            else
            {
                var musicInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
                if (musicInfo != null)
                {
                    title = musicInfo.name;
                    artist = musicInfo.author;
                }
            }

            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] Resolved Metadata: title='{title}', artist='{artist}'");

            // 디버그: 컨트롤러 존재 여부 및 하위 컴포넌트 정보 로깅
            if (__instance.longSongNameController != null)
            {
                var c = __instance.longSongNameController;
                MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] longSongNameController: simpleName={c.m_TxtSimpleName != null}, backupName={c.m_TxtBackupName != null}, midSimpleName={c.m_MidSimpleName != null}");
                
                // 방법 A: 게임 기본 메서드 호출
                c.RefreshText(title);

                // 방법 B: 내부 텍스트 컴포넌트 직접 주입
                if (c.m_TxtSimpleName != null) c.m_TxtSimpleName.text = title;
                if (c.m_TxtBackupName != null) c.m_TxtBackupName.text = title;
                if (c.m_MidSimpleName != null) c.m_MidSimpleName.text = title;
            }
            else
            {
                MelonLogger.Warning("[PnlReportCard.RefreshBestRecord] longSongNameController가 null입니다.");
            }

            if (__instance.longAuthorNameController != null)
            {
                var c = __instance.longAuthorNameController;
                MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] longAuthorNameController: simpleName={c.m_TxtSimpleName != null}, backupName={c.m_TxtBackupName != null}, midSimpleName={c.m_MidSimpleName != null}");
                
                // 방법 A: 게임 기본 메서드 호출
                c.RefreshText(artist);

                // 방법 B: 내부 텍스트 컴포넌트 직접 주입
                if (c.m_TxtSimpleName != null) c.m_TxtSimpleName.text = artist;
                if (c.m_TxtBackupName != null) c.m_TxtBackupName.text = artist;
                if (c.m_MidSimpleName != null) c.m_MidSimpleName.text = artist;
            }
            else
            {
                MelonLogger.Warning("[PnlReportCard.RefreshBestRecord] longAuthorNameController가 null입니다.");
            }

            // 방법 C: PnlMusicOverride를 통한 텍스트 계층 구조 일괄 변경 시도
            try
            {
                PnlMusicOverride.ApplySongTitleOverride("PnlReportCard.RefreshBestRecord", __instance, uid);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlReportCard.RefreshBestRecord] PnlMusicOverride 적용 중 에러: {ex}");
            }

            // 2. 앨범 커버 주입
            if (__instance.imgCover != null)
            {
                if (CoverImageManager.TryGetCoverSprite(uid, out var coverSprite) && coverSprite != null)
                {
                    __instance.imgCover.sprite = coverSprite;
                    MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] 앨범 커버 주입 성공: {uid}");
                }
            }

            // 3. 난이도 별점 및 난이도 수치 주입
            try
            {
                var musicInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
                if (musicInfo != null && __instance.starObjs != null && __instance.starTxtValues != null)
                {
                    int[] levels = {
                        int.TryParse(musicInfo.difficulty1, out int l1) ? l1 : 0,
                        int.TryParse(musicInfo.difficulty2, out int l2) ? l2 : 0,
                        int.TryParse(musicInfo.difficulty3, out int l3) ? l3 : 0,
                        int.TryParse(musicInfo.difficulty4, out int l4) ? l4 : 0,
                        int.TryParse(musicInfo.difficulty5, out int l5) ? l5 : 0
                    };

                    MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] 난이도 매핑: starObjs={__instance.starObjs.Count}, starTxtValues={__instance.starTxtValues.Count}, diff={difficulty}");

                    for (int i = 0; i < __instance.starObjs.Count; i++)
                    {
                        bool isCurrentDiff = (i == (difficulty - 1));
                        if (__instance.starObjs[i] != null)
                        {
                            __instance.starObjs[i].SetActive(isCurrentDiff);
                        }

                        if (i < __instance.starTxtValues.Count && __instance.starTxtValues[i] != null)
                        {
                            int lvl = (i < levels.Length) ? levels[i] : 0;
                            __instance.starTxtValues[i].text = lvl > 0 ? lvl.ToString() : "?";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlReportCard.RefreshBestRecord] 난이도 별점 주입 중 예외: {ex}");
            }

            // 4. 기록 정보 대입
            var record = CustomRecordStore.LoadResult(uid, difficulty);
            if (record != null)
            {
                if (__instance.txtScrore != null)
                {
                    int score = (record.perfect * 300) + (record.great * 150);
                    __instance.txtScrore.text = $"{score:N0}";
                }
                if (__instance.txtAccuracy != null)
                {
                    __instance.txtAccuracy.text = $"{record.accuracy:0.00}%";
                }
                if (__instance.txtCombo != null)
                {
                    if (record.isFullCombo)
                    {
                        __instance.txtCombo.text = $"{record.noteCount}";
                    }
                    else
                    {
                        __instance.txtCombo.text = $"{record.perfect + record.great}";
                    }
                }
                if (__instance.txtTotalPassCountValue != null)
                {
                    __instance.txtTotalPassCountValue.text = "1";
                }
                if (__instance.imgFc != null)
                {
                    __instance.imgFc.gameObject.SetActive(record.isFullCombo);
                }
            }
            else
            {
                if (__instance.txtScrore != null) __instance.txtScrore.text = "-";
                if (__instance.txtAccuracy != null) __instance.txtAccuracy.text = "-";
                if (__instance.txtCombo != null) __instance.txtCombo.text = "-";
                if (__instance.txtTotalPassCountValue != null) __instance.txtTotalPassCountValue.text = "-";
                if (__instance.imgFc != null) __instance.imgFc.gameObject.SetActive(false);
            }

            // 5. 등급 평가 이미지 비활성화 (추후 연동 예정으로 항상 비활성화)
            if (__instance.imgS != null)
            {
                __instance.imgS.gameObject.SetActive(false);
            }

            // 오리지널 메소드 실행을 차단하여 NullReferenceException 방지
            return false;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlReportCard.RefreshBestRecord] Prefix 예외: {ex}");
            return true;
        }
    }
}
