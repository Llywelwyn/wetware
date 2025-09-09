using Friflo.Engine.ECS;

public struct Brain : IComponent
{
    public bool Wanders;
    public int MaxWanderRadius;
    public bool Hostile;
    public bool Calm;
}
