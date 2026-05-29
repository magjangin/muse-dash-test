using MelonLoader;
using Il2CppAssets.Scripts.Database.DataClass;
using Il2CppAssets.Scripts.PeroTools.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;

namespace muse_dash_test
{
    internal partial class CustomTagPatch
    {
        internal partial class MusicTagPatch
        {
            private static bool TryCloneMusicInfo(Il2CppAssets.Scripts.Database.MusicInfo originalInfo, string uid, out Il2CppAssets.Scripts.Database.MusicInfo clonedInfo)
            {
                clonedInfo = null;

                var clonedObj = originalInfo?.MemberwiseClone();
                if (clonedObj == null)
                {
                    MelonLogger.Error($"[CustomTagPatch] [실패] {uid} originalInfo.MemberwiseClone() 결과가 null입니다.");
                    return false;
                }

                clonedInfo = clonedObj.TryCast<Il2CppAssets.Scripts.Database.MusicInfo>();
                if (clonedInfo == null)
                {
                    MelonLogger.Error($"[CustomTagPatch] [실패] {uid} clonedObj를 MusicInfo로 캐스팅하지 못했습니다.");
                    return false;
                }

                return true;
            }

            private static void ApplyVirtualSongMetadata(Il2CppAssets.Scripts.Database.MusicInfo clonedInfo, string uid, string name, string author, string levelDesigner, int diff1, int diff2)
            {
                if (clonedInfo == null)
                {
                    return;
                }

                var wrapper = new MusicInfoWrapper(clonedInfo);
                wrapper.uid = uid;
                wrapper.name = name;
                wrapper.author = author;
                wrapper.levelDesigner = levelDesigner;
                wrapper.difficulty1 = diff1;
                wrapper.difficulty2 = diff2;
                wrapper.difficulty3 = 0;

                ModReflection.SetValue(clonedInfo, "callBackDifficulty1", diff1);
                ModReflection.SetValue(clonedInfo, "callBackDifficulty2", diff2);
                ModReflection.SetValue(clonedInfo, "callBackDifficulty3", 0);
                ModReflection.SetValue(clonedInfo, "callBackDifficulty4", 0);
                ModReflection.SetValue(clonedInfo, "callBackDifficulty5", 0);
            }

            private static bool ApplyVirtualSongAlbumMetadata(Il2CppAssets.Scripts.Database.MusicInfo clonedInfo, string uid)
            {
                try
                {
                    var wrapper = new MusicInfoWrapper(clonedInfo);
                    wrapper.AddMaskValue("albumUidName", (Il2CppSystem.String)AlbumUidString);
                    wrapper.AddMaskValue("albumIndex", new Il2CppSystem.Int32 { m_value = TagUid }.BoxIl2CppObject());
                    wrapper.AddMaskValue("albumJsonName", (Il2CppSystem.String)"custom_album_998_0");
                    SetAlbumMetadata(clonedInfo, AlbumUidString, TagUid, TagUid + 1, "custom_album_998_0");
                    return true;
                }
                catch (Exception ex)
                {
                    MelonLogger.Error($"[CustomTagPatch] [실패] {uid} AddMaskValue 앨범 마스크 적용 예외: {ex}");
                    return false;
                }
            }

            private static void RegisterVirtualSong(Il2CppAssets.Scripts.Database.MusicInfo clonedInfo, string uid, List<string> musicList)
            {
                var allMusicDict = Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusicTag?.m_AllMusicInfo;
                if (allMusicDict == null)
                {
                    return;
                }

                if (!allMusicDict.ContainsKey(uid))
                {
                    allMusicDict.Add(uid, clonedInfo);
                    MelonLogger.Msg($"[CustomTagPatch] [성공] m_AllMusicInfo 맵에 '{uid}' 신규 주입 완료!");
                }
                else
                {
                    allMusicDict[uid] = clonedInfo;
                    MelonLogger.Msg($"[CustomTagPatch] [알림] m_AllMusicInfo에 '{uid}'이 이미 존재하여 덮어썼습니다.");
                }

                var checkInfo = Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusicTag.GetMusicInfoFromAll(uid);
                if (checkInfo != null && checkInfo.uid == uid)
                {
                    MelonLogger.Msg($"[CustomTagPatch] [대성공] GetMusicInfoFromAll('{uid}') 검증 성공! 반환된 곡 이름: '{checkInfo.name}'");
                    musicList.Add(uid);
                    MelonLogger.Msg($"[CustomTagPatch] [성공] 커스텀 태그 노출 목록에 '{uid}' 추가 완료!");
                }
                else
                {
                    MelonLogger.Error($"[CustomTagPatch] [실패] '{uid}' 주입 후 조회 검증에 실패했습니다.");
                }
            }

            private static void SetMemberValue(object target, string memberName, object value)
            {
                ModReflection.SetValue(target, memberName, value);
            }

            private static object GetMemberValue(object target, string memberName)
            {
                return ModReflection.GetValue(target, memberName);
            }

            private static void SetAlbumMetadata(Il2CppAssets.Scripts.Database.MusicInfo info, string albumUidString, int albumIndex, int albumJsonIndex, string albumJsonName)
            {
                if (info == null)
                {
                    return;
                }

                // ModReflection이 내부적으로 백킹 필드(_albumUidName_k__BackingField, m_AlbumUidName 등)를 자동 탐색하므로 단 한번의 명료한 속성 호출로 충분합니다.
                ModReflection.SetValue(info, "albumUidName", albumUidString);
                ModReflection.SetValue(info, "albumIndex", albumIndex);
                ModReflection.SetValue(info, "albumJsonIndex", albumJsonIndex);
                ModReflection.SetValue(info, "albumJsonName", albumJsonName);

                var musicExInfo = ModReflection.GetValue(info, "m_MusicExInfo") ?? ModReflection.GetValue(info, "MusicExInfo");
                if (musicExInfo != null)
                {
                    ModReflection.SetValue(musicExInfo, "albumUidName", albumUidString);
                    ModReflection.SetValue(musicExInfo, "albumIndex", albumIndex);
                    ModReflection.SetValue(musicExInfo, "albumJsonName", albumJsonName);
                }
            }

            private static object DeepCloneObject(object source, Dictionary<object, object> visited)
            {
                if (source == null)
                {
                    return null;
                }

                Type sourceType = source.GetType();
                if (IsAtomicType(sourceType) || source is UnityEngine.Object)
                {
                    return source;
                }

                if (visited.TryGetValue(source, out object cachedClone))
                {
                    return cachedClone;
                }

                if (sourceType.IsArray)
                {
                    Array sourceArray = (Array)source;
                    Array clonedArray = Array.CreateInstance(sourceType.GetElementType(), sourceArray.Length);
                    visited[source] = clonedArray;
                    for (int i = 0; i < sourceArray.Length; i++)
                    {
                        clonedArray.SetValue(DeepCloneObject(sourceArray.GetValue(i), visited), i);
                    }
                    return clonedArray;
                }

                if (source is IDictionary sourceDictionary)
                {
                    IDictionary clonedDictionary = CreateDictionaryInstance(sourceType);
                    if (clonedDictionary != null)
                    {
                        visited[source] = clonedDictionary;
                        foreach (DictionaryEntry entry in sourceDictionary)
                        {
                            clonedDictionary.Add(DeepCloneObject(entry.Key, visited), DeepCloneObject(entry.Value, visited));
                        }
                        return clonedDictionary;
                    }
                }

                if (source is IList sourceList)
                {
                    IList clonedList = CreateListInstance(sourceType);
                    if (clonedList != null)
                    {
                        visited[source] = clonedList;
                        foreach (object item in sourceList)
                        {
                            clonedList.Add(DeepCloneObject(item, visited));
                        }
                        return clonedList;
                    }
                }

                object target = CreateObjectInstance(sourceType);
                if (target == null)
                {
                    return source;
                }

                visited[source] = target;

                foreach (var property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    try
                    {
                        if (!property.CanRead || !property.CanWrite || property.GetIndexParameters().Length != 0)
                        {
                            continue;
                        }

                        object memberValue = property.GetValue(source);
                        property.SetValue(target, DeepCloneObject(memberValue, visited));
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Warning($"[CustomTagPatch] 깊은 복사 실패: type={sourceType.Name}, member={property.Name}, kind=property, error={ex.Message}");
                    }
                }

                foreach (var field in sourceType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    try
                    {
                        object memberValue = field.GetValue(source);
                        field.SetValue(target, DeepCloneObject(memberValue, visited));
                    }
                    catch (Exception ex)
                    {
                        MelonLogger.Warning($"[CustomTagPatch] 깊은 복사 실패: type={sourceType.Name}, member={field.Name}, kind=field, error={ex.Message}");
                    }
                }

                return target;
            }

            private static bool IsAtomicType(Type type)
            {
                return type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime) || type == typeof(TimeSpan) || type == typeof(Guid);
            }

            private static object CreateObjectInstance(Type type)
            {
                try
                {
                    var constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                    if (constructor != null)
                    {
                        return constructor.Invoke(null);
                    }
                }
                catch
                {
                }

                try
                {
                    return FormatterServices.GetUninitializedObject(type);
                }
                catch
                {
                    return null;
                }
            }

            private static IList CreateListInstance(Type type)
            {
                try
                {
                    object instance = CreateObjectInstance(type);
                    return instance as IList;
                }
                catch
                {
                    return null;
                }
            }

            private static IDictionary CreateDictionaryInstance(Type type)
            {
                try
                {
                    object instance = CreateObjectInstance(type);
                    return instance as IDictionary;
                }
                catch
                {
                    return null;
                }
            }

            private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
            {
                public new bool Equals(object x, object y)
                {
                    return ReferenceEquals(x, y);
                }

                public int GetHashCode(object obj)
                {
                    return obj == null ? 0 : RuntimeHelpers.GetHashCode(obj);
                }
            }
        }
    }
}