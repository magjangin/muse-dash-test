using MelonLoader;
using System;

namespace muse_dash_test
{
    /// <summary>
    /// 모든 IL2CPP 래핑 클래스가 상속받는 유니버설 추상 베이스 클래스입니다.
    /// </summary>
    public abstract class Il2CppWrapperBase
    {
        public object RawObject { get; }

        protected Il2CppWrapperBase(object rawObject)
        {
            RawObject = rawObject;
        }

        /// <summary>
        /// 타입 캐스팅 및 캐싱을 활용해 속성/필드 값을 타입 안전하게 가져옵니다.
        /// </summary>
        protected T Get<T>(string memberName, bool silent = false)
        {
            object val = ModReflection.GetValue(RawObject, memberName, silent);
            if (val == null) return default;
            try
            {
                Type targetType = typeof(T);
                Type underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;
                if (underlying.IsInstanceOfType(val)) return (T)val;
                return (T)Convert.ChangeType(val, underlying);
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    MelonLogger.Error($"[Il2CppWrapperBase] '{memberName}' 형변환 오류 (Target: {typeof(T).Name}): {ex.Message}");
                }
            }
            return default;
        }

        /// <summary>
        /// 캐싱 리플렉션을 활용해 속성/필드에 값을 안전하게 주입합니다.
        /// </summary>
        protected void Set<T>(string memberName, T value, bool silent = false)
        {
            ModReflection.SetValue(RawObject, memberName, value, silent);
        }

        /// <summary>
        /// 마스크 맵에 동적 메타데이터 값을 주입합니다.
        /// </summary>
        public void AddMaskValue(string key, object value)
        {
            try
            {
                var method = RawObject.GetType().GetMethod("AddMaskValue", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (method != null)
                {
                    method.Invoke(RawObject, new object[] { key, value });
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[Il2CppWrapperBase] AddMaskValue 호출 실패: {ex.Message}");
            }
        }
    }
}
