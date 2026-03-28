# Object Pooler for Unity

A generic component-based object pool. Derive from `MB_ObjectPooler<T>` to pre-warm and reuse pooled instances by component type, avoiding runtime allocations for frequently spawned objects.

## Features

- **Component-typed access** — get a direct reference to the component you care about, not just a `GameObject`
- **Auto-expanding** — spawns new instances automatically if the pool runs dry
- **Odin Inspector support** — enhanced Inspector layout when Odin is present, fully functional without it

## Setup

### 1. Create a pooler class

Derive from `MB_ObjectPooler<T>` and provide the component type you want access to after spawning:

```csharp
public class ProjectilePooler : MB_ObjectPooler<Projectile> { }
```

### 2. Add it to your scene

Create a GameObject, attach your pooler component, and assign the prefab in the Inspector.

### 3. Spawn from the pool

```csharp
[SerializeField] ProjectilePooler _pooler;

void Shoot()
{
    var projectile = _pooler.NextFromPool;

    // configure before activating
    projectile.transform.position = transform.position;
    projectile.direction = transform.forward;

    projectile.gameObject.SetActive(true);
}
```

> Always configure the object **before** calling `SetActive(true)`.

### 4. Return objects to the pool

Deactivate the GameObject when done — the pool treats inactive objects as available:

```csharp
gameObject.SetActive(false);
```

## Advanced: customising pool initialisation

### Defer pool creation

By default the pool is created and pre-warmed in `Awake`. Override it to suppress auto-init and call `Load()` when you are ready — for example, after a loading screen or once a dependency is available:

```csharp
public class ProjectilePooler : MB_ObjectPooler<Projectile>
{
    // Suppress auto-init
    protected override void Awake() { }

    // Call this from a manager or loading sequence
    public void Init() => Load();
}
```

`Load()` is `virtual`, so you can also override it to run logic before or after the pool is created.

### Separate pool creation from pre-warming

Override `NewPool` to construct the pool without immediately pre-warming it, then call `Pool.Prewarm(StartAmount)` at the exact moment you want the instances spawned — useful when you want to stagger allocation across frames or delay it until the scene is fully loaded:

```csharp
public class ProjectilePooler : MB_ObjectPooler<Projectile>
{
    // Create the pool with startAmount = 0 so the constructor skips pre-warming
    protected override ComponentObjectPool<Projectile> NewPool =>
        new(Prefab, Parent, 0, DefaultName);

    public override void Load()
    {
        base.Load();                    // assigns NewPool to Pool
        Pool.Prewarm(StartAmount);      // pre-warm separately, at a controlled time
    }
}
```

## Odin Inspector

Odin Inspector is **not required**. When installed, the Inspector displays a grouped layout with labels and validation. Without it, the standard Unity Inspector is used.
