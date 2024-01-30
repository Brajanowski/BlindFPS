using UnityEngine;

namespace Core
{
    public static class SystemsLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Load()
        {
            GameObject[] systemsPrefabs = Resources.LoadAll<GameObject>("Systems/");
            
            foreach (GameObject systemPrefab in systemsPrefabs)
            {
                GameObject go = Object.Instantiate(systemPrefab);
                Object.DontDestroyOnLoad(go);
            }
        }
    }
}