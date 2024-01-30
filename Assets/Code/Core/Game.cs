using GameMode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameState
    {
        public int GameLevel;
        public float LevelTimer;
    }

    public class Game : SingletonMonoBehaviour<Game>
    {
        [SerializeField]
        private ScriptableGameMode _mainMenuGameMode;

        [SerializeField]
        private ScriptableGameMode _levelSpeedrunGameMode;

        [SerializeField]
        private BlindFPSGameModeManager _gameModeManager;

        public GameState State { get; private set; } = new();

        protected override void Awake()
        {
            base.Awake();
            _gameModeManager.Register(BlindFPSGameMode.MainMenu, _mainMenuGameMode);
            _gameModeManager.Register(BlindFPSGameMode.LevelSpeedrun, _levelSpeedrunGameMode);
        }

        private void Start()
        {
            GoToMainMenu();
        }

        private void Update()
        {
            _gameModeManager.Tick();
        }

        public bool IsThereNextLevel(int current)
        {
            int sceneBuildIndex = SceneLoader.Instance.GameLevelsStartBuildIndex + current;

            if (sceneBuildIndex < SceneLoader.Instance.GameLevelsStartBuildIndex)
            {
                Debug.LogError("Invalid level");
                return false;
            }

            return sceneBuildIndex < SceneManager.sceneCountInBuildSettings - 1;
        }

        public void StartLevel(int levelIndex)
        {
            State.GameLevel = levelIndex;
            _gameModeManager.Enter(BlindFPSGameMode.LevelSpeedrun);
        }

        public void GoToMainMenu()
        {
            _gameModeManager.Enter(BlindFPSGameMode.MainMenu);
        }
    }
}