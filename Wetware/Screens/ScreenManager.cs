using Raylib_cs;
using Wetware.Components;

namespace Wetware.Screens;

public class ScreenManager
{
    public int TileSize = 16;
    public static int Width => Raylib.GetScreenWidth();
    public static int Height => Raylib.GetScreenHeight();
    public int WidthInTiles => Width / TileSize;
    public int HeightInTiles => Height / TileSize;

    public Stack screens = new();

    public ScreenManager()
    {
        screens.Push(new BackgroundScreen(
            new Position(0, 0),
            new Position(WidthInTiles, HeightInTiles)));

        screens.Push(new MapScreen(
            new Position(1, 1),
            new Position(WidthInTiles, HeightInTiles)));
    }

    public void Render() => screens.Render();
}
