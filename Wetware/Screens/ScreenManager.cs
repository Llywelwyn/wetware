using Raylib_cs;
using Wetware.Assets;
using Wetware.Components;
using Wetware.Maps;

namespace Wetware.Screens;

public class ScreenManager
{
    public readonly Atlas Atlas;

    public static int Width => Raylib.GetScreenWidth();
    public static int Height => Raylib.GetScreenHeight();
    public int WidthInTiles => Width / (int)Atlas.TileSize.X;
    public int HeightInTiles => Height / (int)Atlas.TileSize.Y;
    public int WidthPercentToNearestTile(float width) => (int)(WidthInTiles * width);
    public int HeightPercentToNearestTile(float height) => (int)(HeightInTiles * height);
    public int WidthInTilesMinus(int amount) => WidthInTiles - amount;
    public int HeightInTilesMinus(int amount) => HeightInTiles - amount;
    public Position ScreenPercentToNearestTile(float x, float y) => new((int)(WidthInTiles * x), (int)(HeightInTiles * y));
    public Position ScreenMinusTiles(int x, int y) => new(WidthInTiles - x, HeightInTiles - y);

    private readonly Stack m_screens = new();

    public ScreenManager(MapRepository mapRepository)
    {
        Atlas = new Atlas("Assets/atlas.png");
        m_screens.Push(new BackgroundScreen(
            new Position(0, 0),
            ScreenPercentToNearestTile(1.0f, 1.0f)));

        m_screens.Push(new MapScreen(
            mapRepository,
            new Position(1, 1),
            new Position(WidthPercentToNearestTile(0.75f), HeightInTilesMinus(2)),
            Atlas.TileSize));
    }

    public void Render() => m_screens.Render();
}
