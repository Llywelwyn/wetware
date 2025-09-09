using MessagePack;
using Wetware.Serializer;

[MessagePackObject]
public class SaveState
{
    [Key(0)]
    public byte[] World { get; set; } = Array.Empty<byte>();

    [Key(1)]
    public List<TurnQueueEntry> TurnQueue { get; set; } = new();

    [Key(2)]
    public bool ClockTurn { get; set; } = false;
}
