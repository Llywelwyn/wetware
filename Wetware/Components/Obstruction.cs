using Friflo.Engine.ECS;

namespace Wetware.Components;

public struct Obstruction(float value) : IComponent
{
    public float Value = value;
}