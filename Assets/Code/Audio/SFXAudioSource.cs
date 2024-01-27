using System.Collections;
using Core;
using UnityEngine;

namespace Audio
{
    public class SFXAudioSource : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        public void PlayAndReleaseSelf(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
            StartCoroutine(Co_DestroyAfterFinishedPlaying());
        }

        private IEnumerator Co_DestroyAfterFinishedPlaying()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            ComponentPool<SFXAudioSource>.Release(this);
        }
    }
}