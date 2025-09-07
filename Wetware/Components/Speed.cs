using Friflo.Engine.ECS;

namespace Wetware.Components;

[ComponentKey("s")]
public struct Speed(int value) : IComponent
{
    [Friflo.Json.Fliox.Serialize("v")]
    public int Value = value;
}
