using System;
using System.Collections.Generic;
using UnityEngine;

namespace muse_dash_test
{
    public static partial class ExperimentHitPointInstaller
    {
        private struct SceneCandidate
        {
            public int Scene;
            public string Source;
        }

        private static List<SceneCandidate> ResolveHitPointScenes(string uid)
        {
            var candidates = new List<SceneCandidate>();

            AddSceneCandidate(candidates, lastLoadSceneRedirected, "loadscene-redirected");
            AddSceneCandidate(candidates, lastLoadSceneOriginal, "loadscene-original");

            if (HwaResourceManager.TryGetCachedHwaScene(uid, out int manifestScene))
            {
                AddSceneCandidate(candidates, manifestScene, "manifest");
            }

            AddBmsSceneCandidates(candidates, uid);

            if (!CustomContentIds.IsVirtualSong(uid) && TryParseScenePrefix(uid, out int uidScene))
            {
                AddSceneCandidate(candidates, uidScene, "uid");
            }

            AddLoadedHitPointCandidates(candidates);

            return candidates;
        }

        private static void AddBmsSceneCandidates(List<SceneCandidate> candidates, string uid)
        {
            if (!HwaResourceManager.TryGetCachedHwaBmsChart(uid, CustomRecordStore.ResolveBattleDifficulty(), out BmsChart chart) || chart?.Notes == null)
            {
                return;
            }

            var sceneCounts = new Dictionary<int, int>();
            foreach (var note in chart.Notes)
            {
                BmsWavInfo wavInfo = BmsBossSwapPlanner.ResolveWavInfo(chart, note);
                if (wavInfo == null || !TryParseScenePrefix(wavInfo.Uid, out int wavScene))
                {
                    continue;
                }

                if (wavScene <= 0)
                {
                    continue;
                }

                sceneCounts.TryGetValue(wavScene, out int count);
                sceneCounts[wavScene] = count + 1;
            }

            while (sceneCounts.Count > 0)
            {
                int bestScene = 0;
                int bestCount = 0;
                foreach (var pair in sceneCounts)
                {
                    if (pair.Value > bestCount)
                    {
                        bestScene = pair.Key;
                        bestCount = pair.Value;
                    }
                }

                if (bestCount <= 0)
                {
                    return;
                }

                AddSceneCandidate(candidates, bestScene, $"bms-wav:{bestCount}");
                sceneCounts.Remove(bestScene);
            }
        }

        private static void AddLoadedHitPointCandidates(List<SceneCandidate> candidates)
        {
            if (candidates.Count > 0) return;

            var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject gameObject = gameObjects[i];
                if (gameObject == null || !TryParseHitPointScene(gameObject.name, out int scene))
                {
                    continue;
                }

                AddSceneCandidate(candidates, scene, gameObject.scene.IsValid() ? "loaded-instance" : "loaded-prefab");
            }
        }

        private static bool TryParseScenePrefix(string uid, out int scene)
        {
            scene = default;
            if (string.IsNullOrWhiteSpace(uid) || uid.Length < 2)
            {
                return false;
            }

            return int.TryParse(uid.Substring(0, 2), out scene);
        }

        private static bool TryParseSceneName(string sceneName, out int scene)
        {
            scene = default;
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                return false;
            }

            const string prefix = "scene_";
            if (!sceneName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string value = sceneName.Substring(prefix.Length);
            return int.TryParse(value, out scene);
        }

        private static bool TryParseHitPointScene(string name, out int scene)
        {
            scene = default;
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            const string prefix = "HitPoints_";
            if (!name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string value = name.Substring(prefix.Length);
            int cloneIndex = value.IndexOf("(Clone)", StringComparison.OrdinalIgnoreCase);
            if (cloneIndex >= 0)
            {
                value = value.Substring(0, cloneIndex);
            }

            return int.TryParse(value, out scene);
        }

        private static void AddSceneCandidate(List<SceneCandidate> candidates, int scene, string source)
        {
            if (scene <= 0)
            {
                return;
            }

            for (int i = 0; i < candidates.Count; i++)
            {
                if (candidates[i].Scene == scene)
                {
                    return;
                }
            }

            candidates.Add(new SceneCandidate { Scene = scene, Source = source });
        }

        private static string DescribeCandidates(List<SceneCandidate> candidates)
        {
            if (candidates == null || candidates.Count == 0)
            {
                return "(none)";
            }

            var parts = new List<string>();
            for (int i = 0; i < candidates.Count; i++)
            {
                parts.Add($"HitPoints_{candidates[i].Scene:00}/{candidates[i].Source}");
            }

            return string.Join(", ", parts);
        }

        private static string DescribeLoadedHitPoints()
        {
            var names = new List<string>();
            var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject gameObject = gameObjects[i];
                if (gameObject == null || !TryParseHitPointScene(gameObject.name, out _))
                {
                    continue;
                }

                string scope = gameObject.scene.IsValid()
                    ? $"scene:{gameObject.scene.name},active:{gameObject.activeInHierarchy}"
                    : "prefab";
                names.Add($"{gameObject.name}({scope})");
            }

            return names.Count > 0 ? string.Join(", ", names) : "(none)";
        }
    }
}
