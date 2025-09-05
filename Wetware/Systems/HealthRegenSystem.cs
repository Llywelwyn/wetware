using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Components;

namespace Wetware.Systems;

/// <summary>
/// Iterates entities with Health, checks if the current turn is a regen turn, and regenerates health if so.
/// </summary>
///
/// <remarks>
/// This system should run before or after the existence of <c>TakingTurn</c> tags. Restoring health may impact the
/// decision an entity makes when taking its turn. Therefore, run before turns are assigned or after turns are resolved.
/// </remarks>
public class HealthRegenSystem : QuerySystem<Health>
{
    protected override void OnUpdate()
    {
        Debug.SystemBoundaryStart(nameof(HealthRegenSystem));

        if (!Game.ClockTurn) return;
        
        Query.ForEachEntity((ref Health health, Entity e) =>
        {
            if (IsRegenTurn(e)) health.Current = Math.Min(health.Max, health.Current + GetRegenPerTick(e));
            Debug.Print($"{e.DebugName()} health ticked to {health.Current}/{health.Max}.");
        });
        
        CommandBuffer.Playback();
    }

    /// <summary>Checks if the given entity should restore health on this tick.</summary>
    private static bool IsRegenTurn(Entity e) => true;
    
    /// <summary>Gets the amount of health the entity should restore each tick.</summary>
    private static int GetRegenPerTick(Entity e) => 1;
}