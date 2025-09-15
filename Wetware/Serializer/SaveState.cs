using MessagePack;

namespace Wetware.Serializer;

[MessagePackObject]
public class SaveState
{
    [Key(0)]
    public byte[] World { get; set; } = [];

    [Key(1)]
    public List<TurnQueueEntry> TurnQueue { get; set; } = [];

    [Key(2)]
    public bool ClockTurn { get; set; } = false;
}
