using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        private const int MainMenuSceneBuildIndex = 0;
        private const int TransitionSceneBuildIndex = 1;
        private const int GameLevelsStartBuildIndex = 2;
        
        public IEnumerator LoadMainMenu()
        {
            yield return ScreenFade.Instance.FadeIn();
            
            yield return LoadTransitionScene();
            yield return SceneManager.LoadSceneAsync(MainMenuSceneBuildIndex, LoadSceneMode.Single);
            yield return ScreenFade.Instance.FadeOut();

            UnloadTransitionScene();
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