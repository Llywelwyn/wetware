using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Config;
using Wetware.Flags;
using Wetware.Components;
using Position = Wetware.Components.Position;

namespace Wetware.Systems;

/// <summary>
/// Queries Renderable entities with a Position. Filters out anything not currently seen by the player,
/// and renders to the screen.
/// </summary>
/// <remarks>RenderSystem - no data should change here, save for Render-specific flags.</remarks>
public class EntityRenderSystem : QuerySystem<Position, Renderable>
{
    public EntityRenderSystem() => Filter.AllTags(Tags.Get<SeenByPov>());

    protected override void OnUpdate()
    {
        Debug.SystemBoundaryStart(nameof(EntityRenderSystem));

        Console.Clear();

        Query.ForEachEntity((ref Position position, ref Renderable renderable, Entity e) =>
        {
            Render(position.X, position.Y, renderable.Sprite);
        });
    }

    // TODO: actually render to a ScreenSurface.
    private void Render(int x, int y, int sprite)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write((char)sprite);
        //Console.WriteLine($"drawing {(char)sprite} at ({x}, {y})");
    }
}
