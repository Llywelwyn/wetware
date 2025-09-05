using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Components;
using Wetware.Flags;

namespace Wetware.Systems;

/// <summary>
/// This system removes all <c>TakingTurn</c> tags from entities, so no turns linger into the next tick.
/// </summary>
/// 
/// <remarks>
/// This should always be the last system to run.
/// </remarks>
public class DoNothingSystem : QuerySystem<Name>
{
    public DoNothingSystem() => Filter.AllTags(Tags.Get<TakingTurn>());
    
    protected override void OnUpdate()
    {
        Debug.SystemBoundaryStart(nameof(DoNothingSystem));
        
        Query.ForEachEntity((ref Name name, Entity e) =>
        {
            Debug.Print($"{name.Value} spent its turn doing nothing.");
            CommandBuffer.RemoveTag<TakingTurn>(e.Id);
        });
        
        CommandBuffer.Playback();
    }
}