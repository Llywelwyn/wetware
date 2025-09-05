using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Components;
using Wetware.GameSystem;
using Wetware.Flags;

namespace Wetware.Systems;

/// <summary>
/// Adds energy to entities based on their speed. When energy exceeds TurnCost, the entity is granted a
/// <c>TakingTurn</c> tag.
/// </summary>
/// 
/// <remarks>
/// Save for exceptional circumstances, this should be the first system to run.
/// </remarks>
public class EnergySystem : QuerySystem<Energy, Speed>
{
    private const int NormalSpeed = 12;
    private const int TurnCost = NormalSpeed * 4;
    
    public EnergySystem() => Filter.WithoutAnyTags(Tags.Get<TakingTurn>());

    protected override void OnUpdate()
    {
        Debug.SystemBoundaryStart(nameof(EnergySystem));
        
        Query.ForEachEntity((ref Energy energy, ref Speed speed, Entity e) =>
        {
            if (CullTurnByDistance(e)) return;
            var potentialEnergy = (int)(speed.Value * GetBurdenModifier(e));
            
            var fullNormalSpeedUnits = potentialEnergy / NormalSpeed;
            var leftoverFractionalNormalSpeed = potentialEnergy % NormalSpeed;

            energy.Value += fullNormalSpeedUnits * NormalSpeed;
            if (Dice.Roll(1, NormalSpeed) <= leftoverFractionalNormalSpeed) energy.Value += NormalSpeed;
        
            Debug.Print($"{e.DebugName()} has {energy.Value} energy.");
            
            if (energy.Value < TurnCost) return;
        
            energy.Value -= TurnCost;
            CommandBuffer.AddTag<TakingTurn>(e.Id);
        });
        CommandBuffer.Playback();
    }
    
    private static float GetBurdenModifier(Entity _) => 1f;
    private static bool CullTurnByDistance(Entity _) => false;
}