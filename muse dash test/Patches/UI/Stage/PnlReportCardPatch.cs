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

            int difficulty = CustomRecordStore.ResolveCurrentDifficulty();

            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] 가상 곡 기록 적용: uid={uid}, diff={difficulty}");

            // 1. 곡명 및 아티스트 이름 주입
            string title = "Custom Chart";
            string artist = "Custom Artist";
            bool foundInManifest = MainMod.TryGetHwaPrimarySong(
                    uid,
                    out string manifestTitle,
                    out string manifestArtist,
                    out _,
                    out _, out _, out _, out _, out _, out _);

            if (foundInManifest)
            {
                MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord.Debug] HwaPrimarySong 매니페스트 조회 성공: uid={uid}, title='{manifestTitle}', artist='{manifestArtist}'");
                if (!string.IsNullOrWhiteSpace(manifestTitle)) title = manifestTitle;
                else MelonLogger.Warning($"[PnlReportCard.RefreshBestRecord.Debug] 매니페스트 내 곡명(title)이 비어있습니다. uid={uid}");
                
                if (!string.IsNullOrWhiteSpace(manifestArtist)) artist = manifestArtist;
                else MelonLogger.Warning($"[PnlReportCard.RefreshBestRecord.Debug] 매니페스트 내 아티스트(artist)가 비어있습니다. uid={uid}");
            }
            else
            {
                MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord.Debug] HwaPrimarySong 조회 실패. dbMusicTag 조회를 백업 시도합니다: uid={uid}");
                if (GlobalDataBase.dbMusicTag == null)
                {
                    MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] GlobalDataBase.dbMusicTag가 null입니다!");
                }
                else
                {
                    var musicInfo = GlobalDataBase.dbMusicTag.GetMusicInfoFromAll(uid);
                    if (musicInfo != null)
                    {
                        title = musicInfo.name;
                        artist = musicInfo.author;
                        MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord.Debug] dbMusicTag에서 MusicInfo 획득 성공: name='{title}', author='{artist}'");
                    }
                    else
                    {
                        MelonLogger.Warning($"[PnlReportCard.RefreshBestRecord.Debug] dbMusicTag 내에서도 uid={uid} 정보를 찾을 수 없습니다.");
                    }
                }
            }

            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] 최종 적용 텍스트: title='{title}', artist='{artist}'");

            // 곡 제목 주입: 컨트롤러 상태를 로깅한 뒤, 게임 기본 메서드(RefreshText)와
            // 내부 텍스트 컴포넌트 직접 대입을 함께 적용한다(둘 중 하나만으론 누락되는 경우가 있어 병행).
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
                MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] longSongNameController가 null입니다. 곡 제목 반영 불가.");
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
                MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] longAuthorNameController가 null입니다. 아티스트명 반영 불가.");
            }

            // 방법 C: PnlMusicOverride를 통한 텍스트 계층 구조 일괄 변경 시도
            try
            {
                PnlMusicOverride.ApplySongTitleOverride("PnlReportCard.RefreshBestRecord", __instance, uid);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlReportCard.RefreshBestRecord.Debug] PnlMusicOverride 적용 중 에러: {ex}");
            }

            // 2. 앨범 커버 주입
            if (__instance.imgCover != null)
            {
                if (CoverImageManager.TryGetCoverSprite(uid, out var coverSprite) && coverSprite != null)
                {
                    __instance.imgCover.sprite = coverSprite;
                    MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] 앨범 커버 주입 성공: {uid}");
                }
                else
                {
                    MelonLogger.Warning($"[PnlReportCard.RefreshBestRecord.Debug] 커스텀 앨범 커버(cover.png) 로드 실패: uid={uid}");
                }
            }
            else
            {
                MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] imgCover 컴포넌트가 null입니다. 앨범 커버 반영 불가.");
            }

            // 3. 난이도 별점 및 난이도 수치 주입
            try
            {
                var musicInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
                if (musicInfo == null)
                {
                    MelonLogger.Warning($"[PnlReportCard.RefreshBestRecord.Debug] 난이도 별점 주입 실패: dbMusicTag에서 uid={uid}의 MusicInfo를 찾지 못했습니다.");
                }
                if (__instance.starObjs == null)
                {
                    MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] 난이도 별점 주입 실패: __instance.starObjs가 null입니다.");
                }
                if (__instance.starTxtValues == null)
                {
                    MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] 난이도 별점 주입 실패: __instance.starTxtValues가 null입니다.");
                }

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
                        else
                        {
                            MelonLogger.Warning($"[PnlReportCard.RefreshBestRecord.Debug] starObjs[{i}]가 null입니다.");
                        }

                        if (i < __instance.starTxtValues.Count && __instance.starTxtValues[i] != null)
                        {
                            int lvl = (i < levels.Length) ? levels[i] : 0;
                            __instance.starTxtValues[i].text = lvl > 0 ? lvl.ToString() : "?";
                        }
                        else if (i < __instance.starTxtValues.Count)
                        {
                            MelonLogger.Warning($"[PnlReportCard.RefreshBestRecord.Debug] starTxtValues[{i}]가 null입니다.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[PnlReportCard.RefreshBestRecord.Debug] 난이도 별점 주입 중 예외: {ex}");
            }

            // 4. 기록 정보 대입
            var record = CustomRecordStore.LoadResult(uid, difficulty);
            if (record != null)
            {
                MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord.Debug] 가상 곡 기록 로드 성공: uid={uid}, diff={difficulty}");
                if (__instance.txtScrore != null)
                {
                    __instance.txtScrore.text = CustomRecordUiPatchHelper.FormatScore(record);
                }
                else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] txtScrore가 null입니다.");

                if (__instance.txtAccuracy != null)
                {
                    __instance.txtAccuracy.text = CustomRecordUiPatchHelper.FormatAccuracy(record);
                }
                else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] txtAccuracy가 null입니다.");

                if (__instance.txtCombo != null)
                {
                    __instance.txtCombo.text = CustomRecordUiPatchHelper.FormatCombo(record);
                }
                else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] txtCombo가 null입니다.");

                if (__instance.txtTotalPassCountValue != null)
                {
                    __instance.txtTotalPassCountValue.text = "1";
                }
                else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] txtTotalPassCountValue가 null입니다.");

                if (__instance.imgFc != null)
                {
                    __instance.imgFc.gameObject.SetActive(record.isFullCombo);
                }
                else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] imgFc가 null입니다.");
            }
            else
            {
                MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord.Debug] 가상 곡 플레이 이력 없음. 대시(-) 처리 진행: uid={uid}, diff={difficulty}");
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
