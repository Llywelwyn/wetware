using Friflo.Engine.ECS;

namespace Wetware.Components;

[ComponentKey("p")]
public readonly struct Position(int x, int y) : IIndexedComponent<(int, int)>
{
    public readonly int X = x;
    public readonly int Y = y;
    public (int, int) GetIndexedValue() => (X, Y);
}
