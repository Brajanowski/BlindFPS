using System.Collections;
using Core;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMode.Modes
{
    [CreateAssetMenu(menuName = BlindFPSConstants.AssetCreationMenu + "Game Mode/Level Speed Run")]
    public class ScriptableLevelSpeedrunGameMode : ScriptableGameMode
    {
        [SerializeField]
        private PlayerController _playerPrefab;

        private bool _initialize;
        
        private GameState _gameState;

        private PlayerSpawn _playerSpawn;
        private PlayerController _playerController;

        public override IEnumerator OnEnter()
        {
            _gameState = Game.Instance.State;
            
            // first scene is main menu
            int sceneIndex = _gameState.GameLevel + 1;

            if (sceneIndex < 1 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError("Invalid level index!");
                yield break;
            }

            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
            yield return null;

            _playerSpawn = FindObjectOfType<PlayerSpawn>();
            if (_playerSpawn == null)
            {
                Debug.LogError("PlayerSpawn must be placed somewhere on level!");
                yield break;
            }

            PlayerSpawnLocation spawnLocation = _playerSpawn.GetSpawnLocation();
            _playerController = Instantiate(_playerPrefab, spawnLocation.Position, Quaternion.identity);
            _playerController.GetCameraController().SetLookDirection(spawnLocation.Rotation * Vector3.forward);
        }

        public override IEnumerator OnExit()
        {
            _gameState = null;
            _playerSpawn = null;
            _playerController = null;
            yield break;
        }

        public override void Tick()
        {
        }
    }
}