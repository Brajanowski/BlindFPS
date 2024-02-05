using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        [field: SerializeField]
        public int MainMenuSceneBuildIndex { get; private set; } = 0;

        [field: SerializeField]
        public int TransitionSceneBuildIndex { get; private set; } = 1;

        [field: SerializeField]
        public int GameLevelsStartBuildIndex { get; private set; } = 2;

        public IEnumerator LoadMainMenu()
        {
            if (!SceneManager.GetSceneByBuildIndex(MainMenuSceneBuildIndex).isLoaded)
            {
                yield return ScreenFade.Instance.FadeIn();
                yield return LoadTransitionScene();
                yield return SceneManager.LoadSceneAsync(MainMenuSceneBuildIndex, LoadSceneMode.Single);
                yield return ScreenFade.Instance.FadeOut();

                UnloadTransitionScene();
            }
        }

        public IEnumerator LoadLevel(int level)
        {
            int sceneIndex = GameLevelsStartBuildIndex + level;

            if (sceneIndex < GameLevelsStartBuildIndex || sceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError("Invalid level index!");
                yield break;
            }

            yield return ScreenFade.Instance.FadeIn();

            yield return LoadTransitionScene();
            yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            yield return ScreenFade.Instance.FadeOut();

            UnloadTransitionScene();
        }

        private IEnumerator LoadTransitionScene()
        {
            yield return SceneManager.LoadSceneAsync(TransitionSceneBuildIndex, LoadSceneMode.Single);
        }

        private void UnloadTransitionScene()
        {
            if (SceneManager.GetSceneByBuildIndex(TransitionSceneBuildIndex).isLoaded)
            {
                SceneManager.UnloadSceneAsync(TransitionSceneBuildIndex);
            }
        }
    }
}