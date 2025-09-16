namespace Wetware.Maps;

public enum Edge
{
    Left,
    Right,
    Top,
    Bottom
}

public static class EdgeExtensions
{
    public static Edge Opposite(this Edge edge) => edge switch
    {
        Edge.Left => Edge.Right,
        Edge.Right => Edge.Left,
        Edge.Top => Edge.Bottom,
        Edge.Bottom => Edge.Top,
        _ => throw new ArgumentOutOfRangeException(nameof(edge)),
    };
}
