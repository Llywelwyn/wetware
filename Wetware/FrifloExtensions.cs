using Friflo.Engine.ECS;
using Wetware.Components;

namespace Wetware;

public static class EntityExtensions
{
    /// <summary>
    /// Attempts to get the name of an entity and it's ID. If the entity has no name, it returns the ID only. 
    /// </summary>
    /// <remarks>
    /// This is explicitly used for debugging. It will not respect unidentified entities, or even if the given entity
    /// has a name at all.
    /// </remarks>
    public static string DebugName(this Entity e)
    {
        if (e.TryGetComponent(out Name name))
        {
            return $"{name.Value} ({e.Id})";
        }

        return e.HasName ? $"{e.Name.value} ({e.Id})" : $"unnamed {e.Id}";
    }
}