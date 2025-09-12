using Friflo.Engine.ECS.Systems;
using Friflo.Engine.ECS;
using Wetware.Globals;
using Wetware.Flags;
using Wetware.Components;
using Wetware.Config;

namespace Wetware.Systems.Update;

public class EntityTurnSystem : QuerySystem
{
    protected override void OnUpdate()
    {
        Debug.SystemBoundaryStart(nameof(EntityTurnSystem));

        if (TurnQueue.Next() is not int id) return;

        var entity = Query.Store.GetEntityById(id);
        if (entity.IsNull) return;

        Debug.Print($"{entity.DebugName()}'s turn.");

        var data = entity.Data;

        if (data.Tags.Has<Clock>())
        {
            Debug.Print($"{entity.DebugName()} is the turn clock. Spending turn setting ClockTurn to true.");
            UseEnergy(entity, 1000);
            Game.Instance.ClockTurn = true;
            return;
        }

        UseEnergy(entity, 1000);
        Debug.Print($"{entity.DebugName()} spent its turn doing nothing (-1000).");

        if (!data.TryGet<Brain>(out var brain)) return;

        // TODO: At this point, we have our Entity, EntityData, and Entity's Brain component,
        // and our entity can act. Whatever action it takes, UseEnergy(entity, <amount>).
        return;
    }

    private static void UseEnergy(Entity entity, int amount)
    {
        var energy = entity.GetComponent<Energy>();
        var newValue = energy.Value - amount;
        _ = entity.AddComponent(new Energy(newValue));
        TurnQueue.Enqueue(entity.Id, newValue);
    }
}
