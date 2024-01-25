using System.Collections;
using Core;
using Rendering;
using UnityEngine;

namespace AudioWaves
{
    [CreateAssetMenu(menuName = BlindFPSConstants.AssetCreationMenu + "Wave Animation/Constant")]
    public class ConstantAudioWaveAnimation : ScriptableAudioWaveAnimation
    {
        [SerializeField]
        private float _radius = 5.0f;

        public override IEnumerator Co_Animate(AudioWave audioWave)
        {
            WaitForSeconds delay = new WaitForSeconds(0.05f);
            while (true)
            {
                audioWave.OuterRadius = _radius * Random.Range(0.9f, 1.0f);
                audioWave.Intensity = Random.Range(0.9f, 1.0f);
                yield return delay;
            }
        }
    }
}