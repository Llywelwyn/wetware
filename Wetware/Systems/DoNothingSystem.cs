using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Components;
using Wetware.Flags;

namespace Wetware.Systems;

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