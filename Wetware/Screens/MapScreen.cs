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
        Raylib.ClearBackground(Color.RayWhite);
        Raylib.DrawRectangle(_origin.X * 32, _origin.Y * 32, _size.X * 32, _size.Y * 32, Color.DarkBrown);
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
        Raylib.DrawRectangle(_origin.X * 32, _origin.Y * 32, _size.X * 32, _size.Y * 32, Color.RayWhite);
        systems.Update(new UpdateTick());
    }
}
