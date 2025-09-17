using Wetware.Maps;

namespace Wetware.Serializer.Mappers;

public static class MapMapper
{
    public static MapDto ToDto(Map map, int x, int y)
    {
        return new MapDto
        {
            X = x,
            Y = y,
            Width = map.Width,
            Height = map.Height,
            FlatTiles = WetwareSerializer.Flatten(map.Tiles),
            Entities = WetwareSerializer.SerializeEntityStore(map.Entities),
        };
    }

    public static Map FromDto(MapDto dto)
    {
        var entities = WetwareSerializer.DeserializeEntityStore(dto.Entities);
        var tiles = WetwareSerializer.Restore(dto.FlatTiles, dto.Width, dto.Height);
        return new(dto.Width, dto.Height, entities, tiles);
    }
}
