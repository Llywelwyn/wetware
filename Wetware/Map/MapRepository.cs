namespace Wetware.Map;
using Components;

public class MapRepository
{
    private OnMap _currentMapIndex = new(0, 0);
    private readonly Dictionary<(int, int), Map> _maps = new();

    public Map CurrentMap() => _maps[_currentMapIndex.GetIndexedValue()];
    public void ChangeMap(OnMap map) => _currentMapIndex = map;
}
