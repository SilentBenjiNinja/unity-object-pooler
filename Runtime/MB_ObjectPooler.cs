using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace bnj.object_pooler.Runtime
{
    [HideMonoScript]
    public abstract class MB_ObjectPooler<T> : MonoBehaviour where T : Component
    {
        [BoxGroup("Object Pool")]
        [SerializeField, LabelWidth(100), Range(1, 128)] int _startAmount = 16;

        [BoxGroup("Object Pool")]
        [SerializeField, LabelWidth(100), AssetsOnly, Required] T _prefab;

        [BoxGroup("Object Pool")]
        [Tooltip("Leave empty to use this transform")]
        [SerializeField, LabelWidth(100), LabelText("Parent (optional)")] Transform _parent;
        protected Transform Parent => _parent == null ? transform : _parent;

        [BoxGroup("Object Pool")]
        [Tooltip("Leave empty to use prefab's name")]
        [SerializeField, LabelWidth(100), LabelText("Name (optional)")] string _defaultName;
        protected string DefaultName =>
            !string.IsNullOrWhiteSpace(_defaultName) ? _defaultName : _prefab.name + " (Pooled)";

        ComponentObjectPool<T> _pool;
        protected ComponentObjectPool<T> Pool => _pool;

        public T NextFromPool => _pool.Next();

        protected virtual void Awake() => Load();

        public virtual void Load()
        {
            _pool = new(_prefab, Parent, _startAmount, DefaultName);
        }
    }
}
