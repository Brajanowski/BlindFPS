using UnityEngine;
using UnityEngine.Pool;

namespace Misc
{
    public class ComponentPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static ComponentPool<T> _instance;

        [SerializeField]
        private T _prefab;

        [SerializeField]
        private int _initialCapacity = 20;

        private IObjectPool<T> _objectPool;

        private void Awake()
        {
            _instance = this;
            
            _objectPool = new ObjectPool<T>(CreateObject, OnGetFromPool, OnReleaseToPool, DestroyObject, true, _initialCapacity);
        }

        public static T Get()
        {
            return _instance._objectPool.Get();
        }

        public static void Release(T obj)
        {
            _instance._objectPool.Release(obj);
        }

        private T CreateObject()
        {
            T res = Instantiate(_prefab, transform);
            res.gameObject.SetActive(false);
            return res;
        }

        private void DestroyObject(T obj)
        {
            Destroy(obj.gameObject);
        }

        private void OnGetFromPool(T obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
        }
    }
}