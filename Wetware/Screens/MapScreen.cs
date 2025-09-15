using System.Numerics;
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
        var tileSize = Game.Instance.ScreenManager.Atlas.TileSize;
        Raylib.ClearBackground(Color.RayWhite);
        Raylib.DrawRectangle(
                _origin.X * (int)tileSize.X,
                _origin.Y * (int)tileSize.Y,
                _size.X * (int)tileSize.X,
                _size.Y * (int)tileSize.Y,
                Color.DarkBrown);
    }
}

class MapScreen : Screen
{
    private static readonly SystemRoot m_systems = [];
    private readonly Vector2 m_tileSize;

    public MapScreen(Position origin, Position size, Vector2 tileSize) : base(origin, size)
    {
        m_systems.AddStore(Game.Instance.World);
        m_systems.Add(new EntityRenderSystem());
        m_tileSize = tileSize;
    }

    public override void HandleInput()
    {
        return;
    }

    public override void Render()
    {
        Raylib.DrawRectangle(
                _origin.X * (int)m_tileSize.X,
                _origin.Y * (int)m_tileSize.Y,
                _size.X * (int)m_tileSize.X,
                _size.Y * (int)m_tileSize.Y,
                Color.RayWhite);
        m_systems.Update(new UpdateTick());
    }
}
