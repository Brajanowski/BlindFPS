using System;
using AudioWaves;
using Misc;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float _travelSpeed = 10.0f;

        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private ScriptableAudioWaveAnimation _onHitAudioWaveAnimation;

        [SerializeField]
        private AudioClip _impactAudioClip;

        [SerializeField]
        private int _maxBounces = 3;

        [SerializeField]
        private float _autoDestroyDelay = 5.0f;

        private bool _ready;

        private Vector3 _direction;
        private int _bounce;
        private float _autoDestroyTime;

        private void OnDisable()
        {
            _ready = false;
        }

        // direction must be normalized!
        public void Setup(Vector3 direction)
        {
            _direction = direction;
            _bounce = 0;
            _autoDestroyTime = Time.time + _autoDestroyDelay;
            _ready = true;
        }

        private void Update()
        {
            if (!_ready)
            {
                return;
            }
            
            if (Time.time >= _autoDestroyTime)
            {
                ComponentPool<Bullet>.Release(this);
                return;
            }
            
            Vector3 position = transform.position;
            float travelDistance = Time.deltaTime * _travelSpeed;
            Vector3 targetPosition = travelDistance * _direction + position;

            if (Physics.Raycast(position, _direction, out RaycastHit hit, travelDistance, _layerMask))
            {
                targetPosition = hit.point;
                AudioWavesManager.Instance.Spawn(_onHitAudioWaveAnimation, targetPosition);

                SFXAudioSource sfxAudioSource = ComponentPool<SFXAudioSource>.Get();
                sfxAudioSource.transform.position = hit.point;
                sfxAudioSource.PlayAndReleaseSelf(_impactAudioClip);

                if (_bounce >= _maxBounces)
                {
                    ComponentPool<Bullet>.Release(this);
                    return;
                }

                _direction = Vector3.Reflect(_direction, hit.normal);
                _bounce++;
                return;
            }

            transform.position = targetPosition;
        }
    }
}