using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMode
{
    public class GameModeManager<TGameModeKey> : MonoBehaviour where TGameModeKey : Enum
    {
        private readonly Dictionary<TGameModeKey, IGameMode> _gameModes = new();
        private IGameMode _current;
        private bool _ready;

        public void Register(TGameModeKey key, IGameMode gameMode)
        {
            if (_gameModes.ContainsKey(key))
            {
                Debug.LogError("GameMode with given key is already registered! Key: " + key);
                return;
            }

            _gameModes.Add(key, gameMode);
        }

        public void Tick()
        {
            if (!_ready)
            {
                return;
            }

            _current.Tick();
        }

        public void Enter(TGameModeKey key)
        {
            if (_gameModes.TryGetValue(key, out IGameMode gameMode))
            {
                StopAllCoroutines();
                StartCoroutine(Co_EnterGameMode(gameMode));
            }
            else
            {
                Debug.LogError("GameMode with given key is not registered! Key: " + key);
            }
        }

        private IEnumerator Co_EnterGameMode(IGameMode gameMode)
        {
            _ready = false;

            if (_current != null)
            {
                yield return _current.OnExit();
            }

            _current = gameMode;
            yield return _current.OnEnter();

            _ready = true;
        }
    }
}