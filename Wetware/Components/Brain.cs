using Friflo.Engine.ECS;

namespace Wetware.Components;

public struct Brain : IComponent
{
    public bool Wanders;
    public int MaxWanderRadius;
    public bool Hostile;
    public bool Calm;
}

public static class BrainPresets
{
    public static Brain Passive()
    {
        return new Brain
        {
            Wanders = true,
            MaxWanderRadius = 0,
            Hostile = false,
            Calm = true,
        };
    }
}
