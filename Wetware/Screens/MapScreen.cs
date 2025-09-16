using System.Numerics;
using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Raylib_cs;
using Wetware.Assets;
using Wetware.Maps;
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
        Raylib.ClearBackground(Palette.Background);
        Raylib.DrawRectangle(
                m_origin.X * (int)tileSize.X,
                m_origin.Y * (int)tileSize.Y,
                m_size.X * (int)tileSize.X,
                m_size.Y * (int)tileSize.Y,
                Palette.DarkGrey);
    }
}

class MapScreen : Screen
{
    private readonly SystemRoot m_systems = [];
    private readonly Vector2 m_tileSize;

    public MapScreen(MapRepository mapRepository, Position origin, Position size, Vector2 tileSize) : base(origin, size)
    {
        m_systems.Add(new EntityRenderSystem(origin, size));
        m_tileSize = tileSize;
        mapRepository.OnMapChanged += OnMapChanged;
    }

    private void OnMapChanged(Map? oldMap, Map newMap)
    {
        if (oldMap is not null) m_systems.RemoveStore(oldMap.Entities);
        m_systems.AddStore(newMap.Entities);
    }

    public override void HandleInput()
    {
        return;
    }

    public override void Render()
    {
        Raylib.DrawRectangle(
                m_origin.X * (int)m_tileSize.X,
                m_origin.Y * (int)m_tileSize.Y,
                m_size.X * (int)m_tileSize.X,
                m_size.Y * (int)m_tileSize.Y,
                Palette.Background);
        m_systems.Update(new UpdateTick());
    }
}
