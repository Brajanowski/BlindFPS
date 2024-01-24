using System.Collections;
using Misc;
using Rendering;
using UnityEngine;

namespace AudioWaves
{
    public class AudioWavesManager : SingletonMonoBehaviour<AudioWavesManager>
    {
        public AudioWave Spawn(Vector3 position)
        {
            AudioWave wave = ComponentPool<AudioWave>.Get();
            wave.transform.position = position;
            return wave;
        }

        public void Release(AudioWave audioWave)
        {
            ComponentPool<AudioWave>.Release(audioWave);
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