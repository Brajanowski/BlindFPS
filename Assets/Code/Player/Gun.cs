using AudioWaves;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class Gun : MonoBehaviour
    {
        private static readonly int FireHash = Animator.StringToHash("Fire");

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private Bullet _bulletPrefab;

        [SerializeField]
        private AudioWaveShooter _audioWaveShooter;

        [SerializeField]
        private float _shootRate = 1f;

        private float _nextShootTime;
        private bool _waiting;

        public UnityEvent OnShoot = new();
        public UnityEvent OnReadyToShoot = new();

        public void Shoot(Vector3 origin, Vector3 direction)
        {
            if (Time.time < _nextShootTime)
            {
                return;
            }

            _nextShootTime = Time.time + 1.0f / _shootRate;
            
            _animator.SetTrigger(FireHash);
            
            Instantiate(_bulletPrefab, origin, Quaternion.identity).Setup(direction);
            _audioWaveShooter.Shoot(origin, direction);
            _waiting = true;

            OnShoot?.Invoke();
        }

        private void Update()
        {
            if (_waiting && Time.time >= _nextShootTime)
            {
                _waiting = false;
                OnReadyToShoot?.Invoke();
            }
        }
    }
}