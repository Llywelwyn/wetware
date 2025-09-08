using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Serialize;
using Wetware.Components;
using Wetware.Flags;
using TileFlag = Wetware.Map.TileFlag;

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

    /// <summary>
    /// Collects a TileFlag bitarray from an Entity's EntityData tags.
    /// </summary>
    public static TileFlag GetBlockFlags(this Entity e)
    {
        var flags = TileFlag.None;
        var data = e.Data;
        if (data.Tags.Has<BlocksMovement>()) flags |= TileFlag.BlocksMovement;
        if (data.Tags.Has<BlocksItems>()) flags |= TileFlag.BlocksItems;
        if (data.Tags.Has<BlocksVision>()) flags |= TileFlag.BlocksVision;
        return flags;
    }

    /// <summary>
    /// Moves an Entity from one EntityStore to another, sort of. This method makes an exact clone of an Entity
    /// in another EntityStore, and then deletes the original entity.
    /// </summary>
    /// <remarks>
    /// Be aware of any listeners for OnDelete/OnComponentRemoved. Otherwise, functionally a Move.
    /// </remarks>
    public static Entity MoveTo(this Entity original, EntityStore world)
    {
        var clone = world.CreateEntity();
        original.CopyEntity(clone);
        original.DeleteEntity();
        return clone;
    }
}

public static class EntityStoreExtensions
{
    public static EntityStore LoadFromFile(this EntityStore store, string path)
    {
        var serializer = new EntitySerializer();
        var readStream = new FileStream(path, FileMode.Open);
        serializer.ReadIntoStore(store, readStream);
        readStream.Close();
        return store;
    }
}
