using System;
using System.Collections.Generic;
using System.Reflection;
using Il2CppGameLogic;

namespace muse_dash_test
{
    /// <summary>
    /// 진단 덤프용 리플렉션 접근 경로 탐색 및 필드/프로퍼티 캐시.
    /// </summary>
    internal static partial class SceneZzTransformTracker
    {
        // 리플렉션 컴파일된 접근 경로 클래스
        private sealed class AccessPath
        {
            public readonly string Description;
            public readonly MemberInfo[] Members;

            public AccessPath(string description, List<MemberInfo> members)
            {
                Description = description;
                Members = members.ToArray();
            }

            public object Evaluate(object obj)
            {
                object current = obj;
                for (int i = 0; i < Members.Length; i++)
                {
                    if (current == null) return null;
                    var member = Members[i];
                    if (member is FieldInfo field)
                    {
                        current = field.GetValue(current);
                    }
                    else if (member is PropertyInfo prop)
                    {
                        current = prop.GetValue(current);
                    }
                }
                return current;
            }
        }

        // 특정 타입의 MusicData 및 scalar 진단 필드 스키마 정보
        private sealed class TypeDumpSchema
        {
            public readonly List<AccessPath> MusicDataPaths = new List<AccessPath>();
            public readonly List<AccessPath> ScalarPaths = new List<AccessPath>();
        }

        private static readonly Dictionary<Type, TypeDumpSchema> DumpSchemas = new Dictionary<Type, TypeDumpSchema>();

        private static TypeDumpSchema GetOrCreateDumpSchema(Type type)
        {
            if (!DumpSchemas.TryGetValue(type, out var schema))
            {
                schema = new TypeDumpSchema();
                DiscoverSchemaPaths(type, new List<MemberInfo>(), 0, new HashSet<Type>(), schema);
                DumpSchemas[type] = schema;
            }
            return schema;
        }

        private static void DiscoverSchemaPaths(Type type, List<MemberInfo> currentPath, int depth, HashSet<Type> visitedTypes, TypeDumpSchema schema)
        {
            if (type == null || depth > 2) return;
            if (!visitedTypes.Add(type)) return;

            // 1. Fields 탐색
            foreach (var field in GetFieldsCached(type))
            {
                try
                {
                    var fieldType = field.FieldType;
                    var nextPath = new List<MemberInfo>(currentPath) { field };

                    if (fieldType == typeof(MusicData))
                    {
                        string desc = string.Join(".", nextPath.ConvertAll(m => m.Name));
                        schema.MusicDataPaths.Add(new AccessPath(desc, nextPath));
                    }
                    else if (fieldType == typeof(string) || fieldType == typeof(int) || fieldType == typeof(uint))
                    {
                        if (field.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                        {
                            string desc = string.Join(".", nextPath.ConvertAll(m => m.Name));
                            schema.ScalarPaths.Add(new AccessPath(desc, nextPath));
                        }
                    }
                    else if (ShouldInspectNestedType(fieldType))
                    {
                        DiscoverSchemaPaths(fieldType, nextPath, depth + 1, new HashSet<Type>(visitedTypes), schema);
                    }
                }
                catch (Exception) { }
            }

            // 2. Properties 탐색
            foreach (var prop in GetPropertiesCached(type))
            {
                try
                {
                    var propType = prop.PropertyType;
                    var nextPath = new List<MemberInfo>(currentPath) { prop };

                    if (propType == typeof(MusicData))
                    {
                        string desc = string.Join(".", nextPath.ConvertAll(m => m.Name));
                        schema.MusicDataPaths.Add(new AccessPath(desc, nextPath));
                    }
                    else if (propType == typeof(string) || propType == typeof(int) || propType == typeof(uint))
                    {
                        if (prop.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                        {
                            string desc = string.Join(".", nextPath.ConvertAll(m => m.Name));
                            schema.ScalarPaths.Add(new AccessPath(desc, nextPath));
                        }
                    }
                    else if (ShouldInspectNestedType(propType))
                    {
                        DiscoverSchemaPaths(propType, nextPath, depth + 1, new HashSet<Type>(visitedTypes), schema);
                    }
                }
                catch (Exception) { }
            }
        }

        private static bool ShouldInspectNestedType(Type type)
        {
            if (type == null) return false;
            string typeName = type.FullName ?? string.Empty;
            return typeName.StartsWith("Il2Cpp", StringComparison.Ordinal)
                && !typeName.StartsWith("Il2CppUnityEngine.", StringComparison.Ordinal)
                && !typeName.StartsWith("Il2CppSystem.", StringComparison.Ordinal)
                && !typeName.StartsWith("UnityEngine.", StringComparison.Ordinal)
                && !typeName.StartsWith("System.", StringComparison.Ordinal)
                && !typeName.Contains("String");
        }

        // 리플렉션 캐시
        private static readonly Dictionary<Type, FieldInfo[]> FieldsCache = new Dictionary<Type, FieldInfo[]>();
        private static readonly Dictionary<Type, PropertyInfo[]> PropertiesCache = new Dictionary<Type, PropertyInfo[]>();
        private static readonly Dictionary<Type, PropertyInfo> CountPropertyCache = new Dictionary<Type, PropertyInfo>();
        private static readonly Dictionary<Type, PropertyInfo> ItemPropertyCache = new Dictionary<Type, PropertyInfo>();

        private static FieldInfo[] GetFieldsCached(Type type)
        {
            if (!FieldsCache.TryGetValue(type, out var fields))
            {
                fields = type.GetFields(DefaultFlags);
                FieldsCache[type] = fields;
            }
            return fields;
        }

        private static PropertyInfo[] GetPropertiesCached(Type type)
        {
            if (!PropertiesCache.TryGetValue(type, out var props))
            {
                var list = new List<PropertyInfo>();
                foreach (var prop in type.GetProperties(DefaultFlags))
                {
                    if (prop.CanRead && prop.GetIndexParameters().Length == 0)
                    {
                        list.Add(prop);
                    }
                }
                props = list.ToArray();
                PropertiesCache[type] = props;
            }
            return props;
        }

        private static PropertyInfo GetCountProperty(Type type)
        {
            if (!CountPropertyCache.TryGetValue(type, out var prop))
            {
                prop = type.GetProperty("Count");
                CountPropertyCache[type] = prop;
            }
            return prop;
        }

        private static PropertyInfo GetItemProperty(Type type)
        {
            if (!ItemPropertyCache.TryGetValue(type, out var prop))
            {
                prop = type.GetProperty("Item");
                ItemPropertyCache[type] = prop;
            }
            return prop;
        }
    }
}
