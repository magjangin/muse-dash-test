using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace muse_dash_test
{
    public static class ModReflection
    {
        private static readonly Dictionary<string, MemberInfo> MemberCache = new Dictionary<string, MemberInfo>();
        private static readonly HashSet<string> LoggedFailures = new HashSet<string>();
        private static readonly object CacheLock = new object();

        /// <summary>
        /// 캐싱을 사용하여 객체의 멤버(프로퍼티 또는 필드)로부터 값을 안전하게 읽어옵니다.
        /// </summary>
        public static object GetValue(object target, string memberName, bool silent = false)
        {
            if (target == null) return null;
            Type type = target.GetType();
            string cacheKey = $"{type.FullName}_{memberName}";

            MemberInfo member;
            bool hasCache;

            lock (CacheLock)
            {
                hasCache = MemberCache.TryGetValue(cacheKey, out member);
            }

            if (!hasCache)
            {
                member = ResolveMember(type, memberName);
                lock (CacheLock)
                {
                    // 조회에 실패했더라도 null 값 그대로 캐싱하여 매 틱마다 리플렉션이 재실행되는 것을 방지합니다.
                    MemberCache[cacheKey] = member;
                }
            }

            if (member == null)
            {
                if (!silent)
                {
                    bool isFirstLog = false;
                    lock (CacheLock)
                    {
                        isFirstLog = LoggedFailures.Add(cacheKey);
                    }
                    if (isFirstLog)
                    {
                        MelonLogger.Warning($"[ModReflection] 업데이트 경고: 멤버 '{memberName}'을 '{type.FullName}'에서 찾을 수 없습니다. (이 경고는 1회만 표시됩니다)");
                    }
                }
                return null;
            }

            try
            {
                if (member is PropertyInfo prop) return prop.GetValue(target);
                if (member is FieldInfo field) return field.GetValue(target);
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    bool isFirstLog = false;
                    string errKey = $"{cacheKey}_read_err";
                    lock (CacheLock)
                    {
                        isFirstLog = LoggedFailures.Add(errKey);
                    }
                    if (isFirstLog)
                    {
                        MelonLogger.Error($"[ModReflection] '{memberName}' 값을 읽는 중 오류 발생: {ex.Message} (이 에러는 1회만 표시됩니다)");
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 캐싱을 사용하여 객체의 멤버(프로퍼티 또는 필드)에 값을 안전하게 주입합니다.
        /// </summary>
        public static bool SetValue(object target, string memberName, object value, bool silent = false)
        {
            if (target == null) return false;
            Type type = target.GetType();
            string cacheKey = $"{type.FullName}_{memberName}";

            MemberInfo member;
            bool hasCache;

            lock (CacheLock)
            {
                hasCache = MemberCache.TryGetValue(cacheKey, out member);
            }

            if (!hasCache)
            {
                member = ResolveMember(type, memberName);
                lock (CacheLock)
                {
                    MemberCache[cacheKey] = member;
                }
            }

            if (member == null)
            {
                if (!silent)
                {
                    bool isFirstLog = false;
                    lock (CacheLock)
                    {
                        isFirstLog = LoggedFailures.Add(cacheKey);
                    }
                    if (isFirstLog)
                    {
                        MelonLogger.Warning($"[ModReflection] 업데이트 경고: 멤버 '{memberName}'을 '{type.FullName}'에서 찾을 수 없어 값을 주입할 수 없습니다. (이 경고는 1회만 표시됩니다)");
                    }
                }
                return false;
            }

            try
            {
                if (member is PropertyInfo prop)
                {
                    if (prop.CanWrite)
                    {
                        prop.SetValue(target, ConvertValue(value, prop.PropertyType));
                        return true;
                    }
                }
                else if (member is FieldInfo field)
                {
                    field.SetValue(target, ConvertValue(value, field.FieldType));
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    bool isFirstLog = false;
                    string errKey = $"{cacheKey}_write_err";
                    lock (CacheLock)
                    {
                        isFirstLog = LoggedFailures.Add(errKey);
                    }
                    if (isFirstLog)
                    {
                        MelonLogger.Error($"[ModReflection] '{memberName}' 값 설정 중 오류 발생: {ex.Message} (이 에러는 1회만 표시됩니다)");
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 컴파일 타임 및 런타임 빌드 변경을 극복하기 위해 다각적 패턴으로 멤버를 자동 해제 및 역매핑합니다.
        /// </summary>
        private static MemberInfo ResolveMember(Type type, string memberName)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            // 텍스트 속성을 가질 리 없는 대표적인 Unity 핵심 타입들에 대한 'text' 조회 초고속 바이패스 필터
            if (memberName == "text")
            {
                string typeName = type.FullName;
                if (typeName != null &&
                    (typeName.StartsWith("UnityEngine.Transform") ||
                     typeName.StartsWith("UnityEngine.RectTransform") ||
                     typeName.StartsWith("UnityEngine.CanvasRenderer") ||
                     typeName.StartsWith("UnityEngine.MeshFilter") ||
                     typeName.StartsWith("UnityEngine.MeshRenderer") ||
                     typeName.StartsWith("UnityEngine.BoxCollider") ||
                     typeName.StartsWith("UnityEngine.CircleCollider") ||
                     typeName.StartsWith("UnityEngine.Rigidbody") ||
                     typeName.StartsWith("UnityEngine.Canvas") ||
                     typeName.StartsWith("UnityEngine.AudioSource") ||
                     typeName.StartsWith("UnityEngine.ParticleSystem") ||
                     typeName.StartsWith("UnityEngine.Animator") ||
                     typeName.StartsWith("UnityEngine.Image") ||
                     typeName.StartsWith("UnityEngine.Mask") ||
                     typeName.StartsWith("UnityEngine.Sprite") ||
                     typeName.StartsWith("UnityEngine.Camera") ||
                     typeName.StartsWith("UnityEngine.Shader") ||
                     typeName.StartsWith("UnityEngine.Material")))
                {
                    return null;
                }
            }

            // 패턴 1: 정확한 프로퍼티명
            var prop = type.GetProperty(memberName, flags);
            if (prop != null && prop.GetIndexParameters().Length == 0) return prop;

            // 패턴 2: 정확한 필드명
            var field = type.GetField(memberName, flags);
            if (field != null) return field;

            // 패턴 3: C# 컴파일러 백킹 필드 패턴 (_[name]_k__BackingField)
            string backingFieldName = $"_{memberName}_k__BackingField";
            field = type.GetField(backingFieldName, flags);
            if (field != null) return field;

            // 패턴 4: Unity/PeroTools 등에서 많이 쓰이는 m_ 접두사 패턴 (필드 및 프로퍼티 자동 감지)
            string mPrefixName1 = $"m_{memberName}";
            field = type.GetField(mPrefixName1, flags);
            if (field != null) return field;
            prop = type.GetProperty(mPrefixName1, flags);
            if (prop != null && prop.GetIndexParameters().Length == 0) return prop;

            string mPrefixName2 = $"m_{char.ToUpperInvariant(memberName[0])}{memberName.Substring(1)}";
            field = type.GetField(mPrefixName2, flags);
            if (field != null) return field;
            prop = type.GetProperty(mPrefixName2, flags);
            if (prop != null && prop.GetIndexParameters().Length == 0) return prop;

            // 패턴 5: 대소문자 구분 없는 폴백 스캔
            foreach (var p in type.GetProperties(flags))
            {
                if (p.GetIndexParameters().Length == 0 && string.Equals(p.Name, memberName, StringComparison.OrdinalIgnoreCase))
                    return p;
            }
            foreach (var f in type.GetFields(flags))
            {
                if (string.Equals(f.Name, memberName, StringComparison.OrdinalIgnoreCase))
                    return f;
            }

            return null;
        }

        private static object ConvertValue(object value, Type targetType)
        {
            if (value == null) return null;
            Type underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (underlying.IsInstanceOfType(value)) return value;
            if (underlying == typeof(string)) return value.ToString();
            if (underlying.IsEnum) return Enum.ToObject(underlying, value);
            return Convert.ChangeType(value, underlying);
        }

        /// <summary>
        /// [자가 진단 모듈] 대상 객체의 모든 프로퍼티와 필드 구조를 콘솔에 에러 등급으로 명확히 덤프합니다.
        /// </summary>
        public static void DumpObjectStructure(object target)
        {
            if (target == null)
            {
                MelonLogger.Error("[ModReflection.Diagnostics] Dump 대상 객체가 null입니다.");
                return;
            }

            Type type = target.GetType();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"\n==================== [ModReflection 자가진단 리포트] ====================");
            sb.AppendLine($"대상 객체 타입: {type.FullName}");
            sb.AppendLine($"------------------------------------------------------------------------");
            sb.AppendLine($"[프로퍼티 목록 (Properties)]");
            
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                string valueStr = "(읽기 불가)";
                if (prop.CanRead && prop.GetIndexParameters().Length == 0)
                {
                    try
                    {
                        var val = prop.GetValue(target);
                        valueStr = val != null ? val.ToString() : "null";
                    }
                    catch (Exception ex)
                    {
                        valueStr = $"(예외: {ex.Message})";
                    }
                }
                sb.AppendLine($"  - Name: {prop.Name} | Type: {prop.PropertyType.Name} | Value: {valueStr}");
            }

            sb.AppendLine($"------------------------------------------------------------------------");
            sb.AppendLine($"[필드 목록 (Fields)]");
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                string valueStr = "null";
                try
                {
                    var val = field.GetValue(target);
                    valueStr = val != null ? val.ToString() : "null";
                }
                catch (Exception ex)
                {
                    valueStr = $"(예외: {ex.Message})";
                }
                sb.AppendLine($"  - Name: {field.Name} | Type: {field.FieldType.Name} | Value: {valueStr}");
            }
            sb.AppendLine($"========================================================================\n");

            MelonLogger.Error(sb.ToString());
        }
    }
}
