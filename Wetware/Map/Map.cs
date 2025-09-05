using Friflo.Engine.ECS;
using Wetware.Components;
using Position = Wetware.Components.Position;

namespace Wetware.Map;

public class Map
{
    public readonly int Width;
    public readonly int Height;
    private float[,] _obstruction;
    private readonly PositionChangeListener _listener;

    public Map(int width, int height)
    {
        Width = width;
        Height = height;
        _obstruction = new float[width, height];
        _listener = new PositionChangeListener(this);
    }
    
    public ref float this[Position pos] => ref _obstruction[pos.X, pos.Y];
    
    public bool InBounds(Position pos) => pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;
}