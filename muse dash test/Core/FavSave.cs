using MelonLoader;
using System;

namespace muse_dash_test
{
    public class FavSave
    {
        public static MelonPreferences_Category favCategory;
        public static MelonPreferences_Entry<GirlID> favGirl;
        public static MelonPreferences_Entry<bool> conditionalHideScoreDetails;

        public static GirlID FavGirl
        {
            get => favGirl.Value;
            set
            {
                if (Enum.IsDefined(typeof(GirlID), value))
                    favGirl.Value = value;
                else
                    favGirl.Value = GirlID.NONE;
            }
        }

        public static void Load()
        {
            var girlValuesStr = "";
            foreach (var str in Enum.GetNames(typeof(GirlID))) girlValuesStr += str + "\n";
            girlValuesStr = girlValuesStr.Substring(0, girlValuesStr.Length - 1);

            favCategory = MelonPreferences.CreateCategory("muse-dash-custom-chart");
            favGirl = MelonPreferences.CreateEntry(
                "muse-dash-custom-chart", "favGirl", GirlID.NONE,
                description: "Which girl is currently favorited. Acceptable values:\n" + girlValuesStr
            );
            conditionalHideScoreDetails = MelonPreferences.CreateEntry(
                "muse-dash-custom-chart", "conditionalHideScoreDetails", false,
                description:
                "Whether to automatically hide girl/elfin choices when the ability matches the victory screen.\nFor if you want to get vanilla victory screens."
            );

            if (!Enum.IsDefined(typeof(GirlID), favGirl.Value))
            {
                MelonLogger.Msg("[FavSave] 즐겨찾기 소녀가 정의되지 않음");
                FavGirl = GirlID.NONE;
            }
        }
    }

    // 즐겨찾기로 선택 가능한 유효한 소녀들의 목록
    // 목록에 없는 소녀는 선택할 수 없음
    public enum GirlID
    {
        NONE = -1,
        RIN_BASS = 0,
        RIN_BAD = 1,
        RIN_SLEEP = 2,
        RIN_BUNNY = 3,
        RIN_XMAS = 13,
        RIN_FOOL = 17,
        BURO_PILOT = 4,
        BURO_IDOL = 5,
        BURO_ZOMBIE = 6,
        BURO_JOKER = 7,
        BURO_SAILOR = 14,
        BURO_BIKER = 24,
        OLA_BOXER = 23,
        MARIJA_VIOLIN = 8,
        MARIJA_MAID = 9,
        MARIJA_MAGIC = 10,
        MARIJA_DEVIL = 11,
        MARIJA_BLACK = 12,
        YUME = 15,
        NEKO = 16,
        REIMU = 18,
        EL_CLEAR = 19,
        MARIJA_SISTER = 20,
        MARISA = 21,
        AMIYA = 22,
        MIKU_HATSUNE = 25,
        RIN_LEN = 26,
        RACER = 27,
        BALLERINA = 28,
        WISADEL = 29,
        DIVINE_GEAR = 30,
        BURO_VAMPIRE = 31,
        RIN_PIRATE = 32,
        BURO_DIVER = 33, // DAVE THE DIVER 콜라보 (다이버 부로)
        MARIJA_MADE_BY_ORA_2 = 34, // 만우절 기념 마리쟈 made by ora 2 (구 한정 슬롯, 아래 MARIJA_HORSE와 인덱스 충돌 - 게임이 재사용함)
        MARIJA_HORSE = 34, // 2026-06-26 업데이트로 추가된 말 마리쟈 (CharacterDefine.marija_horse)
        MARIJA_SPECTRE = 35 // 2026-06-26 업데이트로 추가된 유령 마리쟈 (CharacterDefine.marija_spectre)
    }
}
