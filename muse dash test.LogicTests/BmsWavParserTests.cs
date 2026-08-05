namespace muse_dash_test.LogicTests
{
    // BmsWavParser: WAV 파일명 → UID(zzxxyy) 해석, NoteType 매핑, dt 추출, 보스 액션/전환 매핑 검증.
    public static class BmsWavParserTests
    {
        public static void ParseWavName_ReturnsNullForBlankName()
        {
            Assert.Null(BmsWavParser.ParseWavName(null));
            Assert.Null(BmsWavParser.ParseWavName("   "));
        }

        public static void ParseWavName_ParsesUidDtAndPrefabForNormalNote()
        {
            var info = BmsWavParser.ParseWavName("011001_일반 노트1 지상 노멀_dt1.48.wav");

            Assert.NotNull(info);
            Assert.Equal("011001", info.Uid);
            Assert.Equal(1.48, info.Dt);
            Assert.Equal(1, info.NoteType); // 매핑 테이블에 없는 xx는 일반 노트
            // 한글/공백이 섞인 이름은 프리팹명으로 못 쓰므로 UID로 대체됩니다.
            Assert.Equal("011001", info.PrefabName);
        }

        public static void ParseWavName_KeepsAsciiOnlyPrefabNameAndStripsDtSuffix()
        {
            var info = BmsWavParser.ParseWavName("011001_ground_normal_dt1.48.wav");

            Assert.Equal("011001_ground_normal", info.PrefabName);
            Assert.Equal(1.48, info.Dt);
        }

        public static void ParseWavName_MapsHeartPrefixToHpNote()
        {
            var info = BmsWavParser.ParseWavName("000201_하트 지상_dt1.48.wav");

            Assert.Equal(6, info.NoteType);
            Assert.Equal("sfx_hp", info.KeyAudio);
        }

        public static void ParseWavName_MapsScorePrefixToScoreNote()
        {
            var info = BmsWavParser.ParseWavName("000301_점수 노트_dt1.48.wav");

            Assert.Equal(7, info.NoteType);
            Assert.Equal("sfx_score", info.KeyAudio);
        }

        public static void ParseWavName_ScenePrefixWinsOverSandbagXxRule()
        {
            // 0004xx는 UID 중간 두 자리가 "04"라 샌드백(8) 규칙과 충돌합니다.
            // prefix "0004"가 우선해서 SceneToggle(9)로 고정돼야 합니다.
            var info = BmsWavParser.ParseWavName("000401_씬 전환.wav");

            Assert.Equal(9, info.NoteType);
        }

        public static void ParseWavName_MapsXxToHoldSandbagAndGhost()
        {
            Assert.Equal(3, BmsWavParser.ParseWavName("010201_홀드 지상.wav").NoteType);
            Assert.Equal(8, BmsWavParser.ParseWavName("010401_샌드백.wav").NoteType);
            Assert.Equal(4, BmsWavParser.ParseWavName("011701_고스트.wav").NoteType);
            Assert.Equal(2, BmsWavParser.ParseWavName("010301_장애물.wav").NoteType);
        }

        public static void ParseWavName_MapsBossInTransitionFromUid()
        {
            var info = BmsWavParser.ParseWavName("010101_보스 등장.wav");

            Assert.Equal(0, info.NoteType);
            Assert.Equal("empty_000", info.PrefabName);
            Assert.Equal("in", info.BossAction);
            Assert.Equal("in", info.BossTransition);
            Assert.Equal("0101_boss", info.BossName);
            Assert.Equal(1, info.BossScene);
        }

        public static void ParseWavName_MapsBossOutTransitionFromUid()
        {
            var info = BmsWavParser.ParseWavName("010102_보스 퇴장.wav");

            Assert.Equal(0, info.NoteType);
            Assert.Equal("out", info.BossAction);
            Assert.Equal("out", info.BossTransition);
            Assert.Equal("0101_boss", info.BossName);
            Assert.Equal(1, info.BossScene);
        }

        public static void ParseWavName_DerivesBossTargetFromUidScenePrefix()
        {
            var info = BmsWavParser.ParseWavName("040101_보스 등장.wav");

            Assert.Equal("0401_boss", info.BossName);
            Assert.Equal(4, info.BossScene);
        }

        public static void ParseWavName_MapsFarAttackTransitionsFromUid()
        {
            Assert.Equal("boss_far_atk_1_start", BmsWavParser.ParseWavName("010107_원거리 시작.wav").BossAction);
            Assert.Equal("boss_far_atk_1_end", BmsWavParser.ParseWavName("010108_원거리 종료.wav").BossAction);
            Assert.Equal("boss_far_atk_2_start", BmsWavParser.ParseWavName("010109_원거리2 시작.wav").BossAction);
            Assert.Equal("boss_far_atk_2_end", BmsWavParser.ParseWavName("010110_원거리2 종료.wav").BossAction);

            // xx=01 전환 노트는 전환 상태를 갖지 않습니다(등장/퇴장만 in/out).
            Assert.Null(BmsWavParser.ParseWavName("010107_원거리 시작.wav").BossTransition);
        }

        public static void ParseWavName_AssignsProjectileActionOnlyWhenNameMarksBoss()
        {
            var withBossMarker = BmsWavParser.ParseWavName("010601_보스 발사체1 지상 노멀_boss_dt0.7.wav");
            Assert.Equal("boss_far_atk_1_R", withBossMarker.BossAction);
            Assert.Equal(0.7, withBossMarker.Dt);
            Assert.Equal(1, withBossMarker.NoteType);

            // _boss / _atk 표시가 없으면 일반 발사체로 남고 액션이 비워집니다.
            var withoutBossMarker = BmsWavParser.ParseWavName("010601_보스 발사체1 지상 노멀_dt0.7.wav");
            Assert.Equal("", withoutBossMarker.BossAction);
            Assert.Equal(0.7, withoutBossMarker.Dt);
        }

        public static void ParseWavName_MapsBossGearProjectileActions()
        {
            var info = BmsWavParser.ParseWavName("010902_보스 톱니_boss.wav");

            Assert.Equal(2, info.NoteType); // xx=09 → Boss Gear
            Assert.Equal("boss_far_atk_1_R", info.BossAction);
            Assert.Equal(0.7, info.Dt);
        }

        public static void ParseWavName_LeavesUidNullWhenNameHasNoSixDigitPrefix()
        {
            var info = BmsWavParser.ParseWavName("kick.wav");

            Assert.NotNull(info);
            Assert.Null(info.Uid);
            Assert.Equal(1, info.NoteType);
            Assert.Equal("kick.wav", info.RawWavName);
        }
    }
}
