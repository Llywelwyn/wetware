using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Components;
using Wetware.Config;
using Wetware.Globals;

namespace Wetware.Systems;

/// <summary>
/// Adds energy to entities based on their speed.
/// </summary>
///  
/// <remarks>
/// Save for exceptional circumstances, this should be the first system to run.
/// </remarks>
public class GrantEnergySystem : QuerySystem<Energy, Speed>
{
    /// <summary>Returns True if the system should run this tick. Only runs on ClockTurns.</summary>
    private bool ShouldRun()
    {
        if (!Game.ClockTurn)
        {
            Debug.Print($"=== Not a clock turn. Skipping {nameof(GrantEnergySystem)}.");
            return false;
        }
        Debug.SystemBoundaryStart(nameof(GrantEnergySystem));
        return true;
    }

    protected override void OnUpdate()
    {
        if (!ShouldRun()) return;

        Query.ForEachEntity((ref Energy energy, ref Speed speed, Entity e) =>
        {
            energy.Value += SpeedToActionPoints(ref speed);
            if (energy.Value > 0) TurnQueue.Enqueue(e.Id, energy.Value);
        });
        CommandBuffer.Playback();
    }

    private int SpeedToActionPoints(ref Speed speed) => speed.Value * 10;
}
