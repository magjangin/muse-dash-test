using MelonLoader;
using System;
using System.Collections.Generic;

namespace muse_dash_test
{
    /// <summary>
    /// 모든 IL2CPP 및 Unmanaged UnityEngine 객체를 안전하게 감싸 다루는 유니버설 추상 베이스 클래스입니다.
    /// 게임 업데이트로 인해 타입 구조가 변경되더라도 모드가 크래시되는 것을 방지하기 위해 리플렉션을 기반으로 한 바인딩을 제공합니다.
    /// </summary>
    public abstract class Il2CppWrapperBase
    {
        /// <summary>
        /// 래퍼가 감싸고 있는 원본 Unmanaged/IL2CPP 객체 인스턴스입니다.
        /// </summary>
        public object RawObject { get; }
        private static readonly HashSet<string> LoggedFailures = new HashSet<string>(StringComparer.Ordinal);
        private static readonly Dictionary<Type, System.Reflection.MethodInfo> AddMaskMethodCache = new Dictionary<Type, System.Reflection.MethodInfo>();
        private static readonly Type[] AddMaskParamTypes = { typeof(string), typeof(Il2CppSystem.Object) };
        private static readonly object CacheLock = new object();

        /// <summary>
        /// 원본 객체를 전달받아 래퍼 인스턴스를 초기화하는 기저 생성자입니다.
        /// </summary>
        /// <param name="rawObject">감싸고자 하는 IL2CPP 또는 C# 원본 객체 인스턴스</param>
        protected Il2CppWrapperBase(object rawObject)
        {
            RawObject = rawObject;
        }

        /// <summary>
        /// 타입 캐스팅 및 캐싱 리플렉션을 활용해 속성/필드 값을 타입 안전하게 읽어옵니다.
        /// </summary>
        /// <typeparam name="T">반환받고자 하는 C# 타입 (예: string, int, bool 등)</typeparam>
        /// <param name="memberName">읽어올 필드 또는 프로퍼티의 명칭</param>
        /// <param name="silent">true로 설정 시, 읽기 실패 혹은 멤버 부재 시의 Warning 경고 로깅을 생략합니다.</param>
        /// <returns>지정한 타입으로 변환된 값. 실패하거나 멤버가 존재하지 않을 경우 기본값(default)을 반환합니다.</returns>
        protected T Get<T>(string memberName, bool silent = false)
        {
            object val = ModReflection.GetValue(RawObject, memberName, silent);
            if (val == null) return default;
            try
            {
                Type targetType = typeof(T);
                Type underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;
                if (underlying.IsInstanceOfType(val)) return (T)val;
                return (T)Convert.ChangeType(val, underlying, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    string targetName = RawObject != null ? RawObject.GetType().FullName : "Unknown";
                    string cacheKey = $"{targetName}_{memberName}_cast_{typeof(T).Name}";
                    bool isFirstLog = false;
                    lock (CacheLock)
                    {
                        isFirstLog = LoggedFailures.Add(cacheKey);
                    }
                    if (isFirstLog)
                    {
                        ModLogger.Error($"[Il2CppWrapperBase] '{memberName}' 형변환 오류 (Target: {typeof(T).Name}): {ex.Message} (이 에러는 1회만 표시됩니다)");
                    }
                }
            }
            return default;
        }

        /// <summary>
        /// 캐싱 리플렉션을 활용해 원본 객체의 속성/필드에 새로운 값을 안전하게 주입합니다.
        /// </summary>
        /// <typeparam name="T">주입할 데이터의 타입</typeparam>
        /// <param name="memberName">값을 수정할 필드 또는 프로퍼티의 명칭</param>
        /// <param name="value">주입하고자 하는 새로운 값</param>
        /// <param name="silent">true로 설정 시, 쓰기 실패 혹은 멤버 부재 시의 Warning 경고 로깅을 생략합니다.</param>
        protected void Set<T>(string memberName, T value, bool silent = false)
        {
            ModReflection.SetValue(RawObject, memberName, value, silent);
        }

        /// <summary>
        /// PeroTools나 게임 엔진 내부에서 사용되는 마스크 맵(Mask Map)에 동적 메타데이터 값을 안전하게 주입합니다.
        /// </summary>
        /// <param name="key">메타데이터 키 명칭</param>
        /// <param name="value">키에 할당할 객체 값</param>
        public void AddMaskValue(string key, object value)
        {
            if (RawObject == null) return;
            try
            {
                Type type = RawObject.GetType();
                System.Reflection.MethodInfo method;
                bool isFirstMissingLog = false;
                lock (CacheLock)
                {
                    if (!AddMaskMethodCache.TryGetValue(type, out method))
                    {
                        const System.Reflection.BindingFlags flags =
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                        method = type.GetMethod(GameBindings.Il2CppCollection.AddMaskValue, flags, null, AddMaskParamTypes, null);
                        if (method == null)
                        {
                            foreach (var candidate in type.GetMethods(flags))
                            {
                                if (string.Equals(candidate.Name, GameBindings.Il2CppCollection.AddMaskValue, StringComparison.Ordinal)
                                    && candidate.GetParameters().Length == 2)
                                {
                                    method = candidate;
                                    break;
                                }
                            }
                        }
                        AddMaskMethodCache[type] = method;
                    }

                    if (method == null)
                    {
                        isFirstMissingLog = LoggedFailures.Add($"{type.FullName}_{GameBindings.Il2CppCollection.AddMaskValue}_missing");
                    }
                }

                if (method != null)
                {
                    method.Invoke(RawObject, new object[] { key, value });
                }
                else if (isFirstMissingLog)
                {
                    ModLogger.Warning($"[Il2CppWrapperBase] '{type.FullName}'에서 '{GameBindings.Il2CppCollection.AddMaskValue}' 메서드를 찾지 못했습니다 (게임 업데이트 가능성, 1회만 표시됩니다)");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[Il2CppWrapperBase] AddMaskValue 호출 실패: {ex.Message}");
            }
        }
    }
}
