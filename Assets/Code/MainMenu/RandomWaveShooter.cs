using AudioWaves;
using UnityEngine;

namespace MainMenu
{
    public class RandomWaveShooter : MonoBehaviour
    {
        [SerializeField]
        private AudioWaveShooter _audioWaveShooter;

        [SerializeField]
        private Vector2 _minMaxRate = new Vector2(1.0f, 2.0f);

        private float _shootTimer;

        private void Update()
        {
            _shootTimer -= Time.deltaTime;

            if (_shootTimer <= 0.0f)
            {
                _audioWaveShooter.ShootForward();
                _shootTimer = 1.0f / Random.Range(_minMaxRate.x, _minMaxRate.y);
            }
        }
    }
}