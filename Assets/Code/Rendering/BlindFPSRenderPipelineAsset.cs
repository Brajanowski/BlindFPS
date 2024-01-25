using Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace Rendering
{
    [CreateAssetMenu(menuName = BlindFPSConstants.AssetCreationMenu + "/Rendering/Render Pipeline Asset")]
    public class BlindFPSRenderPipelineAsset : RenderPipelineAsset
    {
        [field: SerializeField]
        public float StartFallOff { get; private set; } = 50.0f;

        [field: SerializeField]
        public float EndFallOff { get; private set; } = 80.0f;

        [field: SerializeField]
        public Color BaseColor { get; private set; } = Color.white;
        
        protected override RenderPipeline CreatePipeline()
        {
            return new BlindFPSRenderPipeline(this);
        }
    }
}