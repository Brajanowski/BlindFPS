using UnityEngine;

namespace AudioWaves
{
    public class SingleAudioWaveSpawner : MonoBehaviour
    {
        [SerializeField]
        private ScriptableAudioWaveAnimation _audioWaveAnimation;

        public void Spawn()
        {
            AudioWavesManager.Instance.Spawn(_audioWaveAnimation, transform.position);
        }
    }
}