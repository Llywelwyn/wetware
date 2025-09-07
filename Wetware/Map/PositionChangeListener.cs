using Friflo.Engine.ECS;
using Wetware.Flags;
using Position = Wetware.Components.Position;

namespace Wetware.Map;

public class PositionChangeListener
{
    private readonly Map _map;

    public PositionChangeListener(Map map)
    {
        _map = map;
        Subscribe();
    }

    private void Subscribe()
    {
        var world = Game.Instance.World;
        world.OnComponentAdded += HandleComponentAdded;
        world.OnComponentRemoved += HandleComponentRemoved;
        world.OnEntityDelete += HandleEntityDelete;
    }

    private void HandleComponentAdded(ComponentChanged e)
    {
        if (e.Type != typeof(Position)) return;
        if (!e.Entity.Tags.Has<BlocksMovement>()) return;
        if (e.Action == ComponentChangedAction.Update) _map[e.OldComponent<Position>()] = false;
        _map[e.Component<Position>()] = true;
    }

    private void HandleComponentRemoved(ComponentChanged e)
    {
        if (e.Type != typeof(Position)) return;
        if (!e.Entity.Tags.Has<BlocksMovement>()) return;
        _map[e.OldComponent<Position>()] = false;
    }

    private void HandleEntityDelete(EntityDelete e)
    {
        if (!e.Entity.TryGetComponent(out Position pos)) return;
        if (!e.Entity.Tags.Has<BlocksMovement>()) return;
        _map[pos] = false;
    }
}
