using Friflo.Engine.ECS;

namespace Wetware.Components;

public struct Health(int max, int? current = null) : IComponent
{
    public readonly int Max = max;
    public int Current = current ?? max;
}