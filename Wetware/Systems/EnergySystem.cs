using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Components;
using Wetware.GameSystem;
using Wetware.Flags;

namespace Wetware.Systems;

public class EnergySystem : QuerySystem<Name, Energy, Speed>
{
    private const int NormalSpeed = 12;
    private const int TurnCost = NormalSpeed * 4;
    
    public EnergySystem() => Filter.WithoutAnyTags(Tags.Get<TakingTurn>());

    protected override void OnUpdate()
    {
        Debug.SystemBoundaryStart(nameof(EnergySystem));
        
        Query.ForEachEntity((ref Name name, ref Energy energy, ref Speed speed, Entity e) =>
        {
            if (CullTurnByDistance(e)) return;
            var potentialEnergy = (int)(speed.Value * GetBurdenModifier(e));
            
            var fullNormalSpeedUnits = potentialEnergy / NormalSpeed;
            var leftoverFractionalNormalSpeed = potentialEnergy % NormalSpeed;

            energy.Value += fullNormalSpeedUnits * NormalSpeed;
            if (Dice.Roll(1, NormalSpeed) <= leftoverFractionalNormalSpeed) energy.Value += NormalSpeed;
        
            Debug.Print($"{name.Value} has {energy.Value} energy.");
            
            if (energy.Value < TurnCost) return;
        
            energy.Value -= TurnCost;
            CommandBuffer.AddTag<TakingTurn>(e.Id);
        });
        CommandBuffer.Playback();
    }
    
    private static float GetBurdenModifier(Entity _) => 1f;
    private static bool CullTurnByDistance(Entity _) => false;
}