using MessagePack;
using Wetware.Serializer.Mappers;

namespace Wetware.Serializer;

[MessagePackObject]
public class SaveState
{
    [Key(0)]
    public required string Name { get; set; }

    [Key(1)]
    public required byte[] World { get; set; }

    [Key(2)]
    public required List<TurnQueueEntry> TurnQueue { get; set; }

    [Key(3)]
    public required bool ClockTurn { get; set; }

    [Key(4)]
    public required MapRepositoryDto MapRepository { get; set; }
}
