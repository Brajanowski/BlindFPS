using UnityEngine;

namespace Core
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}