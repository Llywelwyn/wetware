using Friflo.Engine.ECS;

namespace Wetware.Components;


public struct Pos(int x, int y) : IIndexedComponent<(int, int)>
{
    public int X = x;
    public int Y = y;
    public (int, int) GetIndexedValue() => (X, Y);
}