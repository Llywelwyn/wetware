using Friflo.Engine.ECS;

namespace Wetware.Components;

public struct Movement : IComponent
{
    public MovementMode Mode;
}

public enum MovementMode
{
    Static,
    Random,
}