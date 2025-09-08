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
    private (int X, int Y) StandardMapDimensions = (50, 32);

    /// <summary>The index of the currently active map. Determined by the POV entity.</summary>
    private OnMap _currentMapIndex;

    /// <summary>All game maps indexed by position in the world.</summary>
    private readonly Dictionary<(int, int), Map> _maps;

    private readonly EntityStore _world;

    public MapRepository(EntityStore world)
    {
        _world = world;
        _currentMapIndex = new(0, 0);
        _maps = new();
        _maps.Add(_currentMapIndex.GetIndexedValue(), new Map(_currentMapIndex, StandardMapDimensions.X, StandardMapDimensions.Y, _world));
    }

    /// <summary>Fetches the active map.</summary>
    public Map CurrentMap() => _maps[_currentMapIndex.GetIndexedValue()];

    /// <summary>Changes the active map.</summary>
    public void ChangeMap(OnMap map) => _currentMapIndex = map;
}
