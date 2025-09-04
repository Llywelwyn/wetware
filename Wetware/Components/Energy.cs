using Friflo.Engine.ECS;

namespace Wetware.Components;

public struct Energy(int value) : IComponent
{
    public int Value = value;
}