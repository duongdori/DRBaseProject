using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DRG.Utilities
{
    public static class SceneHelpers
    {
        public static IEnumerator LoadSceneAsync(int sceneBuildIndex, bool isAdditive = false, bool setActive = false, System.Action onLoadSceneCompleted = null)
        {
            var scene = SceneManager.LoadSceneAsync(sceneBuildIndex, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            yield return scene;

            if (setActive)
            {
                yield return new WaitForEndOfFrame();
                var curScene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
                SceneManager.SetActiveScene(curScene);
            }

            onLoadSceneCompleted?.Invoke();
        }
        
        public static IEnumerator LoadSceneAsync(string sceneName, bool isAdditive = false, bool setActive = false, System.Action onLoadSceneCompleted = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError($"Invalid sceneName: {sceneName}");
                yield break;
            }

            var scene = SceneManager.LoadSceneAsync(sceneName, isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            yield return scene;

            if (setActive)
            {
                yield return new WaitForEndOfFrame();
                var curScene = SceneManager.GetSceneByName(sceneName);
                SceneManager.SetActiveScene(curScene);
            }

            onLoadSceneCompleted?.Invoke();
        }
        
        public static IEnumerator UnloadSceneAsync(int sceneBuildIndex, System.Action onUnloadSceneCompleted = null)
        {
            var scene = SceneManager.UnloadSceneAsync(sceneBuildIndex);
            yield return scene;
            onUnloadSceneCompleted?.Invoke();
        }
        
        public static IEnumerator UnloadSceneAsync(string sceneName, System.Action onUnloadSceneCompleted = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError($"Invalid sceneName: {sceneName}");
                yield break;
            }

            var scene = SceneManager.UnloadSceneAsync(sceneName);
            yield return scene;
            onUnloadSceneCompleted?.Invoke();
        }
    }
}
