using Position = Wetware.Components.Position;

namespace Wetware.Map;

public class Map
{
    public readonly int Width;
    public readonly int Height;
    private bool[,] _obstruction;
    private readonly PositionChangeListener _listener;

    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        _obstruction = new bool[width, height];
        _listener = new PositionChangeListener(this);
    }

    public ref bool this[Position pos] => ref _obstruction[pos.X, pos.Y];
    public ref bool this[int x, int y] => ref _obstruction[x, y];

    public bool InBounds(Position pos) => pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;
}
