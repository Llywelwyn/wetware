using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Raylib_cs;
using Wetware.Systems.Render;
using Position = Wetware.Components.Position;

namespace Wetware.Screens;

class BackgroundScreen(Position origin, Position size) : Screen(origin, size)
{
    public override void HandleInput()
    {
        return;
    }

    public override void Render()
    {
        Raylib.ClearBackground(Color.Maroon);
    }
}

class MapScreen : Screen
{
    private static readonly SystemRoot systems = [];

    public MapScreen(Position origin, Position size) : base(origin, size)
    {
        systems.AddStore(Game.Instance.World);
        systems.Add(new EntityRenderSystem());
    }

    public override void HandleInput()
    {
        return;
    }

    public override void Render()
    {
        systems.Update(new UpdateTick());
    }
}
