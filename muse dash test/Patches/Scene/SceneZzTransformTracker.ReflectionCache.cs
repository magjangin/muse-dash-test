using System;
using System.Collections.Generic;
using System.Reflection;
using Il2CppGameLogic;

namespace muse_dash_test
{
    /// <summary>
    /// 런타임 오브젝트 복구에 쓰는 필드/프로퍼티 리플렉션 캐시.
    ///
    /// <para>예전에는 여기에 진단 덤프용 <c>AccessPath</c>/<c>TypeDumpSchema</c> 탐색기가 함께 있었습니다.
    /// IL2CPP 타입을 깊이 2까지 재귀로 파고들어 MusicData·스칼라 경로를 모아두는 물건이었는데,
    /// 그걸 쓰던 진단 덤프가 노트가 많은 곡에서 네이티브 크래시를 내서 덤프와 함께 제거했습니다
    /// (2026-08-20, <see cref="RestoreRuntimeObjects"/> 주석 참고).</para>
    /// </summary>
    internal static partial class SceneZzTransformTracker
    {
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
