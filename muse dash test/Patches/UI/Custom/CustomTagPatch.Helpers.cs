using MelonLoader;
using Il2CppAssets.Scripts.Database.DataClass;
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
            private static void SetMemberValue(object target, string memberName, object value)
            {
                if (target == null)
                {
                    return;
                }

                var type = target.GetType();

                var property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(target, ConvertMemberValue(value, property.PropertyType));
                    return;
                }

                var field = type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(target, ConvertMemberValue(value, field.FieldType));
                }
            }

            private static object ConvertMemberValue(object value, Type targetType)
            {
                if (value == null)
                {
                    return null;
                }

                var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
                if (underlyingType.IsInstanceOfType(value))
                {
                    return value;
                }

                if (underlyingType == typeof(string))
                {
                    return value.ToString();
                }

                if (underlyingType.IsEnum)
                {
                    return Enum.ToObject(underlyingType, value);
                }

                return Convert.ChangeType(value, underlyingType);
            }

            private static object DeepCloneObject(object source)
            {
                return DeepCloneObject(source, new Dictionary<object, object>(new ReferenceEqualityComparer()));
            }

            private static void SetAlbumMetadata(Il2CppAssets.Scripts.Database.MusicInfo info, string albumUidString, int albumIndex, int albumJsonIndex, string albumJsonName)
            {
                if (info == null)
                {
                    return;
                }

                SetMemberValue(info, "albumUidName", albumUidString);
                SetMemberValue(info, "albumIndex", albumIndex);
                SetMemberValue(info, "albumJsonIndex", albumJsonIndex);
                SetMemberValue(info, "albumJsonName", albumJsonName);
                SetMemberValue(info, "_albumUidName_k__BackingField", albumUidString);
                SetMemberValue(info, "_albumIndex_k__BackingField", albumIndex);
                SetMemberValue(info, "_albumJsonIndex_k__BackingField", albumJsonIndex);
                SetMemberValue(info, "_albumJsonName_k__BackingField", albumJsonName);
                SetMemberValue(info, "m_AlbumUidName", albumUidString);
                SetMemberValue(info, "m_AlbumIndex", albumIndex);
                SetMemberValue(info, "m_AlbumJsonIndex", albumJsonIndex);
                SetMemberValue(info, "m_AlbumJsonName", albumJsonName);

                var musicExInfo = GetMemberValue(info, "m_MusicExInfo") ?? GetMemberValue(info, "MusicExInfo");
                if (musicExInfo != null)
                {
                    SetMemberValue(musicExInfo, "albumUidName", albumUidString);
                    SetMemberValue(musicExInfo, "albumIndex", albumIndex);
                    SetMemberValue(musicExInfo, "albumJsonIndex", albumJsonIndex);
                    SetMemberValue(musicExInfo, "albumJsonName", albumJsonName);
                    SetMemberValue(musicExInfo, "_albumUidName_k__BackingField", albumUidString);
                    SetMemberValue(musicExInfo, "_albumIndex_k__BackingField", albumIndex);
                    SetMemberValue(musicExInfo, "_albumJsonIndex_k__BackingField", albumJsonIndex);
                    SetMemberValue(musicExInfo, "_albumJsonName_k__BackingField", albumJsonName);
                    SetMemberValue(musicExInfo, "m_AlbumUidName", albumUidString);
                    SetMemberValue(musicExInfo, "m_AlbumIndex", albumIndex);
                    SetMemberValue(musicExInfo, "m_AlbumJsonIndex", albumJsonIndex);
                    SetMemberValue(musicExInfo, "m_AlbumJsonName", albumJsonName);
                }
            }

            private static object GetMemberValue(object target, string memberName)
            {
                if (target == null)
                {
                    return null;
                }

                var type = target.GetType();
                var property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (property != null && property.CanRead && property.GetIndexParameters().Length == 0)
                {
                    return SafeRead(() => property.GetValue(target));
                }

                var field = type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    return SafeRead(() => field.GetValue(target));
                }

                return null;
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