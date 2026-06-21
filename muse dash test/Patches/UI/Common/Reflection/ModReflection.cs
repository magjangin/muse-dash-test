using MelonLoader;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace muse_dash_test
{
    /// <summary>
    /// IL2CPP 환경에서 C# 컴파일 타임 종속성을 완전히 제거하기 위해
    /// 동적 멤버 조회, 캐싱, 타입 변환 및 패턴 매칭 역매핑을 제공하는 고성능 리플렉션 유틸리티입니다.
    /// 게임 업데이트로 인한 변수명 변경이나 난독화 패턴 변형에 대해 강력한 내성(Resilience)을 제공합니다.
    /// </summary>
    public static class ModReflection
    {
        /// <summary>
        /// 리플렉션 탐색 부하를 최소화하기 위해 타겟 타입 및 멤버명을 기반으로 조회 완료된 MemberInfo를 캐싱합니다.
        /// 조회에 실패한 멤버도 null 상태로 캐싱하여 반복적인 실패 탐색 부하를 원천 차단합니다.
        /// </summary>
        private static readonly Dictionary<string, MemberInfo> MemberCache = new Dictionary<string, MemberInfo>();

        /// <summary>
        /// 특정 멤버의 부재 또는 예외로 인해 로그 파일이 스패밍되는 현상을 방지하기 위해
        /// 에러/경고가 출력된 고유 식별자 키들을 기록하고 중복 로그 출력을 억제합니다.
        /// </summary>
        private static readonly HashSet<string> LoggedFailures = new HashSet<string>();

        /// <summary>
        /// 캐시 딕셔너리 및 로그 기록 리스트에 안전하게 동시 접근(Thread-safe)하기 위한 크리티컬 섹션 락 객체입니다.
        /// </summary>
        private static readonly object CacheLock = new object();

        /// <summary>
        /// 캐싱 및 다각 패턴 조회를 지원하며, 특정 객체의 멤버(프로퍼티 또는 필드)로부터 값을 안전하게 읽어옵니다.
        /// </summary>
        /// <param name="target">값을 조회할 대상 인스턴스 객체</param>
        /// <param name="memberName">읽어올 프로퍼티 혹은 필드 명칭</param>
        /// <param name="silent">true로 설정할 경우, 멤버를 찾지 못하거나 읽기에 실패하더라도 로깅 경고를 발생시키지 않습니다.</param>
        /// <returns>조회된 값 객체. 대상이 null이거나 멤버 부재 시, 혹은 에러 발생 시 null을 반환합니다.</returns>
        public static object GetValue(object target, string memberName, bool silent = false)
        {
            if (target == null) return null;
            Type type = target.GetType();
            MemberInfo member = GetCachedMember(type, memberName, out string cacheKey);

            if (member == null)
            {
                if (!silent && ShouldLogFailure(cacheKey))
                {
                    MelonLogger.Warning($"[ModReflection] 업데이트 경고: 멤버 '{memberName}'을 '{type.FullName}'에서 찾을 수 없습니다. (이 경고는 1회만 표시됩니다)");
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
                if (!silent && ShouldLogFailure($"{cacheKey}_read_err"))
                {
                    MelonLogger.Error($"[ModReflection] '{memberName}' 값을 읽는 중 오류 발생: {ex.Message} (이 에러는 1회만 표시됩니다)");
                }
            }
            return null;
        }

        /// <summary>
        /// 캐싱 및 다각 패턴 조회를 지원하며, 특정 객체의 멤버(프로퍼티 또는 필드)에 안전하게 새로운 값을 주입합니다.
        /// 주입 시 대상 멤버의 타입에 맞춰 데이터 형식을 동적으로 변환(Convert)하여 세팅합니다.
        /// </summary>
        /// <param name="target">값을 설정할 대상 인스턴스 객체</param>
        /// <param name="memberName">수정하고자 하는 프로퍼티 혹은 필드 명칭</param>
        /// <param name="value">주입할 데이터 객체</param>
        /// <param name="silent">true로 설정할 경우, 멤버를 찾지 못하거나 설정 실패 시 로깅을 생략합니다.</param>
        /// <returns>성공적으로 값이 주입되었을 경우 true, 실패했을 경우 false를 반환합니다.</returns>
        public static bool SetValue(object target, string memberName, object value, bool silent = false)
        {
            if (target == null) return false;
            Type type = target.GetType();
            MemberInfo member = GetCachedMember(type, memberName, out string cacheKey);

            if (member == null)
            {
                if (!silent && ShouldLogFailure(cacheKey))
                {
                    MelonLogger.Warning($"[ModReflection] 업데이트 경고: 멤버 '{memberName}'을 '{type.FullName}'에서 찾을 수 없어 값을 주입할 수 없습니다. (이 경고는 1회만 표시됩니다)");
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
                if (!silent && ShouldLogFailure($"{cacheKey}_write_err"))
                {
                    MelonLogger.Error($"[ModReflection] '{memberName}' 값 설정 중 오류 발생: {ex.Message} (이 에러는 1회만 표시됩니다)");
                }
            }
            return false;
        }

        /// <summary>
        /// 타입+멤버명으로 멤버를 조회하되, 결과(실패 시 null 포함)를 캐싱하여 반복 리플렉션을 방지합니다.
        /// 무거운 <see cref="ResolveMember"/> 호출은 락 바깥에서 수행합니다.
        /// </summary>
        private static MemberInfo GetCachedMember(Type type, string memberName, out string cacheKey)
        {
            cacheKey = $"{type.FullName}_{memberName}";

            lock (CacheLock)
            {
                if (MemberCache.TryGetValue(cacheKey, out var cached))
                {
                    return cached;
                }
            }

            MemberInfo member = ResolveMember(type, memberName);
            lock (CacheLock)
            {
                // 조회에 실패했더라도 null 값 그대로 캐싱하여 매 틱마다 리플렉션이 재실행되는 것을 방지합니다.
                MemberCache[cacheKey] = member;
            }
            return member;
        }

        /// <summary>
        /// 해당 키의 실패가 처음 기록되는 경우에만 true를 반환합니다. (로그 스패밍 억제)
        /// </summary>
        private static bool ShouldLogFailure(string key)
        {
            lock (CacheLock)
            {
                return LoggedFailures.Add(key);
            }
        }

        /// <summary>
        /// <see cref="GetValue"/> 결과를 int로 변환해 반환합니다. 멤버 부재 또는 변환 실패 시 fallback을 반환합니다.
        /// 게임 업데이트로 필드명이 바뀌어도 예외 대신 fallback으로 안전하게 degrade됩니다.
        /// </summary>
        public static int GetInt(object target, string memberName, int fallback = 0, bool silent = false)
        {
            object value = GetValue(target, memberName, silent);
            if (value == null) return fallback;
            try { return Convert.ToInt32(value); }
            catch { return fallback; }
        }

        /// <summary>
        /// <see cref="GetValue"/> 결과를 float로 변환해 반환합니다. 멤버 부재 또는 변환 실패 시 fallback을 반환합니다.
        /// </summary>
        public static float GetFloat(object target, string memberName, float fallback = 0f, bool silent = false)
        {
            object value = GetValue(target, memberName, silent);
            if (value == null) return fallback;
            try { return Convert.ToSingle(value); }
            catch { return fallback; }
        }

        /// <summary>
        /// 컴파일 타임 빌드 사양 차이 및 런타임 난독화 접두사를 극복하기 위해 다각적 패턴으로 멤버를 스캔하고 반환합니다.
        /// <para>- 패턴 1: 정확히 일치하는 프로퍼티 명칭</para>
        /// <para>- 패턴 2: 정확히 일치하는 필드 명칭</para>
        /// <para>- 패턴 3: C# 컴파일러 자동 생성 백킹 필드 패턴 (_[name]_k__BackingField)</para>
        /// <para>- 패턴 4: Unity/PeroTools 등에서 사용되는 m_ 접두사 패턴 (m_name, m_Name 등)</para>
        /// <para>- 패턴 5: 대소문자 구분을 무시한 폴백 완화 매칭</para>
        /// </summary>
        /// <param name="type">대상 클래스의 시스템 타입(Type)</param>
        /// <param name="memberName">스캔할 멤버의 영문 기준 명칭</param>
        /// <returns>검색에 성공한 PropertyInfo 또는 FieldInfo 인스턴스. 부재 시 null을 반환합니다.</returns>
        /// <summary>
        /// 'text' 멤버를 가질 수 없는 Unity 핵심 타입들의 FullName 접두사 목록.
        /// 이 타입들에 대한 'text' 조회를 즉시 차단해 불필요한 리플렉션 스캔을 막습니다.
        /// </summary>
        private static readonly string[] TextlessUnityTypePrefixes =
        {
            "UnityEngine.Transform",
            "UnityEngine.RectTransform",
            "UnityEngine.CanvasRenderer",
            "UnityEngine.MeshFilter",
            "UnityEngine.MeshRenderer",
            "UnityEngine.BoxCollider",
            "UnityEngine.CircleCollider",
            "UnityEngine.Rigidbody",
            "UnityEngine.Canvas",
            "UnityEngine.AudioSource",
            "UnityEngine.ParticleSystem",
            "UnityEngine.Animator",
            "UnityEngine.Image",
            "UnityEngine.Mask",
            "UnityEngine.Sprite",
            "UnityEngine.Camera",
            "UnityEngine.Shader",
            "UnityEngine.Material",
        };

        private static bool IsTextlessUnityType(string typeName)
        {
            if (typeName == null) return false;
            foreach (var prefix in TextlessUnityTypePrefixes)
            {
                if (typeName.StartsWith(prefix, StringComparison.Ordinal)) return true;
            }
            return false;
        }

        private static MemberInfo ResolveMember(Type type, string memberName)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

            // 텍스트 속성을 가질 리 없는 대표적인 Unity 핵심 타입들에 대한 'text' 조회 초고속 바이패스 필터
            if (memberName == "text" && IsTextlessUnityType(type.FullName))
            {
                return null;
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

        /// <summary>
        /// 입력된 데이터를 타겟 필드/프로퍼티의 지정된 C# 시스템 형식으로 동적으로 변환합니다.
        /// Nullable 언박싱, 이늄(Enum) 매핑, Convert.ChangeType을 차례대로 수행합니다.
        /// </summary>
        /// <param name="value">변환하고자 하는 입력 데이터 값</param>
        /// <param name="targetType">목표하고자 하는 C# 데이터 타입(Type)</param>
        /// <returns>지정 형식으로 최종 변환 완료된 인스턴스 객체</returns>
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
