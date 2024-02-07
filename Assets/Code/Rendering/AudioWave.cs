using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
    public class AudioWave : MonoBehaviour
    {
        public static readonly List<AudioWave> Instances = new();
        
        private static readonly int OuterRadiusShaderId = Shader.PropertyToID("_OuterRadius");
        private static readonly int InnerRadiusShaderId = Shader.PropertyToID("_InnerRadius");
        private static readonly int CenterShaderId = Shader.PropertyToID("_Center");
        private static readonly int IntensityShaderId = Shader.PropertyToID("_Intensity");

        [field: SerializeField]
        public float OuterRadius { get; set; } = 5.0f;

        [field: SerializeField]
        public float InnerRadius { get; set; } = 2.5f;

        [field: SerializeField]
        public float Intensity { get; set; } = 1.0f;

        private MaterialPropertyBlock _materialPropertyBlock;

        public MaterialPropertyBlock MaterialPropertyBlock
        {
            get
            {
                if (_materialPropertyBlock == null)
                {
                    _materialPropertyBlock = new MaterialPropertyBlock();
                }

                _materialPropertyBlock.SetVector(CenterShaderId, transform.position);
                _materialPropertyBlock.SetFloat(OuterRadiusShaderId, OuterRadius);
                _materialPropertyBlock.SetFloat(InnerRadiusShaderId, InnerRadius);
                _materialPropertyBlock.SetFloat(IntensityShaderId, Intensity);

                return _materialPropertyBlock;
            }
        }

        private void OnEnable()
        {
            Instances.Add(this);
        }

        private void OnDisable()
        {
            Instances.Remove(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, InnerRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, OuterRadius);
        }
    }
}