using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace muse_dash_test
{
    /// <summary>
    /// 실험차트에서 씬 로더가 누락하는 게임 원본 HitPoints 프리팹 인스턴스를 설치합니다.
    /// </summary>
    public static class ExperimentHitPointInstaller
    {
        private const float RetryInterval = 0.5f;
        private static bool installed;
        private static float nextAttemptTime;
        private static string lastStatusLog;
        private static int lastLoadSceneOriginal = -1;
        private static int lastLoadSceneRedirected = -1;
        private static readonly Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>(StringComparer.Ordinal);

        private struct SceneCandidate
        {
            public int Scene;
            public string Source;
        }

        public static void Reset()
        {
            installed = false;
            nextAttemptTime = 0f;
            lastStatusLog = null;
            prefabCache.Clear();
        }

        public static void RememberLoadSceneRedirect(string originalSceneName, string redirectedSceneName)
        {
            lastLoadSceneOriginal = TryParseSceneName(originalSceneName, out int originalScene)
                ? originalScene
                : -1;
            lastLoadSceneRedirected = TryParseSceneName(redirectedSceneName, out int redirectedScene)
                ? redirectedScene
                : -1;
        }

        public static void Update(bool isInStage)
        {
            if (installed
                || !isInStage
                || !CustomPlaySession.Current.ShouldApplyExperimentChart
                || Time.unscaledTime < nextAttemptTime)
            {
                return;
            }

            nextAttemptTime = Time.unscaledTime + RetryInterval;

            try
            {
                string uid = CustomPlaySession.Current.SelectedMusicUid;
                if (string.IsNullOrEmpty(uid))
                {
                    uid = PnlStagePatchHelper.GetCurrentSelectedMusicUid()
                        ?? CustomPlaySession.Current.LastClickedMusicUid;
                }

                List<SceneCandidate> candidates = ResolveHitPointScenes(uid);
                if (candidates.Count == 0)
                {
                    LogStatusOnce($"[ExperimentHitPoint] HitPoints 씬 번호를 아직 결정하지 못했습니다. uid={uid ?? "(null)"}");
                    return;
                }

                GameObject sceneObjectController = FindActiveGameObject("SceneObjectController");
                if (sceneObjectController == null)
                {
                    LogStatusOnce($"[ExperimentHitPoint] SceneObjectController 대기 중: candidates={DescribeCandidates(candidates)}");
                    return;
                }

                for (int i = 0; i < candidates.Count; i++)
                {
                    SceneCandidate candidate = candidates[i];
                    string prefabName = $"HitPoints_{candidate.Scene:00}";
                    string instanceName = prefabName + "(Clone)";

                    GameObject existingInstance = FindChildByName(sceneObjectController.transform, instanceName);
                    if (existingInstance != null)
                    {
                        installed = true;
                        MelonLogger.Msg($"[ExperimentHitPoint] 기존 인스턴스 확인: {GetPath(existingInstance.transform)}, source={candidate.Source}");
                        return;
                    }

                    existingInstance = FindActiveHitPointInstance(prefabName);
                    if (existingInstance != null)
                    {
                        if (existingInstance.transform.parent != sceneObjectController.transform)
                        {
                            existingInstance.transform.SetParent(sceneObjectController.transform, false);
                        }

                        installed = true;
                        MelonLogger.Msg($"[ExperimentHitPoint] 기존 전역 인스턴스 확인/연결: {GetPath(existingInstance.transform)}, source={candidate.Source}");
                        return;
                    }
                }

                for (int i = 0; i < candidates.Count; i++)
                {
                    SceneCandidate candidate = candidates[i];
                    string prefabName = $"HitPoints_{candidate.Scene:00}";
                    GameObject prefab = FindInactivePrefab(prefabName);
                    if (prefab == null)
                    {
                        continue;
                    }

                    GameObject instance = UnityEngine.Object.Instantiate(prefab);
                    instance.name = prefabName + "(Clone)";
                    instance.transform.SetParent(sceneObjectController.transform, false);
                    instance.SetActive(true);

                    installed = true;
                    MelonLogger.Msg(
                        $"[ExperimentHitPoint] 게임 원본 HitPoint 설치 완료: path={GetPath(instance.transform)}, " +
                        $"source={candidate.Source}, candidates={DescribeCandidates(candidates)}, scene={instance.scene.name}, " +
                        $"activeHierarchy={instance.activeInHierarchy}, localPos={instance.transform.localPosition}");
                    return;
                }

                string statusMsg = $"[ExperimentHitPoint] 프리팹 대기 중: candidates={DescribeCandidates(candidates)}";
                if (!string.Equals(lastStatusLog, statusMsg, StringComparison.Ordinal))
                {
                    statusMsg += $", loaded={DescribeLoadedHitPoints()}";
                    LogStatusOnce(statusMsg);
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ExperimentHitPoint] 설치 중 예외: {ex}");
            }
        }

        private static List<SceneCandidate> ResolveHitPointScenes(string uid)
        {
            var candidates = new List<SceneCandidate>();

            AddSceneCandidate(candidates, lastLoadSceneRedirected, "loadscene-redirected");
            AddSceneCandidate(candidates, lastLoadSceneOriginal, "loadscene-original");

            if (MainMod.TryGetCachedHwaScene(uid, out int manifestScene))
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
            if (!MainMod.TryGetCachedHwaBmsChart(uid, out BmsChart chart, out _) || chart?.Notes == null)
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

        private static void LogStatusOnce(string message)
        {
            if (string.Equals(lastStatusLog, message, StringComparison.Ordinal))
            {
                return;
            }

            lastStatusLog = message;
            MelonLogger.Msg(message);
        }

        private static GameObject FindActiveGameObject(string name)
        {
            return GameObject.Find(name);
        }

        private static GameObject FindInactivePrefab(string prefabName)
        {
            if (prefabCache.TryGetValue(prefabName, out GameObject cached) && cached != null)
            {
                return cached;
            }

            var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject gameObject = gameObjects[i];
                if (gameObject != null
                    && !gameObject.scene.IsValid()
                    && string.Equals(gameObject.name, prefabName, StringComparison.Ordinal))
                {
                    prefabCache[prefabName] = gameObject;
                    return gameObject;
                }
            }

            return null;
        }

        private static GameObject FindActiveHitPointInstance(string prefabName)
        {
            GameObject clone = GameObject.Find(prefabName + "(Clone)");
            if (clone != null) return clone;
            return GameObject.Find(prefabName);
        }

        private static GameObject FindChildByName(Transform parent, string name)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (string.Equals(child.name, name, StringComparison.Ordinal))
                {
                    return child.gameObject;
                }
            }

            return null;
        }

        private static string GetPath(Transform transform)
        {
            string path = transform.name;
            Transform parent = transform.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }
    }
}
