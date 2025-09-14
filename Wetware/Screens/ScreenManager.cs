using Raylib_cs;
using Wetware.Components;

namespace Wetware.Screens;

public class ScreenManager
{
    public int TileSize = 32;
    public static int Width => Raylib.GetScreenWidth();
    public static int Height => Raylib.GetScreenHeight();
    public int WidthInTiles => Width / TileSize;
    public int HeightInTiles => Height / TileSize;

    public Position ScreenPercentToNearestTile(float x, float y)
    {
        return new Position(
            (int)(WidthInTiles * x),
            (int)(HeightInTiles * y)
        );
    }

    public int WidthPercentToNearestTile(float width) => (int)(WidthInTiles * width);
    public int HeightPercentToNearestTile(float height) => (int)(HeightInTiles * height);
    public int WidthInTilesMinus(int amount) => WidthInTiles - amount;
    public int HeightInTilesMinus(int amount) => HeightInTiles - amount;

    public Position ScreenMinusTiles(int x, int y)
    {
        return new Position(WidthInTiles - x, HeightInTiles - y);
    }

    public Stack screens = new();

    public ScreenManager()
    {
        screens.Push(new BackgroundScreen(
            new Position(0, 0),
            ScreenPercentToNearestTile(1.0f, 1.0f)));

        screens.Push(new MapScreen(
            new Position(1, 1),
            new Position(WidthPercentToNearestTile(0.75f), HeightInTilesMinus(2))));
    }

    public void Render() => screens.Render();
}
