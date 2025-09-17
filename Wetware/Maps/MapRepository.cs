namespace Wetware.Maps;

using Components;

/// <summary>
/// Map storage. Stores all game maps in a Dictionary indexed by their (X, Y) position in the world, as well as the index of the
/// current map being rendered (determined by the map of the POV character).
/// </summary>
public class MapRepository(Dictionary<(int, int), Map> maps, OnMap currentMapIndex)
{
    /// <summary>
    /// The standard dimensions of a map. 50x32 tiles. 
    /// 1200x768 viewport with 12x12 tiles at 2x scale.
    /// </summary>
    private (int X, int Y) m_standardMapDimensions = (45, 31);

    /// <summary>The index of the currently active map. Determined by the POV entity.</summary>
    private OnMap m_currentMapIndex = currentMapIndex;
    public OnMap CurrentMapIndex => m_currentMapIndex;

    /// <summary>All game maps indexed by position in the world.</summary>
    private readonly Dictionary<(int, int), Map> m_maps = maps;
    public IReadOnlyDictionary<(int, int), Map> Maps => m_maps;

    public event Action<Map?, Map>? OnMapChanged;

    /// <summary>Creates a new MapRepository with no maps and the current map index at (0, 0).</summary>
    public MapRepository() : this([], new(0, 0)) { }

    public void Initialise()
    {
        CreateMapIfNeeded(m_currentMapIndex);
        OnMapChanged?.Invoke(null, CurrentMap());
    }

    /// <summary>Fetches the active map.</summary>
    public Map CurrentMap() => m_maps[m_currentMapIndex.GetIndexedValue()];

    /// <summary>Changes the active map.</summary>
    public void ChangeMap(OnMap map)
    {
        var oldMap = CurrentMap();
        CreateMapIfNeeded(map);
        m_currentMapIndex = map;
        var newMap = CurrentMap();
        OnMapChanged?.Invoke(oldMap, newMap);
    }

    private void CreateMapIfNeeded(OnMap map)
    {
        if (!m_maps.ContainsKey(map.GetIndexedValue()))
        {
            m_maps.Add(map.GetIndexedValue(), new Map(m_standardMapDimensions.X, m_standardMapDimensions.Y));
        }
    }
}
