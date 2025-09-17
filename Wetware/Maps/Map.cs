using Position = Wetware.Components.Position;
using Friflo.Engine.ECS;
using Wetware.Components;
using Wetware.Assets;
using Wetware.Flags;

namespace Wetware.Maps;

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
    /// <summary>Map width in tiles.</summary>
    public readonly int Width;

    /// <summary>Map height in tiles.</summary>
    public readonly int Height;

    /// <summary>
    /// An index of important flags set for each tile, like whether the given index blocks
    /// movement or vision.
    /// </summary>
    /// <remarks>
    /// Kept up to date via ECS event listeners for OnComponentChanged and EntityDeleted.
    /// </remarks>
    public TileFlag[,] Tiles;

    public readonly EntityStore Entities;

    /// <summary>Initialises event listeners for maintaining the Tiles index.</summary>
    private readonly PositionChangeListener m_listener;

    public Map(int width, int height, EntityStore entities, TileFlag[,] tiles)
    {
        Width = width;
        Height = height;
        Entities = entities;
        Tiles = tiles;
        m_listener = new PositionChangeListener(this, Entities);
    }

    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        Entities = new EntityStore();
        Tiles = new TileFlag[width, height];
        m_listener = new PositionChangeListener(this, Entities);

        var rng = new Random();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var color = rng.NextDouble() < 0.25 ? Palette.Red : Palette.Brown;
                if (rng.NextDouble() < 0.3)
                {
                    Entities.CreateEntity(new Renderable(Sprite.Wall, color), new Position(x, y), Tags.Get<BlocksMovement>());
                }
                else
                {
                    Entities.CreateEntity(new Renderable(Sprite.Floor, color), new Position(x, y));
                }
            }
        }
    }

    /// <summary>Checks if a given position is within the map bounds.</summary>
    public bool InBounds(Position pos) => pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;

    /// <summary>Sets a TileFlag for the given location.</summary>
    public void Set(Position pos, TileFlag flag) => Set(pos.X, pos.Y, flag);
    public void Set(int x, int y, TileFlag flag) => Tiles[x, y] |= flag;

    /// <summary>Checks for a TileFlag at the given location.</summary>
    public bool Has(Position pos, TileFlag flag) => Has(pos.X, pos.Y, flag);
    public bool Has(int x, int y, TileFlag flag) => (Tiles[x, y] & flag) != 0;

    /// <summary>Clears a TileFlag at the given location.</summary>
    public void Clear(Position pos, TileFlag flag) => Clear(pos.X, pos.Y, flag);
    public void Clear(int x, int y, TileFlag flag) => Tiles[x, y] &= ~flag;

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
