# 1.1.0

### Improvements

* Odin Inspector is now optional — all Odin attributes are guarded by `#if ODIN_INSPECTOR`
* Added XML documentation to all public APIs (`MB_ObjectPooler`, `ComponentObjectPool`)
* Improved `package.json` description
* Added `Prewarm(int amount)` public method to `ComponentObjectPool` for controlled pre-warming
* `ComponentObjectPool` constructor no longer takes a `prewarm` flag — pre-warming is skipped automatically when `startAmount` is `0` or less
* Added `protected` getter properties `Prefab`, `StartAmount` on `MB_ObjectPooler` for use in derived classes
* `NewPool` is now `protected virtual` — override to customise pool construction
* `Load()` is now `public virtual` and delegates to `NewPool`
