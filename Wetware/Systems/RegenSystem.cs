using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Components;

namespace Wetware.Systems;

public class RegenSystem : QuerySystem<Health>
{
    protected override void OnUpdate()
    {
        Debug.SystemBoundaryStart(nameof(RegenSystem));

        if (!Game.ClockTurn) return;
        
        Query.ForEachEntity((ref Health health, Entity e) =>
        {
            if (IsRegenTurn(e)) health.Current = Math.Min(health.Max, health.Current + GetRegenPerTick(e));
            Debug.Print($"{e.DebugName()} health ticked to {health.Current}/{health.Max}.");
        });
        
        CommandBuffer.Playback();
    }

    private static bool IsRegenTurn(Entity e) => true;
    private static int GetRegenPerTick(Entity e) => 1;
}