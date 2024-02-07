using Rendering;
using UnityEngine;

namespace AudioWaves
{
    public class AudioWaveOnTrigger : MonoBehaviour
    {
        private static readonly Collider[] ColliderBuffer = new Collider[1];
        
        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private AudioWave _audioWave;

        [SerializeField]
        private float _testRadius = 0.2f;

        [SerializeField]
        private float _intensity = 1.0f;

        private void Update()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, _testRadius, ColliderBuffer, _layerMask);
            bool inside = count > 0;

            _audioWave.Intensity = Mathf.MoveTowards(_audioWave.Intensity, inside ? _intensity : 0.0f, Time.deltaTime * 5f);
            //if ((_layerMask.value & (1 << other.gameObject.layer)) == (1 << other.gameObject.layer))
        }
    }
}