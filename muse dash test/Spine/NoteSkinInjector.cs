using System;
using System.Collections.Generic;
using System.IO;
using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSpine.Unity;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 노트 스켈레톤을 게임 루트 <c>skin test/{원본 스켈레톤 이름}/</c>의 세트로 바꿉니다(규칙은 <see cref="NoteSkinRules"/>).
    /// 캐릭터 스킨과 같은 폴더 규칙(<see cref="CustomSkinInjector.GetSetDirectory"/>)입니다.
    ///
    /// <para><b>언제</b>: <c>SpineActionController.Init</c> Postfix에서만 합니다. 노트는 풀에서 다시 쓰지 않고
    /// 노트마다 인스턴스가 따로라 한 번 바꾸면 끝까지 그 스킨입니다. <c>OnControllerStart</c>에서 다시 하지 않는 이유는
    /// 그 시점에 <c>Initialize(true)</c>를 부르면 재생 중인 상태를 지우기 때문입니다.</para>
    ///
    /// <para><b>무엇을 지키나</b>
    /// <list type="bullet">
    /// <item><b>공식 곡·커스텀 곡 모두</b>에 적용됩니다(실험 단계라 공식 곡에서 바로 확인하려는 것). 세트 폴더가 없으면 아무것도 하지 않습니다.</item>
    /// <item>캐릭터 주입기가 이미 바꾼 스켈레톤은 건드리지 않습니다.</item>
    /// <item><c>scale</c>은 원본 에셋 값을 그대로 씁니다(씬 05 노트는 0.005, 캐릭터 주입기의 0.01과 다름).</item>
    /// <item>프리팹의 <c>actionData</c>·<c>m_FeverActionData</c>가 부르는 애니메이션이 새 스켈레톤에 하나라도 없으면
    ///   <b>주입을 건너뜁니다</b>. 멈추거나 안 보이는 노트보다 원본 노트가 낫습니다.</item>
    /// <item>주입 뒤 <c>InitSkField</c>를 다시 불러 <c>m_Bones</c>가 새 스켈레톤의 본을 가리키게 합니다.</item>
    /// </list></para>
    ///
    /// <para><b>비용</b>: <c>Init</c>은 곡 로딩 중 노트 수만큼 불립니다. 세트 파일 확인은 세트마다 몇 초에 한 번만 하고, 계약 검사 결과도 (에셋, 애니메이션 이름 목록)별로 보관합니다.
    /// 그래도 노트마다 <c>Initialize(true)</c>는 피할 수 없으니 바뀐 노트 수를 로그로 남겨 로딩 시간과 견줘 봅니다.</para>
    /// </summary>
    internal static class NoteSkinInjector
    {
        /// <summary>세트 파일을 다시 확인하기까지의 간격(초). 곡 로딩은 몇 초 안에 끝나므로 로딩 중에는 한 번만 봅니다.</summary>
        private const float RevalidateSeconds = 2f;

        private sealed class SetEntry
        {
            public SkeletonDataAsset Asset;
            public long Stamp;
            public float CheckedAt;
        }

        // 키: json 경로 + scale. 파일이 없으면 Asset이 null인 항목으로 "없음"을 기억합니다.
        private static readonly Dictionary<string, SetEntry> Sets = new Dictionary<string, SetEntry>(StringComparer.OrdinalIgnoreCase);

        // 우리가 만든 에셋의 포인터. 이미 바꾼 노트를 다시 바꾸지 않으려고 씁니다.
        private static readonly HashSet<IntPtr> OwnAssets = new HashSet<IntPtr>();

        // 키: 에셋 포인터 + 요구 애니메이션 이름 목록. 값: 빠진 이름(없으면 빈 목록).
        private static readonly Dictionary<string, List<string>> ContractVerdicts = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        private static readonly HashSet<string> WarnedContracts = new HashSet<string>(StringComparer.Ordinal);

        private static int injectedCount;

        public static void TryInject(SpineActionController sac)
        {
            if (!ModConfig.EnableSpineSkin) return;
            if (sac == null) return;

            try
            {
                var ska = sac.skeletonAnimation;
                if (ska == null) return;

                var original = ska.skeletonDataAsset;
                if (original == null) return;
                if (OwnAssets.Contains(original.Pointer)) return;
                if (CustomSkinInjector.IsCachedAsset(original)) return;

                string baseName = NoteSkinRules.BaseNameFromAssetName(original.name);
                if (baseName == null) return;

                var custom = GetOrBuild(CustomSkinInjector.GetSetDirectory(baseName), baseName, original.scale);
                if (custom == null) return;

                var skeletonData = custom.GetSkeletonData(true);
                if (skeletonData == null) return;

                var required = CollectRequiredAnimations(sac);
                string contractKey = custom.Pointer.ToInt64() + "|" + string.Join("\u0001", required);
                if (!ContractVerdicts.TryGetValue(contractKey, out var missing))
                {
                    missing = NoteSkinRules.FindMissingAnimations(required, n => skeletonData.FindAnimation(n) != null);
                    ContractVerdicts[contractKey] = missing;
                }

                if (missing.Count > 0)
                {
                    if (WarnedContracts.Add(contractKey))
                    {
                        ModLogger.Warning($"[NoteSkin] {baseName}: 프리팹 '{StripObjId(sac.gameObject.name)}'이(가) 부르는 애니메이션이 없어 원본 노트를 씁니다: {string.Join(", ", missing)}");
                    }
                    return;
                }

                ska.skeletonDataAsset = custom;
                ska.Initialize(true);
                sac.InitSkField(ska);

                injectedCount++;
                if (injectedCount == 1 || injectedCount % 200 == 0)
                {
                    ModLogger.Msg($"[NoteSkin] 노트 스킨 주입 {injectedCount}개째 ({baseName}, {StripObjId(sac.gameObject.name)})");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Warning($"[NoteSkin] 주입 예외: {ex.Message}");
            }
        }

        private static SkeletonDataAsset GetOrBuild(string skinDir, string baseName, float scale)
        {
            NoteSkinRules.GetSetPaths(skinDir, baseName, out string png, out string atlas, out string json);
            string key = json + "|" + scale.ToString(System.Globalization.CultureInfo.InvariantCulture);
            float now = Time.realtimeSinceStartup;

            if (Sets.TryGetValue(key, out var entry))
            {
                bool fresh = now - entry.CheckedAt < RevalidateSeconds;
                if (fresh) return entry.Asset != null ? entry.Asset : null;
            }

            long stamp = ReadStamp(png, atlas, json);
            if (entry != null && entry.Stamp == stamp && (stamp == 0 || entry.Asset != null))
            {
                entry.CheckedAt = now;
                return entry.Asset;
            }

            if (entry?.Asset != null)
            {
                OwnAssets.Remove(entry.Asset.Pointer);
                UnityEngine.Object.Destroy(entry.Asset);
            }

            // 계약 판정은 에셋 포인터로 묶여 있어, 에셋을 새로 만들면 같은 주소가 다시 쓰일 수 있습니다.
            ContractVerdicts.Clear();
            WarnedContracts.Clear();

            SkeletonDataAsset asset = null;
            if (stamp != 0)
            {
                asset = CustomSkinInjector.BuildAsset("note:" + baseName, png, atlas, json, scale);
                if (asset != null) OwnAssets.Add(asset.Pointer);
            }

            Sets[key] = new SetEntry { Asset = asset, Stamp = stamp, CheckedAt = now };
            return asset;
        }

        /// <summary>세 파일의 크기·수정시각을 합친 값입니다. 하나라도 없으면 0입니다.</summary>
        private static long ReadStamp(string png, string atlas, string json)
        {
            long stamp = 17;
            foreach (string path in new[] { png, atlas, json })
            {
                var info = new FileInfo(path);
                if (!info.Exists) return 0;
                stamp = unchecked(stamp * 31 + info.Length);
                stamp = unchecked(stamp * 31 + info.LastWriteTimeUtc.Ticks);
            }
            return stamp == 0 ? 1 : stamp;
        }

        private static List<string> CollectRequiredAnimations(SpineActionController sac)
        {
            var names = new List<string>();
            AppendActionNames(names, sac.actionData);
            AppendActionNames(names, sac.m_FeverActionData);
            return names;
        }

        private static void AppendActionNames(List<string> names, Il2CppReferenceArray<SkeletActionData> actions)
        {
            if (actions == null) return;
            for (int i = 0; i < actions.Length; i++)
            {
                var data = actions[i];
                if (data == null) continue;

                var idx = data.actionIdx;
                if (idx == null) continue;
                for (int j = 0; j < idx.Length; j++)
                {
                    string name = idx[j];
                    if (!string.IsNullOrEmpty(name)) names.Add(name);
                }
            }
        }

        /// <summary><c>051001_road_nor_1(Clone)123</c> → <c>051001_road_nor_1</c>. 로그용입니다.</summary>
        private static string StripObjId(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return objectName;
            int clone = objectName.IndexOf("(Clone)", StringComparison.Ordinal);
            return clone >= 0 ? objectName.Substring(0, clone) : objectName;
        }
    }
}
