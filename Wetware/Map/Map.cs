using Position = Wetware.Components.Position;
using Friflo.Engine.ECS;

namespace Wetware.Map;

[Flags]
public enum TileFlag
{
    None = 0,
    BlocksMovement = 1 << 0,
    BlocksVision = 1 << 2,
    BlocksItems = 1 << 3,
}

public class Map
{
    public readonly int Width;
    public readonly int Height;
    public TileFlag[,] _tiles;
    private readonly PositionChangeListener _listener;

    public Map(int width, int height, EntityStore? world = null)
    {
        Width = width;
        Height = height;
        _tiles = new TileFlag[width, height];
        _listener = new PositionChangeListener(this, world);
    }

    public bool InBounds(Position pos) => pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;

    public void Set(Position pos, TileFlag flag) => Set(pos.X, pos.Y, flag);
    public void Set(int x, int y, TileFlag flag) => _tiles[x, y] |= flag;

    public bool Has(Position pos, TileFlag flag) => Has(pos.X, pos.Y, flag);
    public bool Has(int x, int y, TileFlag flag) => (_tiles[x, y] & flag) != 0;

    public void Clear(Position pos, TileFlag flag) => Clear(pos.X, pos.Y, flag);
    public void Clear(int x, int y, TileFlag flag) => _tiles[x, y] &= ~flag;
}
