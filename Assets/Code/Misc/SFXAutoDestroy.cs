using System.Collections;
using UnityEngine;

namespace Misc
{
    // TODO: this should be removed in future and replaced with pooling
    public class SFXAutoDestroy : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        private void OnEnable()
        {
            _audioSource.Play();

            StartCoroutine(Co_DestroyAfterFinishedPlaying());
        }

        private IEnumerator Co_DestroyAfterFinishedPlaying()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            Destroy(gameObject);
        }
    }
}