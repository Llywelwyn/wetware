using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Flags;
using Wetware.Components;
using Position = Wetware.Components.Position;
using Raylib_cs;

namespace Wetware.Systems.Render;

/// <summary>
/// Queries Renderable entities with a Position. Filters out anything not currently seen by the player,
/// and renders to the screen.
/// </summary>
/// <remarks>RenderSystem - no data should change here, save for Render-specific flags.</remarks>
public class EntityRenderSystem : QuerySystem<Position, Renderable>
{
    // TODO: Uncomment this once FOV is in.
    //public EntityRenderSystem() => Filter.AllTags(Tags.Get<SeenByPov>());

    protected override void OnUpdate()
    {
        Query.ForEachEntity((ref Position position, ref Renderable renderable, Entity e) =>
        {
            Game.Instance.ScreenManager.Atlas.Draw(
                renderable.Sprite,
                new System.Numerics.Vector2(position.X, position.Y),
                renderable.Color
            );
        });
    }
}
