using Friflo.Engine.ECS;
using Wetware.Assets;

namespace Wetware.Components;

public struct Renderable(Sprite sprite) : IComponent
{
    public int Sprite = (int)sprite;
}
