using Friflo.Engine.ECS;
using Wetware.Components;
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
        if (!e.Entity.TryGetComponent(out Obstruction obstruction)) return;
        if (e.Action == ComponentChangedAction.Update) _map[e.OldComponent<Position>()] -= obstruction.Value;
        _map[e.Component<Position>()] += obstruction.Value;
    }

    private void HandleComponentRemoved(ComponentChanged e)
    {
        if (e.Type != typeof(Position)) return;
        if (!e.Entity.TryGetComponent(out Obstruction obstruction)) return;
        _map[e.OldComponent<Position>()] -= obstruction.Value;
    }

    private void HandleEntityDelete(EntityDelete e)
    {
        if (!e.Entity.TryGetComponent(out Position pos)) return;
        if (!e.Entity.TryGetComponent(out Obstruction obstruction)) return;
        _map[pos] -= obstruction.Value;
    }
}