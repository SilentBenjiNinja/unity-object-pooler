# Object Pooler for Unity

* Pools commonly used objects like projectiles, enemies, effects, etc.
* Stores references to the most important Component instead of the GameObject for easy modification access after spawning

## Setup

* Create a class that is derived from ```MB_ObjectPooler```; make sure to provide the concrete type of Component you want to access after spawning as the generic argument (e.g. ```EnemyPooler : MB_ObjectPooler<Enemy>{...}```)
* Create a new GameObject and attach the new Pooler component
* Assign the Prefab you want to spawn to the Pooler
* Instead of instantiating the prefab, call ```NextFromPool()``` on the Pooler instead to get a reference to an unused object from the pool
* After NextFromPool is called and the initial state prepared (e.g. positioning, HP, etc.), dont forget to also set the GameObject active
* Optional: if you want control over when the objects are pooled, override the Pooler's ```Awake()``` function and call ```Load()``` manually
