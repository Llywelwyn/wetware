using Friflo.Engine.ECS;

namespace Wetware.Components;

[ComponentKey("m")]
public struct Movement : IComponent
{
    [Friflo.Json.Fliox.Serialize("m")]
    public MovementMode Mode;
}

public enum MovementMode
{
    Static,
    Random,
}
