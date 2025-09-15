using Wetware.Components;
using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Serialize;
using Wetware.Extensions;

namespace Wetware.Factories;

public class EntityRegistry
{
    private readonly EntityStore m_store = new();
    private readonly Dictionary<string, Entity> m_prefabs = new();

    public EntityRegistry(string prefabFile)
    {
        var serializer = new EntitySerializer();
        using var stream = File.OpenRead(prefabFile);
        serializer.ReadIntoStore(m_store, stream);

        foreach (var e in m_store.Entities)
        {
            m_prefabs[e.GetComponent<Name>().Value] = e;
        }
    }

    public Entity Spawn(string name, EntityStore store)
    {
        if (!m_prefabs.TryGetValue(name, out var prefab))
            throw new InvalidOperationException($"Unknown prefab '{prefab}'");
        return prefab.MoveTo(store);
    }
}
