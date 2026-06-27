using MelonLoader;
using System;
using muse_dash_test;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.UI.Panels;

/// <summary>
/// 곡 선택 화면 및 준비 화면에서 최고 기록 리포트 카드(PnlReportCard)를 갱신할 때
/// 가상 곡(커스텀 채보)의 정보(곡명, 커버, 난이도 별점, 스코어/정확도 기록 등)를 주입하는 패치 클래스입니다.
/// </summary>
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
                // 순정 곡은 오리지널 게임 로직 실행
                return true;
            }

            int difficulty = CustomRecordStore.ResolveCurrentDifficulty();
            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] 가상 곡 기록 적용 시작: uid={uid}, diff={difficulty}");

            // 모듈화된 영역별 텍스트 및 비주얼 주입 실행
            InjectTitleAndArtist(__instance, uid);
            InjectCoverImage(__instance, uid);
            InjectDifficultyStars(__instance, uid, difficulty);
            InjectRecordData(__instance, uid, difficulty);
            InjectEvaluationRank(__instance);

            // 오리지널 메소드 실행을 차단하여 가상 곡 데이터 누락으로 인한 NullReferenceException 방지
            return false;
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlReportCard.RefreshBestRecord] Prefix 예외 발생 (오리지널 로직으로 복구): {ex}");
            return true;
        }
    }

    /// <summary>
    /// 1. 곡 제목 및 아티스트 이름을 UI에 주입합니다.
    /// </summary>
    private static void InjectTitleAndArtist(PnlReportCard instance, string uid)
    {
        string title = "Custom Chart";
        string artist = "Custom Artist";

        bool foundInManifest = MainMod.TryGetHwaPrimarySong(
            uid,
            out string manifestTitle,
            out string manifestArtist,
            out _, out _, out _, out _, out _, out _, out _);

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
            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord.Debug] HwaPrimarySong 조회 실패. dbMusicTag 백업 조회를 시도합니다: uid={uid}");
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

        // 곡 제목 컨트롤러 주입 (게임 기본 RefreshText와 텍스트 컴포넌트 직접 대입 병행)
        if (instance.longSongNameController != null)
        {
            var c = instance.longSongNameController;
            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] longSongNameController: simpleName={c.m_TxtSimpleName != null}, backupName={c.m_TxtBackupName != null}, midSimpleName={c.m_MidSimpleName != null}");
            c.RefreshText(title);
            if (c.m_TxtSimpleName != null) c.m_TxtSimpleName.text = title;
            if (c.m_TxtBackupName != null) c.m_TxtBackupName.text = title;
            if (c.m_MidSimpleName != null) c.m_MidSimpleName.text = title;
        }
        else
        {
            MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] longSongNameController가 null입니다. 곡 제목 반영 불가.");
        }

        // 아티스트 컨트롤러 주입
        if (instance.longAuthorNameController != null)
        {
            var c = instance.longAuthorNameController;
            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] longAuthorNameController: simpleName={c.m_TxtSimpleName != null}, backupName={c.m_TxtBackupName != null}, midSimpleName={c.m_MidSimpleName != null}");
            c.RefreshText(artist);
            if (c.m_TxtSimpleName != null) c.m_TxtSimpleName.text = artist;
            if (c.m_TxtBackupName != null) c.m_TxtBackupName.text = artist;
            if (c.m_MidSimpleName != null) c.m_MidSimpleName.text = artist;
        }
        else
        {
            MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] longAuthorNameController가 null입니다. 아티스트명 반영 불가.");
        }

        // PnlMusicOverride를 통한 계층 구조 일괄 텍스트 갱신 시도
        try
        {
            PnlMusicOverride.ApplySongTitleOverride("PnlReportCard.RefreshBestRecord", instance, uid);
        }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PnlReportCard.RefreshBestRecord.Debug] PnlMusicOverride 적용 중 에러: {ex}");
        }
    }

    /// <summary>
    /// 2. 커스텀 앨범 커버 이미지를 UI에 주입합니다.
    /// </summary>
    private static void InjectCoverImage(PnlReportCard instance, string uid)
    {
        if (instance.imgCover != null)
        {
            if (CoverImageManager.TryGetCoverSprite(uid, out var coverSprite) && coverSprite != null)
            {
                instance.imgCover.sprite = coverSprite;
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
    }

    /// <summary>
    /// 3. 난이도 별점 오브젝트 및 난이도 수치 텍스트를 주입합니다.
    /// </summary>
    private static void InjectDifficultyStars(PnlReportCard instance, string uid, int difficulty)
    {
        try
        {
            var musicInfo = GlobalDataBase.dbMusicTag?.GetMusicInfoFromAll(uid);
            if (musicInfo == null)
            {
                MelonLogger.Warning($"[PnlReportCard.RefreshBestRecord.Debug] 난이도 별점 주입 실패: dbMusicTag에서 uid={uid}의 MusicInfo를 찾지 못했습니다.");
            }
            if (instance.starObjs == null)
            {
                MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] 난이도 별점 주입 실패: instance.starObjs가 null입니다.");
            }
            if (instance.starTxtValues == null)
            {
                MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] 난이도 별점 주입 실패: instance.starTxtValues가 null입니다.");
            }

            if (musicInfo != null && instance.starObjs != null && instance.starTxtValues != null)
            {
                int[] levels = {
                    int.TryParse(musicInfo.difficulty1, out int l1) ? l1 : 0,
                    int.TryParse(musicInfo.difficulty2, out int l2) ? l2 : 0,
                    int.TryParse(musicInfo.difficulty3, out int l3) ? l3 : 0,
                    int.TryParse(musicInfo.difficulty4, out int l4) ? l4 : 0,
                    int.TryParse(musicInfo.difficulty5, out int l5) ? l5 : 0
                };

                MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord] 난이도 매핑: starObjs={instance.starObjs.Count}, starTxtValues={instance.starTxtValues.Count}, diff={difficulty}");

                for (int i = 0; i < instance.starObjs.Count; i++)
                {
                    bool isCurrentDiff = (i == (difficulty - 1));
                    if (instance.starObjs[i] != null)
                    {
                        instance.starObjs[i].SetActive(isCurrentDiff);
                    }
                    else
                    {
                        MelonLogger.Warning($"[PnlReportCard.RefreshBestRecord.Debug] starObjs[{i}]가 null입니다.");
                    }

                    if (i < instance.starTxtValues.Count && instance.starTxtValues[i] != null)
                    {
                        int lvl = (i < levels.Length) ? levels[i] : 0;
                        instance.starTxtValues[i].text = lvl > 0 ? lvl.ToString() : "?";
                    }
                    else if (i < instance.starTxtValues.Count)
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
    }

    /// <summary>
    /// 4. 점수, 정확도, 최대 콤보 등 플레이 기록 데이터를 UI에 표시합니다.
    /// </summary>
    private static void InjectRecordData(PnlReportCard instance, string uid, int difficulty)
    {
        var record = CustomRecordStore.LoadResult(uid, difficulty);
        if (record != null)
        {
            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord.Debug] 가상 곡 기록 로드 성공: uid={uid}, diff={difficulty}");
            if (instance.txtScrore != null) instance.txtScrore.text = CustomRecordUiPatchHelper.FormatScore(record);
            else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] txtScrore가 null입니다.");

            if (instance.txtAccuracy != null) instance.txtAccuracy.text = CustomRecordUiPatchHelper.FormatAccuracy(record);
            else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] txtAccuracy가 null입니다.");

            if (instance.txtCombo != null) instance.txtCombo.text = CustomRecordUiPatchHelper.FormatCombo(record);
            else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] txtCombo가 null입니다.");

            if (instance.txtTotalPassCountValue != null) instance.txtTotalPassCountValue.text = "1";
            else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] txtTotalPassCountValue가 null입니다.");

            if (instance.imgFc != null) instance.imgFc.gameObject.SetActive(record.isFullCombo);
            else MelonLogger.Warning("[PnlReportCard.RefreshBestRecord.Debug] imgFc가 null입니다.");
        }
        else
        {
            MelonLogger.Msg($"[PnlReportCard.RefreshBestRecord.Debug] 가상 곡 플레이 이력 없음. 대시(-) 처리 진행: uid={uid}, diff={difficulty}");
            if (instance.txtScrore != null) instance.txtScrore.text = "-";
            if (instance.txtAccuracy != null) instance.txtAccuracy.text = "-";
            if (instance.txtCombo != null) instance.txtCombo.text = "-";
            if (instance.txtTotalPassCountValue != null) instance.txtTotalPassCountValue.text = "-";
            if (instance.imgFc != null) instance.imgFc.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 5. 평가 랭크(S, SS 등) 아이콘 처리 (현재 기본은 비활성화)
    /// </summary>
    private static void InjectEvaluationRank(PnlReportCard instance)
    {
        if (instance.imgS != null)
        {
            instance.imgS.gameObject.SetActive(false);
        }
    }
}
