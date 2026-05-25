using MelonLoader;
using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Commons;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System;
using System.Reflection;
using System.Collections;

namespace muse_dash_test
{
    public static class CustomUidExperiment
    {
        private const BindingFlags AllInstance = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        private const BindingFlags AllStatic = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        private static bool _hasRun = false;

        public static void RunExperiment()
        {
            if (_hasRun) return;
            
            MelonLogger.Msg("[UidExperiment] === 커스텀 UID DB 등록 실험 시작 ===");
            try
            {
                DBConfigALBUM dbConfigAlbum = null;

                // 1. GlobalDataBase 상태 확인
                if (GlobalDataBase.dbConfig == null)
                {
                    MelonLogger.Warning("[UidExperiment] GlobalDataBase.dbConfig가 null입니다. (아직 DBConfig가 초기화되지 않음)");
                }
                else
                {
                    MelonLogger.Msg("[UidExperiment] GlobalDataBase.dbConfig 획득 성공");
                    if (GlobalDataBase.dbConfig.m_ConfigDic == null)
                    {
                        MelonLogger.Warning("[UidExperiment] GlobalDataBase.dbConfig.m_ConfigDic가 null입니다.");
                    }
                    else
                    {
                        MelonLogger.Msg($"[UidExperiment] m_ConfigDic 검색 중... (총 엔트리 개수: {GlobalDataBase.dbConfig.m_ConfigDic.Count})");
                        foreach (var entry in GlobalDataBase.dbConfig.m_ConfigDic)
                        {
                            var val = entry.Value;
                            string valTypeName = val != null ? val.GetType().FullName : "null";
                            MelonLogger.Msg($"[UidExperiment]   -> Entry Key: '{entry.Key}', Value Type: {valTypeName}");
                            
                            if (val != null)
                            {
                                var casted = val.TryCast<DBConfigALBUM>();
                                if (casted != null)
                                {
                                    dbConfigAlbum = casted;
                                    MelonLogger.Msg($"[UidExperiment]     => DBConfigALBUM 캐스팅 성공! (Key: '{entry.Key}')");
                                    break;
                                }
                            }
                        }
                    }
                }

                // 2. ConfigManager를 통한 시도
                if (dbConfigAlbum == null)
                {
                    MelonLogger.Msg("[UidExperiment] ConfigManager.GetConfigObject<DBConfigALBUM> 조회를 시도합니다...");
                    try
                    {
                        dbConfigAlbum = Singleton<ConfigManager>.instance.GetConfigObject<DBConfigALBUM>();
                        if (dbConfigAlbum != null)
                        {
                            MelonLogger.Msg("[UidExperiment] ConfigManager를 통해 DBConfigALBUM 획득 성공!");
                        }
                        else
                        {
                            MelonLogger.Warning("[UidExperiment] ConfigManager 조회가 null을 반환했습니다.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Warning($"[UidExperiment] ConfigManager 조회 중 예외 발생: {ex.Message}");
                    }
                }

                if (dbConfigAlbum == null)
                {
                    MelonLogger.Error("[UidExperiment] [실패] 최종적으로 DBConfigALBUM 인스턴스를 찾지 못해 실험을 중단합니다.");
                    return;
                }

                MelonLogger.Msg($"[UidExperiment] DBConfigALBUM 인스턴스 최종 확정: {dbConfigAlbum.GetType().FullName}");

                // 3. MusicInfo 조회 및 등록 실험
                string testUid = "0-0";
                MelonLogger.Msg($"[UidExperiment] 원본 곡 '{testUid}' 정보 조회를 시도합니다...");
                var origInfo = dbConfigAlbum.GetMusicInfoByMusicUid(testUid);
                if (origInfo == null)
                {
                    MelonLogger.Error($"[UidExperiment] [실패] 원본 곡 '{testUid}' MusicInfo 조회 결과가 null입니다.");
                    return;
                }

                MelonLogger.Msg($"[UidExperiment] 원본 곡 '{testUid}' 조회 성공: name={origInfo.name}, author={origInfo.author}");

                var newInfo = new MusicInfo();
                CopyMusicInfoFields(origInfo, newInfo);
                
                newInfo.uid = "999-0";
                newInfo.name = "실험용 커스텀 곡";
                newInfo.author = "실험자";
                newInfo.music = "custom_music_999";
                newInfo.demo = "custom_demo_999";
                newInfo.cover = "album_0";
                
                MelonLogger.Msg($"[UidExperiment] 새로운 MusicInfo 생성 완료 (UID: '{newInfo.uid}', Name: '{newInfo.name}')");
                MelonLogger.Msg("[UidExperiment] DBConfigALBUM 내부 컬렉션 주입을 시작합니다...");

                bool injected = TryInjectIntoCollections(dbConfigAlbum, newInfo);
                if (injected)
                {
                    MelonLogger.Msg("[UidExperiment] 내부 컬렉션 주입 완료! GetMusicInfoByMusicUid 검증을 시도합니다...");
                    var queried = dbConfigAlbum.GetMusicInfoByMusicUid("999-0");
                    if (queried != null)
                    {
                        MelonLogger.Msg($"[UidExperiment] ★ 대성공! GetMusicInfoByMusicUid(\"999-0\") 조회 반환: name={queried.name}, author={queried.author}");
                        _hasRun = true;
                    }
                    else
                    {
                        MelonLogger.Error("[UidExperiment] [실패] 주입 후 GetMusicInfoByMusicUid(\"999-0\") 조회가 null을 반환했습니다.");
                    }
                }
                else
                {
                    MelonLogger.Error("[UidExperiment] [실패] 내부 컬렉션 주입에 완전히 실패했습니다.");
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[UidExperiment] 실험 중 치명적 에러 발생: {ex}");
            }
        }

        private static void CopyMusicInfoFields(MusicInfo src, MusicInfo dest)
        {
            var type = typeof(MusicInfo);
            foreach (var prop in type.GetProperties(AllInstance))
            {
                if (prop.CanRead && prop.CanWrite && prop.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        var val = prop.GetValue(src);
                        prop.SetValue(dest, val);
                    }
                    catch { }
                }
            }
        }

        private static bool TryInjectIntoCollections(DBConfigALBUM dbConfig, MusicInfo newInfo)
        {
            bool success = false;
            var type = dbConfig.GetType();
            
            // 1. 프로퍼티 스캔 및 주입
            foreach (var prop in type.GetProperties(AllInstance))
            {
                var propType = prop.PropertyType;
                if (prop.GetIndexParameters().Length == 0 && prop.CanRead)
                {
                    if (propType.FullName.Contains("List") && propType.FullName.Contains("MusicInfo"))
                    {
                        try
                        {
                            var listObj = prop.GetValue(dbConfig);
                            if (listObj != null)
                            {
                                var addMethod = listObj.GetType().GetMethod("Add", new Type[] { typeof(MusicInfo) });
                                if (addMethod != null)
                                {
                                    addMethod.Invoke(listObj, new object[] { newInfo });
                                    MelonLogger.Msg($"[UidExperiment] [성공] 프로퍼티 List '{prop.Name}'에 MusicInfo 주입 완료");
                                    success = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MelonLogger.Warning($"[UidExperiment] 프로퍼티 List '{prop.Name}' 주입 중 에러: {ex.Message}");
                        }
                    }
                    
                    if (propType.FullName.Contains("Dictionary"))
                    {
                        try
                        {
                            var dictObj = prop.GetValue(dbConfig);
                            if (dictObj != null)
                            {
                                var genericArgs = propType.GetGenericArguments();
                                if (genericArgs.Length == 2 && genericArgs[0] == typeof(string))
                                {
                                    if (genericArgs[1].FullName.Contains("MusicInfo"))
                                    {
                                        var addMethod = dictObj.GetType().GetMethod("Add", new Type[] { typeof(string), typeof(MusicInfo) });
                                        if (addMethod != null)
                                        {
                                            addMethod.Invoke(dictObj, new object[] { newInfo.uid, newInfo });
                                            MelonLogger.Msg($"[UidExperiment] [성공] 프로퍼티 Dictionary '{prop.Name}' (string -> MusicInfo)에 주입 완료");
                                            success = true;
                                        }
                                    }
                                    else if (genericArgs[1] == typeof(int))
                                    {
                                        int index = 1000;
                                        var addMethod = dictObj.GetType().GetMethod("Add", new Type[] { typeof(string), typeof(int) });
                                        if (addMethod != null)
                                        {
                                            addMethod.Invoke(dictObj, new object[] { newInfo.uid, index });
                                            MelonLogger.Msg($"[UidExperiment] [성공] 프로퍼티 Dictionary '{prop.Name}' (string -> int index)에 주입 완료");
                                            success = true;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MelonLogger.Warning($"[UidExperiment] 프로퍼티 Dictionary '{prop.Name}' 주입 중 에러: {ex.Message}");
                        }
                    }
                }
            }
            
            // 2. 필드 스캔 및 주입 (백업용)
            foreach (var field in type.GetFields(AllInstance))
            {
                var fieldType = field.FieldType;
                
                if (fieldType.FullName.Contains("List") && fieldType.FullName.Contains("MusicInfo"))
                {
                    try
                    {
                        var listObj = field.GetValue(dbConfig);
                        if (listObj != null)
                        {
                            var addMethod = listObj.GetType().GetMethod("Add", new Type[] { typeof(MusicInfo) });
                            if (addMethod != null)
                            {
                                addMethod.Invoke(listObj, new object[] { newInfo });
                                MelonLogger.Msg($"[UidExperiment] [성공] 필드 List '{field.Name}'에 MusicInfo 주입 완료");
                                success = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Warning($"[UidExperiment] 필드 List '{field.Name}' 주입 중 에러: {ex.Message}");
                    }
                }
                
                if (fieldType.FullName.Contains("Dictionary"))
                {
                    try
                    {
                        var dictObj = field.GetValue(dbConfig);
                        if (dictObj != null)
                        {
                            var genericArgs = fieldType.GetGenericArguments();
                            if (genericArgs.Length == 2 && genericArgs[0] == typeof(string))
                            {
                                if (genericArgs[1].FullName.Contains("MusicInfo"))
                                {
                                    var addMethod = dictObj.GetType().GetMethod("Add", new Type[] { typeof(string), typeof(MusicInfo) });
                                    if (addMethod != null)
                                    {
                                        addMethod.Invoke(dictObj, new object[] { newInfo.uid, newInfo });
                                        MelonLogger.Msg($"[UidExperiment] [성공] 필드 Dictionary '{field.Name}' (string -> MusicInfo)에 주입 완료");
                                        success = true;
                                    }
                                }
                                else if (genericArgs[1] == typeof(int))
                                {
                                    int index = 1000;
                                    var addMethod = dictObj.GetType().GetMethod("Add", new Type[] { typeof(string), typeof(int) });
                                    if (addMethod != null)
                                    {
                                        addMethod.Invoke(dictObj, new object[] { newInfo.uid, index });
                                        MelonLogger.Msg($"[UidExperiment] [성공] 필드 Dictionary '{field.Name}' (string -> int index)에 주입 완료");
                                        success = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Warning($"[UidExperiment] 필드 Dictionary '{field.Name}' 주입 중 에러: {ex.Message}");
                    }
                }
            }
            
            return success;
        }
    }
}
