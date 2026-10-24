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

        /// <summary>이 횟수를 넘겨도 설치되지 않으면 이번 스테이지의 재시도를 포기합니다(≈20초).</summary>
        private const int MaxInstallAttempts = 40;

        private static bool installed;
        private static bool gaveUp;
        private static int installAttempts;
        private static float nextAttemptTime;
        private static string lastStatusLog;
        private static int lastLoadSceneOriginal = -1;
        private static int lastLoadSceneRedirected = -1;
        private static readonly Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>(StringComparer.Ordinal);
        private static List<SceneCandidate> cachedCandidates;

        private struct SceneCandidate
        {
            public int Scene;
            public string Source;
        }

        public static void Reset()
        {
            installed = false;
            gaveUp = false;
            installAttempts = 0;
            nextAttemptTime = 0f;
            lastStatusLog = null;
            lastLoadSceneOriginal = -1;
            lastLoadSceneRedirected = -1;
            prefabCache.Clear();
            cachedCandidates = null;
        }

        public static void RememberLoadSceneRedirect(string originalSceneName, string redirectedSceneName)
        {
            // 인게임 재시작(GameRestart)은 Unity 씬을 다시 로드하지 않고 LoadScene만 다시 호출하므로,
            // 이전 판의 설치 완료 플래그나 후보 캐시, 파괴된 프리팹 참조가 남지 않도록 함께 초기화합니다.
            installed = false;
            gaveUp = false;
            installAttempts = 0;
            nextAttemptTime = 0f;
            lastStatusLog = null;
            cachedCandidates = null;
            PurgeDeadPrefabsFromCache();

            lastLoadSceneOriginal = TryParseSceneName(originalSceneName, out int originalScene)
                ? originalScene
                : -1;
            lastLoadSceneRedirected = TryParseSceneName(redirectedSceneName, out int redirectedScene)
                ? redirectedScene
                : -1;
        }

        private static void PurgeDeadPrefabsFromCache()
        {
            if (prefabCache.Count == 0) return;

            var deadKeys = new List<string>();
            foreach (var kv in prefabCache)
            {
                if (kv.Value == null)
                {
                    deadKeys.Add(kv.Key);
                }
            }

            for (int i = 0; i < deadKeys.Count; i++)
            {
                prefabCache.Remove(deadKeys[i]);
            }
        }

        public static void Update(bool isInStage)
        {
            if (installed
                || gaveUp
                || !isInStage
                || !CustomPlaySession.Current.ShouldApplyExperimentChart
                || Time.unscaledTime < nextAttemptTime)
            {
                return;
            }

            nextAttemptTime = Time.unscaledTime + RetryInterval;

            // 재시도 1회에는 GameObject.Find가 여러 번 들어갑니다(씬 전체 이름 스캔).
            // 설치가 끝내 실패하는 곡에서는 이게 곡 내내 초당 2회 반복되어 프레임을 갉아먹으므로,
            // 일정 횟수를 넘기면 이번 스테이지에서는 포기합니다. (스테이지 전환 시 Reset으로 복구)
            if (++installAttempts > MaxInstallAttempts)
            {
                gaveUp = true;
                ModLogger.Warning(
                    $"[ExperimentHitPoint] {MaxInstallAttempts}회({MaxInstallAttempts * RetryInterval:0.#}초) 시도했지만 HitPoints 설치에 실패해 " +
                    "이번 스테이지에서는 재시도를 중단합니다. (프레임 드랍 방지)");
                return;
            }

            try
            {
                string uid = CustomPlaySession.Current.LastKnownMusicUid;

                if (cachedCandidates == null)
                {
                    cachedCandidates = ResolveHitPointScenes(uid);
                }
                List<SceneCandidate> candidates = cachedCandidates;
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
                        CleanupStaleChildHitPoints(sceneObjectController.transform, existingInstance);
                        installed = true;
                        ModLogger.Msg($"[ExperimentHitPoint] 기존 인스턴스 확인: {GetPath(existingInstance.transform)}, source={candidate.Source}");
                        return;
                    }

                    existingInstance = FindActiveHitPointInstance(prefabName);
                    if (existingInstance != null)
                    {
                        if (existingInstance.transform.parent != sceneObjectController.transform)
                        {
                            existingInstance.transform.SetParent(sceneObjectController.transform, false);
                        }

                        CleanupStaleChildHitPoints(sceneObjectController.transform, existingInstance);
                        installed = true;
                        ModLogger.Msg($"[ExperimentHitPoint] 기존 전역 인스턴스 확인/연결: {GetPath(existingInstance.transform)}, source={candidate.Source}");
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

                    CleanupStaleChildHitPoints(sceneObjectController.transform, instance);
                    installed = true;
                    ModLogger.Msg(
                        $"[ExperimentHitPoint] 게임 원본 HitPoint 설치 완료: path={GetPath(instance.transform)}, " +
                        $"source={candidate.Source}, candidates={DescribeCandidates(candidates)}, scene={instance.scene.name}, " +
                        $"activeHierarchy={instance.activeInHierarchy}, localPos={instance.transform.localPosition}");
                    return;
                }

                // lastStatusLog에는 반드시 '접미사를 붙이기 전' 문자열을 기억시킵니다.
                // 접미사까지 포함해 저장하면 다음 비교가 영원히 불일치가 되어,
                // DescribeLoadedHitPoints()의 Resources.FindObjectsOfTypeAll 전체 스캔이
                // 재시도마다(초당 2회) 인게임 내내 돌게 됩니다.
                string statusMsg = $"[ExperimentHitPoint] 프리팹 대기 중: candidates={DescribeCandidates(candidates)}";
                if (!string.Equals(lastStatusLog, statusMsg, StringComparison.Ordinal))
                {
                    lastStatusLog = statusMsg;
                    ModLogger.Msg($"{statusMsg}, loaded={DescribeLoadedHitPoints()}");
                }
            }
            catch (Exception ex)
            {
                ModLogger.Error($"[ExperimentHitPoint] 설치 중 예외: {ex}");
            }
        }

        /// <summary>
        /// SceneObjectController 하위에 이전 씬/호스트 곡에서 남은 다른 HitPoints 인스턴스가 중복으로 쌓여 있으면 정리합니다.
        /// </summary>
        private static void CleanupStaleChildHitPoints(Transform parent, GameObject activeHitPoint)
        {
            if (parent == null || activeHitPoint == null) return;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                if (child == null || child.gameObject == null || child.gameObject == activeHitPoint)
                {
                    continue;
                }

                if (TryParseHitPointScene(child.name, out _))
                {
                    ModLogger.Verbose($"[ExperimentHitPoint] 중복 판정선(HitPoints) 인스턴스 정리: {child.name}");
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }
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
            if (!HwaResourceManager.TryGetCachedHwaBmsChart(uid, out BmsChart chart) || chart?.Notes == null)
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
            ModLogger.Msg(message);
        }

        private static GameObject FindActiveGameObject(string name)
        {
            return GameObject.Find(name);
        }

        private static GameObject FindInactivePrefab(string prefabName)
        {
            if (prefabCache.TryGetValue(prefabName, out GameObject cached))
            {
                if (cached != null)
                {
                    return cached;
                }
                prefabCache.Remove(prefabName);
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

            // 찾지 못한 결과는 캐시하지 않습니다. 프리팹이 나중에 로드될 수 있어 다음 시도에서 다시 찾아야 합니다.
            // 검색 횟수는 호출부(RetryInterval 0.5초, MaxInstallAttempts 40회)가 제한합니다.
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
