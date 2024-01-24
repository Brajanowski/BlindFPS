﻿using UnityEngine;

namespace Misc
{
    public class SystemsLoader : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Load()
        {
            SystemsLoader systemsLoader = Resources.Load<SystemsLoader>("SystemsLoader");
            DontDestroyOnLoad(Instantiate(systemsLoader));
        }

        [SerializeField]
        private GameObject[] _prefabsToSpawn;

        private void Awake()
        {
            foreach (GameObject prefab in _prefabsToSpawn)
            {
                GameObject go = Instantiate(prefab);
                DontDestroyOnLoad(go);
            }
        }
    }
}