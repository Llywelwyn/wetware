using Friflo.Engine.ECS.Systems;
using Wetware.Config;

namespace Wetware.Systems.Update.Turn;

/// <summary>Clears ClockTurn flag on Game.Instance.</summary>
/// <remarks>Should be the final system in the Turn namespace to run.</remarks>
public class FinalTurnSystem : QuerySystem
{
    protected override void OnUpdate()
    {
        if (!Game.Instance.ClockTurn) return;
        Debug.SystemBoundaryStart(nameof(FinalTurnSystem));
        Game.Instance.ClockTurn = false;
    }
}
