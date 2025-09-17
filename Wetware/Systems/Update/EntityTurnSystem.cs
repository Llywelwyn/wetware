using Friflo.Engine.ECS.Systems;
using Friflo.Engine.ECS;
using Wetware.Globals;
using Wetware.Config;
using Wetware.Flags;
using Wetware.Components;
using Position = Wetware.Components.Position;
using Wetware.Maps;
using Wetware.Extensions;

namespace Wetware.Systems.Update;

public class EntityTurnSystem : QuerySystem
{
    protected override void OnUpdate()
    {
        Debug.SystemBoundaryStart(nameof(EntityTurnSystem));

        if (!Query.Store.TryGetEntityById(TurnQueue.Peek() ?? -1, out var entity)) return;
        if (TurnQueue.Next() is not int id) return;
        if (entity.IsNull) return;

        Debug.Print($"{entity.DebugName()}'s turn.");
        Debug.Print(entity.DebugJSON);

        var data = entity.Data;

        if (data.Tags.Has<Clock>())
        {
            Debug.Print($"{entity.DebugName()} is the turn clock. Spending turn setting ClockTurn to true.");
            UseEnergy(entity, 1000);
            Game.Instance.ClockTurn = true;
            return;
        }

        UseEnergy(entity, 1000);

        if (!data.TryGet<Brain>(out var brain)) return;

        if (!brain.Wanders) return;

        if (!data.TryGet<Position>(out var pos)) return;

        Map map = Game.Instance.MapRepository.CurrentMap();

        var possibleMoves = new List<Position>();
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;
                var position = new Position(pos.X + dx, pos.Y + dy);
                if (map.InBounds(position) && !map.Has(position, TileFlag.BlocksMovement)) possibleMoves.Add(position);
            }
        }

        if (possibleMoves.Count > 0)
            entity.AddComponent(possibleMoves[new Random().Next(possibleMoves.Count)]);
    }

    private static void UseEnergy(Entity entity, int amount)
    {
        var energy = entity.GetComponent<Energy>();
        var newValue = energy.Value - amount;
        _ = entity.AddComponent(new Energy(newValue));
        TurnQueue.Enqueue(entity.Id, newValue);
    }
}
