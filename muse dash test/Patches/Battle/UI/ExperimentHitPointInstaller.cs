using System;
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

        public static void Reset()
        {
            installed = false;
            nextAttemptTime = 0f;
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

                if (!MainMod.TryGetCachedHwaScene(uid, out int scene))
                {
                    return;
                }

                string prefabName = $"HitPoints_{scene:00}";
                string instanceName = prefabName + "(Clone)";
                GameObject sceneObjectController = FindActiveGameObject("SceneObjectController");
                if (sceneObjectController == null)
                {
                    return;
                }

                GameObject existingInstance = FindChildByName(sceneObjectController.transform, instanceName);
                if (existingInstance != null)
                {
                    installed = true;
                    MelonLogger.Msg($"[ExperimentHitPoint] 기존 인스턴스 확인: {GetPath(existingInstance.transform)}");
                    return;
                }

                GameObject prefab = FindInactivePrefab(prefabName);
                if (prefab == null)
                {
                    return;
                }

                GameObject instance = UnityEngine.Object.Instantiate(prefab);
                instance.name = instanceName;
                instance.transform.SetParent(sceneObjectController.transform, false);
                instance.SetActive(true);

                installed = true;
                MelonLogger.Msg(
                    $"[ExperimentHitPoint] 게임 원본 HitPoint 설치 완료: path={GetPath(instance.transform)}, " +
                    $"scene={instance.scene.name}, activeHierarchy={instance.activeInHierarchy}, localPos={instance.transform.localPosition}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"[ExperimentHitPoint] 설치 중 예외: {ex}");
            }
        }

        private static GameObject FindActiveGameObject(string name)
        {
            var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject gameObject = gameObjects[i];
                if (gameObject != null
                    && gameObject.activeInHierarchy
                    && gameObject.scene.IsValid()
                    && string.Equals(gameObject.name, name, StringComparison.Ordinal))
                {
                    return gameObject;
                }
            }

            return null;
        }

        private static GameObject FindInactivePrefab(string prefabName)
        {
            var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject gameObject = gameObjects[i];
                if (gameObject != null
                    && !gameObject.scene.IsValid()
                    && string.Equals(gameObject.name, prefabName, StringComparison.Ordinal))
                {
                    return gameObject;
                }
            }

            return null;
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
