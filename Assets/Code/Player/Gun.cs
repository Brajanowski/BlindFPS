using AudioWaves;
using Core;
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
        private AudioWaveShooter _audioWaveShooter;

        [SerializeField]
        private float _shootRate = 1f;

        private float _nextShootTime;
        private bool _waiting;

        public UnityEvent OnShoot = new();
        public UnityEvent OnReadyToShoot = new();

        public bool Shoot(Vector3 origin, Vector3 direction)
        {
            if (Time.time < _nextShootTime)
            {
                return false;
            }

            _nextShootTime = Time.time + 1.0f / _shootRate;
            
            _animator.SetTrigger(FireHash);

            Bullet bullet = ComponentPool<Bullet>.Get();
            bullet.transform.position = origin;
            bullet.Setup(direction);

            _audioWaveShooter.Shoot(origin, direction);
            _waiting = true;

            OnShoot?.Invoke();
            return true;
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