using Rendering;
using UnityEngine;

namespace AudioWaves
{
    public class AudioWaveAnimationPlayer : MonoBehaviour
    {
        [SerializeField]
        private ScriptableAudioWaveAnimation _audioWaveAnimation;

        [SerializeField]
        private AudioWave _audioWave;

        [SerializeField]
        private bool _playOnEnable = true;

        private void OnEnable()
        {
            if (_playOnEnable)
            {
                Play();
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(_audioWaveAnimation.Co_Animate(_audioWave));
        }
    }
}