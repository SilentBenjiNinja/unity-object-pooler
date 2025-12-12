using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace bnj.object_pooler.Runtime
{
    public class ComponentObjectPool<T> where T : Component
    {
        List<T> _pooledObjects = new();
        Transform _parentTransform;

        T _prefab;
        string _defaultObjectName;

        public List<T> ActiveObjects => _pooledObjects.Where(x => x.gameObject.activeSelf).ToList();

        public ComponentObjectPool(T prefab, Transform parentTransform, int startAmount, string defaultObjectName = "PooledObject")
        {
            _prefab = prefab;
            _parentTransform = parentTransform;
            _defaultObjectName = defaultObjectName;

            for (int i = 0; i < startAmount; i++)
                SpawnNewObject();
        }

        void SpawnNewObject()
        {
            T newObject = Object.Instantiate(_prefab, _parentTransform);
            newObject.name = _defaultObjectName;
            FreeObject(newObject);
            _pooledObjects.Add(newObject);
        }

        public T Next()
        {
            if (_pooledObjects.Count(x => !x.gameObject.activeSelf) == 0)
                SpawnNewObject();

            return _pooledObjects.FirstOrDefault(x => !x.gameObject.activeSelf);
        }

        public void FreeObject(T objectToFree) => objectToFree.gameObject.SetActive(false);
    }
}
