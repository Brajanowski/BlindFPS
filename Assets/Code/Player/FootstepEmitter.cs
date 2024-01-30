using AudioWaves;
using UnityEngine;

namespace Player
{
    public class FootstepEmitter : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private ScriptableAudioWaveAnimation _audioWaveAnimation;

        [SerializeField]
        private float _pitchVariation = 0.1f;

        public void Emit()
        {
            _audioSource.pitch = 1.0f + Random.Range(0.0f, 1.0f) * _pitchVariation;
            _audioSource.Play();
            AudioWavesManager.Instance.Spawn(_audioWaveAnimation, transform.position);
        }
    }
}