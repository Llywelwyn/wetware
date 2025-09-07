using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Wetware.Config;
using Wetware.Components;
using Position = Wetware.Components.Position;

namespace Wetware.Systems;

public class EntityRenderSystem : QuerySystem<Position, Renderable>
{
    protected override void OnUpdate()
    {
        Debug.SystemBoundaryStart(nameof(EntityRenderSystem));

        Query.ForEachEntity((ref Position position, ref Renderable renderable, Entity e) =>
        {
            Render(position.X, position.Y, renderable.Sprite);
        });

        CommandBuffer.Playback();
    }

    // TODO: actually render to a ScreenSurface.
    private void Render(int x, int y, int sprite)
    {
        Console.WriteLine($"drawing {sprite} at ({x}, {y})");
    }
}
