﻿using System.Collections;
using Core;
using Pause;
using Player;
using UI;
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

        private bool _freezeTimer = false;
        private float _levelTimer = 0.0f;

        public override IEnumerator OnEnter()
        {
            _gameState = Game.Instance.State;
            yield return SceneLoader.Instance.LoadLevel(_gameState.GameLevel);

            SpawnPlayer();
            SpawnHUD();

            PauseMenu.Instance.OnShow.AddListener(OnPauseMenuShown);
            PauseMenu.Instance.OnHide.AddListener(OnPauseMenuHidden);

            _freezeTimer = false;
            _levelTimer = 0.0f;

            _playerController.OnLevelCompletionTriggerEnter.AddListener(OnReachedLevelCompletion);
            _playerController.OnDeath.AddListener(OnPlayerDied);
        }

        public override IEnumerator OnExit()
        {
            _playerController.OnLevelCompletionTriggerEnter.RemoveListener(OnReachedLevelCompletion);
            _playerController.OnDeath.RemoveListener(OnPlayerDied);

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
            if (_freezeTimer)
            {
                return;
            }

            _levelTimer += Time.deltaTime;
            _playerHUD.SetTimeElapsed(_levelTimer);
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

        private void OnReachedLevelCompletion()
        {
            _freezeTimer = true;
            LevelCompletedScreen.Instance.Show();
            LevelCompletedScreen.Instance.SetTimer(_levelTimer);
        }

        private void OnPlayerDied()
        {
            _freezeTimer = true;
            GameOverScreen.Instance.Show();
            GameOverScreen.Instance.SetTimer(_levelTimer);
        }
    }
}