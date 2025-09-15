using Friflo.Engine.ECS;
using Raylib_cs;
using Wetware.Assets;

namespace Wetware.Components;

[ComponentKey("r")]
public struct Renderable(Sprite sprite, Color color) : IComponent
{
    [Friflo.Json.Fliox.Serialize("s")]
    public Sprite Sprite = sprite;

    [Friflo.Json.Fliox.Serialize("c")]
    public Color Color = color;
}
