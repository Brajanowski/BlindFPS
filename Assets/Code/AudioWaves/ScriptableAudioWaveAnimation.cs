using System.Collections;
using Rendering;
using UnityEngine;

namespace AudioWaves
{
    public abstract class ScriptableAudioWaveAnimation : ScriptableObject
    {
        public abstract IEnumerator Co_Animate(AudioWave audioWave);
    }
}