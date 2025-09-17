using Wetware.Components;
using Wetware.Maps;

namespace Wetware.Serializer.Mappers;

public static class MapRepositoryMapper
{
    public static MapRepositoryDto ToDto(MapRepository repository)
    {
        return new MapRepositoryDto
        {
            CurrentMapIndex = (repository.CurrentMapIndex.X, repository.CurrentMapIndex.Y),
            Maps = [.. repository.Maps.Select(kvp => MapMapper.ToDto(kvp.Value, kvp.Key.Item1, kvp.Key.Item2))],
        };
    }

    public static MapRepository FromDto(MapRepositoryDto dto)
    {
        OnMap currentMapIndex = new(dto.CurrentMapIndex.X, dto.CurrentMapIndex.Y); ;
        Dictionary<(int, int), Map> maps = [];

        foreach (var mapDto in dto.Maps)
        {
            var map = MapMapper.FromDto(mapDto);
            maps.Add((mapDto.X, mapDto.Y), map);
        }

        return new MapRepository(maps, currentMapIndex);
    }
}
