using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Objects
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        private readonly int _initialSize = 10;

        private Queue<GameObject> _pool;
        private HashSet<GameObject> _activeObjects = new HashSet<GameObject>();

        private void Awake()
        {
            _pool = new Queue<GameObject>();

            for (int i = 0; i < _initialSize; ++i)
            {
                GameObject obj = Instantiate(_prefab, transform);
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public void ResetPool()
        {
            foreach (var obj in new List<GameObject>(_activeObjects))
            {
                ReturnObject(obj);
            }
        }

        public GameObject GetObject(Transform parent = null)
        {
            GameObject obj;
            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
            }
            else
            {
                obj = Instantiate(_prefab);
            }

            if (parent != null)
                obj.transform.SetParent(parent, false);

            obj.SetActive(true);
            _activeObjects.Add(obj);

            return obj;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);

            if (obj.transform.parent != transform)
                obj.transform.SetParent(transform, false);

            _activeObjects.Remove(obj);
            _pool.Enqueue(obj);
        }
    }
}