using Friflo.Engine.ECS;

namespace Wetware.Components;

[ComponentKey("h")]
public struct Health(int max, int? current = null) : IComponent
{
    [Friflo.Json.Fliox.Serialize("m")]
    public readonly int Max = max;
    [Friflo.Json.Fliox.Serialize("c")]
    public int Current = current ?? max;
}
