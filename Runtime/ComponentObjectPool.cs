using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace bnj.object_pooler.Runtime
{
    /// <summary>
    /// A plain C# generic object pool for Unity <see cref="Component"/> types.
    /// Lazily expands by spawning new instances when all pooled objects are active.
    /// </summary>
    /// <typeparam name="T">The <see cref="Component"/> type managed by this pool.</typeparam>
    public class ComponentObjectPool<T> where T : Component
    {
        List<T> _pooledObjects = new();
        Transform _parentTransform;

        T _prefab;
        string _defaultObjectName;

        /// <summary>A snapshot of all currently active (in-use) objects in the pool.</summary>
        public List<T> ActiveObjects => _pooledObjects.Where(x => x.gameObject.activeSelf).ToList();

        /// <summary>
        /// Creates the pool and pre-warms it with inactive instances.
        /// Pass <c>0</c> or a negative value for <paramref name="startAmount"/> to skip pre-warming.
        /// </summary>
        /// <param name="prefab">The prefab to instantiate.</param>
        /// <param name="parentTransform">The transform to parent pooled objects under.</param>
        /// <param name="startAmount">Number of instances to create immediately. Pass <c>0</c> to skip pre-warming.</param>
        /// <param name="defaultObjectName">Name assigned to each pooled GameObject.</param>
        public ComponentObjectPool(T prefab, Transform parentTransform, int startAmount, string defaultObjectName = "PooledObject")
        {
            _prefab = prefab;
            _parentTransform = parentTransform;
            _defaultObjectName = defaultObjectName;

            if (startAmount > 0) Prewarm(startAmount);
        }

        /// <summary>
        /// Returns the next inactive object from the pool.
        /// If all objects are active, spawns a new instance before returning.
        /// </summary>
        public T Next()
        {
            if (_pooledObjects.Count(x => !x.gameObject.activeSelf) == 0)
                SpawnNewObject();

            return _pooledObjects.FirstOrDefault(x => !x.gameObject.activeSelf);
        }

        /// <summary>
        /// Spawns <paramref name="amount"/> additional inactive instances into the pool.
        /// Call this manually when the pool was constructed with <c>startAmount</c> of <c>0</c>.
        /// </summary>
        /// <param name="amount">Number of instances to add.</param>
        public void Prewarm(int amount)
        {
            for (int i = 0; i < amount; i++)
                SpawnNewObject();
        }

        /// <summary>
        /// Returns an object to the pool by deactivating its GameObject.
        /// </summary>
        public void FreeObject(T objectToFree) => objectToFree.gameObject.SetActive(false);

        void SpawnNewObject()
        {
            T newObject = Object.Instantiate(_prefab, _parentTransform);
            newObject.name = _defaultObjectName;
            FreeObject(newObject);
            _pooledObjects.Add(newObject);
        }
    }
}
