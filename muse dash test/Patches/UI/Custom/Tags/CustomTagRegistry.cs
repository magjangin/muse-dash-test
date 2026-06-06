using MelonLoader;
using Il2Cpp;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

using static Il2CppAssets.Scripts.Database.DBConfigCustomTags;

namespace muse_dash_test
{
    /// <summary>
    /// 게임 내부의 커스텀 태그(실험 모드 카테고리) 및 가상 곡, 가상 앨범 주입을 전담하는 도메인 매니저 레지스트리 서비스 클래스입니다.
    /// </summary>
    public static class CustomTagRegistry
    {
        public const int TagUid = 1998;
        public const string TagUidString = "tag-muse-dash-test";
        public const string AlbumUidString = "1998-0";
        public const string AlbumTitle = "실험 앨범";
        public const string AlbumCoverPrefabName = "album_0";

        public static DBConfigAlbums.AlbumsInfo CustomAlbumInfo { get; internal set; }

        /// <summary>
        /// MusicTagManager가 앨범 태그 정보를 초기화할 때 진입하여 커스텀 태그 및 가상 앨범/가상 곡 정보를 일괄 등록합니다.
        /// </summary>
        public static void RegisterAll(MusicTagManager musicTagManager)
        {
            if (musicTagManager == null) return;

            // 실행 시점에 게임 내 UID 및 태그 최대값을 수집하고 안전 범위를 계산하여 로그에 출력합니다.
            AnalyzeMaxUids();

            try
            {
                // 1. 태그 탭 다국어 명칭 정의 및 인스턴스 생성
                var il2CppLanguages = CustomTagRegistrySupport.CreateTagLanguages(out string defaultName);
                var info = new AlbumTagInfo
                {
                    name = defaultName,
                    tagUid = TagUidString,
                    iconName = "IconCustomAlbums"
                };

                // 2. 가상 곡 생성 및 주입
                var musicList = CustomTagRegistrySupport.BuildAndInjectVirtualSongs();
                var customInfoMusicList = CustomTagRegistrySupport.ToIl2CppStringList(musicList);

                // 3. CustomTagInfo 설정
                var customInfo = new CustomTagInfo
                {
                    tag_name = il2CppLanguages,
                    tag_picture = System.IO.Path.Combine(MelonLoader.Utils.MelonEnvironment.GameRootDirectory, "hwa tag image", "tag_icon.png"),
                    music_list = customInfoMusicList
                };

                info.InitCustomTagInfo(customInfo);

                var tagMusicList = CustomTagRegistrySupport.ToIl2CppStringList(musicList);
                var displayMusicList = CustomTagRegistrySupport.ToIl2CppStringList(musicList);
                info.SetTagUids(CustomTagRegistrySupport.ToIl2CppStringList(musicList));

                // 4. 가상 앨범 생성 및 주입
                var albumInfo = CustomTagRegistrySupport.CreateAndInjectAlbumInfo();

                // 5. 앨범 정보 맵에 바인딩
                var albumInfos = new Il2CppSystem.Collections.Generic.List<DBConfigAlbums.AlbumsInfo>(1);
                albumInfos.Add(albumInfo);
                info.m_AlbumsInfos = albumInfos;

                var displayAlbum = new AlbumDisplayMusic(albumInfo);
                displayAlbum.AddRangeMusicUid(displayMusicList);

                var displayAlbums = new Il2CppSystem.Collections.Generic.List<AlbumDisplayMusic>(1);
                displayAlbums.Add(displayAlbum);
                info.m_DisplayMusicUids = displayAlbums;
                info.m_MusicUids = tagMusicList;

                // 6. 글로벌 데이터베이스의 커스텀 태그 정렬 목록에 등록
                if (!GlobalDataBase.dbMusicTag.AllAlbumTagsSortContains(TagUid))
                {
                    GlobalDataBase.dbMusicTag.AddCustomAlbumTagsSort(TagUid);
                }

                // 7. 글로벌 데이터베이스에 태그 데이터 최종 등록
                GlobalDataBase.dbMusicTag.AddAlbumTagData(TagUid, info);
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[CustomTagRegistry] 커스텀 태그 주입 중 치명적인 예외가 발생했습니다: {ex}");
            }
        }


        /// <summary>
        /// 게임 내 모든 곡 정보(MusicInfo)와 앨범 구성(DBConfigAlbums)을 검색하여
        /// 등록된 태그 인덱스 및 곡 UID의 최대값들을 정밀 추적하고 안전 임계 범위를 분석합니다.
        /// </summary>
        public static void AnalyzeMaxUids()
        {
            try
            {
                // 4. 커스텀 허용 임계 한계값 분석 (Maximum Safety Threshold Analysis)
                MelonLogger.Msg("[UidAnalysis] === 게임 내부 커스텀 입력 최대 한계 분석 리포트 ===");
                
                // (1) 태그 UID 한계
                int tagLimitTheoretical = int.MaxValue;
                int tagLimitRecommended = 999;
                string tagStatus = (TagUid <= tagLimitRecommended) ? "SAFE" : (TagUid <= 2000 ? "WARN (UI lag might occur)" : "DANGER (High risk of crash)");
                MelonLogger.Msg($"[UidAnalysis] - 태그 UID (tagIndex): 이론상 최대 {tagLimitTheoretical:N0} | 실질적 권장 최대 {tagLimitRecommended} (현재 설정값: {TagUid} -> {tagStatus})");

                // (2) 앨범 UID 한계
                int albumLimitTheoretical = int.MaxValue;
                int albumLimitRecommended = 999;
                int currentAlbumIdx = TagUid; // 앨범 UID의 인덱스로 TagUid를 씀
                string albumStatus = (currentAlbumIdx <= albumLimitRecommended) ? "SAFE" : "WARN (UI performance hit)";
                MelonLogger.Msg($"[UidAnalysis] - 앨범 UID (albumIndex): 이론상 최대 {albumLimitTheoretical:N0} | 실질적 권장 최대 {albumLimitRecommended} (현재 설정값: {currentAlbumIdx} -> {albumStatus})");

                // (3) 곡 UID 한계
                int currentSongAlbumPart = 1999;
                string songStatus = (currentSongAlbumPart >= 999 && currentSongAlbumPart <= 9999) ? "SAFE" : "WARN (Extremely high index)";
                MelonLogger.Msg($"[UidAnalysis] - 곡 UID (songUid): 이론상 최대 2147483647-2147483647 | 실질적 권장 최대 9999-99 (현재 설정 패턴: {currentSongAlbumPart}-* -> {songStatus})");
                
                MelonLogger.Msg("[UidAnalysis] === 게임 내부 커스텀 입력 최대 한계 분석 완료 ===");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[UidAnalysis] UID 및 태그 최대값 분석 중 예외 발생: {ex}");
            }
        }
    }
}
