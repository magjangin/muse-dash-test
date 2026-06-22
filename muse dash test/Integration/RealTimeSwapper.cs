using System;
using System.IO;
using UnityEngine;
using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.GameCore.Managers;
using Il2Cpp;

namespace muse_dash_test
{
    // 실시간 외형 교체를 위한 정적 클래스
    public static class RealTimeSwapper
    {
        private static bool _isInitialized = false;
        
        public static void Initialize()
        {
            if (!_isInitialized)
            {
                MelonLogger.Msg("[FavGirl] RealTimeSwapper가 초기화되었습니다.");
                _isInitialized = true;
            }
        }
        
        public static void CheckForOKeyPress()
        {
            try
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    MelonLogger.Msg("[FavGirl] O키가 눌렸습니다!");
                    OnKeyPressed();
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error($"[FavGirl] Input 오류: {e.Message}");
            }
        }
        
        public static void OnKeyPressed()
        {
            if (RealTimeSwapManager.IsRealTimeModeActive())
            {
                MelonLogger.Msg("[FavGirl] 실시간 교체 모드가 활성화되어 있습니다. 교체를 시도합니다...");
                RealTimeSwapManager.PerformRealTimeSwap();
            }
            else
            {
                MelonLogger.Msg("[FavGirl] 실시간 교체 모드가 비활성화되어 있습니다. P키를 눌러서 실시간 모드를 활성화하세요.");
            }
        }
    }

    // 실시간 교체 기능 관리자
    public static class RealTimeSwapManager
    {
        private static bool _isRealTimeMode = false;
        private static bool _isInitialized = false;
        private static int _originalSkillRole = -1; // 원래 스킬 캐릭터 저장
        
        public static void Initialize()
        {
            if (!_isInitialized)
            {
                RealTimeSwapper.Initialize();
                _isInitialized = true;
                MelonLogger.Msg("[FavGirl] RealTimeSwapManager가 초기화되었습니다.");
            }
        }
        
        public static bool IsRealTimeModeActive()
        {
            return _isRealTimeMode;
        }
        
        public static bool IsInitialized()
        {
            return _isInitialized;
        }
        
        public static void ToggleRealTimeMode()
        {
            _isRealTimeMode = !_isRealTimeMode;
            
            if (_isRealTimeMode)
            {
                MelonLogger.Msg("[FavGirl] 실시간 외형 교체 모드 활성화! O키를 눌러서 외형을 교체하세요.");
                ResetToggleState();
            }
            else
            {
                MelonLogger.Msg("[FavGirl] 실시간 외형 교체 모드 비활성화");
                ResetToOriginalState();
            }
        }
        
        private static void ResetToggleState()
        {
            _originalSkillRole = -1;
            MelonLogger.Msg("[FavGirl] 토글 상태가 초기화되었습니다.");
        }
        
        private static void ResetToOriginalState()
        {
            if (_originalSkillRole != -1)
            {
                FavSave.FavGirl = (GirlID)_originalSkillRole;
                MelonLogger.Msg($"[FavGirl] 원래 스킬 캐릭터로 복원: {_originalSkillRole}");
            }
            ResetToggleState();
        }
        
        public static void HandleOKeyPress()
        {
            if (_isInitialized)
            {
                RealTimeSwapper.OnKeyPressed();
            }
            else
            {
                MelonLogger.Msg("[FavGirl] RealTimeSwapper가 초기화되지 않았습니다. 초기화를 시도합니다...");
                Initialize();
                if (_isInitialized)
                {
                    RealTimeSwapper.OnKeyPressed();
                }
            }
        }
        
        public static void PerformRealTimeSwap()
        {
            try
            {
                MelonLogger.Msg("=== [FavGirl] 실시간 외형 교체 디버깅 시작 ===");
                
                var currentRole = GlobalDataBase.s_DbBattleStage.m_SelectedRole;
                var currentFavGirl = FavSave.FavGirl;
                
                MelonLogger.Msg($"현재 스킬 캐릭터: {currentRole}, 현재 외형 캐릭터: {currentFavGirl}");
                
                if (_originalSkillRole == -1)
                {
                    _originalSkillRole = currentRole;
                    MelonLogger.Msg($"원래 스킬 캐릭터 저장: {_originalSkillRole}");
                }
                
                var (skillCharacter, appearanceCharacter, testCharacter) = ReadSkinSettings();
                
                if (FavSave.FavGirl == skillCharacter)
                {
                    MelonLogger.Msg($"스킬 캐릭터 → 외형 캐릭터: {skillCharacter} → {appearanceCharacter}");
                    FavSave.FavGirl = appearanceCharacter;
                }
                else if (FavSave.FavGirl == appearanceCharacter)
                {
                    MelonLogger.Msg($"외형 캐릭터 → 테스트 캐릭터: {appearanceCharacter} → {testCharacter}");
                    FavSave.FavGirl = testCharacter;
                }
                else
                {
                    MelonLogger.Msg($"테스트 캐릭터 → 스킬 캐릭터: {FavSave.FavGirl} → {skillCharacter}");
                    FavSave.FavGirl = skillCharacter;
                }
                
                MelonLogger.Msg($"실시간 외형 교체 완료: {currentFavGirl} → {FavSave.FavGirl}");
                
                ForceCharacterRecreation();
                
                MelonLogger.Msg("=== [FavGirl] 실시간 외형 교체 디버깅 완료 ===");
            }
            catch (Exception e)
            {
                MelonLogger.Error($"[FavGirl] 실시간 외형 교체 실패: {e.Message}");
            }
        }
        
        private static void ForceCharacterRecreation()
        {
            try
            {
                MelonLogger.Msg("=== [FavGirl] 캐릭터 강제 재생성 시작 ===");
                
                if (GlobalManagers.girlManager != null)
                {
                    if (GlobalManagers.girlManager.girl != null)
                    {
                        var oldCharacter = GlobalManagers.girlManager.girl;
                        MelonLogger.Msg($"기존 캐릭터 제거: {oldCharacter.name}");
                        
                        UnityEngine.Object.Destroy(oldCharacter);
                        GlobalManagers.girlManager.girl = null;
                        
                        System.Threading.Thread.Sleep(50);
                    }
                    
                    MelonLogger.Msg("새로운 캐릭터 생성 중...");
                    GlobalManagers.girlManager.InstanceGirl();
                    
                    if (GlobalManagers.girlManager.girl != null)
                    {
                        var newCharacter = GlobalManagers.girlManager.girl;
                        MelonLogger.Msg($"새 캐릭터 생성 완료: {newCharacter.name}");
                        
                        ForceRecoverAllRenderers(newCharacter);
                        
                        if (!newCharacter.activeSelf)
                        {
                            newCharacter.SetActive(true);
                        }
                    }
                }
                MelonLogger.Msg("=== [FavGirl] 캐릭터 강제 재생성 완료 ===");
            }
            catch (Exception e)
            {
                MelonLogger.Error($"[FavGirl] 캐릭터 강제 재생성 실패: {e.Message}");
            }
        }

        private static void ForceRecoverAllRenderers(GameObject characterObject)
        {
            try
            {
                var allRenderers = characterObject.GetComponentsInChildren<UnityEngine.Renderer>();
                foreach (var renderer in allRenderers)
                {
                    if (renderer != null)
                    {
                        renderer.enabled = true;
                        
                        if (renderer.material == null)
                        {
                            var defaultMaterial = Resources.GetBuiltinResource<Material>("Default-Material");
                            if (defaultMaterial != null)
                            {
                                renderer.material = defaultMaterial;
                            }
                            else
                            {
                                renderer.material = new Material(Shader.Find("Standard"));
                            }
                        }
                        
                        renderer.enabled = false;
                        renderer.enabled = true;
                    }
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error($"[FavGirl] 렌더러 강제 복구 실패: {e.Message}");
            }
        }
        
        private static (GirlID skillCharacter, GirlID appearanceCharacter, GirlID testCharacter) ReadSkinSettings()
        {
            try
            {
                var skinPath = @"H:\steam\steamapps\common\Muse Dash\skins\skins.txt";
                
                if (!File.Exists(skinPath))
                {
                    MelonLogger.Error($"[FavGirl] 설정 파일을 찾을 수 없습니다: {skinPath}");
                    return (GirlID.MARIJA_BLACK, GirlID.MARIJA_DEVIL, GirlID.RIN_BASS);
                }
                
                var content = File.ReadAllText(skinPath);
                var parts = content.Split(',');
                if (parts.Length >= 3)
                {
                    var skillCharacter = ParseCharacterName(parts[0].Trim());
                    var appearanceCharacter = ParseCharacterName(parts[1].Trim());
                    var testCharacter = ParseCharacterName(parts[2].Trim());
                    
                    return (skillCharacter, appearanceCharacter, testCharacter);
                }
                
                return (GirlID.MARIJA_BLACK, GirlID.MARIJA_DEVIL, GirlID.RIN_BASS);
            }
            catch (Exception e)
            {
                MelonLogger.Error($"[FavGirl] 설정 파일 읽기 실패: {e.Message}");
                return (GirlID.MARIJA_BLACK, GirlID.MARIJA_DEVIL, GirlID.RIN_BASS);
            }
        }
        
        private static GirlID ParseCharacterName(string text)
        {
            // RIN 시리즈
            if (text.Contains("RIN_BASS")) return GirlID.RIN_BASS;
            if (text.Contains("RIN_BAD")) return GirlID.RIN_BAD;
            if (text.Contains("RIN_SLEEP")) return GirlID.RIN_SLEEP;
            if (text.Contains("RIN_BUNNY")) return GirlID.RIN_BUNNY;
            if (text.Contains("RIN_XMAS")) return GirlID.RIN_XMAS;
            if (text.Contains("RIN_FOOL")) return GirlID.RIN_FOOL;
            if (text.Contains("RIN_PIRATE")) return GirlID.RIN_PIRATE;
            if (text.Contains("RIN_LEN")) return GirlID.RIN_LEN;
            if (text.Contains("RACER")) return GirlID.RACER;
            
            // BURO 시리즈
            if (text.Contains("BURO_PILOT")) return GirlID.BURO_PILOT;
            if (text.Contains("BURO_IDOL")) return GirlID.BURO_IDOL;
            if (text.Contains("BURO_ZOMBIE")) return GirlID.BURO_ZOMBIE;
            if (text.Contains("BURO_JOKER")) return GirlID.BURO_JOKER;
            if (text.Contains("BURO_SAILOR")) return GirlID.BURO_SAILOR;
            if (text.Contains("BURO_BIKER")) return GirlID.BURO_BIKER;
            if (text.Contains("BURO_VAMPIRE")) return GirlID.BURO_VAMPIRE;
            if (text.Contains("BURO_DIVER")) return GirlID.BURO_DIVER;
            
            // MARIJA 시리즈
            if (text.Contains("MARIJA_VIOLIN")) return GirlID.MARIJA_VIOLIN;
            if (text.Contains("MARIJA_MAID")) return GirlID.MARIJA_MAID;
            if (text.Contains("MARIJA_MAGIC")) return GirlID.MARIJA_MAGIC;
            if (text.Contains("MARIJA_DEVIL")) return GirlID.MARIJA_DEVIL;
            if (text.Contains("MARIJA_BLACK")) return GirlID.MARIJA_BLACK;
            if (text.Contains("MARIJA_SISTER")) return GirlID.MARIJA_SISTER;
            if (text.Contains("MARIJA_MADE_BY_ORA_2") || text.Contains("ORA_2") || text.Contains("MADE_BY_ORA_2") || text.Contains("MADE_BY_ORA")) return GirlID.MARIJA_MADE_BY_ORA_2;
            
            // 기타 캐릭터들
            if (text.Contains("OLA_BOXER")) return GirlID.OLA_BOXER;
            if (text.Contains("YUME")) return GirlID.YUME;
            if (text.Contains("NEKO")) return GirlID.NEKO;
            if (text.Contains("REIMU")) return GirlID.REIMU;
            if (text.Contains("EL_CLEAR")) return GirlID.EL_CLEAR;
            if (text.Contains("MARISA")) return GirlID.MARISA;
            if (text.Contains("AMIYA")) return GirlID.AMIYA;
            if (text.Contains("MIKU_HATSUNE")) return GirlID.MIKU_HATSUNE;
            if (text.Contains("BALLERINA")) return GirlID.BALLERINA;
            if (text.Contains("WISADEL")) return GirlID.WISADEL;
            if (text.Contains("DIVINE_GEAR")) return GirlID.DIVINE_GEAR;
            
            return GirlID.MARIJA_BLACK;
        }
    }
}
