using System.Collections;
using Rendering;
using UnityEngine;

namespace AudioWaves
{
    public class AudioWaveShooter : MonoBehaviour
    {
        [SerializeField]
        private float _travelSpeed = 10.0f;

        [SerializeField]
        private float _duration = 2.0f;

        [SerializeField]
        private float _startFadeOut = 1.5f;

        [SerializeField]
        private float _startRadius = 1.0f;

        [SerializeField]
        private float _endRadius = 10.0f;

        public void Shoot(Vector3 startPosition, Vector3 direction)
        {
            StartCoroutine(Co_Shoot(direction, AudioWavesManager.Instance.Spawn(startPosition)));
        }
        
        public void Shoot(Vector3 direction)
        {
            Shoot(transform.position, direction);
        }

        public void ShootForward()
        {
            Shoot(transform.forward);
        }
        
        public void ShootBack()
        {
            Shoot(transform.forward * -1.0f);
        }

        private IEnumerator Co_Shoot(Vector3 direction, AudioWave wave)
        {
            wave.InnerRadius = 0.0f;

            float fadeOutDuration = _duration - _startFadeOut;
            
            for (float timer = 0.0f; timer <= _duration; timer += Time.deltaTime)
            {
                float t = timer / _duration;
                float t1 = Mathf.Clamp01((timer - _startFadeOut) / fadeOutDuration);

                wave.Intensity = Mathf.Lerp(1.0f, 0.0f, t1);
                wave.OuterRadius = Mathf.Lerp(_startRadius, _endRadius, t);
                wave.transform.position += Time.deltaTime * _travelSpeed * direction;

                yield return null;
            }

            AudioWavesManager.Instance.Release(wave);
        }
    }
}