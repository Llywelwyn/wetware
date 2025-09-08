namespace Wetware.Map;
using Components;

/// <summary>
/// Map storage. Stores all game maps in a Dictionary indexed by their (X, Y) position in the world, as well as the index of the
/// current map being rendered (determined by the map of the POV character).
/// </summary>
public class MapRepository
{
    /// <summary>The index of the currently active map. Determined by the POV entity.</summary>
    private OnMap _currentMapIndex;

    /// <summary>All game maps indexed by position in the world.</summary>
    private readonly Dictionary<(int, int), Map> _maps;

    public MapRepository()
    {
        _currentMapIndex = new(0, 0);
        _maps = new();
    }

    /// <summary>Fetches the active map.</summary>
    public Map CurrentMap() => _maps[_currentMapIndex.GetIndexedValue()];

    /// <summary>Changes the active map.</summary>
    public void ChangeMap(OnMap map) => _currentMapIndex = map;
}
