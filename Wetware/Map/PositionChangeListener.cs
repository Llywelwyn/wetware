using Friflo.Engine.ECS;
using Position = Wetware.Components.Position;

namespace Wetware.Map;

public class PositionChangeListener
{
    private readonly Map _map;

    public PositionChangeListener(Map map, EntityStore? world = null)
    {
        _map = map;
        Subscribe(world);
    }

    // TODO: This listener should be moved to the MapRepository, and query the OnMap value of the entity,
    // pushing flag changes to that map. Right now, each map has its own listener, which is overly complex.

    private void Subscribe(EntityStore? world)
    {
        var _world = world ?? Game.Instance.World;
        _world.OnComponentAdded += HandleComponentAdded;
        _world.OnComponentRemoved += HandleComponentRemoved;
        _world.OnEntityDelete += HandleEntityDelete;
    }

    private void HandleComponentAdded(ComponentChanged e)
    {
        if (e.Type != typeof(Position)) return;
        var flags = e.Entity.GetBlockFlags();
        if (e.Action == ComponentChangedAction.Update) _map.Clear(e.OldComponent<Position>(), flags);
        _map.Set(e.Component<Position>(), flags);
    }

    private void HandleComponentRemoved(ComponentChanged e)
    {
        if (e.Type != typeof(Position)) return;
        var flags = e.Entity.GetBlockFlags();
        _map.Clear(e.OldComponent<Position>(), flags);
    }

    private void HandleEntityDelete(EntityDelete e)
    {
        if (!e.Entity.TryGetComponent(out Position pos)) return;
        var flags = e.Entity.GetBlockFlags();
        _map.Clear(pos, flags);
    }
}
