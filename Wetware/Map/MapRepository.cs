namespace Wetware.Map;

using Components;
using Friflo.Engine.ECS;

/// <summary>
/// Map storage. Stores all game maps in a Dictionary indexed by their (X, Y) position in the world, as well as the index of the
/// current map being rendered (determined by the map of the POV character).
/// </summary>
public class MapRepository
{
    /// <summary>
    /// The standard dimensions of a map. 50x32 tiles. 
    /// 1200x768 viewport with 12x12 tiles at 2x scale.
    /// </summary>
    private (int X, int Y) m_standardMapDimensions = (45, 31);

    /// <summary>The index of the currently active map. Determined by the POV entity.</summary>
    private OnMap m_currentMapIndex;

    /// <summary>All game maps indexed by position in the world.</summary>
    private readonly Dictionary<(int, int), Map> m_maps;

    private readonly EntityStore m_world;

    public MapRepository(EntityStore world)
    {
        m_world = world;
        m_currentMapIndex = new(0, 0);
        m_maps = [];
        m_maps.Add(m_currentMapIndex.GetIndexedValue(), new Map(m_currentMapIndex, m_standardMapDimensions.X, m_standardMapDimensions.Y, m_world));
    }

    /// <summary>Fetches the active map.</summary>
    public Map CurrentMap() => m_maps[m_currentMapIndex.GetIndexedValue()];

    /// <summary>Changes the active map.</summary>
    public void ChangeMap(OnMap map) => m_currentMapIndex = map;
}
