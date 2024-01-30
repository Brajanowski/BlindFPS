using System.Collections;
using Core;
using Pause;
using Player;
using UnityEngine;

namespace GameMode.Modes
{
    [CreateAssetMenu(menuName = BlindFPSConstants.AssetCreationMenu + "Game Mode/Level Speed Run")]
    public class ScriptableLevelSpeedrunGameMode : ScriptableGameMode
    {
        [SerializeField]
        private PlayerController _playerPrefab;

        [SerializeField]
        private PlayerHUD _playerHUDPrefab;

        private bool _initialize;

        private GameState _gameState;

        private PlayerSpawn _playerSpawn;
        private PlayerController _playerController;
        private PlayerHUD _playerHUD;

        public override IEnumerator OnEnter()
        {
            _gameState = Game.Instance.State;
            yield return SceneLoader.Instance.LoadLevel(_gameState.GameLevel);

            SpawnPlayer();
            SpawnHUD();

            PauseMenu.Instance.OnShow.AddListener(OnPauseMenuShown);
            PauseMenu.Instance.OnHide.AddListener(OnPauseMenuHidden);
        }

        public override IEnumerator OnExit()
        {
            Destroy(_playerController);
            Destroy(_playerHUD);

            _gameState = null;
            _playerSpawn = null;
            _playerController = null;
            _playerHUD = null;
            yield break;
        }

        public override void Tick()
        {
        }

        private void SpawnPlayer()
        {
            _playerSpawn = FindObjectOfType<PlayerSpawn>();
            if (_playerSpawn == null)
            {
                Debug.LogError("PlayerSpawn must be placed somewhere on level!");
                return;
            }

            PlayerSpawnLocation spawnLocation = _playerSpawn.GetSpawnLocation();
            _playerController = Instantiate(_playerPrefab, spawnLocation.Position, Quaternion.identity);
            _playerController.GetCameraController().SetLookDirection(spawnLocation.Rotation * Vector3.forward);
        }

        private void SpawnHUD()
        {
            _playerHUD = Instantiate(_playerHUDPrefab);
        }

        private void OnPauseMenuShown()
        {
            _playerHUD.gameObject.SetActive(false);
        }

        private void OnPauseMenuHidden()
        {
            _playerHUD.gameObject.SetActive(true);
        }
    }
}