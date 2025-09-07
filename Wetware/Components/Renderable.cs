using Friflo.Engine.ECS;
using Wetware.Assets;

namespace Wetware.Components;

[ComponentKey("r")]
public struct Renderable(Sprite sprite) : IComponent
{
    [Friflo.Json.Fliox.Serialize("s")]
    public int Sprite = (int)sprite;
}
