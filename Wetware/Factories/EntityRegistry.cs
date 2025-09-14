using Wetware.Components;
using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Serialize;
using Wetware.Extensions;

public class EntityRegistry
{
    private readonly EntityStore _store = new();
    private readonly Dictionary<string, Entity> _prefabs = new();

    public EntityRegistry(string prefabFile)
    {
        var serializer = new EntitySerializer();
        using var stream = File.OpenRead(prefabFile);
        serializer.ReadIntoStore(_store, stream);

        foreach (var e in _store.Entities)
        {
            _prefabs[e.GetComponent<Name>().Value] = e;
        }
    }

    public Entity Spawn(string name, EntityStore store)
    {
        if (!_prefabs.TryGetValue(name, out var prefab))
            throw new InvalidOperationException($"Unknown prefab '{prefab}'");
        return prefab.MoveTo(store);
    }
}
