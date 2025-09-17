using MessagePack;
using Wetware.Maps;

namespace Wetware.Serializer.Mappers;

[MessagePackObject]
public class MapRepositoryDto
{
    [Key(0)]
    public (int X, int Y) CurrentMapIndex { get; set; }

    [Key(1)]
    public List<MapDto> Maps = [];
}

[MessagePackObject]
public class MapDto
{
    /// <summary>Indexes for MapRepository.</summary>
    [Key(0)] public int X { get; set; }
    [Key(1)] public int Y { get; set; }

    [Key(2)]
    public int Width { get; set; }

    [Key(3)]
    public int Height { get; set; }

    [Key(4)]
    public TileFlag[] FlatTiles { get; set; } = [];

    [Key(5)]
    public byte[] Entities { get; set; } = [];
}
