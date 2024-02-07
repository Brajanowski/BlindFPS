using System.Collections;
using Rendering;
using UnityEngine;

namespace AudioWaves
{
    public class AudioWavesManager : MonoBehaviour
    {
        public static AudioWavesManager Instance { get; private set; }

        [SerializeField]
        private AudioWave _prefab;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // TODO: object pooling
        public AudioWave Spawn(Vector3 position)
        {
            AudioWave wave = Instantiate(_prefab);
            wave.transform.position = position;
            return wave;
        }

        public void Release(AudioWave audioWave)
        {
            Destroy(audioWave.gameObject);
        }

        public void Spawn(ScriptableAudioWaveAnimation waveAnimation, Vector3 position)
        {
            StartCoroutine(Co_AnimateWave(waveAnimation, Spawn(position)));
        }

        private IEnumerator Co_AnimateWave(ScriptableAudioWaveAnimation waveAnimation, AudioWave wave)
        {
            yield return waveAnimation.Co_Animate(wave);
            Release(wave);
        }
    }
}