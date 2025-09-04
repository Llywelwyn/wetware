using Friflo.Engine.ECS;

namespace Wetware.Components;

public struct Speed(int value) : IComponent
{
    public int Value = value;
}