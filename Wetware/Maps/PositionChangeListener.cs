using Friflo.Engine.ECS;
using Wetware.Extensions;
using Position = Wetware.Components.Position;

namespace Wetware.Maps;

public class PositionChangeListener
{
    private readonly Map m_map;

    public PositionChangeListener(Map map, EntityStore world)
    {
        m_map = map;
        Subscribe(world);
    }

    // TODO: This listener should be moved to the MapRepository, and query the OnMap value of the entity,
    // pushing flag changes to that map. Right now, each map has its own listener, which is overly complex.

    private void Subscribe(EntityStore world)
    {
        world.OnComponentAdded += HandleComponentAdded;
        world.OnComponentRemoved += HandleComponentRemoved;
        world.OnEntityDelete += HandleEntityDelete;
    }

    private void HandleComponentAdded(ComponentChanged e)
    {
        if (e.Type != typeof(Position)) return;
        var flags = e.Entity.GetBlockFlags();
        if (e.Action == ComponentChangedAction.Update) m_map.Clear(e.OldComponent<Position>(), flags);
        m_map.Set(e.Component<Position>(), flags);
    }

    private void HandleComponentRemoved(ComponentChanged e)
    {
        if (e.Type != typeof(Position)) return;
        var flags = e.Entity.GetBlockFlags();
        m_map.Clear(e.OldComponent<Position>(), flags);
    }

    private void HandleEntityDelete(EntityDelete e)
    {
        if (!e.Entity.TryGetComponent(out Position pos)) return;
        var flags = e.Entity.GetBlockFlags();
        m_map.Clear(pos, flags);
    }
}
