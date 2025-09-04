using Friflo.Engine.ECS;

namespace Wetware.Components;

public struct Name(string value) : IComponent
{
    public string Value = value;
}