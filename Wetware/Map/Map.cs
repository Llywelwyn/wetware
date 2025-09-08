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

public enum Edge
{
    Left,
    Right,
    Top,
    Bottom
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

    /// <summary>
    /// Sweeps across the map from the given Edge to find the closest, walkable tile to the given Position.
    /// </summary>
    /// <remarks>
    /// This can, in theory, return null, but such would only be the case if the entire map was non-walkable. This case
    /// should never occur in any scenario I can think of. If some design decision means that this can occur, a fallback
    /// should be in place for this function finding no place to position the entity on their new map.
    /// </remarks>
    public Position? FindNearestTileOnEdge(Position pos, Edge dir)
    {
        if (!InBounds(pos)) throw new ArgumentOutOfRangeException(nameof(pos));

        int initialX = pos.X;
        int initialY = pos.Y;

        switch (dir)
        {
            case Edge.Left:
                initialX = 0;
                break;
            case Edge.Right:
                initialX = Width - 1;
                break;
            case Edge.Top:
                initialY = 0;
                break;
            case Edge.Bottom:
                initialY = Height - 1;
                break;
        }

        int maxDepth = dir == Edge.Left || dir == Edge.Right ? Width : Height;

        for (int depth = 0; depth < maxDepth; depth++)
        {
            int x = dir switch
            {
                Edge.Left => initialX + depth,
                Edge.Right => initialX - depth,
                _ => initialX,
            };

            int y = dir switch
            {
                Edge.Top => initialY + depth,
                Edge.Bottom => initialY - depth,
                _ => initialY,
            };

            if (dir == Edge.Left || dir == Edge.Right)
            {
                for (int offset = 0; offset < Height; offset++)
                {
                    int up = initialY + offset;
                    int down = initialY - offset;

                    if (up < Height && !Has(x, up, TileFlag.BlocksMovement))
                        return new Position(x, up);

                    if (down >= 0 && !Has(x, down, TileFlag.BlocksMovement))
                        return new Position(x, down);
                }
            }
            else
            {
                for (int offset = 0; offset < Width; offset++)
                {
                    int right = initialX + offset;
                    int left = initialX - offset;

                    if (right < Width && !Has(right, y, TileFlag.BlocksMovement))
                        return new Position(right, y);

                    if (left >= 0 && !Has(left, y, TileFlag.BlocksMovement))
                        return new Position(left, y);
                }
            }
        }

        return null;
    }
}
