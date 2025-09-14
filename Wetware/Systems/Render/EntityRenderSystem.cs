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
            Render(position.X, position.Y, renderable.Sprite);
        });
    }

    // TODO: actually render to a ScreenSurface.
    private void Render(int x, int y, int sprite)
    {
        Raylib.DrawRectangle(x.Scale() + 1.Scale(), y.Scale() + 1.Scale(), 1.Scale(), 1.Scale(), Color.Black);
    }
}

public static class IntExtensions
{
    private static readonly int SpriteSize = 16;
    private static readonly int ScaleFactor = 2;

    public static int Scale(this int i) => i * SpriteSize * ScaleFactor;
}
