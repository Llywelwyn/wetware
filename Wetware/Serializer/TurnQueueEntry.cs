using MessagePack;

[MessagePackObject]
public struct TurnQueueEntry
{
    [Key(0)]
    public int Id { get; set; }

    [Key(1)]
    public int Priority { get; set; }
}
