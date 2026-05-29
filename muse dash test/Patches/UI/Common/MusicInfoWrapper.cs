using System;

namespace muse_dash_test
{
    /// <summary>
    /// 게임 내부의 원본 MusicInfo 객체와의 강결합을 방지하고 필드 업데이트 취약성을 격리하는 추상화 어댑터 래퍼입니다.
    /// </summary>
    public class MusicInfoWrapper
    {
        public object RawObject { get; }

        public MusicInfoWrapper(object rawMusicInfo)
        {
            RawObject = rawMusicInfo;
        }

        public string uid
        {
            get => ModReflection.GetValue(RawObject, "uid") as string;
            set => ModReflection.SetValue(RawObject, "uid", value);
        }

        public string name
        {
            get => ModReflection.GetValue(RawObject, "name") as string;
            set => ModReflection.SetValue(RawObject, "name", value);
        }

        public string author
        {
            get => ModReflection.GetValue(RawObject, "author") as string;
            set => ModReflection.SetValue(RawObject, "author", value);
        }

        public string levelDesigner
        {
            get => ModReflection.GetValue(RawObject, "levelDesigner") as string;
            set => ModReflection.SetValue(RawObject, "levelDesigner", value);
        }

        public string cover
        {
            get => ModReflection.GetValue(RawObject, "cover") as string;
            set => ModReflection.SetValue(RawObject, "cover", value);
        }

        public int difficulty1
        {
            get => Convert.ToInt32(ModReflection.GetValue(RawObject, "difficulty1") ?? 0);
            set => ModReflection.SetValue(RawObject, "difficulty1", value);
        }

        public int difficulty2
        {
            get => Convert.ToInt32(ModReflection.GetValue(RawObject, "difficulty2") ?? 0);
            set => ModReflection.SetValue(RawObject, "difficulty2", value);
        }

        public int difficulty3
        {
            get => Convert.ToInt32(ModReflection.GetValue(RawObject, "difficulty3") ?? 0);
            set => ModReflection.SetValue(RawObject, "difficulty3", value);
        }

        public int difficulty4
        {
            get => Convert.ToInt32(ModReflection.GetValue(RawObject, "difficulty4") ?? 0);
            set => ModReflection.SetValue(RawObject, "difficulty4", value);
        }

        public int difficulty5
        {
            get => Convert.ToInt32(ModReflection.GetValue(RawObject, "difficulty5") ?? 0);
            set => ModReflection.SetValue(RawObject, "difficulty5", value);
        }

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
                MelonLoader.MelonLogger.Error($"[MusicInfoWrapper] AddMaskValue 호출 실패: {ex.Message}");
            }
        }
    }
}
