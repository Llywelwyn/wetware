using Friflo.Engine.ECS.Systems;
using Friflo.Engine.ECS;
using Wetware.Globals;
using Wetware.Flags;
using Wetware.Components;

namespace Wetware.Systems;

public class EntityTurnSystem : QuerySystem
{
    protected override void OnUpdate()
    {
        if (TurnQueue.Next() is not int id) return;

        var entity = Query.Store.GetEntityById(id);
        if (entity.IsNull) return;

        var data = entity.Data;

        if (data.Tags.Has<Clock>())
        {
            entity.AddComponent<Energy>(new Energy(0));
            Game.Instance.ClockTurn = true;
        }

        if (!data.TryGet<Brain>(out var brain)) return;

        // At this point, we have our Entity, EntityData, and Entity's Brain component,
        // and our entity can act. Whatever action it takes, UseEnergy(entity, <amount>).
        return;
    }

    private void UseEnergy(Entity entity, int amount)
    {
        var energy = entity.GetComponent<Energy>();
        energy.Value -= amount;
        TurnQueue.Enqueue(entity.Id, energy.Value);
    }
}
