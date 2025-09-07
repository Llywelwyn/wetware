using Friflo.Engine.ECS;

namespace Wetware.Components;

[ComponentKey("n")]
public struct Name(string value) : IComponent
{
    [Friflo.Json.Fliox.Serialize("v")]
    public string Value = value;
}
