using Friflo.Engine.ECS;

namespace Wetware.Components;

[ComponentKey("e")]
public struct Energy(int value) : IComponent
{
    [Friflo.Json.Fliox.Serialize("v")]
    public int Value = value;
}
