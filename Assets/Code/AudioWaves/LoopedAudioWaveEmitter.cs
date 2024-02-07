using UnityEngine;

namespace AudioWaves
{
    public class LoopedAudioWaveEmitter : MonoBehaviour
    {
        [SerializeField]
        private float _rate = 1.0f;

        [SerializeField]
        private ScriptableAudioWaveAnimation _audioWaveAnimation;

        private float _nextSpawnTime;

        private void Update()
        {
            if (Time.time > _nextSpawnTime)
            {
                _nextSpawnTime = Time.time + 1.0f / _rate;
                AudioWavesManager.Instance.Spawn(_audioWaveAnimation, transform.position);
            }
        }
    }
}