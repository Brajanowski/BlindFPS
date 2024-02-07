using System.Collections;
using Rendering;
using UnityEngine;

namespace AudioWaves
{
    [CreateAssetMenu]
    public class ExpandAndFadoutWaveAnimation : ScriptableAudioWaveAnimation
    {
        [SerializeField]
        private float _duration = 2.0f;
        
        [SerializeField]
        private float _startFadeOut = 1.5f;

        [SerializeField]
        private float _radius = 10.0f;

        [SerializeField]
        private bool _animateInnerRadius = true;

        public override IEnumerator Co_Animate(AudioWave audioWave)
        {
            audioWave.InnerRadius = 0.0f;
            audioWave.Intensity = 1.0f;
            
            float fadeOutDuration = _duration - _startFadeOut;

            for (float timer = 0.0f; timer <= _duration; timer += Time.deltaTime)
            {
                float t = timer / _duration;
                float t1 = Mathf.Clamp01((timer - _startFadeOut) / fadeOutDuration);

                audioWave.OuterRadius = Mathf.Lerp(0, _radius, t);

                if (_animateInnerRadius)
                {
                    audioWave.InnerRadius = Mathf.Lerp(0, _radius * 0.8f, t1);
                }
                
                audioWave.Intensity = Mathf.Lerp(1, 0, t1);
                yield return null;
            }

            audioWave.OuterRadius = 0.0f;
            audioWave.InnerRadius = 0.0f;
        }
    }
}