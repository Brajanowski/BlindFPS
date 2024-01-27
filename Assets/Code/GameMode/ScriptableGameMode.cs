using System.Collections;
using UnityEngine;

namespace GameMode
{
    public abstract class ScriptableGameMode : ScriptableObject, IGameMode
    {
        public abstract IEnumerator OnEnter();
        public abstract IEnumerator OnExit();
        public abstract void Tick();
    }
}