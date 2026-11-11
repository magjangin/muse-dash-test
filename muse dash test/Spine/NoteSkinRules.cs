using System;
using System.Collections.Generic;
using System.IO;

namespace muse_dash_test
{
    /// <summary>
    /// 노트 스킨 주입의 규칙 중 게임에 의존하지 않는 부분입니다. 로직 테스트(<c>NoteSkinRulesTests</c>)로 검증합니다.
    ///
    /// <para><b>파일 배치</b>: 게임 루트 <c>skin test/{세트 이름}/</c>에 Spine 3.8 에디터가 내보낸 세트를 둡니다(캐릭터 스킨과 같은 규칙).
    /// 세트 이름은 <b>바꿀 원본 스켈레톤 이름</b>입니다. 원본 에셋 이름이 <c>0411_road_SkeletonData</c>면
    /// <c>skin test/0411_road/0411_road.json</c>·<c>.atlas</c>·<c>.png</c>입니다. 에디터에서 원본 <c>.skel</c>을 가져와 고쳤다면
    /// 스켈레톤 이름이 이미 원본과 같으므로 내보낸 파일을 그대로 넣으면 됩니다.</para>
    ///
    /// <para>프리팹 이름이 아니라 스켈레톤 이름으로 고르는 이유: 한 스켈레톤을 여러 프리팹(속도 1·2·3, 위·아래)이
    /// 같이 쓰므로, 스켈레톤 하나를 고치면 그 스켈레톤을 쓰는 노트가 모두 바뀌는 쪽이 에디터에서 작업한 단위와 맞습니다.
    /// 오브젝트 이름은 <c>{프리팹}(Clone){objId}</c>로 바뀌어 완전 일치로는 고를 수 없습니다.</para>
    /// </summary>
    public static class NoteSkinRules
    {
        private const string SkeletonDataSuffix = "_SkeletonData";

        /// <summary>
        /// 원본 <c>SkeletonDataAsset</c> 이름에서 세트 이름을 얻습니다. <c>0411_road_SkeletonData</c> → <c>0411_road</c>.
        /// 꼬리가 없으면 이름 그대로입니다. 비어 있으면 null입니다.
        /// </summary>
        public static string BaseNameFromAssetName(string assetName)
        {
            if (string.IsNullOrWhiteSpace(assetName)) return null;

            string name = assetName.Trim();
            if (name.EndsWith(SkeletonDataSuffix, StringComparison.OrdinalIgnoreCase))
            {
                name = name.Substring(0, name.Length - SkeletonDataSuffix.Length);
            }

            if (name.Length == 0) return null;

            // 세트 이름이 경로를 벗어나지 못하게 합니다(에셋 이름은 게임 데이터지만 파일 경로로 쓰이므로).
            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || name == "." || name == "..") return null;

            return name;
        }

        /// <summary>세트 파일 세 개의 경로입니다(존재 여부는 보지 않습니다).</summary>
        public static void GetSetPaths(string skinDir, string baseName, out string png, out string atlas, out string json)
        {
            png = Path.Combine(skinDir, baseName + ".png");
            atlas = Path.Combine(skinDir, baseName + ".atlas");
            json = Path.Combine(skinDir, baseName + ".json");
        }

        /// <summary>
        /// 프리팹이 부르는 애니메이션 이름 중 새 스켈레톤에 없는 것을 돌려줍니다(중복·빈 이름 제외, 처음 나온 순서).
        /// 하나라도 있으면 그 노트는 그 동작에서 멈추거나 사라지므로, 호출자는 주입을 건너뜁니다.
        /// </summary>
        public static List<string> FindMissingAnimations(IEnumerable<string> required, Func<string, bool> hasAnimation)
        {
            var missing = new List<string>();
            if (required == null || hasAnimation == null) return missing;

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (string name in required)
            {
                if (string.IsNullOrEmpty(name)) continue;
                if (!seen.Add(name)) continue;
                if (!hasAnimation(name)) missing.Add(name);
            }

            return missing;
        }
    }
}
