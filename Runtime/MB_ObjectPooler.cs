using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace bnj.object_pooler.Runtime
{
    /// <summary>
    /// Abstract MonoBehaviour base class for component-based object pools.
    /// Derive from this and provide the <see cref="Component"/> type you need access to after spawning.
    /// </summary>
    /// <typeparam name="T">The <see cref="Component"/> type stored by this pool.</typeparam>
#if ODIN_INSPECTOR
    [HideMonoScript]
#endif
    public abstract class MB_ObjectPooler<T> : MonoBehaviour where T : Component
    {
#if ODIN_INSPECTOR
        [BoxGroup("Object Pool")]
        [LabelWidth(100), AssetsOnly, Required]
#endif
        [SerializeField] T _prefab;

#if ODIN_INSPECTOR
        [BoxGroup("Object Pool")]
        [LabelWidth(100)]
#endif
        [SerializeField, Range(1, 128)] int _startAmount = 16;

#if ODIN_INSPECTOR
        [BoxGroup("Object Pool")]
        [LabelWidth(100), LabelText("Parent (optional)")]
#endif
        [SerializeField, Tooltip("Leave empty to use this transform")] Transform _parent;

        /// <summary>
        /// The transform under which pooled objects are parented.
        /// Defaults to this transform if left unassigned.
        /// </summary>
        protected Transform Parent => _parent == null ? transform : _parent;

        /// <summary>The prefab used to instantiate pooled objects.</summary>
        protected T Prefab => _prefab;

        /// <summary>The number of instances to pre-warm on initialisation, as set in the Inspector.</summary>
        protected int StartAmount => _startAmount;

#if ODIN_INSPECTOR
        [BoxGroup("Object Pool")]
        [LabelWidth(100), LabelText("Name (optional)")]
#endif
        [SerializeField, Tooltip("Leave empty to use prefab's name")] string _defaultName;

        /// <summary>
        /// The name assigned to pooled GameObjects.
        /// Defaults to the prefab name with a <c>(Pooled)</c> suffix if left unassigned.
        /// </summary>
        protected string DefaultName =>
            !string.IsNullOrWhiteSpace(_defaultName) ? _defaultName : _prefab.name + " (Pooled)";

        ComponentObjectPool<T> _pool;

        /// <summary>
        /// The underlying pool instance. Available after <see cref="Load"/> has been called.
        /// </summary>
        protected ComponentObjectPool<T> Pool => _pool;

        /// <summary>
        /// Creates the <see cref="ComponentObjectPool{T}"/> assigned to <see cref="Pool"/>.
        /// Override to customise how the pool is constructed, e.g. to separate creation from pre-warming.
        /// </summary>
        protected virtual ComponentObjectPool<T> NewPool =>
            new(Prefab, Parent, StartAmount, DefaultName);

        /// <summary>
        /// Returns the next available inactive object from the pool.
        /// Automatically expands the pool if all objects are currently active.
        /// </summary>
        public T NextFromPool => _pool.Next();

        /// <summary>
        /// Initialises the pool on Awake. Override and call <see cref="Load"/> manually
        /// if you need control over initialisation timing.
        /// </summary>
        protected virtual void Awake() => Load();

        /// <summary>
        /// Assigns <see cref="NewPool"/> to <see cref="Pool"/>.
        /// Call manually if <see cref="Awake"/> is overridden.
        /// </summary>
        public virtual void Load()
        {
            _pool = NewPool;
        }
    }
}
